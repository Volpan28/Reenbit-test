using MediatR;
using Microsoft.EntityFrameworkCore;
using ReenbitBooking.Application.Common.Exceptions;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Domain.Entities;
using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandHandler(
    IApplicationDbContext _context,
    IScheduleNotifier _scheduleNotifier) : IRequestHandler<CreateBookingCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var slot = await _context.Slots.FindAsync(new object[] { request.SlotId }, cancellationToken)
                   ?? throw new NotFoundException($"Slot with ID {request.SlotId} not found.");

        if (slot.Status != SlotStatus.Available)
            throw new ConflictException("This slot is already booked.");

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            SlotId = request.SlotId,
            UserId = request.UserId,
            Status = BookingStatus.Confirmed
        };

        slot.Status = SlotStatus.Booked;
        _context.Bookings.Add(booking);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("The slot was booked by another user at that very moment.");
        }

        await _scheduleNotifier.NotifySlotStatusChangedAsync(slot.RoomId, slot.Id, "Booked", cancellationToken);

        return booking.Id;
    }
}