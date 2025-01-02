namespace JupyterSharp.Client.Content.Post;

public record PostContentHttpResponse
{
    public required string Location { get; init; }
    public string? Content { get; init; }
    public required string Created { get; init; }
    public required string Format { get; init; }
    public string? Hash { get; init; }
    public string? HashAlgorithm { get; init; }
    public required string LastModified { get; init; }
    public required string MimeType { get; init; }
    public required string Name { get; init; }
    public required string Path { get; init; }
    public int? Size { get; init; }
    public required string Type { get; init; }
    public required bool Writable { get; init; }
}
