using MediatR;

namespace ReenbitBooking.Application.Features.Auth.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<string>;
