namespace JupyterSharp.Client.Content.Post;

public record PostContentResponse
{
    public required PostContentStatus PostContentStatus { get; init; }
    public PostContentHttpResponse? Response { get; set; }
}
