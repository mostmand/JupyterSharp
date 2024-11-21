using System;

namespace JupyterSharp.Options;

internal sealed class JupyterOptions : IJupyterOptions
{
    public string Host { get; init; } = null!;
    public int Port { get; init; }
    public string Token { get; init; } = null!;
}
