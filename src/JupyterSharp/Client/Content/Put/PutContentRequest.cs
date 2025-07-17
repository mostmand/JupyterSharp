namespace JupyterSharp.Client.Content.Put;

public record PutContentRequest
{
    public required string Path { get; init; }
    public required string Content { get; init; }
    public required string Format { get; init; }
    public required string Name { get; init; }
    public string? Type { get; init; }
}
