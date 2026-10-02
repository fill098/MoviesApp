using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Common.Exceptions;
using MoviesApp.Domain.Models;
using MoviesApp.Dto.Dto;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieReadDto>>> GetAll(int? genreId = null, int? year = null, string? title = null)
        {
            try
            {
                List<MovieReadDto> result = await _movieService.GetAllAsync(genreId, year, title);
                return Ok(result);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<MovieReadDto>> GetById(int id)
        {

            try
            {
                MovieReadDto result = await _movieService.GetById(id);
                return Ok(result);
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
        public async Task<ActionResult<MovieReadDto>> Create([FromBody] MovieCreateDto createDto)
        {
            try
            {
                MovieReadDto readDto = await _movieService.CreateAsync(createDto);

                return CreatedAtAction(nameof(GetById), new { id = readDto.Id }, readDto);

            }
            catch (BadRequestException ex)
            {

                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
            }
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, MovieUpdateDto updateDto)
        {
            try
            {
                await _movieService.UpdateAsync(id, updateDto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred, please contact the administrator.");
            }
        }






    }
}
