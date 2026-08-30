using ReenbitBooking.Application.Common.Interfaces;

namespace ReenbitBooking.Infrastructure.Security;

public class PasswordHasher : IPasswordHasher
{
    public bool Verify(string password, string passwordHash)
        => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
