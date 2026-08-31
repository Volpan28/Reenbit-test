using MediatR;

namespace ReenbitBooking.Application.Features.Auth.Commands.Registration;

public record RegisterUserCommand(string Email, string Password) : IRequest<Guid>;
