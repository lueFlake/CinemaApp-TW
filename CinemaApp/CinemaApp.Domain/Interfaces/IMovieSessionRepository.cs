
using CinemaApp.Domain.Criterias;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Domain.Interfaces.Repositories
{
    public interface IMovieSessionRepository : IRepository<MovieSession>
    {
        Task<bool> HasOverlappingSessions(int hallId, DateTime startTime, TimeSpan duration, TimeSpan breakDuration);
        Task<IEnumerable<Movie>> GetActiveSessionsGroupedByMovieAsync(SessionSearchCriteria criteria);
    }
}