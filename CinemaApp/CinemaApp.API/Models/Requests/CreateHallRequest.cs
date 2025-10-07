namespace CinemaApp.API.Models.Requests
{
    public class CreateHallRequest
    {
        public string Name { get; set; }
        public int Seats { get; set; }
        public TimeSpan BreakDuration { get; set; }
    }
}