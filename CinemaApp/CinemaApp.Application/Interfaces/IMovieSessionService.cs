
using CinemaApp.Application.Dtos.HallDtos;
using CinemaApp.Application.Dtos.MovieDtos;
using CinemaApp.Application.Dtos.MovieSessionDtos;
using CinemaApp.Domain.Criterias;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Interfaces
{
    public interface IMovieSessionService
    {
        Task<MovieSession> AddSession(CreateSessionDto movie);
        Task<IEnumerable<GetSessionDto>> GetAllSessions();
        Task<GetSessionDto> GetSessionById(int id);
        Task<List<MovieWithSessionsDto>> Search(SessionSearchCriteria criteria);
    }
}