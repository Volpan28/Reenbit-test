using ReenbitBooking.Domain.Entities;

namespace ReenbitBooking.Application.Common.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
