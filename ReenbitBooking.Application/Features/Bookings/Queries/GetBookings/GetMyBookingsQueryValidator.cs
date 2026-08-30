using FluentValidation;

namespace ReenbitBooking.Application.Features.Bookings.Queries.GetBookings;

public class GetMyBookingsQueryValidator : AbstractValidator<GetMyBookingsQuery>
{
    public GetMyBookingsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
    }
}