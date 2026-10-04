using MoviesApp.Dto.Dto;

namespace MoviesApp.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieReadDto>> GetAllMoviesAsync(int? genreId = null, int? year = null, string? title = null);
        Task<MovieReadDto> GetMovieById(int id);
        Task<MovieReadDto> CreateMovieAsync(MovieCreateDto createDto);
        Task UpdateMovieAsync(int id, MovieUpdateDto updateDto);
        Task DeleteMovieAsync(int id);
    }
}
