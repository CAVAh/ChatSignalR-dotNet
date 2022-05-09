using System.ComponentModel.DataAnnotations.Schema;

namespace ChatSignalR.Models
{
    public class Message
    {
        //public Message(int groupId, int userId, string? text)
        //{
        //    GroupId = groupId;
        //    UserId = userId;
        //    Text = text;
        //}

        public int Id { get; set; }
        public string? Text { get; set; }
        public System.Nullable<System.DateTime> Created_At { get; set; }
        [Column("Id_Group")]
        public int GroupId { get; set; }
        [Column("Id_User")]
        public int UserId { get; set; }
    }
}
