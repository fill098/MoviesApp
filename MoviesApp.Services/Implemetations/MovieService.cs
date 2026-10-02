using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using MoviesApp.Common.Exceptions;
using MoviesApp.DateAccess.Interfaces;
using MoviesApp.Domain.Domain;
using MoviesApp.Domain.Models;
using MoviesApp.Dto.Dto;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Services.Implemetations
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _moveRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IDirectorRepository _directorRepository;
        private readonly IGenreRepository _genreRepository;

        public MovieService(
            IMovieRepository movieRepository,
            IActorRepository actorRepository,
            IDirectorRepository directorRepository,
            IGenreRepository genreRepository)
        {
            _moveRepository = movieRepository;
            _actorRepository = actorRepository;
            _directorRepository = directorRepository;
            _genreRepository = genreRepository;
        }


        public async Task<List<MovieReadDto>> GetAllAsync(int? genreId = null, int? year = null, string? title = null)
        {
            var moviesDb = await _moveRepository.GetAllAsync(genreId, year, title);

            List<MovieReadDto> moviesDto = moviesDb.Select(movie => new MovieReadDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Year = movie.Year,
                DurationMinutes = movie.DurationMinutes,
                GenreName = movie.Genre.Name,
                DirectorName = movie.Director != null
                ? $"{movie.Director.FirstName} {movie.Director.LastName}" 
                :"Unknown",
                ActorNames = movie.Actors.Where(movie => movie != null).Select(actor => actor.FirstName + " " + actor.LastName).ToList()
            }).ToList();

            return moviesDto;
        }

        public async Task<MovieReadDto> GetById(int id)
        {

            var movieIdDb = await _moveRepository.GetByIdAsync(id);

            if (movieIdDb == null)
            {
                throw new NotFoundException($"Movie with id {id} was not found.");
            }

            var movieReadDto = Mapper.MovieMapper.ToMovieReadDto(movieIdDb);

            return movieReadDto;

        }

        public async Task<MovieReadDto> CreateAsync(MovieCreateDto createDto)
        {
 
            var gereIdResult = await _genreRepository.GetByIdAsync(createDto.GenreId);

            if (gereIdResult == null)
            {
                throw new BadRequestException($"There is no genre with that id: {createDto.GenreId}");
            }

            if (createDto.DirectorId.HasValue)
            {
                var directorIdresult = await _directorRepository.GetByIdAsync(createDto.DirectorId.Value);

                if (directorIdresult == null)
                {
                    throw new BadRequestException($"There is no movie director with this id: {createDto.DirectorId.Value}");
                }
            }

            List<Actor> validatedActors = new List<Actor>();
            foreach (int actorId in createDto.ActorsId)
            {
                var actorResult = await _actorRepository.GetByIdAsync(actorId);
                if (actorResult == null)
                {
                    throw new BadRequestException($"There is no actor with id: {actorId}");
                }
                validatedActors.Add(actorResult);
            }

            if (createDto.Year > DateTime.UtcNow.Year)
            {
                throw new BadRequestException($"The film can not be created in the future: {createDto.Year}");
            }


            var movie = new Movie
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Year = createDto.Year,
                DurationMinutes = createDto.DurationMinutes,
                GenreId = createDto.GenreId,
                DirectorId = createDto.DirectorId,
                Actors = validatedActors
            };

            await _moveRepository.AddAsync(movie);

            var movieReadDto = Mapper.MovieMapper.ToMovieReadDto(movie);

            return movieReadDto;
        }

    }
}
