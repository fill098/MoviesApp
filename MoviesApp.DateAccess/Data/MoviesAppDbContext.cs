using Microsoft.EntityFrameworkCore;
using MoviesApp.DateAccess.Helpers;
using MoviesApp.Domain.Domain;
using MoviesApp.Domain.Models;

namespace MoviesApp.DateAccess.Data
{
    public class MoviesAppDbContext : DbContext
    {
        public MoviesAppDbContext(DbContextOptions<MoviesAppDbContext> options) : base(options) 
        {  
        }


        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Genre> Genres { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureMovieApp();

            modelBuilder.SeedData();

            base.OnModelCreating(modelBuilder);
        }
    }
}
