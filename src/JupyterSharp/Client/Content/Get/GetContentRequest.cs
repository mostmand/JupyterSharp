namespace JupyterSharp.Client.Content.Get;

public record GetContentRequest
{
    public required string Path { get; init; }
    public string? Type { get; init; }
    public string? Format { get; init; }
    public bool Content { get; init; } = true;
    public int? Hash { get; init; }
}
