using MoviesApp.Domain.Domain;

namespace MoviesApp.Domain.Models
{
    public class Director : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<Movie> Movies { get; set; } = new();
    }
}
