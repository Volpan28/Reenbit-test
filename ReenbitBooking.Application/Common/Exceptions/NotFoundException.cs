namespace ReenbitBooking.Application.Common.Exceptions;

/// <summary>
/// Thrown when a requested entity does not exist, so the global exception handler can map it to a 404.
/// </summary>
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string entityName, object key)
        : base($"Entity \"{entityName}\" ({key}) was not found.")
    {
    }
}
