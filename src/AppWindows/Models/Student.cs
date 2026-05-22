namespace AppWindows.Models;

public sealed class Student
{
    public string Id { get; init; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string DniNie { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public string Notes { get; set; } = string.Empty;

    public List<Tag> Tags { get; } = [];

    public string TagsDisplay => Tags.Count == 0 ? "Sin tags" : string.Join(", ", Tags.Select(tag => tag.Name));

    public string CreatedAtDisplay => CreatedAt.ToString("dd/MM/yyyy");
}
