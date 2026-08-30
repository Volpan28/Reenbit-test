using MediatR;
using Microsoft.EntityFrameworkCore;
using ReenbitBooking.Application.Common.Exceptions;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Domain.Entities;
using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandHandler(IApplicationDbContext _context) : IRequestHandler<CreateBookingCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var slot = await _context.Slots.FirstOrDefaultAsync(s => s.Id == request.SlotId, cancellationToken)
                   ?? throw new NotFoundException(nameof(Slot), request.SlotId);
        
        slot.Status = SlotStatus.Booked;
        
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            SlotId = request.SlotId,
            UserId = request.UserId,
            Status = BookingStatus.Confirmed,
            BookedAtUtc = DateTimeOffset.UtcNow
        };
        
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(cancellationToken);
        return booking.Id;
    }
}