using MediatR;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Domain.Entities;

namespace ReenbitBooking.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler(IApplicationDbContext _context) : IRequestHandler<CreateRoomCommand, Guid>
{
    public async Task<Guid> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
    {
        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Location = request.Location,
            Capacity = request.Capacity,
            IsActive = true
        };

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync(cancellationToken);
        
        return room.Id;
    }
}