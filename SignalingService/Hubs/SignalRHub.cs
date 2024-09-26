using Azure.Messaging;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub : Hub
{

    private readonly SignalRDbContext _dbContext;

    public ChatHub(SignalRDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    // Notify other users when someone joins the chat
    public async Task JoinRoom(string roomId,string userName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("ReceiveMessage", $" {userName} has joined the room.");
    }

    // Handle sending WebRTC offers
    public async Task SendOffer(string roomId, string offer)
    {
        await Clients.Group(roomId).SendAsync("ReceiveOffer", offer);
    }

    // Handle sending WebRTC answers
    public async Task SendAnswer(string roomId, string answer)
    {
        await Clients.Group(roomId).SendAsync("ReceiveAnswer", answer);
    }

    // Handle sending ICE candidates
    public async Task SendIceCandidate(string roomId, string candidate)
    {
        await Clients.Group(roomId).SendAsync("ReceiveIceCandidate", candidate);
    }

    // Handle sending text messages
    public async Task SendMessage(string roomId, string user, string message)
    {
        var msg = new Message
        {
            RoomId = roomId,
            Sender = user,
            Content = message,
            Timestamp = DateTime.Now
        };

        // Save the message to the database
        _dbContext.Messages.Add(msg);
        await _dbContext.SaveChangesAsync();

        await Clients.Group(roomId).SendAsync("ReceiveMessage", message);
        
    }

    // Notify other users when someone leaves the chat
    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("ReceiveMessage", $"{Context.ConnectionId} has left the room.");
    }
}
