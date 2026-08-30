using MediatR;
using ReenbitBooking.Application.Common.Interfaces;

namespace ReenbitBooking.Application.Features.Bookings.Queries.GetBookings;

public class GetAllBookingsQueryHandler(IBookingQueries _bookingQueries) : IRequestHandler<GetAllBookingsQuery, IEnumerable<BookingDto>>
{
    public async Task<IEnumerable<BookingDto>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        return await _bookingQueries.GetAllBookingsAsync(cancellationToken);
    }
}