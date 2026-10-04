using MoviesApp.Domain.Domain;

namespace MoviesApp.Dto.Dto
{
    public class GenreReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> MoviesReadDto { get; set; } = new();
    }
}
