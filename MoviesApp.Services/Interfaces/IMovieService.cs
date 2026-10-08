using MoviesApp.Dto.Dto;

namespace MoviesApp.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieReadDto>> GetAllMoviesAsync(int? genreId = null, int? year = null, string? title = null);
        Task<MovieReadDto> GetMovieByIdAsync(int id);
        Task<MovieReadDto> CreateMovieAsync(MovieCreateDto createDto);
        Task UpdateMovieAsync(int id, MovieUpdateDto updateDto);
        Task DeleteMovieAsync(int id);
        Task AddActorToMovieAsync(int movieId, int actorId);
        Task DeleteActorToMovieAsync(int movieId, int actorId);
    }
}
