using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReenbitBooking.Application.Features.Bookings.Commands.CreateBooking;
using ReenbitBooking.Application.Features.Bookings.Queries.GetBookings;

namespace ReenbitBooking.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookingsController(IMediator _mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingCommand command)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var secureCommand = command with { UserId = userId };
        
        var bookingId = await _mediator.Send(secureCommand);
        return Ok(new { Id = bookingId });
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _mediator.Send(new GetMyBookingsQuery(userId));
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllBookings()
    {
        var result = await _mediator.Send(new GetAllBookingsQuery());
        return Ok(result);
    }
}
