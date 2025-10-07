using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AgeRating> AgeRatings { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieSession> MovieSessions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.HasOne(m => m.ClosestSession)
                      .WithMany()
                      .HasForeignKey(m => m.ClosestSessionId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Navigation(s => s.AgeRating).AutoInclude();
                entity.Navigation(s => s.Genres).AutoInclude();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
