using Microsoft.Data.Sqlite;

namespace AppWindows.Data;

public sealed class DatabaseService
{
    private const int SchemaVersion = 1;

    private static readonly string[] PresetTags =
    [
        "AM",
        "A1",
        "A2",
        "A",
        "B",
        "B+E",
        "C1",
        "C1+E",
        "C",
        "C+E",
        "D1",
        "D1+E",
        "D",
        "D+E"
    ];

    public DatabaseService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DatabaseDirectory = Path.Combine(appData, "AppWindows");
        DatabasePath = Path.Combine(DatabaseDirectory, "appwindows.db");
    }

    public string DatabaseDirectory { get; }

    public string DatabasePath { get; }

    public string ConnectionString
    {
        get
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = DatabasePath
            };

            return builder.ToString();
        }
    }

    public void Initialize()
    {
        Directory.CreateDirectory(DatabaseDirectory);

        using var connection = CreateConnection();
        connection.Open();

        ExecuteNonQuery(connection, "PRAGMA foreign_keys = ON;");
        ExecuteNonQuery(
            connection,
            """
            CREATE TABLE IF NOT EXISTS AppMetadata (
                Key TEXT PRIMARY KEY,
                Value TEXT NOT NULL
            );
            """);

        ExecuteNonQuery(
            connection,
            """
            CREATE TABLE IF NOT EXISTS Students (
                Id TEXT PRIMARY KEY,
                FullName TEXT NOT NULL,
                DniNie TEXT NOT NULL COLLATE NOCASE UNIQUE,
                Phone TEXT NOT NULL,
                CreatedAt TEXT NOT NULL,
                Notes TEXT NOT NULL DEFAULT ''
            );
            """);

        ExecuteNonQuery(
            connection,
            """
            CREATE TABLE IF NOT EXISTS Tags (
                Id TEXT PRIMARY KEY,
                Name TEXT NOT NULL COLLATE NOCASE UNIQUE,
                IsPreset INTEGER NOT NULL DEFAULT 0
            );
            """);

        ExecuteNonQuery(
            connection,
            """
            CREATE TABLE IF NOT EXISTS StudentTags (
                StudentId TEXT NOT NULL,
                TagId TEXT NOT NULL,
                PRIMARY KEY (StudentId, TagId),
                FOREIGN KEY (StudentId) REFERENCES Students(Id) ON DELETE CASCADE,
                FOREIGN KEY (TagId) REFERENCES Tags(Id) ON DELETE CASCADE
            );
            """);

        using var transaction = connection.BeginTransaction();
        UpsertMetadata(connection, transaction, "schema_version", SchemaVersion.ToString());
        foreach (var tag in PresetTags)
        {
            SeedTag(connection, transaction, tag);
        }

        transaction.Commit();
    }

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(ConnectionString);
    }

    private static void ExecuteNonQuery(SqliteConnection connection, string commandText)
    {
        using var command = connection.CreateCommand();
        command.CommandText = commandText;
        command.ExecuteNonQuery();
    }

    private static void UpsertMetadata(SqliteConnection connection, SqliteTransaction transaction, string key, string value)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT INTO AppMetadata (Key, Value)
            VALUES ($key, $value)
            ON CONFLICT(Key) DO UPDATE SET Value = excluded.Value;
            """;
        command.Parameters.AddWithValue("$key", key);
        command.Parameters.AddWithValue("$value", value);
        command.ExecuteNonQuery();
    }

    private static void SeedTag(SqliteConnection connection, SqliteTransaction transaction, string name)
    {
        using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT INTO Tags (Id, Name, IsPreset)
            VALUES ($id, $name, 1)
            ON CONFLICT(Name) DO NOTHING;
            """;
        command.Parameters.AddWithValue("$id", Guid.NewGuid().ToString("N"));
        command.Parameters.AddWithValue("$name", name);
        command.ExecuteNonQuery();
    }
}
