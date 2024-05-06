using Microsoft.EntityFrameworkCore;

namespace Dz06._05._2024.Models {
    public class Context : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public Context(DbContextOptions<Context> options) : base(options) {
            if (Database.EnsureCreated()) SaveChanges();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies();
    }
}