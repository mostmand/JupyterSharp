using System;
using JupyterSharp.Options;

namespace JupyterSharp;

public interface IJupyterOptionsBuilder
{
    IJupyterOptionsBuilder WithHost(Uri host);
    IJupyterOptionsBuilder WithToken(string token);
    IJupyterOptions Build();
}
