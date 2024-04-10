using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Dz09._04._2024.Models {
    public class Register {
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        [DataType(DataType.Password)]
        public string? RepeatPassword { get; set; }
    }
    public class Logon {
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
    public class User {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
    public class Message {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Content { get; set; }
        public DateTime Date { get; set; }
    }
    public class UserContext : DbContext {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options) => Database.EnsureCreated();
    }
    public class CombinedMessages {
        public Message? MessageModel { get; set; }
        public IEnumerable<Message>? Messages { get; set; }
    }
}