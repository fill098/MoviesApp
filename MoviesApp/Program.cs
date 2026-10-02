using Microsoft.EntityFrameworkCore;
using MoviesApp.DateAccess.Data;
using MoviesApp.DateAccess.Implementaions;
using MoviesApp.DateAccess.Interfaces;
using MoviesApp.Services.Implemetations;
using MoviesApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MoviesAppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MoviesAppDb")));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IDirectorRepository, DirectorRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();


builder.Services.AddScoped<IMovieService, MovieService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
