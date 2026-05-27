namespace AppWindows.Models;

public sealed class Invoice
{
    public string Id { get; init; } = string.Empty;

    public string Number { get; set; } = string.Empty;

    public string StudentId { get; set; } = string.Empty;

    public string ClientName { get; set; } = string.Empty;

    public string? TemplateId { get; set; }

    public string TemplateName { get; set; } = string.Empty;

    public DateTime IssueDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string Concept { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public decimal TaxRate { get; set; } = 21m;

    public string Notes { get; set; } = string.Empty;

    public string Status { get; set; } = InvoiceStatuses.Draft;

    public DateTime CreatedAt { get; init; }

    public decimal TaxAmount => Math.Round(Amount * TaxRate / 100m, 2);

    public decimal Total => Amount + TaxAmount;

    public string IssueDateDisplay => IssueDate.ToString("dd/MM/yyyy");

    public string DueDateDisplay => DueDate?.ToString("dd/MM/yyyy") ?? "Sin vencimiento";

    public string AmountDisplay => Amount.ToString("C");

    public string TotalDisplay => Total.ToString("C");

    public string StatusDisplay => InvoiceStatuses.GetDisplayName(Status);
}
