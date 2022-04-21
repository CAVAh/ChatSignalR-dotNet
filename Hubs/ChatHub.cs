using Microsoft.AspNetCore.SignalR;

namespace ChatSignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string group, string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", group, user, message);
        }
    }
}