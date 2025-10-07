using AutoMapper;
using CinemaApp.Application.Dtos.MovieDtos;
using CinemaApp.Application.Dtos.MovieSessionDtos;
using CinemaApp.Application.Interfaces;
using CinemaApp.Domain.Criterias;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Exceptions;
using CinemaApp.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Services
{
    public class MovieSessionService : IMovieSessionService
    {
        private IMovieRepository _movieRepository;
        private IHallRepository _hallRepository;
        private IMovieSessionRepository _movieSessionRepository;
        private IMapper _mapper;
        private OpenHoursService _openHoursService;

        public MovieSessionService(IMovieRepository movieRepository,
                                   IMovieSessionRepository movieSessionRepository,
                                   IMapper mapper,
                                   IHallRepository hallRepository,
                                   OpenHoursService openHoursService)
        {
            _movieRepository = movieRepository;
            _movieSessionRepository = movieSessionRepository;
            _mapper = mapper;
            _hallRepository = hallRepository;
            _openHoursService = openHoursService;
        }

        public async Task<MovieSession> AddSession(CreateSessionDto sessionDto)
        {
            var dbHall = await _hallRepository.GetByIdAsync(sessionDto.HallId);
            ValidationException.ThrowIf(dbHall is null, $"Invalid hall id: {sessionDto.HallId}");

            var dbMovie = await _movieRepository.GetByIdAsync(sessionDto.MovieId);
            ValidationException.ThrowIf(dbMovie is null, $"Invalid movie id: {sessionDto.HallId}");
            ValidationException.ThrowIf(dbMovie.IsWithdrawn, $"Movie is withdrawn: {sessionDto.MovieId}");

            ValidationException.ThrowIf(
                await _movieSessionRepository.HasOverlappingSessions(dbHall.Id, sessionDto.StartTime, dbMovie.Duration, dbHall.BreakDuration),
                "Has overlapping sessions");

            ValidationException.ThrowIf(sessionDto.StartTime >= DateTime.Now, $"Start time ({sessionDto.StartTime}) is more then current ({DateTime.Now})");

            var endTime = sessionDto.StartTime + dbMovie.Duration + dbHall.BreakDuration;
            ValidationException.ThrowIf(_openHoursService.IsWithinOpenHours(sessionDto.StartTime) &&
                                        _openHoursService.IsWithinOpenHours(endTime),
                                        "Not within open hours");

            var session = new MovieSession()
            {
                Movie = dbMovie,
                Hall = dbHall,
                StartTime = sessionDto.StartTime,
                Price = sessionDto.Price,
                IsActive = false,
                EndTime = endTime
            };
            await _movieSessionRepository.AddAsync(session);
            await _movieSessionRepository.SaveAsync();
            return session;
        }

        public async Task<IEnumerable<GetSessionDto>> GetAllSessions()
        {
            return (await _movieSessionRepository.GetAllAsync()).Select(_mapper.Map<GetSessionDto>);
        }

        public async Task<GetSessionDto> GetSessionById(int id)
        {
            return _mapper.Map<GetSessionDto>(await _movieSessionRepository.GetByIdAsync(id));
        }

        public async Task<List<MovieWithSessionsDto>> Search(SessionSearchCriteria criteria)
        {
            return (await _movieSessionRepository.GetActiveSessionsGroupedByMovieAsync(criteria)).Select(_mapper.Map<MovieWithSessionsDto>).ToList();
        }
    }
}
