using MediatR;

namespace ReenbitBooking.Application.Features.Auth.Commands.Registration;

public record RegisterUserCommand(string FullName, string Email, string Password) : IRequest<Guid>;
