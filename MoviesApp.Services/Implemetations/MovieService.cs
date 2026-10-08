using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using MoviesApp.Common.Exceptions;
using MoviesApp.DateAccess.Interfaces;
using MoviesApp.Domain.Domain;
using MoviesApp.Domain.Models;
using MoviesApp.Dto.Dto;
using MoviesApp.Mapper;
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
        public async Task<List<MovieReadDto>> GetAllMoviesAsync(int? genreId = null, int? year = null, string? title = null)
        {
            List<Movie> moviesDb = await _moveRepository.GetAllAsync(genreId, year, title);

            List<MovieReadDto> moviesDto = moviesDb.ToMoviesReadDtoList();

            return moviesDto;
        }
        public async Task<MovieReadDto> GetMovieByIdAsync(int id)
        {
            Movie movieIdDb = await _moveRepository.GetByIdAsync(id);

            if (movieIdDb == null)
            {
                throw new NotFoundException($"Movie with id {id} was not found.");
            }

            MovieReadDto movieReadDto = Mapper.MovieMapper.ToMovieReadDto(movieIdDb);

            return movieReadDto;
        }

        public async Task<MovieReadDto> CreateMovieAsync(MovieCreateDto createDto)
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

            var movieReadDto = movie.ToMovieReadDto();

            return movieReadDto;
        }

        public async Task UpdateMovieAsync(int id, MovieUpdateDto updateDto)
        {
            var movieDb = await _moveRepository.GetByIdAsync(id);

            if (movieDb is null)
            {
                throw new NotFoundException($"Movie with id {id} dose not existes!");
            }

            Genre genre = await GetValidGenreAsync(updateDto.GenreId);
            Director? director = await GetValidDirectorAsync(updateDto.DirectorId);
            List<Actor> actors = await GetValidActorsAsync(updateDto.ActorsId);

            updateDto.ApplyTo(movieDb, genre, director, actors);

            await _moveRepository.UpdateAsync(movieDb); 
        }


        public async Task DeleteMovieAsync(int id)
        {
            var movieDb = await _moveRepository.GetByIdAsync(id);

            if(movieDb is null)
            {
                throw new NotFoundException($"Movie with id {id} was not found.");
            }

            await _moveRepository.DeleteAsync(movieDb);
        }

        public async Task AddActorToMovieAsync(int movieId, int actorId)
        {
            var movieDb = await _moveRepository.GetByIdAsync(movieId);
            var actorDb = await _actorRepository.GetByIdAsync(actorId);

            if(movieDb is null)
            {
                throw new NotFoundException($"Movie with id: {movieId} was not found.");
            }

            if (actorDb is null)
            {
                throw new NotFoundException($"Actor with id: {actorId} was not found.");
            }

            var isLinkedActor = movieDb.Actors.Any(x => x.Id == actorId);

            if (isLinkedActor)
            {
                throw new ConflictException($"The actor with id: {actorId} is alrady cast in the movie with id: {movieId}");
            }

            movieDb.Actors.Add(actorDb);

            await _moveRepository.UpdateAsync(movieDb);

        }
        public async Task DeleteActorToMovieAsync(int movieId, int actorId)
        {
            var movieDb = await _moveRepository.GetByIdAsync(movieId);
            var actorDb = await _actorRepository.GetByIdAsync(actorId);

            if (movieDb is null)
            {
                throw new NotFoundException($"Movie with id: {movieId} was not found.");
            }

            if (actorDb is null)
            {
                throw new NotFoundException($"Actor with id: {actorId} was not found.");
            }

            var isLinkedActor = movieDb.Actors.Any(x => x.Id == actorId);

            if (!isLinkedActor)
            {
                throw new NotFoundException($"There is no actor with the id: {actorId} in the movie with id: {movieId}");
            }

            movieDb.Actors.Remove(actorDb);
            await _moveRepository.UpdateAsync(movieDb);

        }

      


        #region Private helpers
        private async Task<Genre> GetValidGenreAsync(int genreId)
        {
            var genre = await _genreRepository.GetByIdAsync(genreId);
            if (genre == null)
            {
                throw new BadRequestException($"There is no genre with id: {genreId}");
            }
            return genre;
        }

        private async Task<Director?> GetValidDirectorAsync(int? directorId)
        {
            if (!directorId.HasValue)
            {
                return null;
            }

            var director = await _directorRepository.GetByIdAsync(directorId.Value);
            if (director == null)
            {
                throw new BadRequestException($"There is no movie director with id: {directorId.Value}");
            }
            return director;
        }

        private async Task<List<Actor>> GetValidActorsAsync(List<int> actorIds)
        {
            var actors = await _actorRepository.GetByIdsAsync(actorIds);
            if (actors.Count != actorIds.Count)
            {
                throw new BadRequestException("One or more actor ids are invalid.");
            }
            return actors;
        }


        #endregion
    }
}
