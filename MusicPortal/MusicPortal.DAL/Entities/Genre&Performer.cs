namespace MusicPortal.DAL.Entities {
    public class Genre {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Song>? Songs { get; set; }
    }
    public class Performer {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public virtual ICollection<Song>? Songs { get; set; }
    }
}