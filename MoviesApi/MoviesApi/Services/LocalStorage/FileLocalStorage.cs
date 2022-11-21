using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MoviesApi.Services.ServicesInterface;

namespace MoviesApi.Services.LocalStorage
{
    public class FileLocalStorage : IFileStorage
    {
        // We can get the route for us wwwroot
        private readonly IWebHostEnvironment _environment;
        
        // We can determine the domain for us api
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileLocalStorage(IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<string> SaveFile(byte[] content, string extension, string container, string contentType)
        {
            var fileName = $"{Guid.NewGuid()}{extension}";
            var folder = Path.Combine(_environment.WebRootPath, container);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var rute = Path.Combine(folder, fileName);
            await File.WriteAllBytesAsync(rute, content);

            if (_httpContextAccessor.HttpContext is null) return null;
            var actualUrl =
                $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            var bdUrl = Path.Combine(actualUrl, container, fileName).Replace("\\", "/");
            return bdUrl;
        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string urlRute, string contentType)
        {
            await DeleteFile(urlRute, container);
            return await SaveFile(content, extension, container, contentType);
        }

        public Task DeleteFile(string rute, string container)
        {
            if (rute is null) return Task.FromResult(0);
            var fileName = Path.GetFileName(rute);
            var fileDirectory = Path.Combine(_environment.WebRootPath, container, fileName);
            if (File.Exists(fileDirectory))
            {
                File.Delete(fileDirectory);
            }

            return Task.FromResult(0);
        }
    }
}