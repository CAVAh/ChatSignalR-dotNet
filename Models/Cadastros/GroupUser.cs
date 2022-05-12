using System.ComponentModel.DataAnnotations.Schema;

namespace ChatSignalR.Models
{
    [Table("ChatGroupUsers", Schema = "App")]
    public class GroupUser
    {
        public int Id { get; set; }
        [Column("IdGroup")]
        public int GroupId { get; set; }
        [Column("IdUser")]
        public string? UserId { get; set; }
        [NotMapped]
        public string[]? UserIds { get; set; }
    }
}
