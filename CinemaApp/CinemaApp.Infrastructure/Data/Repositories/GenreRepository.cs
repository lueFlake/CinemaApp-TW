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
    public class GenreRepository : IGenreRepository
    {
        private ApplicationDbContext _context;
        private DbSet<Genre> _dbSet;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Genre>();
        }

        public async Task<Genre> GetByNameAsync(string name) => await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }
}
