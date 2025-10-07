using AutoMapper;
using CinemaApp.API.Models.Requests;
using CinemaApp.API.Models.Responses;
using CinemaApp.Application.Dtos.HallDtos;
using CinemaApp.Application.Interfaces;
using CinemaApp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace CinemaApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallController : ControllerBase
    {
        private IMapper _mapper;
        private IHallService _hallService;

        public HallController(IHallService hallService, IMapper mapper)
        {
            _hallService = hallService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<GetHallsResponse> GetAll()
        {
            return new GetHallsResponse()
            {
                Halls = await _hallService.GetAllHalls()
            };
        }

        [HttpGet("{id}")]
        public async Task<GetHallDto> Get(int id)
        {
            return await _hallService.GetHallById(id);
        }

        [HttpPost]
        public async Task<HallResponse> Post([FromBody] CreateHallRequest request)
        {
            var hallDto = _mapper.Map<CreateHallDto>(request);
            var result = await _hallService.AddHall(hallDto);
            return new HallResponse()
            {
                Id = result.Id,
                Name = result.Name
            };
        }
    }
}
