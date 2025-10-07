using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Dtos.MovieDtos
{
    public class CreateMovieDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string AgeRating { get; set; }
        public string[] Genres { get; set; } = Array.Empty<string>();
    }
}
