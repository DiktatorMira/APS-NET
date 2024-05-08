using Microsoft.EntityFrameworkCore;

namespace WebApiServer.Models {
    public class Context : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Performer> Performers { get; set; }
        public Context(DbContextOptions<Context> options) : base(options) {
            if (Database.EnsureCreated()) {
                Users!.AddRange(new List<User> {
                    new User {
                        Login = "Admin",
                        UFIO = "Червоный Данила Юрьевич",
                        Password = "WeRt1234"
                    },
                    new User {
                        Login = "Diktator",
                        UFIO = "Дмитрий Демидов Донской",
                        Password = "QwEr1234"
                    },
                    new User {
                        Login = "Test",
                        UFIO = "Тестовой Тест Тестович",
                        Password = "AwSe1234"
                    }
                });
                Genres!.AddRange(new List<Genre> {
                    new Genre { Name = "Металл" },
                    new Genre { Name = "Джаз" },
                    new Genre { Name = "Рэп" }
                });
                Performers!.AddRange(new List<Performer> {
                    new Performer { FIO = "The Weekend" },
                    new Performer { FIO = "Post Malone" },
                    new Performer { FIO = "Kordhell" }
                });
                SaveChanges();
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLazyLoadingProxies();
    }
}