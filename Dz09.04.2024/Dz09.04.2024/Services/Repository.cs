using Dz09._04._2024.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Dz09._04._2024.Services {
    public interface IRepository {
        Task<User> GetUserByLoginAsync(string login);
        Task AddUserAsync(User user);
        Task<bool> IsLoginTakenAsync(string login);
        Task<List<Message>> GetAllMessagesAsync();
        Task AddMessageAsync(Message message);
    }
    public class Repository : IRepository {
        private readonly Context db;
        public Repository(Context context) => db = context;
        public async Task<User> GetUserByLoginAsync(string login) {
            return await db.Users.FirstOrDefaultAsync(u => u.Login == login);
        }
        public async Task AddUserAsync(User user) {
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }
        public async Task<bool> IsLoginTakenAsync(string login) {
            return await db.Users.AnyAsync(u => u.Login == login);
        }
        public async Task<List<Message>> GetAllMessagesAsync() {
            return await db.Messages.Include(m => m.User).ToListAsync();
        }
        public async Task AddMessageAsync(Message message) {
            await db.Messages.AddAsync(message);
            await db.SaveChangesAsync();
        }
    }
}
