using MoviesApp.Common.Exceptions;
using MoviesApp.DateAccess.Interfaces;
using MoviesApp.Domain.Models;
using MoviesApp.Dto.Dto;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Services.Implemetations
{
    public class GenreService : IGenreService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository, IMovieRepository movieRepositroy)
        {
            _genreRepository = genreRepository;
            _movieRepository = movieRepositroy;
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

        public async Task<GenreReadDto> GetGenreByIdAsync(int id)
        {
            Genre genredb = await _genreRepository.GetByIdAsync(id);

            if (genredb is null)
            {
                throw new NotFoundException($"Genre with id {id} was not found.");
            }

            GenreReadDto movieReadDto = new GenreReadDto
            {
                Id = genredb.Id,
                Name = genredb.Name,
                MoviesReadDto = genredb.Movies.Select(movie => movie.Title.ToString()).ToList()
            };

            return movieReadDto;

        }
        public async Task<GenreReadDto> CreateGenreAsync(CreateGenreDto createGenreDto)
        {
            Genre genreExistsInDb = await _genreRepository.GetByNameAsync(createGenreDto.Name);

            if (genreExistsInDb != null)
            {
                throw new ConflictException($"A genre with this name: {createGenreDto.Name} already exists.");
            }

            Genre genreDd = new Genre
            {
                Name = createGenreDto.Name,
            };

            await _genreRepository.AddAsync(genreDd);

            GenreReadDto genreReadDto = new GenreReadDto 
            { 
                Id = genreDd.Id,
                Name = genreDd.Name,
                MoviesReadDto = genreDd.Movies.Select(movie => movie.Title.ToString()).ToList()
            };

            return genreReadDto;
        }

        public async Task DeleteGenreById(int id)
        {
            var genreDb = await _genreRepository.GetByIdAsync(id);

            if (genreDb == null)
            {
                throw new NotFoundException($"A genre with id: {id} dose not exist.");
            }

            bool isGenreInUse = await _movieRepository.ExistsByGenreIdAsync(id);

            if (isGenreInUse)
            {
                throw new ConflictException($"Cannot delete genre with id {id} because movies still reference it.");
            }

            await _genreRepository.DeleteAsync(genreDb);
        }
    }
}
