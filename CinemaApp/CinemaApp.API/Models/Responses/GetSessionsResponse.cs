using CinemaApp.Application.Dtos.MovieSessionDtos;

namespace CinemaApp.API.Models.Responses
{
    public class GetSessionsResponse
    {
        public GetSessionsResponse()
        {
        }

        public IEnumerable<GetSessionDto> Sessions { get; set; }
    }
}