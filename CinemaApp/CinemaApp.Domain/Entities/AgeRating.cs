using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class AgeRating
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string MinAge { get; set; }
    }
}
