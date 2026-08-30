using FluentValidation;

namespace ReenbitBooking.Application.Features.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Room ID is required.");
    }
}