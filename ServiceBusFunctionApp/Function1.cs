using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OfficeOpenXml;
using ServiceBusFunctionApp.Helpers;

namespace ServiceBusFunctionApp
{
    public class Function1
    {
        [FunctionName("Function1")]
        public async Task RunAsync([ServiceBusTrigger("task9-queue", Connection = "AzureWebJobsStorage")] string myQueueItem,
            [SignalR(HubName = "Task9Hub")] IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            var queueItemArray = myQueueItem.Split(':');

            if (queueItemArray.Length == 2)
            {
                var containerName = queueItemArray[0];
                var filename = queueItemArray[1];

                string connectionString = await BlobHelper.GetBlobConnectionStringAsync(context);

                BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
                var blobClient = containerClient.GetBlobClient(filename);
                try
                {
                    BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
                    using var memoryStream = new MemoryStream();
                    await blobDownloadInfo.Content.CopyToAsync(memoryStream);
                    string jsonContent = ConvertExcelToJson(memoryStream);
                    log.LogInformation(jsonContent);

                    var config = new ConfigurationBuilder()
                        .SetBasePath(context.FunctionAppDirectory)
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
                    string processedContainerName = config["ProcessedContainerName"] ?? throw new Exception("Processed container name is null");
                    string processedFileName = Guid.NewGuid() + ProcessedFilename.Test.ToString() + "." + FileExtensions.json.ToString();
                    var destinationBlobContainerClient = new BlobContainerClient(connectionString, processedContainerName);
                    var processedBlobClient = destinationBlobContainerClient.GetBlobClient(processedFileName);
                    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));
                    await processedBlobClient.UploadAsync(stream, true);

                    var message = new SignalRMessage
                    {
                        Target = "FileProcessed",
                        Arguments = new[] { $"File processed successfully:{processedContainerName}:{processedFileName}" }
                    };
                    await signalRMessages.AddAsync(message);
                }
                catch (Exception e)
                {
                    log.LogError($"Error retrieving file: {e.Message}");
                }
            }
            else
            {
                log.LogError("Incorrect filename");
            }
        }

        private static string ConvertExcelToJson(Stream excelStream)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(excelStream))
                {
                    if (package.Workbook != null)
                    {
                        if (package.Workbook.Worksheets.Count > 0)
                        {


                            var worksheet = package.Workbook.Worksheets[0];
                            var startCell = worksheet.Dimension.Start;
                            var endCell = worksheet.Dimension.End;

                            var fileData = new List<Dictionary<string, object>>();
                            for (int row = startCell.Row; row <= endCell.Row - 1; ++row)
                            {
                                var rowData = new Dictionary<string, object>();
                                for (int col = startCell.Column; col <= endCell.Column; col++)
                                {
                                    var key = worksheet.Cells[startCell.Row, col].Text;
                                    var value = worksheet.Cells[row + 1, col].Text;
                                    rowData[key] = value;
                                }
                                fileData.Add(rowData);
                            }
                            return JsonConvert.SerializeObject(fileData, Formatting.Indented);
                        }
                        else
                        {
                            return "No worksheet exists";
                        }
                    }
                    else
                    {
                        return "Workbook doesn't exist";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
