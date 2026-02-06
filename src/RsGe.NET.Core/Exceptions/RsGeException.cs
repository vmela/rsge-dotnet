namespace RsGe.NET.Core.Exceptions;

/// <summary>
/// Base exception for RS.GE service errors
/// </summary>
public class RsGeException : Exception
{
    public RsGeException() { }
    public RsGeException(string message) : base(message) { }
    public RsGeException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Thrown when RS.GE authentication fails (invalid credentials)
/// </summary>
public class RsGeAuthenticationException : RsGeException
{
    public RsGeAuthenticationException() : base("RS.GE authentication failed. Check your service credentials.") { }
    public RsGeAuthenticationException(string message) : base(message) { }
    public RsGeAuthenticationException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Thrown when RS.GE returns a SOAP fault or error code
/// </summary>
public class RsGeSoapFaultException : RsGeException
{
    /// <summary>
    /// RS.GE error code
    /// </summary>
    public int ErrorCode { get; }

    public RsGeSoapFaultException(int errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public RsGeSoapFaultException(int errorCode, string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
