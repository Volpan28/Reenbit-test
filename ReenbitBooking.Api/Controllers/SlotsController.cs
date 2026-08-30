using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReenbitBooking.Application.Features.Slots.Commands.CreateSlot;
using ReenbitBooking.Application.Features.Slots.Commands.DeleteSlot;
using ReenbitBooking.Application.Features.Slots.Queries.GetSlotsForRoom;

namespace ReenbitBooking.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SlotsController(IMediator _mediator) : ControllerBase
{
    [HttpGet("room/{roomId:guid}")]
    public async Task<IActionResult> GetSlotsForRoom(Guid roomId)
    {
        var result = await _mediator.Send(new GetSlotsForRoomQuery(roomId));
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateSlot([FromBody] CreateSlotCommand command)
    {
        var slotId = await _mediator.Send(command);
        return Ok(new { Id = slotId });
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSlot(Guid id)
    {
        await _mediator.Send(new DeleteSlotCommand(id));
        return NoContent();
    }
}