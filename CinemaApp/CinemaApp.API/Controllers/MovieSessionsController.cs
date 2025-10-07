using AutoMapper;
using CinemaApp.API.Models.Requests;
using CinemaApp.API.Models.Responses;
using CinemaApp.Application.Dtos.MovieDtos;
using CinemaApp.Application.Dtos.MovieSessionDtos;
using CinemaApp.Application.Interfaces;
using CinemaApp.Domain.Criterias;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces.Repositories;
using CinemaApp.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieSessionsController : Controller
    {
        private IMovieSessionService _movieSessionService;
        private IMapper _mapper;

        public MovieSessionsController(IMovieSessionService movieSessionRepository, IMapper mapper)
        {
            _movieSessionService = movieSessionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<GetSessionsResponse> GetAll()
        {
            return new GetSessionsResponse()
            {
                Sessions = await _movieSessionService.GetAllSessions()
            };
        }

        [HttpGet("{id}")]
        public async Task<GetSessionDto> Get(int id)
        {
            return await _movieSessionService.GetSessionById(id);
        }

        [HttpGet("Search")]
        public async Task<List<MovieWithSessionsDto>> Search([FromQuery] SessionSearchCriteria criteria)
        {
            return await _movieSessionService.Search(criteria);
        }

        [HttpPost]
        public async Task<SessionResponse> Post([FromBody] CreateSessionRequest request)
        {
            var movieSessionDto = _mapper.Map<CreateSessionDto>(request);
            var result = await _movieSessionService.AddSession(movieSessionDto);
            return new SessionResponse()
            {
                Id = result.Id
            };
        }
    }
}
