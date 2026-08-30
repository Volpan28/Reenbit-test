using Dapper;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Application.Features.Slots.Queries.GetSlotsForRoom;

namespace ReenbitBooking.Infrastructure.Queries.Slots;

public class SlotQueries(ISqlConnectionFactory _sqlConnectionFactory) : ISlotQueries
{
    public async Task<IEnumerable<SlotDto>> GetSlotsForRoomAsync(Guid roomId, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        
        var sql = """
                  SELECT 
                      Id, 
                      StartTimeUtc, 
                      EndTimeUtc, 
                      CASE Status 
                          WHEN 0 THEN 'Available' 
                          WHEN 1 THEN 'Booked' 
                      END AS Status 
                  FROM Slots
                  WHERE RoomId = @RoomId
                  ORDER BY StartTimeUtc
                  """;

        return await connection.QueryAsync<SlotDto>(sql, new { RoomId = roomId });
    }
}