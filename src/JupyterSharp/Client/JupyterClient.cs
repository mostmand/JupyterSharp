using System.Threading;
using System.Threading.Tasks;
using JupyterSharp.Abstraction;
using JupyterSharp.Dto;

namespace JupyterSharp.Client;

public sealed class JupyterClient : IJupyterClient
{
    public Task<VersionInfo> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }
}
