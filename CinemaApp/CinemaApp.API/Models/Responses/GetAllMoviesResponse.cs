using CinemaApp.Application.Dtos.MovieDtos;

namespace CinemaApp.API.Models.Responses
{
    public class GetMoviesResponse
    {
        public IEnumerable<GetMovieDto> Movies { get; set; } = new List<GetMovieDto>();
    }
}
