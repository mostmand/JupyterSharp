namespace JupyterSharp.Client.Content.Get;

public record GetContentRequest
{
    public required string Path { get; init; }
    public required string Type { get; init; }
    public required string Format { get; init; }
    public bool ReturnContent { get; init; }
    public int Hash { get; init; }
}
