using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AZ204.BlobStorageApp
{
    class Program
    {
        private static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=storagejimbuss;AccountKey=89hx/wiDE0R0PSgIXkTAcsHTrITtK0MOJnBS9qY5NWU6/Iw9kNaXtx43Ks8JyUmsZjA691q1WTa8+ASt6aylsg==;EndpointSuffix=core.windows.net";

        private static readonly string blobContainerName = "authors";
        private static readonly string blobName = "jamie.html";

        private async static Task Main(string[] args)
        {
            try
            {
                await CreateContainerAndUploadBlobAsync();
                await ListContainersWithTheirBlobsAsync();
                await DownloadBlobAsync();
                Console.WriteLine();
                Console.WriteLine($"Press ENTER to delete blob container '{blobContainerName}'");
                Console.ReadLine();
                await DeleteContainerAsync();
            }
            catch(RequestFailedException exception)
            {
                Console.WriteLine($"Error: {exception.ErrorCode}");
            }
        }

        private static async Task CreateContainerAndUploadBlobAsync()
        {
            // 1. Create the Blob Container
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            Console.WriteLine($"1. Creating blob container '{blobContainerName}'");
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
            // 2. Upload a Blob
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            Console.WriteLine($"2. Uploading blob '{blobClient.Name}'");
            Console.WriteLine($"\t> {blobClient.Uri}");
            using FileStream fileStream = File.OpenRead("fileToUpload.html");
            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders {ContentType = "text/html"});
        }

        private static async Task ListContainersWithTheirBlobsAsync()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            Console.WriteLine("3. Listing containers and blobs "
                + $" of '{blobServiceClient.AccountName}' account");
            await foreach (BlobContainerItem blobContainerItem in blobServiceClient.GetBlobContainersAsync())
            {
                Console.WriteLine($"\t> {blobContainerItem.Name}");
                BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerItem.Name);
                await foreach (BlobItem blobItem in blobContainerClient.GetBlobsAsync())
                {
                    Console.WriteLine($"\t- {blobItem.Name}");
                }
            }
        }

        private static async Task DownloadBlobAsync()
        {
            string localFileName = "downloaded.html";
            Console.WriteLine($"4. Downloading blob '{blobName}' to local file '{localFileName}'");
            BlobClient blobClient = new BlobClient(connectionString, blobContainerName, blobName);
            bool exists = await blobClient.ExistsAsync();
            if (exists)
            {
                BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
                using FileStream fileStream = File.OpenWrite(localFileName);
                await blobDownloadInfo.Content.CopyToAsync(fileStream);
            }
        }

        private static async Task DeleteContainerAsync()
        {
            Console.WriteLine($"5. Deleting blob container '{blobContainerName}'");
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, blobContainerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }
    }
}