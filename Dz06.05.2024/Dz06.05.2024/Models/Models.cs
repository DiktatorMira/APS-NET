using System.ComponentModel.DataAnnotations;

namespace Dz06._05._2024.Models {
    public class User {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Password { get; set; }
    }
    public class Genre {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Name { get; set; }
    }
    public class Performer {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? FullName { get; set; }
    }
}