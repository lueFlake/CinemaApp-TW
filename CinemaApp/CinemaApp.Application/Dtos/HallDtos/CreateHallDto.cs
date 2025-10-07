namespace CinemaApp.Application.Dtos.HallDtos
{
    public class CreateHallDto
    {
        public string Name { get; set; }
        public int Seats { get; set; }
        public TimeSpan BreakDuration { get; set; }
    }
}