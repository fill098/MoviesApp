using MoviesApp.Domain.Domain;

namespace MoviesApp.Dto.Dto
{
    public class DirectorReadDto
    {
        public string FullName{ get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<string> MoviesDto { get; set; } = new();
    }
}
