namespace JupyterSharp.Options.Validation.Errors;

internal sealed class TokenNotProvidedError : IJupyterConfigurationError
{
    public static readonly IJupyterConfigurationError Instance = new TokenNotProvidedError();
    
    private TokenNotProvidedError()
    {
    }
    
    public string Message => "Token is null or whitespace";
}
