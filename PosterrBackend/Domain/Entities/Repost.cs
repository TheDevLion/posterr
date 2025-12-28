using System.ComponentModel.DataAnnotations.Schema;

namespace PosterrBackend.Domain.Entities
{
    public class Repost
    {
        public int Id { get; set; }
        public int IdPost { get; set; }
        [ForeignKey("UserName")]
        public string Creator { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
