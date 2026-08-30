using Dapper;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Application.Features.Bookings.Queries.GetBookings;

namespace ReenbitBooking.Infrastructure.Queries.Booking;

public class BookingQueries(ISqlConnectionFactory _sqlConnectionFactory) : IBookingQueries
{
    public async Task<IEnumerable<BookingDto>> GetBookingsByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        var sql = GetBaseSql() + " WHERE b.UserId = @UserId ORDER BY s.StartTimeUtc DESC";
        return await connection.QueryAsync<BookingDto>(sql, new { UserId = userId });
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync(CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();
        var sql = GetBaseSql() + " ORDER BY s.StartTimeUtc DESC";
        return await connection.QueryAsync<BookingDto>(sql);
    }

    private string GetBaseSql() => """
                                   SELECT 
                                       b.Id, 
                                       r.Name AS RoomName, 
                                       r.Location AS RoomLocation, 
                                       s.StartTimeUtc, 
                                       s.EndTimeUtc, 
                                       CASE b.Status WHEN 0 THEN 'Confirmed' WHEN 1 THEN 'Cancelled' END AS Status,
                                       u.Email AS UserEmail
                                   FROM Bookings b
                                   INNER JOIN Slots s ON b.SlotId = s.Id
                                   INNER JOIN Rooms r ON s.RoomId = r.Id
                                   INNER JOIN Users u ON b.UserId = u.Id
                                   """;
}