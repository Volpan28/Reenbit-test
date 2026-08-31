using MediatR;
using ReenbitBooking.Application.Common.Exceptions;
using ReenbitBooking.Application.Common.Interfaces;

namespace ReenbitBooking.Application.Features.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandHandler(IApplicationDbContext _context) : IRequestHandler<DeleteRoomCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms.FindAsync(new object[] { request.Id }, cancellationToken)
                   ?? throw new NotFoundException($"Room with ID {request.Id} not found.");

        room.IsActive = false; 

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}