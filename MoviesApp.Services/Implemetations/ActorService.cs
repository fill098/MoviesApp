using MoviesApp.Common.Exceptions;
using MoviesApp.DateAccess.Interfaces;
using MoviesApp.Domain.Models;
using MoviesApp.Dto.Dto;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Services.Implemetations
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;

        public ActorService(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }


        public async Task<ActorReadDto> GetActorByIdAsync(int id)
        {
            var actorDb = await _actorRepository.GetByIdAsync(id);

            if (actorDb is null)
            {
                throw new NotFoundException($"Actor wiht id: {id} dose not exists.");
            }

            ActorReadDto actorReadDto = new ActorReadDto
            {
                Id = actorDb.Id,
                FirstName = actorDb.FirstName,
                LastName = actorDb.LastName,
                MoviesDto = actorDb.Movies.Select(m => m.Title).ToList(),
            };

            return actorReadDto;
        }
        public async Task<List<ActorReadDto>> GetActorsAsync(int? movieId)
        {
            List<Actor> actorsDb = await _actorRepository.GetAllAsync(movieId);

            List<ActorReadDto> actorReadDtos = actorsDb.Select(a => new ActorReadDto
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                MoviesDto = a.Movies.Select(x =>  x.Title.ToString()).ToList(),
            }).ToList();

            return actorReadDtos;
        }
        public async Task<ActorReadDto> CreateActorAsync(ActorCreateDto actorCreateDto)
        {
            Actor newActor = new Actor
            {
                FirstName = actorCreateDto.FirstName,
                LastName = actorCreateDto.LastName,
            };

            await _actorRepository.AddAsync(newActor);

            ActorReadDto actorReadDto = new ActorReadDto
            {   Id = newActor.Id,
                FirstName = newActor.FirstName,
                LastName = newActor.LastName,
                MoviesDto = newActor.Movies.Select(m => m.Title.ToString()).ToList(),
            };

            return actorReadDto;
        }

    }
}
