namespace Dz09._04._2024.Models {
    public class User {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
