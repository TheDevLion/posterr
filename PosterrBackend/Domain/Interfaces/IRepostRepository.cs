using PosterrBackend.Domain.Entities;
using PosterrBackend.Domain.Enums;

namespace PosterrBackend.Domain.Interfaces
{
    public interface IRepostRepository
    {
        Task<List<Repost>> GetReposts();
        Task<int> CreateRepost(Repost newRepost);
        Task<bool> HasUserAlreadyRepostedThisPost(string userName, int idPost);
        Task<int> CountRepostsByUserAndDate(string userName, DateTime date);
    }
}
