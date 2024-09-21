using Microsoft.AspNetCore.SignalR;

public class SignalRHub : Hub
{
    public async Task SendSignal(string signal)
    {
        await Clients.All.SendAsync("ReceiveSignal", signal);
    }
}
