using MediatR;
using Microsoft.EntityFrameworkCore;
using ReenbitBooking.Application.Common.Exceptions;
using ReenbitBooking.Application.Common.Interfaces;

namespace ReenbitBooking.Application.Features.Auth.Queries.Login;

public class LoginQueryHandler(
    IApplicationDbContext _context,
    IPasswordHasher _passwordHasher,
    IJwtProvider _jwtProvider) : IRequestHandler<LoginQuery, string>
{
    public async Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        return _jwtProvider.GenerateToken(user);
    }
}
