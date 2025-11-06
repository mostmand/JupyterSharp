using System.Text.Json.Serialization;

namespace JupyterSharp.Client.Content.Post;

public record PostContentRequest
{
    public required string Path { get; init; }
    
    [JsonPropertyName("copy_from")]
    public string? CopyFrom { get; init; }
    
    [JsonPropertyName("ext")]
    public string? Ext { get; init; }
    
    [JsonPropertyName("type")]
    public string? Type { get; init; }
    
    [JsonPropertyName("content")]
    public object? Content { get; init; }
    
    [JsonPropertyName("format")]
    public string? Format { get; init; }
}
