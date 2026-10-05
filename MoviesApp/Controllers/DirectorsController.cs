using Microsoft.AspNetCore.Mvc;
using MoviesApp.Dto.Dto;
using MoviesApp.Services.Implemetations;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorService _derectorService;

        public DirectorsController(IDirectorService derectorService)
        {
            _derectorService = derectorService;
        }

        [HttpGet]

        public async Task<ActionResult<List<DirectorReadDto>>> GetAll()
        {
            try
            {
                var allDirectors = await _derectorService.GetAllDirectorsAsync();
                return Ok(allDirectors);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
            }
        }
    }
}
