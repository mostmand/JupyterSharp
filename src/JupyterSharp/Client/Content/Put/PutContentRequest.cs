namespace JupyterSharp.Client.Content.Put;

public record PutContentRequest
{
    public required string Path { get; init; }
    public string Content { get; init; }
    public string Format { get; init; }
    public string Name { get; init; }
    public string? Type { get; init; }
}
