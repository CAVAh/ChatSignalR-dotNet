using Microsoft.AspNetCore.SignalR;
using ChatSignalR.Controllers;
using ChatSignalR.Models;
using ChatSignalR.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

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
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var contextOptions = new DbContextOptionsBuilder<ChatContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;
            using var context = new ChatContext(contextOptions);

            var GroupUserController = new GroupUserController(context);
            var groupUserId = await GroupUserController.GetGroupUserByIds(Int32.Parse(user), Int32.Parse(group));

            var result = groupUserId.Result as OkObjectResult;

            Console.Write(result?.Value?.ToString());

            if (result?.Value?.ToString() != "0")
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group);
                await Clients.Group(group).SendAsync("ReceiveMessage", group, user, message);
                var MessageController = new MessageController(context);
                await MessageController.CreateMessage(new Message { GroupId = Int32.Parse(group), UserId = Int32.Parse(user), Text = message });
            }
            else
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", "0", "0", "Sem permissão");
            }
        }
    }
}