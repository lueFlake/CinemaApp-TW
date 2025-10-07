using CinemaApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Domain.Interfaces.Repositories;
using CinemaApp.Domain.Criterias;

namespace CinemaApp.Infrastructure.Data.Repositories
{
    internal class MovieSessionRepository : BaseRepository<MovieSession>, IMovieSessionRepository
    {
        public MovieSessionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> HasOverlappingSessions(int hallId, DateTime startTime, TimeSpan duration, TimeSpan breakDuration)
        {
            var endTime = startTime + duration + breakDuration;

            return await dbSet
                .Where(s => s.Id == hallId &&
                           s.IsActive &&
                           s.StartTime < endTime &&
                           s.EndTime > startTime)
                .AnyAsync();
        }

        public async Task<IEnumerable<Movie>> GetActiveSessionsGroupedByMovieAsync(SessionSearchCriteria criteria)
        {
            var query = context.MovieSessions
                .Include(s => s.Movie)
                    .ThenInclude(m => m.Genres)
                .Include(s => s.Hall)
                .Where(s => s.IsActive && s.StartTime >= DateTime.Now)
                .AsQueryable();

            // Фильтрация по дате/интервалу
            if (criteria.Date.HasValue)
            {
                var date = criteria.Date.Value.Date;
                query = query.Where(s => s.StartTime.Date == date);
            }
            else if (criteria.StartDate.HasValue && criteria.EndDate.HasValue)
            {
                query = query.Where(s => s.StartTime >= criteria.StartDate.Value &&
                                        s.StartTime <= criteria.EndDate.Value);
            }

            // Фильтрация по жанрам
            if (criteria.GenreIds != null && criteria.GenreIds.Any())
            {
                query = query.Where(s => s.Movie.Genres.Any(g => criteria.GenreIds.Contains(g.Id)));
            }

            // Фильтрация по году выпуска фильма
            if (criteria.YearFrom.HasValue)
            {
                query = query.Where(s => s.Movie.Year >= criteria.YearFrom.Value);
            }

            if (criteria.YearTo.HasValue)
            {
                query = query.Where(s => s.Movie.Year <= criteria.YearTo.Value);
            }

            // Фильтрация по возрастному ограничению
            if (criteria.AgeRating != null)
            {
                query = query.Where(s => s.Movie.AgeRating == criteria.AgeRating);
            }

            // Фильтрация по продолжительности фильма
            if (criteria.MaxDuration.HasValue)
            {
                query = query.Where(s => s.Movie.Duration <= criteria.MaxDuration.Value);
            }

            // Фильтрация по цене (учитывая скидки)
            if (criteria.MaxPrice.HasValue || criteria.MinPrice.HasValue)
            {
                query = criteria.MaxPrice.HasValue && criteria.MinPrice.HasValue
                    ? query.Where(s => s.Price >= criteria.MinPrice.Value &&
                                      s.Price <= criteria.MaxPrice.Value)
                    : criteria.MaxPrice.HasValue
                        ? query.Where(s => s.Price <= criteria.MaxPrice.Value)
                        : query.Where(s => s.Price >= criteria.MinPrice!.Value);
            }

            // Фильтрация по залам
            if (criteria.HallIds != null && criteria.HallIds.Any())
            {
                query = query.Where(s => criteria.HallIds.Contains(s.HallId));
            }

            // Группируем по фильмам и получаем фильмы с их сеансами
            var movieIds = await query
                .OrderBy(s => s.StartTime)
                .Select(s => s.MovieId)
                .Distinct()
                .ToListAsync();

            // Получаем фильмы с их сеансами, отсортированными по времени
            var movies = await context.Movies
                .Include(m => m.Sessions
                    .Where(s => s.IsActive && s.StartTime >= DateTime.Now)
                    .OrderBy(s => s.StartTime))
                .ThenInclude(s => s.Hall)
                .Include(m => m.Genres)
                .Where(m => movieIds.Contains(m.Id))
                .ToListAsync();

            // Сортируем фильмы по времени первого сеанса
            return movies
                .OrderBy(m => m.Sessions
                    .Where(s => s.IsActive && s.StartTime >= DateTime.Now)
                    .Min(s => (DateTime?)s.StartTime))
                .ToList();
        }

        public async Task UpdateClosestActiveScreeningAsync(int movieId)
        {
            var closest = await context.MovieSessions
                .Where(s => s.MovieId == movieId &&
                           s.IsActive && s.StartTime >= DateTime.UtcNow)
                .OrderBy(s => s.StartTime)
                .FirstOrDefaultAsync();

            var movie = await context.Movies.FindAsync(movieId);
            movie.ClosestSession = closest;

            context.Movies.Update(movie);
        }
    }
}
