namespace JupyterSharp.Client.Content.Post;

public record PostContentRequest
{
    public required string Path { get; init; }
    public string? CopyFrom { get; init; }
    public string? Ext { get; init; }
    public string? Type { get; init; }
}
