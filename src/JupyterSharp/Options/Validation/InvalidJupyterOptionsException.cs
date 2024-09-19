using System;

namespace JupyterSharp.Options.Validation;

[Serializable]
public sealed class InvalidJupyterOptionsException : Exception
{
    public InvalidJupyterOptionsException()
    {
    }

    public InvalidJupyterOptionsException(string? message) : base(message)
    {
    }

    public InvalidJupyterOptionsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
