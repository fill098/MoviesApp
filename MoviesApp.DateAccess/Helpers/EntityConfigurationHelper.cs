using Microsoft.EntityFrameworkCore;
using MoviesApp.Domain.Domain;
using MoviesApp.Domain.Models;

namespace MoviesApp.DateAccess.Helpers
{
    internal static class EntityConfigurationHelper
    {

        public static void ConfigureMovieApp(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(genre => genre.Name)
                      .IsRequired()
                      .HasMaxLength(50);

                // Genre.Name must be unique — a duplicate insert should fail at the DB level too,
                // not just get caught by the service.
                entity.HasIndex(genre => genre.Name)
                      .IsUnique();
            });

            modelBuilder.Entity<Director>(entity =>
            {
                entity.ToTable("Director");

                entity.Property(director => director.FirstName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(director => director.LastName)
                      .IsRequired()
                      .HasMaxLength(50);

                // DateOfBirth is optional (DateTime?) and must be stored as "date",
                // not the default "datetime2" — a birth date has no time component.
                entity.Property(director => director.DateOfBirth)
                      .HasColumnType("date");
            });

            modelBuilder.Entity<Actor>(entity =>
            {
                entity.ToTable("Actor");

                entity.Property(actor => actor.FirstName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(actor => actor.LastName)
                      .IsRequired()
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("Movie");

                entity.Property(movie => movie.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(movie => movie.Description)
                      .HasMaxLength(1000);
                // No IsRequired() here — Description is optional (string?).

                // Year and DurationMinutes are plain int (required by CLR default),
                // so there's nothing to configure here beyond what the DTO/service validate.
                // If you want a DB-level NOT NULL guarantee it's already implicit for a
                // non-nullable int — EF never generates a nullable column for it.

                // ===> One to Many (1:M) — Genre
                // A genre with movies must NOT be deletable in a way that silently
                // deletes the movies. Restrict makes SQL Server refuse the delete
                // instead of cascading or nulling — the service turns that refusal
                // into a 409.
                entity.HasOne(movie => movie.Genre)
                      .WithMany(genre => genre.Movies)
                      .HasForeignKey(movie => movie.GenreId)
                      .OnDelete(DeleteBehavior.Restrict);

                // ===> One to Many (1:M) — Director
                // DirectorId is nullable (int?), so deleting a director should leave
                // their movies behind with DirectorId set to null, not fail and not
                // delete the movie. SetNull is only valid because the FK is optional.
                entity.HasOne(movie => movie.Director)
                      .WithMany(director => director.Movies)
                      .HasForeignKey(movie => movie.DirectorId)
                      .OnDelete(DeleteBehavior.SetNull);

                // We filter by these two most often (GenreId, Year), so both get indexed.
                entity.HasIndex(movie => movie.GenreId);
                entity.HasIndex(movie => movie.Year);

                // ===> Many to Many (M:M) — Actor
                // The spec requires an explicit join table name (MovieActor) with
                // columns named MovieId / ActorId — not the default "ActorsId" you'd
                // get from an unnamed FK on the navigation property.
                entity.HasMany(movie => movie.Actors)
                      .WithMany(actor => actor.Movies)
                      .UsingEntity(
                          "MovieActor",
                          right => right.HasOne(typeof(Actor)).WithMany().HasForeignKey("ActorId"),
                          left => left.HasOne(typeof(Movie)).WithMany().HasForeignKey("MovieId")
                      );
            });
        }
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            // Constant dates — HasData refuses DateTime.UtcNow because the seed
            // must be deterministic (otherwise every Add-Migration regenerates it).
            var seedDate = new DateTime(2024, 1, 1);

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Action", CreatedDate = seedDate, UpdatedDate = seedDate },
                new Genre { Id = 2, Name = "Comedy", CreatedDate = seedDate, UpdatedDate = seedDate },
                new Genre { Id = 3, Name = "Drama", CreatedDate = seedDate, UpdatedDate = seedDate },
                new Genre { Id = 4, Name = "Crime", CreatedDate = seedDate, UpdatedDate = seedDate }
            );

            modelBuilder.Entity<Director>().HasData(
                new Director { Id = 1, FirstName = "Quentin", LastName = "Tarantino", DateOfBirth = new DateTime(1963, 3, 27), CreatedDate = seedDate, UpdatedDate = seedDate },
                new Director { Id = 2, FirstName = "Christopher", LastName = "Nolan", DateOfBirth = new DateTime(1970, 7, 30), CreatedDate = seedDate, UpdatedDate = seedDate },
                new Director { Id = 3, FirstName = "Martin", LastName = "Scorsese", DateOfBirth = new DateTime(1942, 11, 17), CreatedDate = seedDate, UpdatedDate = seedDate }
            );

            modelBuilder.Entity<Actor>().HasData(
                new Actor { Id = 1, FirstName = "Uma", LastName = "Thurman", CreatedDate = seedDate, UpdatedDate = seedDate },
                new Actor { Id = 2, FirstName = "Leonardo", LastName = "DiCaprio", CreatedDate = seedDate, UpdatedDate = seedDate },
                new Actor { Id = 3, FirstName = "Samuel", LastName = "Jackson", CreatedDate = seedDate, UpdatedDate = seedDate },
                new Actor { Id = 4, FirstName = "Robert", LastName = "De Niro", CreatedDate = seedDate, UpdatedDate = seedDate },
                new Actor { Id = 5, FirstName = "Christian", LastName = "Bale", CreatedDate = seedDate, UpdatedDate = seedDate },
                new Actor { Id = 6, FirstName = "Scarlett", LastName = "Johansson", CreatedDate = seedDate, UpdatedDate = seedDate }
            );

            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Title = "Pulp Fiction", Description = "Crime stories intertwine in LA.", Year = 1994, DurationMinutes = 154, GenreId = 4, DirectorId = 1, CreatedDate = seedDate, UpdatedDate = seedDate },
                new Movie { Id = 2, Title = "Inception", Description = "A thief steals secrets through dreams.", Year = 2010, DurationMinutes = 148, GenreId = 1, DirectorId = 2, CreatedDate = seedDate, UpdatedDate = seedDate },
                new Movie { Id = 3, Title = "The Wolf of Wall Street", Description = "A stockbroker's rise and fall.", Year = 2013, DurationMinutes = 180, GenreId = 3, DirectorId = 3, CreatedDate = seedDate, UpdatedDate = seedDate },
                new Movie { Id = 4, Title = "Kill Bill", Description = "A bride seeks revenge.", Year = 2003, DurationMinutes = 111, GenreId = 1, DirectorId = 1, CreatedDate = seedDate, UpdatedDate = seedDate },
                new Movie { Id = 5, Title = "The Dark Knight", Description = "Batman faces the Joker.", Year = 2008, DurationMinutes = 152, GenreId = 1, DirectorId = 2, CreatedDate = seedDate, UpdatedDate = seedDate },
                new Movie { Id = 6, Title = "Goodfellas", Description = "The rise of a mob associate.", Year = 1990, DurationMinutes = 145, GenreId = 4, DirectorId = 3, CreatedDate = seedDate, UpdatedDate = seedDate },
                new Movie { Id = 7, Title = "Unnamed Indie Comedy", Description = "A low-budget comedy with no credited director.", Year = 2015, DurationMinutes = 95, GenreId = 2, DirectorId = null, CreatedDate = seedDate, UpdatedDate = seedDate },
                new Movie { Id = 8, Title = "Ensemble Drama", Description = "Several lives intersect over one summer.", Year = 2019, DurationMinutes = 130, GenreId = 3, DirectorId = 2, CreatedDate = seedDate, UpdatedDate = seedDate }
            );

            // Join table rows — named columns MovieId / ActorId, matching the
            // UsingEntity config. Movie 1 and Movie 8 get 3 actors each,
            // Movie 7 gets none (it stays out of this list entirely).
            modelBuilder.Entity("MovieActor").HasData(
                new { MovieId = 1, ActorId = 1 },
                new { MovieId = 1, ActorId = 3 },
                new { MovieId = 1, ActorId = 4 },
                new { MovieId = 2, ActorId = 5 },
                new { MovieId = 2, ActorId = 6 },
                new { MovieId = 3, ActorId = 2 },
                new { MovieId = 3, ActorId = 4 },
                new { MovieId = 4, ActorId = 1 },
                new { MovieId = 5, ActorId = 5 },
                new { MovieId = 6, ActorId = 4 },
                new { MovieId = 6, ActorId = 3 },
                new { MovieId = 8, ActorId = 2 },
                new { MovieId = 8, ActorId = 5 },
                new { MovieId = 8, ActorId = 6 }
            );
        }

    }
}
