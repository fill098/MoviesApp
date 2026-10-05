using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Dto.Dto
{
    public class CreateGenreDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
