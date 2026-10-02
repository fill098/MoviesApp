using MoviesApp.Domain.Domain;
using MoviesApp.Domain.Models;

namespace MoviesApp.DateAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsynce(T entity);

    }
}
