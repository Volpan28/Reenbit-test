using FluentValidation;

namespace ReenbitBooking.Application.Features.Slots.Commands.DeleteSlot;

public class DeleteSlotCommandValidator : AbstractValidator<DeleteSlotCommand>
{
    public DeleteSlotCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Slot ID is required.");
    }
}