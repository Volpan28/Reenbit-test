using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReenbitBooking.Application.Features.Auth.Commands.Registration;
using ReenbitBooking.Application.Features.Auth.Queries.Login;

namespace ReenbitBooking.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IMediator _mediator) : ControllerBase
{
    public record LoginRequest(string Email, string Password);

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await _mediator.Send(new LoginQuery(request.Email, request.Password));
        return Ok(new { token });
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(new { Id = userId });
    }
}
