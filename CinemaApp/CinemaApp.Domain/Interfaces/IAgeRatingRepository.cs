using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Domain.Interfaces
{
    public interface IAgeRatingRepository
    {
        public Task<AgeRating> GetByNameAsync(string name);
    }
}
