using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models {
    public class Song {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Title { get; set; }
        public string? Path { get; set; }
        public int UserId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public virtual User? User { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public virtual Genre? Genre { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public virtual Performer? Performer { get; set; }
    }
    public class Genre {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? Name { get; set; }
        public virtual ICollection<Song>? Songs { get; set; }
    }
    public class Performer {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Поле не должно быть пустым!")]
        public string? FullName { get; set; }
        public virtual ICollection<Song>? Songs { get; set; }
    }
}
