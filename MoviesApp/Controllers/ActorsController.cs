using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Common.Exceptions;
using MoviesApp.Dto.Dto;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<ActorReadDto>> GetById(int id)
        {
            try
            {
                ActorReadDto result = await _actorService.GetActorByIdAsync(id);
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

        [HttpGet()]

        public async Task<ActionResult<List<ActorReadDto>>> Get([FromQuery]int? movieId)
        {
            try
            {
                var resut = await _actorService.GetActorsAsync(movieId);
                return Ok(resut);
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

        [HttpPost]
        public async Task<ActionResult<ActorReadDto>> Create(ActorCreateDto actorCreateDto)
        {
            try
            {
                var result = await _actorService.CreateActorAsync(actorCreateDto);
                return CreatedAtAction(nameof(GetById), new { Id = result.Id }, result);
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
