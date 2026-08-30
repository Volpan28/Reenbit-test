namespace ReenbitBooking.Application.Features.Slots.Queries.GetSlotsForRoom;

public record SlotDto(Guid Id, DateTimeOffset StartTimeUtc, DateTimeOffset EndTimeUtc, string Status);