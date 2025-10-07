using CinemaApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.API.Models.Requests
{
    public class UpdateMovieRequest
    {
        [Required]
        public int Id { get; set; }
        public string[] Genres { get; set; } = Array.Empty<string>();
        public string? AgeRating { get; set; }
        public string? Title { get; set; }
        public int? Year { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool? IsWithdrawn { get; set; }
    }
}