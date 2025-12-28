using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PosterrBackend.Application.DTOs;
using PosterrBackend.Application.Interfaces;
using PosterrBackend.Application.RequestModels;
using PosterrBackend.Domain.Exceptions;
using Swashbuckle.AspNetCore.Filters;
using static PosterrBackend.PosterrAPI.SwaggerExamples.CreateNewRepostExample;

namespace PosterrAPI.Controllers
{
    [Route("api/[controller]")]
    public class RepostController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepostService _repostService;

        public RepostController(IRepostService repostService, IMapper mapper)
        {
            _repostService = repostService;
            _mapper = mapper;
        }

        /// <summary>
        /// Create a new repost
        /// </summary>
        /// <param name="newRepost">The details of the repost to create</param>
        /// <returns>New repost's id</returns>
        /// <response code="200">Returns the new repost's id</response>
        /// <response code="400">If post id reference passed is not valid</response>
        /// <response code="403">
        /// If user is the owner of the post OR
        /// if user has already reposted this post OR
        /// if the user has exceeded the limit of 5 daily action OR
        /// user is not found
        /// </response>
        /// <response code="422">If the post does not exist</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPost]
        [UserVerification]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(CreateNewRepostRequest), typeof(CreateNewRepostRequestExample))]
        public async Task<ActionResult<int>> CreateNewRepost([FromBody] CreateNewRepostRequest newRepost)
        {
            try
            {
                var newRepostDTO = _mapper.Map<RepostDTO>(newRepost);
                var newRepostId = await _repostService.CreateRepost(newRepostDTO);
                return Ok(newRepostId);
            }
            catch (NotProperContentException e)
            {
                return StatusCode(400, new { message = e.Message });
            }
            catch (UserIsOwnerException e)
            {
                return StatusCode(403, new { message = e.Message });
            }
            catch (PostNonExistentException e)
            {
                return StatusCode(404, new { message = e.Message });
            }
            catch (AlreadyRepostedByUserException e)
            {
                return StatusCode(403, new { message = e.Message });
            }
            catch (DailyActionsExceededException e)
            {
                return StatusCode(422, new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

