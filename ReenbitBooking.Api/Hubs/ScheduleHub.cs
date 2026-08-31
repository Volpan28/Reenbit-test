using Microsoft.AspNetCore.SignalR;

namespace ReenbitBooking.Api.Hubs;

public class ScheduleHub : Hub
{
    public async Task JoinRoomGroup(Guid roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    public async Task LeaveRoomGroup(Guid roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
    }
}