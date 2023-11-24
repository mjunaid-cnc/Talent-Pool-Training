using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Task8FunctionApp.Helpers;
using Task8FunctionApp.Models;

namespace Task8FunctionApp
{
    public class Function1
    {
        [FunctionName("Function1")]
        public async Task RunAsync([BlobTrigger("task8container/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
                using var reader = new StreamReader(myBlob);
                var content = await reader.ReadToEndAsync();
                var jsonContent = JsonSerializer.Deserialize<UserModel>(content);

                var userRepo = new UserRepository();
                await userRepo.AddUser(jsonContent);

                jsonContent.Company = "Netsol";
                var modifiedFile = JsonSerializer.Serialize(jsonContent);
                await BlobHelper.UploadToBlobAsync(name, modifiedFile);
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message);
            }
        }
    }
}
