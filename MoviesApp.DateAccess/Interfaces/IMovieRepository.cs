using MoviesApp.Domain.Domain;

namespace MoviesApp.DateAccess.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<List<Movie>> GetAllAsync(int? genreId, int? year, string? title);
    }
}
