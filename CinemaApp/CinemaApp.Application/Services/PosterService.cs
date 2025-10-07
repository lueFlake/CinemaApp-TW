using CinemaApp.Application.Interfaces;
using CinemaApp.Domain.Exceptions;
using CinemaApp.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;

namespace CinemaApp.Application.Services
{
    public class PosterService : IPosterService
    {
        private IMovieRepository _movieRepository;

        public PosterService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<(byte[], string, string)> GetPoster(int id, string rootPath)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            ValidationException.ThrowIf(movie == null || string.IsNullOrEmpty(movie.PosterPath), "Poster not found");

            var fileName = Path.GetFileName(movie.PosterPath);
            var filePath = Path.Combine(rootPath, fileName);

            ValidationException.ThrowIf(!System.IO.File.Exists(filePath), "Poster file not found");

            var contentType = GetContentType(fileName);
            return (await System.IO.File.ReadAllBytesAsync(filePath), contentType, fileName);
        }

        public async Task<string> UploadPoster(int id, IFormFile file, string rootPath, string baseUrl)
        {
            ValidationException.ThrowIf(file == null || file.Length == 0, "No file uploaded");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            ValidationException.ThrowIf(!allowedExtensions.Contains(fileExtension),
                "Invalid file format. Allowed: JPG, JPEG, PNG, WEBP");

            ValidationException.ThrowIf(file.Length > 5 * 1024 * 1024, "File size exceeds 5MB limit");

            var movie = await _movieRepository.GetByIdAsync(id);
            ValidationException.ThrowIf(movie == null, $"Movie with id {id} not found");

            var uploadsPath = rootPath;
            if (!Path.IsPathRooted(uploadsPath))
            {
                uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), uploadsPath);
            }

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var posterUrl = $"{baseUrl}/api/Movie/{id}/Poster";

            movie.PosterPath = filePath;
            _movieRepository.Update(movie);
            await _movieRepository.SaveAsync();

            return posterUrl;
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }
    }
}
