namespace JupyterSharp.Client.Content.Get;

public record GetContentHttpResponse
{
    public required string LastModified { get; init; }
    public required string Content { get; init; }
    public required string Created { get; init; }
    public required string Format { get; init; }
    public string? Hash { get; init; }
    public string? HashAlgorithm { get; init; }
}
