using MediatR;
using Microsoft.EntityFrameworkCore;
using ReenbitBooking.Application.Common.Exceptions;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Domain.Entities;
using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Application.Features.Auth.Commands.Registration;

public class RegisterUserCommandHandler(
    IApplicationDbContext _context, 
    IPasswordHasher _passwordHasher) : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            throw new BadRequestException("User with this email already exists.");

        var isFirstUser = !await _context.Users.AnyAsync(cancellationToken);
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = _passwordHasher.Hash(request.Password),
            Role = isFirstUser ? UserRole.Admin : UserRole.RegularUser
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}