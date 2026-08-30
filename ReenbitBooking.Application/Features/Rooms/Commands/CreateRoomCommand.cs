using MediatR;

namespace ReenbitBooking.Application.Features.Rooms.Commands;

public record CreateRoomCommand(string Name, string Location, int Capacity) : IRequest<Guid>;