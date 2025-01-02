using JupyterSharp.Client.Content.Post;

namespace JupyterSharp.Client.Content.Put;

public record PutContentResponse
{
    public required PutContentStatus PutContentStatus { get; init; }
    public PutContentHttpResponse? Response { get; set; }
}
