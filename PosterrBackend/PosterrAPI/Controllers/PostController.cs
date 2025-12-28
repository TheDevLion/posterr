using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PosterrBackend.Application.DTOs;
using PosterrBackend.Application.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using PosterrBackend.PosterrAPI.SwaggerExamples;
using PosterrBackend.Application.RequestModels;
using PosterrBackend.Domain.Exceptions;

namespace PosterrAPI.Controllers
{
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPostService _postService;

        public PostController(IMapper mapper, IPostService postService)
        {
            _mapper = mapper;
            _postService = postService;
        }

        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="newPost">The details of the post to create</param>
        /// <returns>New post's id</returns>
        /// <response code="200">Returns the new post's id</response>
        /// <response code="400">If the post content is not in specified range of characters (1 <= lenght <= 777)</response>
        /// <response code="403">
        /// If the user has exceeded the limit of 5 daily actions OR
        /// user is not found
        /// </response>
        /// <response code="500">If there is an internal server error</response>
        [HttpPost]
        [UserVerification]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(CreateNewPostRequest), typeof(CreateNewPostRequestExample))]
        public async Task<ActionResult<int>> CreateNewPost([FromBody] CreateNewPostRequest newPost)
        {
            try
            {
                var newPostDTO = _mapper.Map<PostDTO>(newPost);
                var newPostId = await _postService.CreatePost(newPostDTO);
                return Ok(newPostId);
            }
            catch (NotProperContentException e)
            {
                return StatusCode(400, new { message = e.Message });
            }
            catch (DailyActionsExceededException e)
            {
                return StatusCode(403, new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        /// <summary>
        /// Get filtered posts by keywords
        /// </summary>
        /// <param name="keywords">Proper text used to filter the posts</param>
        /// <param name="page">Page number to load data</param>
        /// <returns>List of filtered posts</returns>
        /// <response code="200">Returns the list of filtered posts</response>
        /// <response code="400">If keywords are empty</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PostDTO>>> FilterPostsByKeywords([FromQuery] string keywords, [FromQuery] int page)
        {
            try
            {
                var filteredPosts = await _postService.FilterPostsByKeywords(keywords, page);
                return Ok(filteredPosts);
            }
            catch (NotProperContentException e)
            {
                return StatusCode(400, new { message = e.Message });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

