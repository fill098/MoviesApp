using MoviesApp.Domain.Models;

namespace MoviesApp.DateAccess.Interfaces
{
    public interface IActorRepository : IRepository<Actor>
    {
        Task<List<Actor>> GetByIdsAsync(List<int> ids);

        Task<List<Actor>> GetAllAsync(int? movieId);
    }
}
