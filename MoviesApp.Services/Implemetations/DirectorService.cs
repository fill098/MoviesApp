using MoviesApp.DateAccess.Interfaces;
using MoviesApp.Domain.Models;
using MoviesApp.Dto.Dto;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Services.Implemetations
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorRepository _directorRepository;

        public DirectorService(IDirectorRepository directorRepository)
        {
            _directorRepository = directorRepository;
        }
        public async Task<List<DirectorReadDto>> GetAllDirectorsAsync()
        {
            List<Director> allDirectorsDb = await _directorRepository.GetAllAsync();

            List<DirectorReadDto> directorReadDtos = allDirectorsDb.Select(d => new DirectorReadDto
            {
               FullName = d.FirstName + " " + d.LastName,
               DateOfBirth = d.DateOfBirth,
               MoviesDto = d.Movies.Select(m => m.Title).ToList()
            }).ToList();

            return directorReadDtos;
        }
    }
}
