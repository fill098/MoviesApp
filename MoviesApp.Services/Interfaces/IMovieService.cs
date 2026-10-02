using MoviesApp.Dto.Dto;

namespace MoviesApp.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieReadDto>> GetAllAsync(int? genreId = null, int? year = null, string? title = null);
        Task<MovieReadDto> GetById(int id);
        Task<MovieReadDto> CreateAsync(MovieCreateDto createDto);
    }
}
