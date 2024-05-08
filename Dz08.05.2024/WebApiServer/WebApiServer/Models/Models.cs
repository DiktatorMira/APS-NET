using System.ComponentModel.DataAnnotations;

namespace WebApiServer.Models {
    public class User {
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? UFIO { get; set; }
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
        public string? FIO { get; set; }
    }
}