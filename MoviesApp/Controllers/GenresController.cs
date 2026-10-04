using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Dto.Dto;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]

        public async Task<ActionResult<List<GenreReadDto>>> GetAll()
        {
            try
            {
                List<GenreReadDto> result = await _genreService.GetAllGenresAsync();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
            }
           
        }

    }
}
