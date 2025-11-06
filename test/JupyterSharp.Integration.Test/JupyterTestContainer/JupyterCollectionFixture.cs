using System;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Xunit;

namespace JupyterSharp.Integration.Test.JupyterTestContainer;

[CollectionDefinition("Jupyter")]
public sealed class JupyterCollectionFixture : ICollectionFixture<JupyterCollectionFixture>, IAsyncLifetime
{
    private IContainer? _container;
    
    public JupyterConnectionInfo? ConnectionInfo { get; private set; }
    
    public async ValueTask InitializeAsync()
    {
        const int containerPort = 8888;
        _container = new ContainerBuilder()
            .WithImage("quay.io/jupyter/scipy-notebook")
            .WithPortBinding(containerPort, true)
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilInternalTcpPortIsAvailable(containerPort)
                .UntilMessageIsLogged("To access the server, open this file in a browser:"))
            .Build();
        
        await _container.StartAsync();

        ConnectionInfo = new JupyterConnectionInfo
        {
            Host = _container.Hostname,
            Port = _container.GetMappedPublicPort(containerPort),
            Token = await GetTokenAsync()
        };
    }

    private async Task<string> GetTokenAsync()
    {
        var (_, stderr) = await _container!.GetLogsAsync();
        var token = stderr.Split(["\n", "\r\n"], StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault(line => line.Contains("token="))?
            .Split('=')
            .LastOrDefault();

        if (token is null)
        {
            throw new NotSupportedException();
        }

        return token;
    }

    public async ValueTask DisposeAsync()
    {
        if (_container == null)
        {
            return;
        }

        await _container.DisposeAsync();
    }
}
