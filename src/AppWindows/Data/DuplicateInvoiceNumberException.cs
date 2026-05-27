namespace AppWindows.Data;

public sealed class DuplicateInvoiceNumberException : Exception
{
    public DuplicateInvoiceNumberException(string number)
        : base($"Ya existe una factura con el numero {number}.")
    {
    }
}
