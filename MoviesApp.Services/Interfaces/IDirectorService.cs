using MoviesApp.Dto.Dto;

namespace MoviesApp.Services.Interfaces
{
    public interface IDirectorService
    {
        Task<List<DirectorReadDto>> GetAllDirectorsAsync();
    }
}
