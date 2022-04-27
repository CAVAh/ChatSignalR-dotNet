namespace ChatSignalR.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public System.Nullable<System.DateTime> CreatedAt { get; set; }
        public Group? Group { get; set; }
        public User? User { get; set; }
    }
}
