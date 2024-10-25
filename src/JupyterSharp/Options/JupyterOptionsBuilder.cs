using System;
using System.Linq;
using JupyterSharp.Options.Validation;

namespace JupyterSharp.Options;

internal sealed class JupyterOptionsBuilder : IJupyterOptionsBuilder
{
    private readonly IJupyterOptionsValidator _jupyterOptionsValidator;
    private Uri? _host;
    private string? _token;

    private JupyterOptionsBuilder(IJupyterOptionsValidator jupyterOptionsValidator)
    {
        _jupyterOptionsValidator = jupyterOptionsValidator ?? throw new ArgumentNullException(nameof(jupyterOptionsValidator));
    }

    public static IJupyterOptionsBuilder Empty(IJupyterOptionsValidator jupyterOptionsValidator)
    {
        return new JupyterOptionsBuilder(jupyterOptionsValidator);
    }

    public IJupyterOptionsBuilder WithHost(Uri host)
    {
        _host = host;

        return this;
    }

    public IJupyterOptionsBuilder WithToken(string token)
    {
        _token = token;

        return this;
    }

    public IJupyterOptions Build()
    {
        var result = new JupyterOptions { Host = _host!, Token = _token! };
        
        ValidateJupyterOptions(result);

        return result;
    }

    private void ValidateJupyterOptions(JupyterOptions result)
    {
        var errors = _jupyterOptionsValidator.Validate(result);
        if (errors.Count != 0)
        {
            throw new InvalidJupyterOptionsException(string.Join(Environment.NewLine, errors.Select(e => e.Message)));
        }
    }
}
