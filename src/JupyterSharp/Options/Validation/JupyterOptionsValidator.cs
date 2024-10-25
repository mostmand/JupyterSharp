using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JupyterSharp.Options.Validation.Errors;

namespace JupyterSharp.Options.Validation;

[SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract")]
internal sealed class JupyterOptionsValidator : IJupyterOptionsValidator
{
    public IReadOnlyCollection<IJupyterConfigurationError> Validate(IJupyterOptions jupyterOptions)
    {
        var errors = new List<IJupyterConfigurationError>();
        ValidateHost(jupyterOptions, errors);
        ValidateToken(jupyterOptions, errors);
        return errors;
    }

    private static void ValidateToken(IJupyterOptions jupyterOptions, List<IJupyterConfigurationError> errors)
    {
        if (string.IsNullOrWhiteSpace(jupyterOptions.Token))
        {
            errors.Add(TokenNotProvidedError.Instance);
        }
    }

    private static void ValidateHost(IJupyterOptions jupyterOptions, List<IJupyterConfigurationError> errors)
    {
        if (jupyterOptions.Host is null)
        {
            errors.Add(NullHostError.Instance);
        }
    }
}
