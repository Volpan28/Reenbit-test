using MediatR;
using ReenbitBooking.Application.Common.Exceptions;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Application.Features.Slots.Commands.DeleteSlot;

public class DeleteSlotCommandHandler(IApplicationDbContext _context, 
    IScheduleNotifier _scheduleNotifier) : IRequestHandler<DeleteSlotCommand, Unit>
{
    public async Task<Unit> Handle(DeleteSlotCommand request, CancellationToken cancellationToken)
    {
        var slot = await _context.Slots.FindAsync(new object[] { request.Id }, cancellationToken)
                   ?? throw new NotFoundException($"Slot with ID {request.Id} not found.");

        if (slot.Status == SlotStatus.Booked)
            throw new BadRequestException("Cannot delete a booked slot.");

        _context.Slots.Remove(slot);
        await _context.SaveChangesAsync(cancellationToken);
        
        await _scheduleNotifier.NotifySlotDeletedAsync(slot.RoomId, slot.Id, cancellationToken);
        return Unit.Value;
    }
}