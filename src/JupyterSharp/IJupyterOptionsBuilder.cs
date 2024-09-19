using System;
using JupyterSharp.Options;

namespace JupyterSharp;

public interface IJupyterOptionsBuilder
{
    IJupyterOptionsBuilder WithHost(Uri host);
    IJupyterOptionsBuilder WithApiKey(string apiKey);
    IJupyterOptions Build();
}
