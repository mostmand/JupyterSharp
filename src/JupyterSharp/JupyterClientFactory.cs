using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace JupyterSharp;

public sealed class JupyterClientFactory : IDisposable, IAsyncDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly bool _disposeServiceProvider;

    internal JupyterClientFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public JupyterClientFactory(Action<IJupyterOptionsBuilder> options)
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddJupyterClient(options);
        _serviceProvider = serviceCollection.BuildServiceProvider();
        _disposeServiceProvider = true;
    }

    public void Dispose()
    {
        if (!_disposeServiceProvider)
        {
            return;
        }

        if (_serviceProvider is not IDisposable disposable)
        {
            return;
        }

        disposable.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposeServiceProvider)
        {
            return;
        }

        if (_serviceProvider is not IAsyncDisposable asyncDisposable)
        {
            return;
        }

        await asyncDisposable.DisposeAsync().ConfigureAwait(false);
    }
}
