namespace CinemaApp.Application.Dtos.HallDtos
{
    public class GetHallDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public TimeSpan BreakDuration { get; set; }
    }
}