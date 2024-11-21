namespace JupyterSharp.Options.Validation.Errors;

internal sealed class PortIsNotProvidedError : IJupyterConfigurationError
{
    public static readonly IJupyterConfigurationError Instance = new PortIsNotProvidedError();
    
    private PortIsNotProvidedError()
    {
    }
    
    public string Message => "Port is not provided.";
}
