namespace ReenbitBooking.Application.Features.Bookings.Queries.GetBookings;

public record BookingDto(Guid Id, string RoomName, string RoomLocation, DateTimeOffset StartTimeUtc, DateTimeOffset EndTimeUtc, string Status, string? UserEmail = null);