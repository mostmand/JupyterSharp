using System.Collections.Generic;
using JupyterSharp.Options.Validation.Errors;

namespace JupyterSharp.Options.Validation;

internal interface IJupyterOptionsValidator
{
    IReadOnlyCollection<IJupyterConfigurationError> Validate(IJupyterOptions jupyterOptions);
}
