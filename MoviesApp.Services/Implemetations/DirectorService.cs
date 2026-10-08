using MoviesApp.Common.Exceptions;
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

        public async Task<DirectorReadDto> GetDirectorByIdAsync(int id)
        {
            Director directorDd = await _directorRepository.GetByIdAsync(id);

            if (directorDd == null)
            {
                throw new NotFoundException($"A Director with id: {id} dose not exists.");
            }

            DirectorReadDto directorReadDto = new DirectorReadDto
            {
                FullName = directorDd.FirstName + " " + directorDd.LastName,
                DateOfBirth = directorDd.DateOfBirth,
                MoviesDto = directorDd.Movies.Select(m => m.Title).ToList()
            };
            return directorReadDto;
        }
        public async Task<DirectorReadDto> CreateDirectorAsync(DirectorCreateDto directorCreateDto)
        {
            var director = new Director
            {
                FirstName = directorCreateDto.FirstName,
                LastName = directorCreateDto.LastName,
                DateOfBirth = directorCreateDto.DateOfBirth,
            };

            await _directorRepository.AddAsync(director);

            var directorReadDto = new DirectorReadDto
            {
                Id = director.Id,
                DateOfBirth = director.DateOfBirth,
                FullName = $"{director.FirstName} {director.LastName}",
                MoviesDto = director.Movies.Select(m => m.Title.ToString()).ToList()
            };
            return directorReadDto;
        }
    }
}
