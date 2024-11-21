using System.Threading;
using System.Threading.Tasks;
using JupyterSharp.Dto;

namespace JupyterSharp.Abstraction;

public interface IJupyterClient
{
    Task<VersionInfo> GetVersionAsync(CancellationToken cancellationToken = default);
}
