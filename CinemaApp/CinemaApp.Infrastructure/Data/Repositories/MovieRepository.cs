using CinemaApp.Domain.Criterias;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Infrastructure.Data.Repositories
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {

        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task WithdrawMovie(int id)
        {
            await context.MovieSessions
                .Where(s => s.Movie.Id == id && (!s.IsActive || s.StartTime < DateTime.Now))
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.IsActive, false));
        }

        public async Task<Movie?> GetMovieSessions(int id)
        {
            return await dbSet
                .Include(s => s.Sessions)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Movie>> SearchAsync(MovieSearchCriteria criteria)
        {
            var query = context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Sessions
                    .Where(s => s.IsActive && s.StartTime >= DateTime.UtcNow)
                    .OrderBy(s => s.StartTime)
                    .Take(1))
                .ThenInclude(s => s.Hall)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(criteria.Title))
            {
                query = query.Where(m => m.Title.ToLower().Contains(criteria.Title.ToLower()));
            }

            if (criteria.GenreIds != null && criteria.GenreIds.Any())
            {
                query = query.Where(m => m.Genres.Any(g => criteria.GenreIds.Contains(g.Id)));
            }

            if (criteria.YearFrom.HasValue)
            {
                query = query.Where(m => m.Year >= criteria.YearFrom.Value);
            }

            if (criteria.YearTo.HasValue)
            {
                query = query.Where(m => m.Year <= criteria.YearTo.Value);
            }

            if (criteria.AgeRating != null)
            {
                query = query.Where(m => m.AgeRating == criteria.AgeRating);
            }

            if (criteria.MaxDuration.HasValue)
            {
                query = query.Where(m => m.Duration <= criteria.MaxDuration.Value);
            }

            if (criteria.MaxPrice.HasValue)
            {
                query = query.Where(m => m.Sessions
                    .Any(s => s.IsActive && s.StartTime >= DateTime.UtcNow && s.Price <= criteria.MaxPrice.Value));
            }

            query = query.Where(m => !m.IsWithdrawn);

            return await query.ToListAsync();
        }
    }
}
