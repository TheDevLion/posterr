using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PosterrBackend.Application.DTOs;
using PosterrBackend.Application.Interfaces;
using PosterrBackend.Application.RequestModels;
using PosterrBackend.Domain.Exceptions;
using PosterrBackend.PosterrAPI.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;
using static PosterrBackend.PosterrAPI.SwaggerExamples.CreateNewRepostExample;

namespace PosterrAPI.Controllers
{
    [Route("api/[controller]")]
    public class RecordController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHelperService _helperService;

        public RecordController(IMapper mapper, IHelperService helperService)
        {
            _mapper = mapper;
            _helperService = helperService;
        }

        /// <summary>
        /// Load page records
        /// </summary>
        /// <param name="page">Page number to load data</param>
        /// <param name="sortByDate">If records will be sorted by date or trend</param>
        /// <returns>List of Records to show</returns>
        /// <response code="200">Returns list of records for proper sort and page</response>
        /// <response code="204">No record found</response>
        /// <response code="400">Error in payload</response>
        /// <response code="500">If there is an internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RecordDTO>>> GetRecords([FromQuery] int page, [FromQuery] bool sortByDate = true)
        {
            try
            {
                var results = await _helperService.LoadRecords(page, sortByDate);
                if (!results.Any()) return NoContent();
                return Ok(results);
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

