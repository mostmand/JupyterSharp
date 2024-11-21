using System;
using System.Net.Http;
using JupyterSharp.Abstraction;
using JupyterSharp.Client;
using JupyterSharp.Options;
using JupyterSharp.Options.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace JupyterSharp;

public static class JupyterClientFactory
{
    private static readonly IJupyterOptionsValidator _jupyterOptionsValidator = new JupyterOptionsValidator();
    
    public static IJupyterClient Create(Action<IJupyterOptionsBuilder> options)
    {
        var serviceCollection = new ServiceCollection();
        var name = "RandomName";

        var optionsBuilder = JupyterOptionsBuilder.Empty(_jupyterOptionsValidator);
        options?.Invoke(optionsBuilder);
        var jupyterOptions = optionsBuilder.Build();
        
        serviceCollection.AddHttpClient(name, client =>client.BaseAddress = new Uri($"http://{jupyterOptions.Host}:{jupyterOptions.Port}"));
        serviceCollection.AddKeyedTransient<IJupyterClient, JupyterClient>(name, (sp, _) =>
        {
            return new JupyterClient(sp.GetRequiredService<IHttpClientFactory>().CreateClient(name));
        });
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        return serviceProvider.GetRequiredKeyedService<IJupyterClient>(name);
    }
}
