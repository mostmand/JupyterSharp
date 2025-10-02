using System.Text.Json.Serialization;

namespace JupyterSharp.Client.Content.Put;

public record PutContentHttpResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; init; }
    
    [JsonPropertyName("path")]
    public required string Path { get; init; }
    
    [JsonPropertyName("type")]
    public required string Type { get; init; }
    
    [JsonPropertyName("created")]
    public required string Created { get; init; }
    
    [JsonPropertyName("last_modified")]
    public required string LastModified { get; init; }
    
    [JsonPropertyName("content")]
    public object? Content { get; init; }
    
    [JsonPropertyName("format")]
    public string? Format { get; init; }
    
    [JsonPropertyName("mimetype")]
    public string? MimeType { get; init; }
    
    [JsonPropertyName("size")]
    public int? Size { get; init; }
    
    [JsonPropertyName("writable")]
    public required bool Writable { get; init; }
    
    [JsonPropertyName("hash")]
    public string? Hash { get; init; }
    
    [JsonPropertyName("hash_algorithm")]
    public string? HashAlgorithm { get; init; }
}
