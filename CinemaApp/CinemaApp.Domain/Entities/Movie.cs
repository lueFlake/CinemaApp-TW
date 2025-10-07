using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public TimeSpan Duration { get; set; }
        public AgeRating AgeRating { get; set; }
        public string? PosterPath { get; set; }
        public bool IsWithdrawn { get; set; }
        public ICollection<Genre> Genres { 
            get; 
            set; 
        } = new List<Genre>();
        public ICollection<MovieSession> Sessions { get; set; } = new List<MovieSession>();
        public MovieSession? ClosestSession { get; set; }
        public int? ClosestSessionId { get; set; }
    }
}
