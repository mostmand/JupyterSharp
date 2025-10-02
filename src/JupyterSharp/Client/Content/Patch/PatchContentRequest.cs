using System.Text.Json.Serialization;

namespace JupyterSharp.Client.Content.Patch;

public record PatchContentRequest
{
    public required string CurrentPath { get; init; }
    
    [JsonPropertyName("path")]
    public required string NewPath { get; init; }
}
