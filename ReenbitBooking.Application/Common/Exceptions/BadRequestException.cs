namespace ReenbitBooking.Application.Common.Exceptions;

/// <summary>
/// Thrown for request-level business rule violations that aren't field-level validation errors,
/// so the global exception handler can map it to a 400.
/// </summary>
public sealed class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }
}
