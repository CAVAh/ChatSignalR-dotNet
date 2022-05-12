using System.ComponentModel.DataAnnotations.Schema;

namespace ChatSignalR.Models
{
    [Table("ChatGroups", Schema = "App")]
    public class Group
    {
        public int Id { get; set; }
        public String? Name { get; set; }
    }
}
