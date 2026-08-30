using MediatR;

namespace ReenbitBooking.Application.Features.Rooms.Commands.DeleteRoom;

public record DeleteRoomCommand(Guid Id) : IRequest<Unit>;