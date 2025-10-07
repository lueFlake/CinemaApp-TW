namespace CinemaApp.Application.Dtos.MovieSessionDtos
{
    public class CreateSessionDto
    {
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTime StartTime { get; set; }
        public int Price { get; set; }
    }
}