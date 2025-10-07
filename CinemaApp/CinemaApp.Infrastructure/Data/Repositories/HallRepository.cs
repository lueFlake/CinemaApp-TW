using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces.Repositories;

namespace CinemaApp.Infrastructure.Data.Repositories
{
    public class HallRepository : BaseRepository<Hall>, IHallRepository
    {
        public HallRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
