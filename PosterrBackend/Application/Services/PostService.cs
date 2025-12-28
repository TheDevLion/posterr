
using AutoMapper;
using PosterrBackend.Application.DTOs;
using PosterrBackend.Application.Interfaces;
using PosterrBackend.Domain.Entities;
using PosterrBackend.Domain.Exceptions;
using PosterrBackend.Domain.Interfaces;

namespace PosterrBackend.Application.Services
{
	public class PostService : IPostService
	{
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
		private readonly IHelperService _helperService;

		public PostService(IMapper mapper, IPostRepository postRepository, IHelperService helperService)
		{
			_postRepository = postRepository;
			_mapper = mapper;
			_helperService = helperService;
		}

        public async Task<int> CreatePost(PostDTO newPostDTO)
        {
			// Fatest exception case coming first (performance reason)
			if (newPostDTO.Text.Count() == 0 || newPostDTO.Text.Count() > 777)
				throw new NotProperContentException("Post's text is out of limits 1 <= length <= 777 characters");

			newPostDTO.CreationDate = DateTime.UtcNow;

            // It is debatable if the exception for breaking the posts daily limit should be evaluated first
            bool userCannotCreateAnother =
				await _helperService.HasUserExceededDailyActions(newPostDTO.Creator);
			if (userCannotCreateAnother)
				throw new DailyActionsExceededException($@"The maximum limit of 5 posts/reposts per day has been
					reached for user = '{newPostDTO.Creator}' in date = '{newPostDTO.CreationDate.Date}'");

			var newPostEntity = _mapper.Map<Post>(newPostDTO);
			var newPostId = await _postRepository.CreatePost(newPostEntity);
			return newPostId;
        }

        public async Task<List<PostDTO>> FilterPostsByKeywords(string keywords, int page)
		{
			if (!keywords.Any()) throw new NotProperContentException("Invalid keywords");

			var results = await _postRepository.FilterPostsByKeywords(keywords, page);
			var resultsDTO = _mapper.Map<List<PostDTO>>(results);
			return resultsDTO;
		}
    }
}
