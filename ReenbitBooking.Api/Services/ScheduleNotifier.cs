using Microsoft.AspNetCore.SignalR;
using ReenbitBooking.Api.Hubs;
using ReenbitBooking.Application.Common.Interfaces;

namespace ReenbitBooking.Api.Services;

public class ScheduleNotifier(IHubContext<ScheduleHub> _hubContext) : IScheduleNotifier
{
    public async Task NotifySlotStatusChangedAsync(Guid roomId, Guid slotId, string status, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.Group(roomId.ToString()).SendAsync("SlotUpdated", new { SlotId = slotId, Status = status }, cancellationToken);
    }

    public async Task NotifySlotDeletedAsync(Guid roomId, Guid slotId, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.Group(roomId.ToString()).SendAsync("SlotDeleted", new { SlotId = slotId }, cancellationToken);
    }

}