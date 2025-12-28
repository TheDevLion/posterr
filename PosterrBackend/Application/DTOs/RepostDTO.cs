using System.ComponentModel.DataAnnotations.Schema;

namespace PosterrBackend.Application.DTOs
{
    public class RepostDTO
    {
        public int Id { get; set; }
        public int IdPost { get; set; }
        public string Creator { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
