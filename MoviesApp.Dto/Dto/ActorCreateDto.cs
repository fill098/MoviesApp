using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Dto.Dto
{
    public class ActorCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
    }
}
