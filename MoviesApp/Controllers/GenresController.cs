using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Common.Exceptions;
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

        [HttpGet("id:int")]

        public async Task<ActionResult<GenreReadDto>> GetById(int id)
        {
            try
            {
                GenreReadDto genreReadDto = await _genreService.GetGenreByIdAsync(id);
                return Ok(genreReadDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
            }
        }

        [HttpPost]

        public async Task<ActionResult<GenreReadDto>> Create(CreateGenreDto createGenreDto)
        {
            try
            {
                GenreReadDto genreReadDto = await _genreService.CreateGenreAsync(createGenreDto);

                return CreatedAtAction(nameof(GetById), new { id = genreReadDto.Id }, genreReadDto);
            }
            catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
            }

        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _genreService.DeleteGenreById(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
            }
        }
    }
}
