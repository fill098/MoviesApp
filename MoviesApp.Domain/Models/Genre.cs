using MoviesApp.Domain.Domain;

namespace MoviesApp.Domain.Models
{
    public class Genre : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Movie> Movies { get; set; } = new();
    }
}
