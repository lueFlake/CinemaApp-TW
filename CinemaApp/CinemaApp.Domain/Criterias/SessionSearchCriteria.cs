using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Criterias
{
    public class SessionSearchCriteria
    {
        public DateTime? Date { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<int> GenreIds { get; set; } = new();
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }
        public AgeRating? AgeRating { get; set; }
        public TimeSpan? MaxDuration { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<int> HallIds { get; set; } = new();
    }
}
