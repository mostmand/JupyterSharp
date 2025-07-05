using System.Threading.Tasks;
using AwesomeAssertions;
using JupyterSharp.Integration.Test.JupyterTestContainer;
using Xunit;

namespace JupyterSharp.Integration.Test;

public class GetJupyterServerVersionTest : JupyterIntegrationTest
{
    public GetJupyterServerVersionTest(JupyterCollectionFixture jupyterCollectionFixture) : base(jupyterCollectionFixture)
    {
    }
    
    [Fact]
    public async Task GetJupyterServerVersion_ShouldReturnJupyterServerVersion()
    {
        var versionInfo = await JupyterClient.GetVersionAsync();

        versionInfo.Version.Should().NotBeEmpty();
    }
}
