namespace CinemaApp.Domain.Entities
{
    public class MovieSession
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public int MovieId { get; set; }
        public Hall Hall { get; set; }
        public int HallId { get; set; }
        public DateTime StartTime { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime EndTime { get; set; }
    }
}
