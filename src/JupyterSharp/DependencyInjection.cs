using System;
using JupyterSharp.Abstraction;
using JupyterSharp.Client;
using Microsoft.Extensions.DependencyInjection;

namespace JupyterSharp;

public static class DependencyInjection
{
    public static IServiceCollection AddJupyterClient(this IServiceCollection services, Action<IJupyterOptionsBuilder> options)
    {
        services.AddSingleton<IJupyterClient, JupyterClient>();
        
        return services;
    }
}
