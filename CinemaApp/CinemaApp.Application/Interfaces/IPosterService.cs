using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Application.Interfaces
{
    public interface IPosterService
    {
        Task<string> UploadPoster(int id, IFormFile file, string rootPath, string baseUrl);
        Task<(byte[], string, string)> GetPoster(int id, string rootPath);
    }
}
