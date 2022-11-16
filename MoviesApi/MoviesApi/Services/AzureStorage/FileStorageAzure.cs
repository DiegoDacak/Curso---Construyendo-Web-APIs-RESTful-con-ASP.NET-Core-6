using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using MoviesApi.Services.ServicesInterface;

namespace MoviesApi.Services.AzureStorage
{
    public class FileStorageAzure : IFileStorage
    {
        private readonly string _connectionString;

        public FileStorageAzure(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureStorage");
        }
        
        public async Task<string> SaveArchive(byte[] content, string extension, string container, string contentType)
        {
            var client = new BlobContainerClient(_connectionString, container);
            // If not exist going to create
            await client.CreateIfNotExistsAsync();
            await client.SetAccessPolicyAsync(PublicAccessType.Blob);
            
            // Generate archive name in an aleatory manner
            var archiveName = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(archiveName);
            
            // We going to specify the type of the archive
            var blobUploadOptions = new BlobUploadOptions();
            // Pass the content type
            var blobHttpHeader = new BlobHttpHeaders
            {
                ContentType = contentType
            };
            // Add the content type previous created
            blobUploadOptions.HttpHeaders = blobHttpHeader;
            
            // Upload to azure
            await blob.UploadAsync(new BinaryData(content), blobUploadOptions);

            return blob.Uri.ToString();
        }

        public async Task<string> EditArchive(byte[] content, string extension, string container, 
            string urlRute, string contentType)
        {
            await DeleteArchive(urlRute, container);
            return await SaveArchive(content, extension, container, contentType);
        }

        public async Task DeleteArchive(string urlRute, string container)
        {
            if (string.IsNullOrEmpty(urlRute))
            {
                return;
            }

            var client = new BlobContainerClient(_connectionString, container);
            await client.CreateIfNotExistsAsync();
            var archive = Path.GetFileName(urlRute);
            var blob = client.GetBlobClient(archive);
            await blob.DeleteIfExistsAsync();
        }
    }
}