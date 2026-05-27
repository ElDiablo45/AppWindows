namespace AppWindows.Models;

public static class InvoiceStatuses
{
    public const string Draft = "draft";
    public const string Issued = "issued";
    public const string Paid = "paid";

    public static readonly IReadOnlyList<InvoiceStatusOption> Options =
    [
        new(Draft, "Borrador"),
        new(Issued, "Emitida"),
        new(Paid, "Pagada")
    ];

    public static string GetDisplayName(string status)
    {
        return Options.FirstOrDefault(option => option.Id == status)?.Name ?? "Borrador";
    }
}

public sealed record InvoiceStatusOption(string Id, string Name);
