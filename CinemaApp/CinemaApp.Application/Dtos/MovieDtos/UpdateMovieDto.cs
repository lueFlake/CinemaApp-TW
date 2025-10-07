namespace CinemaApp.Application.Dtos.MovieDtos
{
    public class UpdateMovieDto
    {
        public string[] Genres { get; set; } = Array.Empty<string>();
        public string? AgeRating { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Year { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool? IsWithdrawn { get; set; }
    }
}