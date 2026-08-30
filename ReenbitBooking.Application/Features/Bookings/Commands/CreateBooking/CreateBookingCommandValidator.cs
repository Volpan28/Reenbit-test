using FluentValidation;

namespace ReenbitBooking.Application.Features.Bookings.Commands.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");
        
        RuleFor(x => x.SlotId)
            .NotEmpty().WithMessage("Enter a slot id.");
    }
}