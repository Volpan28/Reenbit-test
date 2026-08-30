using MediatR;

namespace ReenbitBooking.Application.Features.Rooms.Commands.UpdateRoom;

public record UpdateRoomCommand(Guid Id, string Name, string Location, int Capacity) : IRequest<Unit>;