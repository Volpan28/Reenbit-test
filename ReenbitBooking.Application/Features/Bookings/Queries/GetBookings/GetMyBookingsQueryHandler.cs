using MediatR;
using ReenbitBooking.Application.Common.Interfaces;

namespace ReenbitBooking.Application.Features.Bookings.Queries.GetBookings;

public class GetMyBookingsQueryHandler(IBookingQueries _bookingQueries) : IRequestHandler<GetMyBookingsQuery, IEnumerable<BookingDto>>
{
    public async Task<IEnumerable<BookingDto>> Handle(GetMyBookingsQuery request, CancellationToken cancellationToken)
    {
        return await _bookingQueries.GetBookingsByUserAsync(request.UserId, cancellationToken);
    }
}