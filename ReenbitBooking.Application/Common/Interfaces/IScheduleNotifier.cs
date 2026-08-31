namespace ReenbitBooking.Application.Common.Interfaces;

public interface IScheduleNotifier
{
    Task NotifySlotStatusChangedAsync(Guid roomId, Guid slotId, string status, CancellationToken cancellationToken);
    Task NotifySlotDeletedAsync(Guid roomId, Guid slotId, CancellationToken cancellationToken);
}