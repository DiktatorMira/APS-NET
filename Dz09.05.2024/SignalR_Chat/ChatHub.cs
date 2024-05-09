using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR_Chat.Models;

namespace SignalR_Chat {
    public class ChatHub : Hub {
        private readonly Context db;
        public ChatHub(Context context) => db = context;
        public async Task Send(string username, string message)  {
            await Clients.All.SendAsync("AddMessage", username, message);
            var user = await db.Users.FirstOrDefaultAsync(u => u.Name == username);
            if (user != null) {
                var newMessage = new Message {
                    Content = message,
                    UserId = user.Id,
                    User = user
                };
                db.Messages.Add(newMessage);
                await db.SaveChangesAsync();
            }
        }
        public async Task Connect(string userName)
        {
            var currentUser = await db.Users
                .Include(u => u.Messages)
                .ThenInclude(m => m.User)
                .FirstOrDefaultAsync(u => u.Name == userName);

            if (currentUser == null)
            {
                var newUser = new User
                {
                    ConnectionId = Context.ConnectionId,
                    Name = userName
                };
                db.Users.Add(newUser);
            }
            else
            {
                currentUser.ConnectionId = Context.ConnectionId;
            }
            await db.SaveChangesAsync();

            // Отправляем сообщения текущего пользователя в чат
            if (currentUser != null && currentUser.Messages != null)
            {
                foreach (var message in currentUser.Messages)
                {
                    await Clients.Caller.SendAsync("AddMessage", message.User?.Name, message.Content);
                }
            }

            // Отправляем информацию о подключении нового пользователя и сообщения всех пользователей
            var allUsers = await db.Users.Include(u => u.Messages).ToListAsync();
            var messagesData = currentUser?.Messages?.Select(message => new { UserName = message.User?.Name, message.Content }).ToList();
            await Clients.Caller.SendAsync("AddMessages", messagesData);

            // Отправляем информацию о подключении нового пользователя и сообщения всех пользователей
            var allUsersData = allUsers.Select(user => new
            {
                ConnectionId = user.ConnectionId,
                Name = user.Name,
                Messages = user.Messages?
                    .Select(message => new { UserName = message.User?.Name, message.Content })
                    .ToList()
            }).ToList();

            await Clients.Caller.SendAsync("Connected", Context.ConnectionId, userName, allUsersData);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("NewUserConnected", Context.ConnectionId, userName);
        }
        public override async Task OnDisconnectedAsync(Exception? exception) {
            var user = db.Users.FirstOrDefault(u => u.ConnectionId == Context.ConnectionId);
            if (user != null) await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId, user.Name);
            await base.OnDisconnectedAsync(exception);
        }
    }
}