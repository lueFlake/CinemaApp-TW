using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Infrastructure.Data.Repositories
{
    public class AgeRatingRepository : IAgeRatingRepository

    {
        private ApplicationDbContext _context;
        private DbSet<AgeRating> _dbSet;

        public AgeRatingRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<AgeRating>();
        }

        public async Task<AgeRating> GetByNameAsync(string name) => await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }
}
