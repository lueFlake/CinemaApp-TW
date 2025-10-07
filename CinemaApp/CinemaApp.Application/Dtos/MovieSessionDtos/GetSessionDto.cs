namespace CinemaApp.Application.Dtos.MovieSessionDtos
{
    public class GetSessionDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int HallId { get; set; }
        public DateTime StartTime { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime EndTime { get; set; }
    }
}