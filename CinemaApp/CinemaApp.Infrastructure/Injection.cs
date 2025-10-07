using CinemaApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CinemaApp.Domain.Interfaces.Repositories;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Infrastructure.Data.Repositories;

namespace CinemaApp.Infrastructure
{
    public static class Injection
    {
        public static void AddInfrastructure(this IServiceCollection provider, IConfiguration config)
        {
            provider.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            provider.AddScoped<IMovieRepository, MovieRepository>();
            provider.AddScoped<IAgeRatingRepository, AgeRatingRepository>();
            provider.AddScoped<IGenreRepository, GenreRepository>();
            provider.AddScoped<IMovieSessionRepository, MovieSessionRepository>();
            provider.AddScoped<IHallRepository, HallRepository>();
        } 

        public static async void DbHealthCheckAsync(this IServiceProvider provider)
        {
            try
            {
                using var scope = provider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var canConnect = await dbContext.Database.CanConnectAsync();

                if (!canConnect)
                {
                    throw new Exception("Cannot connect to database");
                }

                Console.WriteLine("✅ Health Check: Database connection - OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Health Check Failed: {ex.Message}");
            }
        }
    }
}
