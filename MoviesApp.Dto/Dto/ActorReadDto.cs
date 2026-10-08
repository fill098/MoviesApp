using MoviesApp.Domain.Domain;

namespace MoviesApp.Dto.Dto
{
    public class ActorReadDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> MoviesDto { get; set; } = [];
    }
}
