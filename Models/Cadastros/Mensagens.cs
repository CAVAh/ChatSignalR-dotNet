namespace ChatSignalR.Models
{
    public class Mensagens
    {
        public int Id { get; set; }
        public string? Mensagem { get; set; }
        public System.Nullable<System.DateTime> CreatedAt { get; set; }
        public Grupo? Grupo { get; set; }
        public User? User { get; set; }
    }
}
