namespace JupyterSharp.Options.Validation.Errors;

internal sealed class ApiKeyNotProvidedError : IJupyterConfigurationError
{
    public static readonly IJupyterConfigurationError Instance = new ApiKeyNotProvidedError();
    
    private ApiKeyNotProvidedError()
    {
    }
    
    public string Message => "API key is null or whitespace";
}
