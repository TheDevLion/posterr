using PosterrBackend.Application.DTOs;
using PosterrBackend.Domain.Entities;

namespace PosterrBackend.Application.Interfaces
{
	public interface IPostService
	{
        Task<int> CreatePost(PostDTO newPostDTO);
        Task<List<PostDTO>> FilterPostsByKeywords(string keywords, int page);
    }
}

