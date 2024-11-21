using System;
using JupyterSharp.Options;

namespace JupyterSharp.Abstraction;

public interface IJupyterOptionsBuilder
{
    IJupyterOptionsBuilder WithHost(Uri host);
    IJupyterOptionsBuilder WithToken(string token);
    IJupyterOptions Build();
}
