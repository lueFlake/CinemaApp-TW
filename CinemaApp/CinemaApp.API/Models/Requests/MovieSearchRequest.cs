using CinemaApp.Domain.Entities;

namespace CinemaApp.API.Models.Requests
{
    public class MovieSearchRequest
    {
        public string? Title { get; set; }
        public string? GenreIds { get; set; }
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public int? AgeRatingId { get; set; }
        public TimeSpan? MaxDuration { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
