using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using JupyterSharp.Abstraction;
using JupyterSharp.Dto;

namespace JupyterSharp.Client;

internal sealed class JupyterClient : IJupyterClient
{
    private readonly HttpClient _httpClient;

    public JupyterClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    
    public Task<VersionInfo> GetVersionAsync(CancellationToken cancellationToken = default)
    {
        return _httpClient.GetFromJsonAsync<VersionInfo>("/api", cancellationToken)!;
    }
}
