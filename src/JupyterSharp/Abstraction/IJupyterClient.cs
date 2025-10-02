using System.Threading;
using System.Threading.Tasks;
using JupyterSharp.Client.Content.Get;
using JupyterSharp.Client.Content.Patch;
using JupyterSharp.Client.Content.Post;
using JupyterSharp.Client.Content.Put;
using JupyterSharp.Dto;

namespace JupyterSharp.Abstraction;

public interface IJupyterClient
{
    Task<VersionInfo> GetVersionAsync(CancellationToken cancellationToken = default);
    Task<GetContentResponse> GetContentAsync(GetContentRequest getContentRequest, CancellationToken cancellationToken = default);
    Task<PostContentResponse> PostContentAsync(PostContentRequest postContentRequest, CancellationToken cancellationToken);
    Task<PutContentResponse> PutContentAsync(PutContentRequest putContentRequest, CancellationToken cancellationToken);
    Task<PatchContentResponse> PatchContentAsync(PatchContentRequest patchContentRequest, CancellationToken cancellationToken);
}
