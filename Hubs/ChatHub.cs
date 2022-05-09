using Microsoft.AspNetCore.SignalR;
using ChatSignalR.Controllers;
using ChatSignalR.Models;
using ChatSignalR.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatSignalR.Hubs
{
    public class ChatHub : Hub
    {
        public IConfiguration Configuration { get; }
        public ChatHub(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task SendMessage(string group, string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", group, user, message);

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            var contextOptions = new DbContextOptionsBuilder<ChatContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;

            

            using var context = new ChatContext(contextOptions);

            var MessageController = new MessageController(context);
            await MessageController.CreateMessage(new Message { GroupId = Int32.Parse(group), UserId = Int32.Parse(user), Text = message });
        }
    }
}