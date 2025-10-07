using CinemaApp.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.API.Models.Requests
{
    public class CreateMovieRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public TimeSpan Duration { get; set; }
        [Required]
        public string AgeRating { get; set; }
        public string[] Genres { get; set; } = Array.Empty<string>();
    }
}
