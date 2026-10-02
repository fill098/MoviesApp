using Microsoft.EntityFrameworkCore;
using MoviesApp.DateAccess.Data;
using MoviesApp.DateAccess.Interfaces;
using MoviesApp.Domain.Domain;
using MoviesApp.Domain.Models;

namespace MoviesApp.DateAccess.Implementaions
{
    public class ActorRepository : IActorRepository
    {
        private readonly MoviesAppDbContext _context;

        public ActorRepository(MoviesAppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Actor>> GetAllAsync()
        {
            var actors = _context.Actors
                .Include(m => m.Movies)
                .AsQueryable();

            return await actors.ToListAsync();
        }
        public async Task<Actor?> GetByIdAsync(int id)
        {
            return await _context.Actors
                .Include(m => m.Movies)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
        public async Task AddAsync(Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Actor actor)
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsynce(Actor actor)
        {
            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
        }


    }
}
