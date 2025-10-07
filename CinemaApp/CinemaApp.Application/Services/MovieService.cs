using AutoMapper;
using CinemaApp.Application.Dtos.MovieDtos;
using CinemaApp.Application.Interfaces;
using CinemaApp.Domain.Criterias;
using CinemaApp.Domain.Entities;
using CinemaApp.Domain.Exceptions;
using CinemaApp.Domain.Interfaces;
using CinemaApp.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Services
{
    public class MovieService : IMovieService
    {
        private IMapper _mapper;
        private IMovieRepository _movieRepository;
        private IAgeRatingRepository _ageRatingRepository;
        private IGenreRepository _genreRepository;

        public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository, IAgeRatingRepository ageRatingRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _ageRatingRepository = ageRatingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetMovieDto>> GetAllMovies()
        {
            var movies = await _movieRepository.GetAllAsync();
            return movies.Select(_mapper.Map<GetMovieDto>);
        }

        public async Task<GetMovieDto> GetMovieById(int id) 
        {
            var movie = _mapper.Map<GetMovieDto>(await _movieRepository.GetByIdAsync(id));
            ValidationException.ThrowIf(movie is null, $"Movie with id {id} is not found.");
            
            return movie;
        }

        public async Task<Movie> GetMovieSessions(int id)
        {
            var dbMovie = await _movieRepository.GetByIdAsync(id);
            ValidationException.ThrowIf(dbMovie is null, $"Movie with id {id} is not found.");
            return (await _movieRepository.GetMovieSessions(id))!;
        }

        public async Task<IEnumerable<Movie>> Search(MovieSearchCriteria criteria)
        {
            return (await _movieRepository.SearchAsync(criteria)).ToList();
        }

        public async Task<Movie> UpdateMovie(UpdateMovieDto movieDto)
        {
            var dbMovie = await _movieRepository.GetByIdAsync(movieDto.Id);
            ValidationException.ThrowIf(dbMovie is null, $"Movie with id {movieDto.Id} is not found.");

            var newGenres = movieDto.Genres is null ? dbMovie.Genres : await GetValidGenres(movieDto.Genres);

            var newAgeRating = movieDto.AgeRating is null ? dbMovie.AgeRating : await GetValidAgeRating(movieDto.AgeRating);

            var result = new Movie()
            {
                Title = movieDto.Title ?? dbMovie.Title,
                Year = movieDto.Year ?? dbMovie.Year,
                Duration = movieDto.Duration ?? dbMovie.Duration,
                AgeRating = newAgeRating,
                Genres = newGenres,
            };
            
            await WithdrawMovieIfNeeded(result);
            result.IsWithdrawn = movieDto.IsWithdrawn ?? dbMovie.IsWithdrawn;

            _movieRepository.Update(dbMovie);
            await _movieRepository.SaveAsync();
            return result;
        }

        public async Task WithdrawMovieIfNeeded(Movie movie)
        {
            if (movie.IsWithdrawn)
            {
                return;
            }
            await _movieRepository.WithdrawMovie(movie.Id);
        }

        public async Task<Movie> AddMovie(CreateMovieDto movieDto)
        {
            var dbGenres = await GetValidGenres(movieDto.Genres);

            var dbAgeRating = await GetValidAgeRating(movieDto.AgeRating);

            var result = new Movie()
            {
                Title = movieDto.Title!,
                Year = movieDto.Year,
                Duration = movieDto.Duration,
                AgeRating = dbAgeRating,
                Genres = dbGenres,
                IsWithdrawn = false
            };
            await _movieRepository.AddAsync(result);
            await _movieRepository.SaveAsync();
            return result;
        }

        private async Task<List<Genre>> GetValidGenres(string[] inputGenres)
        {
            var dbGenres = new List<Genre>(inputGenres.Length + 1);
            foreach (var g in inputGenres)
            {
                dbGenres.Add(await _genreRepository.GetByNameAsync(g));
            }

            var invalidGenres = inputGenres.Except(dbGenres.Select(x => x?.Name));
            ValidationException.ThrowIf(invalidGenres.Any(), $"Invalid genres: {string.Join(", ", invalidGenres)}");

            return dbGenres;
        }

        private async Task<AgeRating?> GetValidAgeRating(string inputAgeRating)
        {
            var dbAgeRating = await _ageRatingRepository.GetByNameAsync(inputAgeRating);
            ValidationException.ThrowIf(dbAgeRating is null, $"Invalid age rating: {inputAgeRating}");

            return dbAgeRating;
        }
    }
}
