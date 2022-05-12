using System.ComponentModel.DataAnnotations.Schema;

namespace ChatSignalR.Models
{
    [Table("ChatMessages", Schema = "App")]
    public class Message
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public System.Nullable<System.DateTime> CreatedAt { get; set; }
        [Column("IdGroup")]
        public int GroupId { get; set; }
        [Column("IdUser")]
        public string? UserId { get; set; }
    }
}
