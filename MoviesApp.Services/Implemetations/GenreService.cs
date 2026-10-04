using MoviesApp.DateAccess.Interfaces;
using MoviesApp.Domain.Models;
using MoviesApp.Dto.Dto;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Services.Implemetations
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<List<GenreReadDto>> GetAllGenresAsync()
        {
            List<Genre> genreDb = await _genreRepository.GetAllAsync();

            List<GenreReadDto> genreReadDtos = genreDb.Select(genre => new GenreReadDto
            {
                Id = genre.Id,
                Name = genre.Name,
                MoviesReadDto = genre.Movies
                    .Select(movie => movie.Title)
                    .ToList()
            }).ToList();

            return genreReadDtos;
        }

        public Task<GenreReadDto> GetGenreByIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
