using System;
using JupyterSharp.Abstraction;
using JupyterSharp.Integration.Test.JupyterTestContainer;
using Xunit;

namespace JupyterSharp.Integration.Test;

[Collection("Jupyter")]
public abstract class JupyterIntegrationTest
{
    public IJupyterClient JupyterClient { get; }

    protected JupyterIntegrationTest(JupyterCollectionFixture jupyterCollectionFixture)
    {
        var connectionInfo = jupyterCollectionFixture.ConnectionInfo ??
                             throw new ArgumentNullException(nameof(jupyterCollectionFixture));
        JupyterClient = JupyterClientFactory.Create(builder => builder
            .WithHost(connectionInfo.Host)
            .WithPort(connectionInfo.Port)
            .WithToken(connectionInfo.Token)
        );
    }
}
