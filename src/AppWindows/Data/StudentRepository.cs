using AppWindows.Models;
using Microsoft.Data.Sqlite;

namespace AppWindows.Data;

public sealed class StudentRepository
{
    private readonly DatabaseService databaseService;

    public StudentRepository(DatabaseService databaseService)
    {
        this.databaseService = databaseService;
    }

    public List<Tag> GetTags()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Name, IsPreset
            FROM Tags
            ORDER BY IsPreset DESC, Name COLLATE NOCASE;
            """;

        var tags = new List<Tag>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            tags.Add(ReadTag(reader));
        }

        return tags;
    }

    public List<Student> GetStudents(string? searchText, string? tagId)
    {
        using var connection = OpenConnection();
        var students = QueryStudents(connection, searchText, tagId);
        HydrateTags(connection, students);
        return students;
    }

    public List<Student> GetRecentStudents(int count)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, FullName, DniNie, Phone, CreatedAt, Notes
            FROM Students
            ORDER BY CreatedAt DESC, FullName COLLATE NOCASE
            LIMIT $count;
            """;
        command.Parameters.AddWithValue("$count", Math.Max(0, count));

        var students = new List<Student>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            students.Add(ReadStudent(reader));
        }

        HydrateTags(connection, students);
        return students;
    }

    public int GetStudentCount()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Students;";
        return Convert.ToInt32(command.ExecuteScalar());
    }

    public int GetTagCount()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Tags;";
        return Convert.ToInt32(command.ExecuteScalar());
    }

    public Student CreateStudent(string fullName, string dniNie, string phone, string notes, IReadOnlyCollection<string> tagIds)
    {
        var student = new Student
        {
            Id = Guid.NewGuid().ToString("N"),
            FullName = fullName.Trim(),
            DniNie = NormalizeDni(dniNie),
            Phone = phone.Trim(),
            Notes = notes.Trim(),
            CreatedAt = DateTime.Today
        };

        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            InsertStudent(connection, transaction, student);
            ReplaceStudentTags(connection, transaction, student.Id, tagIds);
            transaction.Commit();
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 19)
        {
            throw new DuplicateDniException(student.DniNie);
        }

        HydrateTags(connection, [student]);
        return student;
    }

    public void UpdateStudent(Student student, IReadOnlyCollection<string> tagIds)
    {
        student.FullName = student.FullName.Trim();
        student.DniNie = NormalizeDni(student.DniNie);
        student.Phone = student.Phone.Trim();
        student.Notes = student.Notes.Trim();

        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText =
                """
                UPDATE Students
                SET FullName = $fullName,
                    DniNie = $dniNie,
                    Phone = $phone,
                    Notes = $notes
                WHERE Id = $id;
                """;
            command.Parameters.AddWithValue("$id", student.Id);
            command.Parameters.AddWithValue("$fullName", student.FullName);
            command.Parameters.AddWithValue("$dniNie", student.DniNie);
            command.Parameters.AddWithValue("$phone", student.Phone);
            command.Parameters.AddWithValue("$notes", student.Notes);
            command.ExecuteNonQuery();

            ReplaceStudentTags(connection, transaction, student.Id, tagIds);
            transaction.Commit();
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 19)
        {
            throw new DuplicateDniException(student.DniNie);
        }
    }

    public Tag CreateCustomTag(string name)
    {
        var trimmedName = name.Trim();
        if (string.IsNullOrWhiteSpace(trimmedName))
        {
            throw new ArgumentException("El tag no puede estar vacio.", nameof(name));
        }

        using var connection = OpenConnection();
        using var transaction = connection.BeginTransaction();

        using var insertCommand = connection.CreateCommand();
        insertCommand.Transaction = transaction;
        insertCommand.CommandText =
            """
            INSERT INTO Tags (Id, Name, IsPreset)
            VALUES ($id, $name, 0)
            ON CONFLICT(Name) DO NOTHING;
            """;
        insertCommand.Parameters.AddWithValue("$id", Guid.NewGuid().ToString("N"));
        insertCommand.Parameters.AddWithValue("$name", trimmedName);
        insertCommand.ExecuteNonQuery();

        using var selectCommand = connection.CreateCommand();
        selectCommand.Transaction = transaction;
        selectCommand.CommandText =
            """
            SELECT Id, Name, IsPreset
            FROM Tags
            WHERE Name = $name COLLATE NOCASE;
            """;
        selectCommand.Parameters.AddWithValue("$name", trimmedName);

        using var reader = selectCommand.ExecuteReader();
        if (!reader.Read())
        {
            throw new InvalidOperationException("No se pudo crear el tag.");
        }

        var tag = ReadTag(reader);
        transaction.Commit();
        return tag;
    }

    private SqliteConnection OpenConnection()
    {
        var connection = databaseService.CreateConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();

        return connection;
    }

    private static List<Student> QueryStudents(SqliteConnection connection, string? searchText, string? tagId)
    {
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, FullName, DniNie, Phone, CreatedAt, Notes
            FROM Students
            WHERE ($search = ''
                   OR FullName LIKE $searchLike COLLATE NOCASE
                   OR DniNie LIKE $searchLike COLLATE NOCASE
                   OR Phone LIKE $searchLike COLLATE NOCASE)
              AND ($tagId = ''
                   OR EXISTS (
                       SELECT 1
                       FROM StudentTags
                       WHERE StudentTags.StudentId = Students.Id
                         AND StudentTags.TagId = $tagId
                   ))
            ORDER BY CreatedAt DESC, FullName COLLATE NOCASE;
            """;

        var search = searchText?.Trim() ?? string.Empty;
        command.Parameters.AddWithValue("$search", search);
        command.Parameters.AddWithValue("$searchLike", $"%{search}%");
        command.Parameters.AddWithValue("$tagId", tagId ?? string.Empty);

        var students = new List<Student>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            students.Add(ReadStudent(reader));
        }

        return students;
    }

    private static void HydrateTags(SqliteConnection connection, IReadOnlyCollection<Student> students)
    {
        if (students.Count == 0)
        {
            return;
        }

        var studentsById = students.ToDictionary(student => student.Id);

        using var command = connection.CreateCommand();
        command.CommandText =
            $"""
            SELECT StudentTags.StudentId, Tags.Id, Tags.Name, Tags.IsPreset
            FROM StudentTags
            INNER JOIN Tags ON Tags.Id = StudentTags.TagId
            WHERE StudentTags.StudentId IN ({string.Join(", ", students.Select((_, index) => $"$id{index}"))})
            ORDER BY Tags.IsPreset DESC, Tags.Name COLLATE NOCASE;
            """;

        var parameterIndex = 0;
        foreach (var student in students)
        {
            command.Parameters.AddWithValue($"$id{parameterIndex}", student.Id);
            parameterIndex++;
        }

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var studentId = reader.GetString(0);
            if (studentsById.TryGetValue(studentId, out var student))
            {
                student.Tags.Add(new Tag
                {
                    Id = reader.GetString(1),
                    Name = reader.GetString(2),
                    IsPreset = reader.GetInt32(3) == 1
                });
            }
        }
    }

    private static void InsertStudent(SqliteConnection connection, SqliteTransaction transaction, Student student)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT INTO Students (Id, FullName, DniNie, Phone, CreatedAt, Notes)
            VALUES ($id, $fullName, $dniNie, $phone, $createdAt, $notes);
            """;
        command.Parameters.AddWithValue("$id", student.Id);
        command.Parameters.AddWithValue("$fullName", student.FullName);
        command.Parameters.AddWithValue("$dniNie", student.DniNie);
        command.Parameters.AddWithValue("$phone", student.Phone);
        command.Parameters.AddWithValue("$createdAt", student.CreatedAt.ToString("O"));
        command.Parameters.AddWithValue("$notes", student.Notes);
        command.ExecuteNonQuery();
    }

    private static void ReplaceStudentTags(
        SqliteConnection connection,
        SqliteTransaction transaction,
        string studentId,
        IReadOnlyCollection<string> tagIds)
    {
        using var deleteCommand = connection.CreateCommand();
        deleteCommand.Transaction = transaction;
        deleteCommand.CommandText = "DELETE FROM StudentTags WHERE StudentId = $studentId;";
        deleteCommand.Parameters.AddWithValue("$studentId", studentId);
        deleteCommand.ExecuteNonQuery();

        foreach (var tagId in tagIds.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            using var insertCommand = connection.CreateCommand();
            insertCommand.Transaction = transaction;
            insertCommand.CommandText =
                """
                INSERT INTO StudentTags (StudentId, TagId)
                VALUES ($studentId, $tagId);
                """;
            insertCommand.Parameters.AddWithValue("$studentId", studentId);
            insertCommand.Parameters.AddWithValue("$tagId", tagId);
            insertCommand.ExecuteNonQuery();
        }
    }

    private static Student ReadStudent(SqliteDataReader reader)
    {
        return new Student
        {
            Id = reader.GetString(0),
            FullName = reader.GetString(1),
            DniNie = reader.GetString(2),
            Phone = reader.GetString(3),
            CreatedAt = DateTime.Parse(reader.GetString(4)),
            Notes = reader.GetString(5)
        };
    }

    private static Tag ReadTag(SqliteDataReader reader)
    {
        return new Tag
        {
            Id = reader.GetString(0),
            Name = reader.GetString(1),
            IsPreset = reader.GetInt32(2) == 1
        };
    }

    private static string NormalizeDni(string dniNie)
    {
        return dniNie.Trim().ToUpperInvariant();
    }
}
