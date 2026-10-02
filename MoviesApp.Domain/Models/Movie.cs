using MoviesApp.Domain.Models;

namespace MoviesApp.Domain.Domain
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Year { get; set; }
        public int DurationMinutes { get; set; }
        public Genre Genre { get; set; }
        public int GenreId { get; set; }
        public Director Director { get; set; }
        public int? DirectorId { get; set; }
        public List<Actor> Actors { get; set; } = new();
    }
}
