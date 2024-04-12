using MusicPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicPortal.Services {
    public interface IRepository {
        Task<User> GetUserByLogin(string login);
        Task<User> GetUserById(int userId);
        Task<IEnumerable<User>> GetUsers();
        Task AddUser(User user);
        void DeleteUser(User user);
        Task<bool> IsLoginTaken(string login);
        Task SaveDb();
    }
    public class Repository : IRepository{
        private readonly Context db;
        public Repository(Context context) => db = context;
        public async Task<User> GetUserByLogin(string login) {
            return await db.Users.FirstOrDefaultAsync(u => u.Login == login);
        }
        public async Task<User> GetUserById(int userId) {
            return await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        public async Task<IEnumerable<User>> GetUsers() { return await db.Users.ToListAsync(); }
        public void DeleteUser(User user) => db.Users.Remove(user);
        public async Task AddUser(User user) => await db.Users.AddAsync(user);
        public async Task<bool> IsLoginTaken(string login) {
            return await db.Users.AnyAsync(u => u.Login == login);
        }
        public async Task SaveDb() => await db.SaveChangesAsync();
    }
}
