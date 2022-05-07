using Microsoft.AspNetCore.SignalR;
using ChatSignalR.Controllers;
using ChatSignalR.Models;
using ChatSignalR.Data;

namespace ChatSignalR.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string group, string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", group, user, message);
            var _context = new ChatContext();
            var MessageController = new MessageController(_context);
            await MessageController.CreateMessage(new Message());
        }
    }
}