using Azure.Storage.Blobs;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Task8FunctionApp.Helpers
{
    public class BlobHelper
    {
        public static async Task UploadToBlobAsync(string blobName, string jsonContent)
        {
            string containerName = Environment.GetEnvironmentVariable("ContainerName");
            string connectionString = Environment.GetEnvironmentVariable("StorageAccConnectionString");
            blobName = "processed_" + blobName;
            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(blobName);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));
            await blobClient.UploadAsync(stream, true);
        }
    }
}
