using System.Text.Json.Serialization;

namespace JupyterSharp.Client.Content.Put;

public record PutContentRequest
{
    public required string Path { get; init; }
    
    [JsonPropertyName("content")]
    public object? Content { get; init; }
    
    [JsonPropertyName("format")]
    public string? Format { get; init; }
    
    [JsonPropertyName("type")]
    public string? Type { get; init; }
}
