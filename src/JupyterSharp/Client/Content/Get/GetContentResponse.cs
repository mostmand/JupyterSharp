namespace JupyterSharp.Client.Content.Get;

public record GetContentResponse
{
    public required GetContentStatus GetContentStatus { get; init; }
    public GetContentHttpResponse? Response { get; set; }
}
