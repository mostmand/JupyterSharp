using JupyterSharp.Client;
using Microsoft.Extensions.DependencyInjection;

namespace JupyterSharp;

public static class DependencyInjection
{
    public static IServiceCollection AddJupyterClient(this IServiceCollection services)
    {
        services.AddSingleton<IJupyterClient, JupyterClient>();
        
        return services;
    }
}
