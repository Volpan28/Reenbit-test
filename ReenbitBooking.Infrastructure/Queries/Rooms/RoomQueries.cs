using Dapper;
using ReenbitBooking.Application.Common.Exceptions;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Application.Features.Rooms.Queries.GetRooms;

namespace ReenbitBooking.Infrastructure.Queries.Rooms;

public class RoomQueries(ISqlConnectionFactory _sqlConnectionFactory) : IRoomQueries
{
    public async Task<IEnumerable<RoomDto>> GetRoomsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        var sql = """
                  SELECT Id, Name, Location, Capacity 
                  FROM Rooms
                  WHERE IsActive = 1
                  ORDER BY Name
                  OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
                  """;

        var parameters = new
        {
            Offset = (pageNumber - 1) * pageSize,
            PageSize = pageSize
        };

        return await connection.QueryAsync<RoomDto>(sql, parameters);
    }

    public async Task<RoomDto> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        var sql = """
                  SELECT Id, Name, Location, Capacity 
                  FROM Rooms
                  WHERE Id = @Id AND IsActive = 1
                  """;

        var room = await connection.QuerySingleOrDefaultAsync<RoomDto>(sql, new { Id = id });
        
        return room ?? throw new NotFoundException($"Room with ID {id} not found.");
    }
}