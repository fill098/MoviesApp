using Microsoft.EntityFrameworkCore;
using MoviesApp.DateAccess.Data;
using MoviesApp.DateAccess.Interfaces;
using MoviesApp.Domain.Models;

namespace MoviesApp.DateAccess.Implementaions
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MoviesAppDbContext _context;

        public GenreRepository(MoviesAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Genre>> GetAllAsync()
        {
            var generes = _context.Genres
                .Include(m => m.Movies)
                .AsQueryable();

            return await generes.ToListAsync();
        }
        public async Task<Genre?> GetByIdAsync(int id)
        {
            return await _context.Genres
                .Include(m => m.Movies)
                .FirstOrDefaultAsync(g => g.Id == id);
        }
        public async Task AddAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Genre genre)
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsynce(Genre genre)
        {
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }
    }
}
