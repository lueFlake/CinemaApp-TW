using CinemaApp.Application.Dtos.MovieDtos;
using CinemaApp.Domain.Criterias;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Interfaces
{
    public interface IMovieService
    {
        Task<Movie> AddMovie(CreateMovieDto movie);
        Task<IEnumerable<GetMovieDto>> GetAllMovies();
        Task<GetMovieDto> GetMovieById(int id);
        Task<Movie> GetMovieSessions(int id);
        Task<IEnumerable<Movie>> Search(MovieSearchCriteria criteria);
        Task<Movie> UpdateMovie(UpdateMovieDto movieDto);
    }
}