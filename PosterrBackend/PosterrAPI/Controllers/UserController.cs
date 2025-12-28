using PosterrBackend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PosterrBackend.Application.DTOs;

namespace PosterrAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService) { 
            _userService = userService;
        }

        /// <summary>
        /// Get all users from the database
        /// </summary>
        /// <returns>A list of users</returns>
        /// <response code="200">Returns the list of users</response>
        /// <response code="204">No user founded</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetUsers();
                if (!users.Any()) return NoContent();

                return Ok(users);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e}");
            }
        }
    }
}
