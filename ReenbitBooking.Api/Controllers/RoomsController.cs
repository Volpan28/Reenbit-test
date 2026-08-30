using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReenbitBooking.Application.Features.Rooms.Commands.CreateRoom;
using ReenbitBooking.Application.Features.Rooms.Commands.DeleteRoom;
using ReenbitBooking.Application.Features.Rooms.Commands.UpdateRoom;
using ReenbitBooking.Application.Features.Rooms.Queries.GetRoomById;
using ReenbitBooking.Application.Features.Rooms.Queries.GetRooms;

namespace ReenbitBooking.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RoomsController(IMediator _mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRooms([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetRoomsQuery(pageNumber, pageSize));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoom(Guid id)
    {
        var result = await _mediator.Send(new GetRoomByIdQuery(id));
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomCommand command)
    {
        var roomId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetRoom), new { id = roomId }, new { Id = roomId });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRoom(Guid id, [FromBody] UpdateRoomCommand command)
    {
        if (id != command.Id) return BadRequest("Route ID does not match Command ID.");
        await _mediator.Send(command);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRoom(Guid id)
    {
        await _mediator.Send(new DeleteRoomCommand(id));
        return NoContent();
    }
}