using MoviesApp.Domain.Domain;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Dto.Dto
{
    public class DirectorCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
