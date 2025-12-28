using PosterrBackend.Domain.Entities;

namespace PosterrBackend.Domain.Interfaces
{
	public interface IHelperRepository
	{
        Task<List<Record>> GetSortedPostsByPage(int page);
        Task<List<Post>> GetSortedPostsByTrend(int page);
    }
}
