using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models {
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
}
