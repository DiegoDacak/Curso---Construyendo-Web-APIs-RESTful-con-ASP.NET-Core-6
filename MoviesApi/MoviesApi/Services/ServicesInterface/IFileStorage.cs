using System.Threading.Tasks;

namespace MoviesApi.Services.ServicesInterface
{
    public interface IFileStorage
    {
        Task<string> SaveArchive(byte[] content, string extension, string container, string contentType);

        Task<string> EditArchive(byte[] content, string extension, string container, string urlRute,
            string contentType);

        Task DeleteArchive(string rute, string container);
    }
}