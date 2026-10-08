using MoviesApp.Domain.Models;
using MoviesApp.Dto.Dto;

namespace MoviesApp.Services.Interfaces
{
    public interface IActorService 
    {
        Task<List<ActorReadDto>> GetActorsAsync(int? movieId);
        Task<ActorReadDto> GetActorByIdAsync(int id);
        Task<ActorReadDto> CreateActorAsync(ActorCreateDto actorCreateDto);
    }
}
