using AutoMapper;
using CinemaApp.API.Models.Requests;
using CinemaApp.API.Models.Responses;
using CinemaApp.Application.Dtos.MovieDtos;
using CinemaApp.Application.Interfaces;
using CinemaApp.Application.Services;
using CinemaApp.Domain.Criterias;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CinemaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private IMapper _mapper;
        private IMovieService _movieService;
        private IWebHostEnvironment _environment;
        private IConfiguration _configuration;
        private IPosterService _posterService;

        public MovieController(IMovieService movieService,
                               IMapper mapper,
                               IWebHostEnvironment environment,
                               IPosterService posterService,
                               IConfiguration configuration)
        {
            _movieService = movieService;
            _mapper = mapper;
            _environment = environment;
            _posterService = posterService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<GetMoviesResponse> GetAll()
        {
            return new GetMoviesResponse()
            {
                Movies = await _movieService.GetAllMovies()
            };
        }

        [HttpGet("{id}")]
        public async Task<GetMovieDto> Get(int id)
        {
            return await _movieService.GetMovieById(id);
        }

        [HttpGet("{id}/Sessions")]
        public async Task<MovieWithSessionsDto> GetMovieSessions(int id)
        {
            var movie = await _movieService.GetMovieSessions(id);
            return _mapper.Map<MovieWithSessionsDto>(movie);
        }

        [HttpGet("Search")]
        public async Task<List<GetMovieDto>> Search([FromQuery] MovieSearchCriteria criteria)
        {
            return (await _movieService.Search(criteria)).Select(_mapper.Map<GetMovieDto>).ToList();
        }

        [HttpPost]
        public async Task<MovieResponse> Post([FromBody] CreateMovieRequest request)
        {
            var movieDto = _mapper.Map<CreateMovieDto>(request);
            var result = await _movieService.AddMovie(movieDto);
            return new MovieResponse()
            {
                Id = result.Id,
                Title = result.Title,
            };
        }

        [HttpPut]
        public async Task<MovieResponse> Put(int id, [FromBody] UpdateMovieRequest request)
        {
            var movieDto = _mapper.Map<UpdateMovieDto>(request);
            var result = await _movieService.UpdateMovie(movieDto);
            return new MovieResponse()
            {
                Id = result.Id,
                Title = result.Title,
            };
        }

        [HttpPost("{id}/Poster")]
        [Consumes("multipart/form-data")]
        public async Task<string> UploadPoster(int id, IFormFile file)
        {
            var baseUrl = _configuration["BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
            return await _posterService.UploadPoster(id, file, _configuration["FileStorage:Path"] ?? "Uploads/Posters", baseUrl);
        }

        [HttpGet("{id}/Poster")]
        public async Task<IActionResult> GetPoster(int id)
        {
            var uploadsPath = _configuration["FileStorage:Path"] ?? "Uploads/Posters";
            if (!Path.IsPathRooted(uploadsPath))
            {
                uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), uploadsPath);
            }

            var (bytes, contentType, fileName) = await _posterService.GetPoster(id, uploadsPath);

            return File(bytes, contentType, fileName);
        }
    }
}
