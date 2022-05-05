using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatSignalR.Models
{

    [Table("group_users")]
    public class GroupUser
    {
        public int Id { get; set; }
        [Column("Id_Group")]
        public int GroupId { get; set; }
        public Group? Group { get; set; }
        [Column("Id_User")]
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
