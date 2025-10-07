using AutoMapper;
using CinemaApp.Application.Dtos.HallDtos;
using CinemaApp.Application.Interfaces;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Interfaces.Repositories;

namespace CinemaApp.Application.Services
{
    public class HallService : IHallService
    {
        private IHallRepository _hallRepository;
        private IMapper _mapper;

        public HallService(IHallRepository hallRepository, IMapper mapper)
        {
            _hallRepository = hallRepository;
            _mapper = mapper;
        }

        public async Task<Hall> AddHall(CreateHallDto hallDto)
        {
            var hall = _mapper.Map<Hall>(hallDto);
            await _hallRepository.AddAsync(hall);
            await _hallRepository.SaveAsync();
            return hall;
        }

        public async Task<IEnumerable<GetHallDto>> GetAllHalls()
        {
            return (await _hallRepository.GetAllAsync()).Select(_mapper.Map<GetHallDto>);
        }

        public async Task<GetHallDto> GetHallById(int id)
        {
            return _mapper.Map<GetHallDto>(await _hallRepository.GetByIdAsync(id));
        }
    }
}
