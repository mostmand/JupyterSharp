using System;
using JupyterSharp.Abstraction;
using JupyterSharp.DependencyInjection;
using JupyterSharp.DependencyInjection.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace JupyterSharp;

public static class JupyterClientFactory
{
    private static readonly IRandomNameGenerator _randomNameGenerator = new RandomNameGenerator();
    
    public static IJupyterClient Create(Action<IJupyterOptionsBuilder> options)
    {
        var serviceCollection = new ServiceCollection();
        var name = _randomNameGenerator.GenerateRandomName();
        serviceCollection.AddKeyedJupyterClient(name, options);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider.GetRequiredKeyedService<IJupyterClient>(name);
    }
}
