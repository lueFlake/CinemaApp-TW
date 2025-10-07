using CinemaApp.Application.Dtos.HallDtos;

namespace CinemaApp.API.Models.Responses
{
    public class GetHallsResponse
    {
        public GetHallsResponse()
        {
        }

        public IEnumerable<GetHallDto> Halls { get; set; }
    }
}