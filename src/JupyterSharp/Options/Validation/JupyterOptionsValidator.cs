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
        ValidateApiKey(jupyterOptions, errors);
        return errors;
    }

    private static void ValidateApiKey(IJupyterOptions jupyterOptions, List<IJupyterConfigurationError> errors)
    {
        if (string.IsNullOrWhiteSpace(jupyterOptions.ApiKey))
        {
            errors.Add(ApiKeyNotProvidedError.Instance);
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
