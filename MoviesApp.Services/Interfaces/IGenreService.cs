using MoviesApp.Dto.Dto;

namespace MoviesApp.Services.Interfaces
{
    public interface IGenreService
    {
        Task<List<GenreReadDto>> GetAllGenresAsync();

        Task<GenreReadDto> GetGenreByIdAsync();
    }
}
