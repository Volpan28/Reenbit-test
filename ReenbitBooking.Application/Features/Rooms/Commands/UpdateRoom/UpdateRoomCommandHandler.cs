using MediatR;
using ReenbitBooking.Application.Common.Exceptions;
using ReenbitBooking.Application.Common.Interfaces;

namespace ReenbitBooking.Application.Features.Rooms.Commands.UpdateRoom;

public class UpdateRoomCommandHandler(IApplicationDbContext _context) : IRequestHandler<UpdateRoomCommand, Unit>
{
    public async Task<Unit> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms.FindAsync(new object[] { request.Id }, cancellationToken)
                   ?? throw new NotFoundException($"Room with ID {request.Id} not found.");

        room.Name = request.Name;
        room.Location = request.Location;
        room.Capacity = request.Capacity;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}