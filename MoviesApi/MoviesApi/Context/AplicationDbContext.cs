using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Entities;

namespace MoviesApi.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MovieActor>()
                .HasKey(x => new {x.ActorId, x.MovieId});

            modelBuilder.Entity<MovieGender>()
                .HasKey(x => new {x.GenderId, x.MovieId});

            modelBuilder.Entity<MoviesCinema>()
                .HasKey(x => new {x.CinemaId, x.MovieId});

            base.OnModelCreating(modelBuilder);
        }
        
        public DbSet<Gender> Genders { get;  set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActor> MoviesActors { get; set; }
        public DbSet<MovieGender> MoviesGenders { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<MoviesCinema> MoviesCinema { get; set; }
    }
}