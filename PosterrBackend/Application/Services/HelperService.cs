using System.Collections.Generic;
using AutoMapper;
using PosterrBackend.Application.DTOs;
using PosterrBackend.Application.Interfaces;
using PosterrBackend.Domain.Entities;
using PosterrBackend.Domain.Enums;
using PosterrBackend.Domain.Exceptions;
using PosterrBackend.Domain.Interfaces;

namespace PosterrBackend.Application.Services
{
	public class HelperService : IHelperService
	{
		private readonly IMapper _mapper;
		private readonly IPostRepository _postRepository;
        private readonly IRepostRepository _repostRepository;
		private readonly IHelperRepository _helperRepository;

        public HelperService(IMapper mapper, IPostRepository postRepository,
			IRepostRepository repostRepository, IHelperRepository helperRepository)
		{
			_mapper = mapper;
			_postRepository = postRepository;
			_repostRepository = repostRepository;
			_helperRepository = helperRepository;
		}

		public async Task<bool> HasUserExceededDailyActions(string userName)
		{
			DateTime date = DateTime.UtcNow;

			int userPostsInDay = await _postRepository.CountPostsByUserAndDate(userName, date);
            int userRepostsInDay = await _repostRepository.CountRepostsByUserAndDate(userName, date);

			int total = userPostsInDay + userRepostsInDay;

			return total >= 5;
        }

		public async Task<RepostAvailabilityStatus> IsRepostAvailableForUser(string userName, int idPost)
		{
			Post? post = await _postRepository.GetPostById(idPost);
			if (post == null) return RepostAvailabilityStatus.PostNonExistent;
			if (post.Creator == userName) return RepostAvailabilityStatus.UserIsOwner;

			bool hasUserAlreadyRepostedThisPost = await _repostRepository.HasUserAlreadyRepostedThisPost(userName, idPost);
			if (hasUserAlreadyRepostedThisPost) return RepostAvailabilityStatus.AlreadyRepostedByUser;

            bool userCannotCreateAnother = await HasUserExceededDailyActions(userName);
			if (userCannotCreateAnother) return RepostAvailabilityStatus.UserExceededDailyActions;

            return RepostAvailabilityStatus.AvailableToRepost;
        }

		public async Task<List<RecordDTO>> LoadRecords(int page, bool orderByDate = true)
		{
			if (page < 0) throw new NotProperContentException("Page size cannot be negative");
			List<Record> results = new List<Record>();

			if (orderByDate) results = await _helperRepository.GetSortedPostsByPage(page);
			else
			{
				var data = await _helperRepository.GetSortedPostsByTrend(page);
				results = _mapper.Map<List<Record>>(data);
				results.All(r => r.IsPost = true);
			}
            
            var resultsDTO = _mapper.Map<List<RecordDTO>>(results);
			return resultsDTO;
		}

    }
}
