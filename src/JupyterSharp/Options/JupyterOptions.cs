using System;

namespace JupyterSharp.Options;

internal sealed class JupyterOptions : IJupyterOptions
{
    public Uri Host { get; init; } = null!;
    public string ApiKey { get; init; } = null!;
}
