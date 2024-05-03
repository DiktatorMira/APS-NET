using System.ComponentModel.DataAnnotations;

namespace Dz09._04._2024.Models {
    public class Logon {
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Login { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
