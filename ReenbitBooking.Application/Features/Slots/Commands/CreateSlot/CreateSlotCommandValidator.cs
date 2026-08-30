using FluentValidation;

namespace ReenbitBooking.Application.Features.Slots.Commands.CreateSlot;

public class CreateSlotCommandValidator : AbstractValidator<CreateSlotCommand>
{
    public CreateSlotCommandValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Room ID is required.");
        RuleFor(x => x.StartTimeUtc)
            .NotEmpty()
            .GreaterThan(DateTimeOffset.UtcNow).WithMessage("Start time must be in the future.");
        RuleFor(x => x.EndTimeUtc)
            .NotEmpty()
            .GreaterThan(x => x.StartTimeUtc).WithMessage("End time must be after start time.");
    }
}