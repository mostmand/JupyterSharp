using System;

namespace JupyterSharp.Options;

public interface IJupyterOptions
{
    public Uri Host { get; }
    public string ApiKey { get; }
}
