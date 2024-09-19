namespace JupyterSharp.Options.Validation.Errors;

internal sealed class NullHostError : IJupyterConfigurationError
{
    public static readonly IJupyterConfigurationError Instance = new NullHostError();

    private NullHostError()
    {
    }

    public string Message => "Host is required";
}
