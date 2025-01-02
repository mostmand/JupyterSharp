using System;
using System.Net.Http;
using JupyterSharp.Abstraction;
using JupyterSharp.Client;
using JupyterSharp.Options;
using JupyterSharp.Options.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace JupyterSharp.DependencyInjection;

public static class DependencyInjection
{
    private static readonly IJupyterOptionsValidator _jupyterOptionsValidator = new JupyterOptionsValidator();
    
    public static IServiceCollection AddKeyedJupyterClient(this IServiceCollection services, string key, Action<IJupyterOptionsBuilder> options)
    {
        var optionsBuilder = JupyterOptionsBuilder.Empty(_jupyterOptionsValidator);
        options?.Invoke(optionsBuilder);
        var jupyterOptions = optionsBuilder.Build();
        
        services.AddHttpClient(key, client =>client.BaseAddress = new Uri($"http://{jupyterOptions.Host}:{jupyterOptions.Port}"));
        services.AddKeyedTransient<IJupyterClient, JupyterClient>(key, (sp, _) =>
        {
            return new JupyterClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient(key));
        });
        
        return services;
    }
}
