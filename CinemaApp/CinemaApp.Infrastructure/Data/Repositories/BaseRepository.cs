using CinemaApp.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Infrastructure.Data.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext context;
        protected DbSet<T> dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public virtual async Task<T> GetByIdAsync(int id) => await dbSet.FindAsync(id);

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await dbSet.ToListAsync();

        public virtual async Task AddAsync(T entity) => await dbSet.AddAsync(entity);

        public virtual void Update(T entity) => dbSet.Update(entity);

        public virtual void Delete(T entity) => dbSet.Remove(entity);

        public virtual async Task SaveAsync() => await context.SaveChangesAsync();
    }
}
