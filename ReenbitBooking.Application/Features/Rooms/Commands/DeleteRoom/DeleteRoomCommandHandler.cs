using MediatR;
using Microsoft.EntityFrameworkCore;
using ReenbitBooking.Application.Common.Exceptions;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Application.Features.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandHandler(IApplicationDbContext _context) : IRequestHandler<DeleteRoomCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _context.Rooms.FindAsync(new object[] { request.Id }, cancellationToken)
                   ?? throw new NotFoundException($"Room with ID {request.Id} not found.");

        var hasBookedSlots = await _context.Slots
            .AnyAsync(s => s.RoomId == request.Id && s.Status == SlotStatus.Booked, cancellationToken);

        if (hasBookedSlots)
            throw new ConflictException("Cannot delete the room because it has booked slots.");

        room.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}