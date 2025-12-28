using System.ComponentModel.DataAnnotations.Schema;

namespace PosterrBackend.Application.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Creator { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
