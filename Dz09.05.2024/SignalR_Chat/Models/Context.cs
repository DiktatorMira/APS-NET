using Microsoft.EntityFrameworkCore;

namespace SignalR_Chat.Models {
    public class Context : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public Context(DbContextOptions<Context> options) : base(options) => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies();
    }
}