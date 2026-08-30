using ReenbitBooking.Application.Features.Bookings.Queries.GetBookings;

namespace ReenbitBooking.Application.Common.Interfaces;

public interface IBookingQueries
{
    Task<IEnumerable<BookingDto>> GetBookingsByUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<BookingDto>> GetAllBookingsAsync(CancellationToken cancellationToken);
}