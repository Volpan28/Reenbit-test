namespace ReenbitBooking.Application.Common.Exceptions;

/// <summary>
/// Thrown when credentials are missing or invalid, so the global exception handler can map it to a 401.
/// </summary>
public sealed class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}
