using System.Threading.Tasks;

namespace MoviesApi.Services.ServicesInterface
{
    public interface IFileStorage
    {
        Task<string> SaveFile(byte[] content, string extension, string container, string contentType);

        Task<string> EditFile(byte[] content, string extension, string container, string urlRute,
            string contentType);

        Task DeleteFile(string rute, string container);
    }
}