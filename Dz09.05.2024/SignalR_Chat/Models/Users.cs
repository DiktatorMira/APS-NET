﻿namespace SignalR_Chat.Models {
    public class User {
        public int Id { get; set; }
        public string? ConnectionId { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }
    }
}