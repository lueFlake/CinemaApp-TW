using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public TimeSpan BreakDuration { get; set; }
    }
}
