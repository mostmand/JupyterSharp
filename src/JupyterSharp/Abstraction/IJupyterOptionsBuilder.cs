using System;
using JupyterSharp.Options;

namespace JupyterSharp.Abstraction;

public interface IJupyterOptionsBuilder
{
    IJupyterOptionsBuilder WithHost(string host);
    IJupyterOptionsBuilder WithPort(int port);
    IJupyterOptionsBuilder WithToken(string token);
    IJupyterOptions Build();
}
