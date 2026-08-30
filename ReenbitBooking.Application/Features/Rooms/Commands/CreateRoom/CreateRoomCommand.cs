using MediatR;

namespace ReenbitBooking.Application.Features.Rooms.Commands.CreateRoom;

public record CreateRoomCommand(string Name, string Location, int Capacity) : IRequest<Guid>;