using CinemaApp.Domain.Entities;

namespace CinemaApp.Domain.Criterias
{
    public class MovieSearchCriteria
    {
        public string Title { get; set; }
        public List<int> GenreIds { get; set; } = new();
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public AgeRating? AgeRating { get; set; }
        public TimeSpan? MaxDuration { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
