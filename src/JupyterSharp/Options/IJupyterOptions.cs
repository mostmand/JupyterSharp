using System;

namespace JupyterSharp.Options;

public interface IJupyterOptions
{
    public string Host { get; }
    public int Port { get; }
    public string Token { get; }
}
