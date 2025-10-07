namespace CinemaApp.API.Models.Requests
{
    public class CreateSessionRequest
    {
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTime StartTime { get; set; }
        public int Price { get; set; }
    }
}