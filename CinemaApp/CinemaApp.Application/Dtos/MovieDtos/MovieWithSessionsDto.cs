using CinemaApp.Application.Dtos.MovieSessionDtos;
using CinemaApp.Domain.Entities;

namespace CinemaApp.Application.Dtos.MovieDtos
{
    public class MovieWithSessionsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<GetSessionDto> Sessions { get; set; }
    }
}
