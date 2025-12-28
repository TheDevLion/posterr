using PosterrBackend.Application.DTOs;

namespace PosterrBackend.Application.Interfaces
{
	public interface IRepostService
	{
        Task<int> CreateRepost(RepostDTO newRepostDTO);
    }
}

