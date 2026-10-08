using MoviesApp.Domain.Domain;
using MoviesApp.Domain.Models;
using MoviesApp.Dto.Dto;

namespace MoviesApp.Mapper
{
    public static class MovieMapper
    {
        public static List<MovieReadDto> ToMoviesReadDtoList(this List<Movie> movies)
        {
            Func<Movie, MovieReadDto> movieDtoMapper = movie => movie.ToMovieReadDto();
            return movies.Select(movieDtoMapper).ToList();
        }
        
        public static MovieReadDto ToMovieReadDto(this Movie movie)
        {
            return new MovieReadDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Year = movie.Year,
                DurationMinutes = movie.DurationMinutes,
                GenreName = movie.Genre.Name,
                DirectorName = movie.Director != null
                ? $"{movie.Director.FirstName} {movie.Director.LastName}"
                : "Unknown",
                ActorNames = movie.Actors.Where(movie => movie != null).Select(actor => actor.FirstName + " " + actor.LastName).ToList()
            };
        }

        public static void ApplyTo(this MovieUpdateDto updateDto, Movie existingMovie, Genre genre, Director? director, List<Actor> actors)
        {
            existingMovie.Title = updateDto.Title;
            existingMovie.Description = updateDto.Description;
            existingMovie.Year = updateDto.Year;
            existingMovie.DurationMinutes = updateDto.DurationMinutes;
            existingMovie.GenreId = updateDto.GenreId;
            existingMovie.Genre = genre;
            existingMovie.DirectorId = updateDto.DirectorId;
            existingMovie.Director = director;
            existingMovie.Actors = actors;
        }
    }
}
