using AutoMapper;
using PosterrBackend.Application.DTOs;
using PosterrBackend.Application.Interfaces;
using PosterrBackend.Domain.Entities;
using PosterrBackend.Domain.Enums;
using PosterrBackend.Domain.Exceptions;
using PosterrBackend.Domain.Interfaces;

namespace PosterrBackend.Application.Services
{
	public class RepostService : IRepostService
	{
        private readonly IMapper _mapper;
        private readonly IRepostRepository _repostRepository;
        private readonly IHelperService _helperService;

        public RepostService(IMapper mapper, IRepostRepository repostRepository, IHelperService helperService)
        {
            _repostRepository = repostRepository;
            _mapper = mapper;
            _helperService = helperService;
        }

        public async Task<int> CreateRepost(RepostDTO newRepostDTO)
		{
            if (newRepostDTO.IdPost <= 0)
                throw new NotProperContentException("The post reference is invalid");

            newRepostDTO.CreationDate = DateTime.UtcNow;

            var repostAvailability = await _helperService.IsRepostAvailableForUser(newRepostDTO.Creator, newRepostDTO.IdPost);

            if (repostAvailability.Equals(RepostAvailabilityStatus.UserIsOwner))
                throw new UserIsOwnerException("The user cannot repost his own post");
            if (repostAvailability.Equals(RepostAvailabilityStatus.AlreadyRepostedByUser))
                throw new AlreadyRepostedByUserException("The user cannot repost again the same post");
            if (repostAvailability.Equals(RepostAvailabilityStatus.PostNonExistent))
                throw new PostNonExistentException("The post does not exist");
            if (repostAvailability.Equals(RepostAvailabilityStatus.UserExceededDailyActions))
                throw new DailyActionsExceededException($@"The maximum limit of 5 posts/reposts per day has been reached for
                        user = '{newRepostDTO.Creator}' in date = '{newRepostDTO.CreationDate.Date}'");

            var newRepostEntity = _mapper.Map<Repost>(newRepostDTO);            
            var newRepostId = await _repostRepository.CreateRepost(newRepostEntity);
            return newRepostId;
        }
        
	}
}
