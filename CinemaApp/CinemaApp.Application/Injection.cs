using CinemaApp.Application.Interfaces;
using CinemaApp.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application
{
    public static class Injection
    {
        public static void AddApplication(this IServiceCollection provider)
        {
            provider.AddScoped<IMovieService, MovieService>();
            provider.AddScoped<IHallService, HallService>();
            provider.AddScoped<IMovieSessionService, MovieSessionService>();
            provider.AddTransient<OpenHoursService>();
            provider.AddScoped<IPosterService, PosterService>();
        }
    }
}
