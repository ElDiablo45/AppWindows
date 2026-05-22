namespace AppWindows.Models;

public sealed class Tag
{
    public string Id { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public bool IsPreset { get; init; }
}
