using System.ComponentModel.DataAnnotations;

namespace Dz09._04._2024.Models {
    public class Message {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Content { get; set; }
        public DateTime Date { get; set; }
    }
}
