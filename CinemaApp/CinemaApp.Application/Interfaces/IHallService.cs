using CinemaApp.Application.Dtos.HallDtos;
using CinemaApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Interfaces
{
    public interface IHallService
    {
        Task<Hall> AddHall(CreateHallDto movie);
        Task<IEnumerable<GetHallDto>> GetAllHalls();
        Task<GetHallDto> GetHallById(int id);
    }
}
