using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReenbitBooking.Application.Features.Bookings.Commands.CreateBooking;

namespace ReenbitBooking.Api.Controllers;


[ApiController]
[Route("api/bookings")]
public class BookingController(IMediator _mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateBookingCommand request)
    {
        var command = new CreateBookingCommand(
            request.SlotId,
            request.UserId
        );
        
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}