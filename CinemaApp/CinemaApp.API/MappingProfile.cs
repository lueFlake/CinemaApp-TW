using AutoMapper;
using CinemaApp.API.Models.Requests;
using CinemaApp.Application.Dtos.HallDtos;
using CinemaApp.Application.Dtos.MovieDtos;
using CinemaApp.Application.Dtos.MovieSessionDtos;
using CinemaApp.Domain.Criterias;
using CinemaApp.Domain.Entities;

namespace CinemaApp.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateMovieRequest, CreateMovieDto>();
            CreateMap<UpdateMovieRequest, UpdateMovieDto>();
            CreateMap<Movie, GetMovieDto>()
                .ForMember(dest => dest.AgeRating, opt => opt.MapFrom(src => src.AgeRating.Name))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToArray()))
                .ForMember(dest => dest.ClosestSessionDate,
                    opt =>
                    {
                        opt.PreCondition(src => src.ClosestSession != null);
                        opt.MapFrom(src => src.ClosestSession!.StartTime);
                    })
                .ForMember(dest => dest.ClosestSessionPrice,
                    opt =>
                    {
                        opt.PreCondition(src => src.ClosestSession != null);
                        opt.MapFrom(src => src.ClosestSession!.Price);
                    })
                .ForMember(dest => dest.ClosestSessionHallId,
                    opt =>
                    {
                        opt.PreCondition(src => src.ClosestSession != null);
                        opt.MapFrom(src => src.ClosestSession!.HallId);
                    });
            CreateMap<Movie, MovieWithSessionsDto>();

            CreateMap<MovieSearchRequest, MovieSearchCriteria>()
                .ForMember(dest => dest.GenreIds, opt =>
                    opt.MapFrom(src => src.GenreIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()))
                .ForMember(dest => dest.AgeRating, opt => opt.MapFrom(src => src.AgeRatingId == null ? null : new AgeRating() { Id = (int)src.AgeRatingId }));

            CreateMap<CreateHallRequest, CreateHallDto>();
            CreateMap<CreateHallDto, Hall>();
            CreateMap<Hall, GetHallDto>();

            CreateMap<CreateSessionRequest, CreateSessionDto>();
            CreateMap<CreateSessionDto, MovieSession>();
            CreateMap<MovieSession, GetSessionDto>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive && src.StartTime >= DateTime.Now));

        }
    }
}
