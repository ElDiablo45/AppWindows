namespace AppWindows.Data;

public sealed class DuplicateDniException : Exception
{
    public DuplicateDniException(string dniNie)
        : base($"Ya existe un alumno con el DNI/NIE {dniNie}.")
    {
    }
}
