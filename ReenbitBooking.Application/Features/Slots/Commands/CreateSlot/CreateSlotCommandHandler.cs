using MediatR;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Domain.Entities;
using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Application.Features.Slots.Commands.CreateSlot;

public class CreateSlotCommandHandler(IApplicationDbContext _context) : IRequestHandler<CreateSlotCommand, Guid>
{
    public async Task<Guid> Handle(CreateSlotCommand request, CancellationToken cancellationToken)
    {
        var slot = new Slot
        {
            Id = Guid.NewGuid(),
            RoomId = request.RoomId,
            StartTimeUtc = request.StartTimeUtc,
            EndTimeUtc = request.EndTimeUtc,
            Status = SlotStatus.Available
        };

        _context.Slots.Add(slot);
        await _context.SaveChangesAsync(cancellationToken);
        return slot.Id;
    }
}