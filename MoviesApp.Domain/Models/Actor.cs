using MoviesApp.Domain.Domain;

namespace MoviesApp.Domain.Models
{
    public class Actor : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Movie> Movies { get; set; } = [];
    }
}
