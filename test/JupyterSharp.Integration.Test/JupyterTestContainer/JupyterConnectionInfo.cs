namespace JupyterSharp.Integration.Test.JupyterTestContainer;

public sealed class JupyterConnectionInfo
{
    public required string Host { get; init; }
    public required  int Port { get; init; }
    public required string Token { get; init; }
}
