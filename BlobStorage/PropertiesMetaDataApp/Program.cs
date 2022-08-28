using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using static System.Console;

namespace AZ204.PropertiesMetadataApp
{
    class Program
    {
        private static readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=storagejimbuss;AccountKey=89hx/wiDE0R0PSgIXkTAcsHTrITtK0MOJnBS9qY5NWU6/Iw9kNaXtx43Ks8JyUmsZjA691q1WTa8+ASt6aylsg==;EndpointSuffix=core.windows.net";
        private static readonly string blobContainerName = "authors";
        private static readonly string blobName = "jamie.html";

        private static async Task Main(string[] args)
        {
            try
            {
                BlobClient blobClient = await CreateContainerAndUploadBlobAsync();
                await SetBlobPropertiesAsync(blobClient);
                await GetBlobPropertiesAsync(blobClient);
                await SetBlobMetadataAsync(blobClient);
                await GetBlobMetadataAsync(blobClient);
                WriteLine();
                WriteLine($"Press ENTER to delete blob container '{blobContainerName}'");
                ReadLine();
                await DeleteContainerAsync();
            }
            catch (RequestFailedException exception)
            {
                WriteLine($"Error: {exception.ErrorCode}");
                if (exception.ErrorCode == "ContainerBeingDeleted")
                {
                    WriteLine($"\tThe container '{blobContainerName}' is currently being deleted.");
                    WriteLine("\tThis takes usually up to 30 seconds.");
                    WriteLine("\tWait a bit and run the program again.");
                }
            }
            catch (Exception exception)
            {
                WriteLine(exception.Message);
            }
        }

        private static async Task<BlobClient> CreateContainerAndUploadBlobAsync()
        {
            // 1. Create the Blob Container
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            Console.WriteLine($"1. Creating blob container '{blobContainerName}'");
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            // 2. Upload a Blob
            BlobClient blobClient = blobContainerClient.GetBlobClient(blobName);
            WriteLine($"2. Uploading blob '{blobClient.Name}'");
            WriteLine($"\t{blobClient.Uri}");
            using FileStream fileStream = File.OpenRead("fileToUpload.html");
            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = "text/html" });
            return blobClient;
        }

        private static async Task SetBlobPropertiesAsync(BlobClient blobClient)
        {
            WriteLine($"3. Set blob properties");
            BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
            BlobHttpHeaders blobHttpHeaders = new BlobHttpHeaders
            {
                ContentType = "text/html",
                ContentLanguage = "en-us",
                CacheControl = blobProperties.CacheControl, //Properties not specified are deleted
                ContentDisposition = blobProperties.ContentDisposition,
                ContentEncoding = blobProperties.ContentEncoding,
                ContentHash = blobProperties.ContentHash
            };
            await blobClient.SetHttpHeadersAsync(blobHttpHeaders);
        }

        private static async Task GetBlobPropertiesAsync(BlobClient blobClient)
        {
            WriteLine($"4. Get blob properties");
            BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
            //Display just a few of the many blob properties
            WriteLine($"\t- ContentType: {blobProperties.ContentType}");
            WriteLine($"\t- BlobType: {blobProperties.BlobType}");
            WriteLine($"\t- CreatedOn: {blobProperties.CreatedOn}");
            WriteLine($"\t- LastModified: {blobProperties.LastModified}");
        }

        private static async Task SetBlobMetadataAsync(BlobClient blobClient)
        {
            WriteLine($" Set blob Metadata");
            IDictionary<string, string> metadata = new Dictionary<string, string>();
            metadata.Add("category", "author profile");
            metadata.Add("fullname", "Jamie Sandell");
            await blobClient.SetMetadataAsync(metadata);
        }

        private static async Task GetBlobMetadataAsync(BlobClient blobClient)
        {
            WriteLine($"6. Get blob metadata");
            BlobProperties blobProperties = await blobClient.GetPropertiesAsync();
            foreach (var metadataItem in blobProperties.Metadata)
            {
                WriteLine($"   - {metadataItem.Key} : {metadataItem.Value}");
            }
        }

    private static async Task DeleteContainerAsync()
        {
            WriteLine($"7. Deleting blob container '{blobContainerName}'");
            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, blobContainerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }
    }    
}