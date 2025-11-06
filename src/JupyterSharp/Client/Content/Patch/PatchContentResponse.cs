namespace JupyterSharp.Client.Content.Patch;

public record PatchContentResponse
{
    public required PatchContentStatus PatchContentStatus { get; init; }
    public PatchContentHttpResponse? Response { get; set; }
}
