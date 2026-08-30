using FluentValidation;

namespace ReenbitBooking.Application.Features.Rooms.Commands.UpdateRoom;

public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
{
    public UpdateRoomCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Room ID is required.");
        
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100).WithMessage("Valid room name is required.");
        
        RuleFor(x => x.Location).NotEmpty().MaximumLength(100).WithMessage("Valid location is required.");
        
        RuleFor(x => x.Capacity).GreaterThan(0).WithMessage("Capacity must be at least 1.");
    }
}