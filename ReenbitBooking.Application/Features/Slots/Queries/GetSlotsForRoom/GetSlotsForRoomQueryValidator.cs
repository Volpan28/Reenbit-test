using FluentValidation;

namespace ReenbitBooking.Application.Features.Slots.Queries.GetSlotsForRoom;

public class GetSlotsForRoomQueryValidator : AbstractValidator<GetSlotsForRoomQuery>
{
    public GetSlotsForRoomQueryValidator()
    {
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("Room ID is required.");
    }
}