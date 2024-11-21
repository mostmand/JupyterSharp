using System;
using System.Threading.Tasks;
using FluentAssertions;
using JupyterSharp.Integration.Test.JupyterTestContainer;
using Xunit;

namespace JupyterSharp.Integration.Test;

[Collection("Jupyter")]
public class GetJupyterServerVersionTest
{
    private readonly JupyterConnectionInfo _connectionInfo;

    public GetJupyterServerVersionTest(JupyterCollectionFixture jupyterCollectionFixture)
    {
        _connectionInfo = jupyterCollectionFixture.ConnectionInfo ?? throw new ArgumentNullException(nameof(jupyterCollectionFixture));
    }
    
    [Fact]
    public async Task GetJupyterServerVersion_ShouldReturnJupyterServerVersion()
    {
        var jupyterClient = JupyterClientFactory.Create(builder => builder.WithHost(new Uri($"http://{_connectionInfo.Host}:{_connectionInfo.Port}/")));

        var versionInfo = await jupyterClient.GetVersionAsync();

        versionInfo.Version.Should().NotBeEmpty();
    }
}
