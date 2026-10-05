using MoviesApp.Domain.Models;

namespace MoviesApp.DateAccess.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        Task<Genre?> GetByNameAsync(string name);
    }
}
