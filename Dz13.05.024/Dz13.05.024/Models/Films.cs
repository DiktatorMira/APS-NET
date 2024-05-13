using Dz13._05._024.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Dz13._05._024.Models {
    public class Films {
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        public string? Director { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        [Genre(["Комедия", "Мелодрама"], ErrorMessage = "Недопустимый жанр!")]
        public string? Genre { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [FutureDate(ErrorMessage = "Дата не может быть в будущем")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        public string? PosterPath { get; set; }
        [Required(ErrorMessage = "Заполните поле!")]
        public string? Description { get; set; }
    }
}