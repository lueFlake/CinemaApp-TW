using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Entities
{
    public class Genre
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
