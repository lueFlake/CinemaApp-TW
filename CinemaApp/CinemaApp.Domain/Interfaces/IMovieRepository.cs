
using CinemaApp.Domain.Criterias;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Domain.Interfaces.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task WithdrawMovie(int id);
        Task<Movie?> GetMovieSessions(int id);
        Task<IEnumerable<Movie>> SearchAsync(MovieSearchCriteria criteria);
    }
}