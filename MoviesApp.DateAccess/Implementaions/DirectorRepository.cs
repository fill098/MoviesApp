using Microsoft.EntityFrameworkCore;
using MoviesApp.DateAccess.Data;
using MoviesApp.DateAccess.Interfaces;
using MoviesApp.Domain.Models;

namespace MoviesApp.DateAccess.Implementaions
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly MoviesAppDbContext _context;

        public DirectorRepository(MoviesAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Director>> GetAllAsync()
        {
            var directors = _context.Directors
                .Include(m => m.Movies)
                .AsQueryable();

           return await directors.ToListAsync();
        }
        public async Task<Director?> GetByIdAsync(int id)
        {
            return await _context.Directors
                .Include(m => m.Movies)
                .FirstOrDefaultAsync(d => d.Id == id);
        }
        public async Task AddAsync(Director director)
        {
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Director director)
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsynce(Director entity)
        {
            _context.Directors.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
