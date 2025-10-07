using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Dtos.MovieDtos
{
    public class GetMovieDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string AgeRating { get; set; }
        public string[] Genres { get; set; } = new string[0];
        public bool IsWithdrawn { get; set; }
        public DateTime? ClosestSessionDate { get; set; }
        public int? ClosestSessionPrice { get; set; }
        public int? ClosestSessionHallId { get; set; }
    }
}
