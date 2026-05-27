namespace AppWindows.Models;

public sealed class InvoiceTemplate
{
    public string Id { get; init; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Concept { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public decimal TaxRate { get; set; } = 21m;

    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public string Summary => $"{Name} - {Amount:C}";
}
