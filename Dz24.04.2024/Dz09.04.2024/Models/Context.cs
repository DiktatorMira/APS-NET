using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Dz09._04._2024.Models {
    public class Context : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public Context(DbContextOptions<Context> options) : base(options) => Database.EnsureCreated();
    }
}