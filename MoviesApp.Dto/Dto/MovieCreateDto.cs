using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Dto.Dto
{
    public class MovieCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Range(1500, 2100)]
        public int Year { get; set; }
        [Range(1, int.MaxValue)]
        public int DurationMinutes { get; set; }
        [Range(1, int.MaxValue)]
        public int GenreId { get; set; }
        public int? DirectorId { get; set; }
        public List<int> ActorsId { get; set; }
    }
}
