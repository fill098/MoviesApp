using Microsoft.AspNetCore.Mvc;
using MoviesApp.Common.Exceptions;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DirectorReadDto>> GetById(int id)
        {
            try
            {
                var resut = await _derectorService.GetDirectorByIdAsync(id);
                return Ok(resut);
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

        public async Task<ActionResult<DirectorReadDto>> Create(DirectorCreateDto directorCreateDto)
        {
            try
            {
                DirectorReadDto directorReadDto = await _derectorService.CreateDirectorAsync(directorCreateDto);
                return CreatedAtAction(nameof(GetById), new { Id = directorReadDto.Id }, directorReadDto);
            }
            catch(BadRequestException ex)
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
