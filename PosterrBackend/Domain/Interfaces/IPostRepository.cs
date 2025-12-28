using PosterrBackend.Domain.Entities;

namespace PosterrBackend.Domain.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPosts();
        Task<Post?> GetPostById(int idPost);
        Task<int> CreatePost(Post newPost);
        Task<int> CountPostsByUserAndDate(string userName, DateTime date);
        Task<List<Post>> FilterPostsByKeywords(string keywords, int page);
    }
}
