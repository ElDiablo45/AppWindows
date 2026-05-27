using AppWindows.Models;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace AppWindows.Data;

public sealed class InvoiceRepository
{
    private readonly DatabaseService databaseService;

    public InvoiceRepository(DatabaseService databaseService)
    {
        this.databaseService = databaseService;
    }

    public List<InvoiceTemplate> GetTemplates()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Name, Concept, Amount, TaxRate, Notes, CreatedAt
            FROM InvoiceTemplates
            ORDER BY Name COLLATE NOCASE;
            """;

        var templates = new List<InvoiceTemplate>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            templates.Add(ReadTemplate(reader));
        }

        return templates;
    }

    public List<Invoice> GetInvoices(string? searchText, string? status)
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Invoices.Id,
                   Invoices.Number,
                   Invoices.StudentId,
                   Students.FullName,
                   Invoices.TemplateId,
                   COALESCE(InvoiceTemplates.Name, ''),
                   Invoices.IssueDate,
                   Invoices.DueDate,
                   Invoices.Concept,
                   Invoices.Amount,
                   Invoices.TaxRate,
                   Invoices.Notes,
                   Invoices.Status,
                   Invoices.CreatedAt
            FROM Invoices
            INNER JOIN Students ON Students.Id = Invoices.StudentId
            LEFT JOIN InvoiceTemplates ON InvoiceTemplates.Id = Invoices.TemplateId
            WHERE ($search = ''
                   OR Invoices.Number LIKE $searchLike COLLATE NOCASE
                   OR Students.FullName LIKE $searchLike COLLATE NOCASE
                   OR Invoices.Concept LIKE $searchLike COLLATE NOCASE)
              AND ($status = '' OR Invoices.Status = $status)
            ORDER BY Invoices.IssueDate DESC, Invoices.Number COLLATE NOCASE DESC;
            """;

        var search = searchText?.Trim() ?? string.Empty;
        command.Parameters.AddWithValue("$search", search);
        command.Parameters.AddWithValue("$searchLike", $"%{search}%");
        command.Parameters.AddWithValue("$status", status ?? string.Empty);

        var invoices = new List<Invoice>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            invoices.Add(ReadInvoice(reader));
        }

        return invoices;
    }

    public int GetInvoiceCount()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Invoices;";
        return Convert.ToInt32(command.ExecuteScalar());
    }

    public decimal GetPendingTotal()
    {
        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Amount, TaxRate
            FROM Invoices
            WHERE Status <> 'paid';
            """;

        var total = 0m;
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var amount = ReadDecimal(reader, 0);
            var taxRate = ReadDecimal(reader, 1);
            total += amount + Math.Round(amount * taxRate / 100m, 2);
        }

        return total;
    }

    public string GetNextInvoiceNumber()
    {
        var year = DateTime.Today.Year;

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT COUNT(*)
            FROM Invoices
            WHERE Number LIKE $prefix COLLATE NOCASE;
            """;
        command.Parameters.AddWithValue("$prefix", $"F-{year}-%");

        var count = Convert.ToInt32(command.ExecuteScalar());
        return $"F-{year}-{count + 1:0000}";
    }

    public InvoiceTemplate CreateTemplate(string name, string concept, decimal amount, decimal taxRate, string notes)
    {
        var template = new InvoiceTemplate
        {
            Id = Guid.NewGuid().ToString("N"),
            Name = name.Trim(),
            Concept = concept.Trim(),
            Amount = amount,
            TaxRate = taxRate,
            Notes = notes.Trim(),
            CreatedAt = DateTime.Today
        };

        if (string.IsNullOrWhiteSpace(template.Name))
        {
            throw new ArgumentException("El nombre de la plantilla es obligatorio.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(template.Concept))
        {
            throw new ArgumentException("El concepto de la plantilla es obligatorio.", nameof(concept));
        }

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO InvoiceTemplates (Id, Name, Concept, Amount, TaxRate, Notes, CreatedAt)
            VALUES ($id, $name, $concept, $amount, $taxRate, $notes, $createdAt);
            """;
        command.Parameters.AddWithValue("$id", template.Id);
        command.Parameters.AddWithValue("$name", template.Name);
        command.Parameters.AddWithValue("$concept", template.Concept);
        command.Parameters.AddWithValue("$amount", WriteDecimal(template.Amount));
        command.Parameters.AddWithValue("$taxRate", WriteDecimal(template.TaxRate));
        command.Parameters.AddWithValue("$notes", template.Notes);
        command.Parameters.AddWithValue("$createdAt", template.CreatedAt.ToString("O"));
        command.ExecuteNonQuery();

        return template;
    }

    public Invoice CreateInvoice(
        string number,
        string studentId,
        string? templateId,
        DateTime issueDate,
        DateTime? dueDate,
        string concept,
        decimal amount,
        decimal taxRate,
        string notes,
        string status)
    {
        var invoice = new Invoice
        {
            Id = Guid.NewGuid().ToString("N"),
            Number = number.Trim(),
            StudentId = studentId,
            TemplateId = string.IsNullOrWhiteSpace(templateId) ? null : templateId,
            IssueDate = issueDate,
            DueDate = dueDate,
            Concept = concept.Trim(),
            Amount = amount,
            TaxRate = taxRate,
            Notes = notes.Trim(),
            Status = status,
            CreatedAt = DateTime.Today
        };

        ValidateInvoice(invoice);

        using var connection = OpenConnection();
        try
        {
            InsertInvoice(connection, invoice);
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 19)
        {
            throw new DuplicateInvoiceNumberException(invoice.Number);
        }

        return HydrateInvoice(connection, invoice);
    }

    public void UpdateInvoice(Invoice invoice)
    {
        invoice.Number = invoice.Number.Trim();
        invoice.TemplateId = string.IsNullOrWhiteSpace(invoice.TemplateId) ? null : invoice.TemplateId;
        invoice.Concept = invoice.Concept.Trim();
        invoice.Notes = invoice.Notes.Trim();
        ValidateInvoice(invoice);

        using var connection = OpenConnection();
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            UPDATE Invoices
            SET Number = $number,
                StudentId = $studentId,
                TemplateId = $templateId,
                IssueDate = $issueDate,
                DueDate = $dueDate,
                Concept = $concept,
                Amount = $amount,
                TaxRate = $taxRate,
                Notes = $notes,
                Status = $status
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$id", invoice.Id);
        AddInvoiceParameters(command, invoice);

        try
        {
            command.ExecuteNonQuery();
        }
        catch (SqliteException exception) when (exception.SqliteErrorCode == 19)
        {
            throw new DuplicateInvoiceNumberException(invoice.Number);
        }
    }

    private static void ValidateInvoice(Invoice invoice)
    {
        if (string.IsNullOrWhiteSpace(invoice.Number))
        {
            throw new ArgumentException("El numero de factura es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(invoice.StudentId))
        {
            throw new ArgumentException("El cliente es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(invoice.Concept))
        {
            throw new ArgumentException("El concepto es obligatorio.");
        }

        if (invoice.Amount < 0)
        {
            throw new ArgumentException("El importe no puede ser negativo.");
        }

        if (invoice.TaxRate < 0)
        {
            throw new ArgumentException("El IVA no puede ser negativo.");
        }
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

    private static void InsertInvoice(SqliteConnection connection, Invoice invoice)
    {
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO Invoices (Id, Number, StudentId, TemplateId, IssueDate, DueDate, Concept, Amount, TaxRate, Notes, Status, CreatedAt)
            VALUES ($id, $number, $studentId, $templateId, $issueDate, $dueDate, $concept, $amount, $taxRate, $notes, $status, $createdAt);
            """;
        command.Parameters.AddWithValue("$id", invoice.Id);
        AddInvoiceParameters(command, invoice);
        command.Parameters.AddWithValue("$createdAt", invoice.CreatedAt.ToString("O"));
        command.ExecuteNonQuery();
    }

    private static void AddInvoiceParameters(SqliteCommand command, Invoice invoice)
    {
        command.Parameters.AddWithValue("$number", invoice.Number);
        command.Parameters.AddWithValue("$studentId", invoice.StudentId);
        command.Parameters.AddWithValue("$templateId", (object?)invoice.TemplateId ?? DBNull.Value);
        command.Parameters.AddWithValue("$issueDate", invoice.IssueDate.ToString("O"));
        command.Parameters.AddWithValue("$dueDate", invoice.DueDate?.ToString("O") ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("$concept", invoice.Concept);
        command.Parameters.AddWithValue("$amount", WriteDecimal(invoice.Amount));
        command.Parameters.AddWithValue("$taxRate", WriteDecimal(invoice.TaxRate));
        command.Parameters.AddWithValue("$notes", invoice.Notes);
        command.Parameters.AddWithValue("$status", invoice.Status);
    }

    private static Invoice HydrateInvoice(SqliteConnection connection, Invoice invoice)
    {
        using var command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Students.FullName, COALESCE(InvoiceTemplates.Name, '')
            FROM Invoices
            INNER JOIN Students ON Students.Id = Invoices.StudentId
            LEFT JOIN InvoiceTemplates ON InvoiceTemplates.Id = Invoices.TemplateId
            WHERE Invoices.Id = $id;
            """;
        command.Parameters.AddWithValue("$id", invoice.Id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            invoice.ClientName = reader.GetString(0);
            invoice.TemplateName = reader.GetString(1);
        }

        return invoice;
    }

    private static InvoiceTemplate ReadTemplate(SqliteDataReader reader)
    {
        return new InvoiceTemplate
        {
            Id = reader.GetString(0),
            Name = reader.GetString(1),
            Concept = reader.GetString(2),
            Amount = ReadDecimal(reader, 3),
            TaxRate = ReadDecimal(reader, 4),
            Notes = reader.GetString(5),
            CreatedAt = DateTime.Parse(reader.GetString(6), CultureInfo.InvariantCulture)
        };
    }

    private static Invoice ReadInvoice(SqliteDataReader reader)
    {
        return new Invoice
        {
            Id = reader.GetString(0),
            Number = reader.GetString(1),
            StudentId = reader.GetString(2),
            ClientName = reader.GetString(3),
            TemplateId = reader.IsDBNull(4) ? null : reader.GetString(4),
            TemplateName = reader.GetString(5),
            IssueDate = DateTime.Parse(reader.GetString(6), CultureInfo.InvariantCulture),
            DueDate = reader.IsDBNull(7) ? null : DateTime.Parse(reader.GetString(7), CultureInfo.InvariantCulture),
            Concept = reader.GetString(8),
            Amount = ReadDecimal(reader, 9),
            TaxRate = ReadDecimal(reader, 10),
            Notes = reader.GetString(11),
            Status = reader.GetString(12),
            CreatedAt = DateTime.Parse(reader.GetString(13), CultureInfo.InvariantCulture)
        };
    }

    private static decimal ReadDecimal(SqliteDataReader reader, int ordinal)
    {
        return decimal.Parse(reader.GetString(ordinal), CultureInfo.InvariantCulture);
    }

    private static string WriteDecimal(decimal value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}
