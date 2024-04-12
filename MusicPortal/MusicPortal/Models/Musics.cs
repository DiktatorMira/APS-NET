using System.ComponentModel.DataAnnotations;

namespace MusicPortal.Models {
    public class Song {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Path { get; set; }
        public int UserId { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public User? User { get; set; }
        public Genre? Genre { get; set; }
        public Performer? Performer { get; set; }
    }
    public class Genre {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Song>? Songs { get; set; }
    }
    public class Performer {
        [Key]
        public int Id { get; set; }
        public string? FullName { get; set; }
        public ICollection<Song>? Songs { get; set; }
    }
}
