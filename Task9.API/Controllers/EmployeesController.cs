using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text;
using Task9.API.Models;

namespace Task9.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("post")]
        public async Task<IActionResult> PostEmployee(IFormFile file)
        {
            try
            {
                string filename = Path.GetFileName(file.FileName);
                string fileExtension = Path.GetExtension(file.FileName);
                if (file != null && file.Length > 0 && fileExtension == ".xlsx")
                {
                    string storageAccountConnectionString = _configuration["StorageAccountConnectionString"];
                    string containerName = _configuration["ContainerName"];
                    string uniqueFilename = Guid.NewGuid() + filename;
                    if (!Directory.Exists("uploads"))
                    {
                        Directory.CreateDirectory("uploads");
                    }
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        string destinationFilePath = Path.Combine("uploads", uniqueFilename);
                        using (var stream = new FileStream(destinationFilePath, FileMode.Create))
                        {
                            await memoryStream.CopyToAsync(stream);
                        }
                            var blobServiceClient = new BlobServiceClient(storageAccountConnectionString);
                            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                            var blobClient = containerClient.GetBlobClient(uniqueFilename);
                            await blobClient.UploadAsync(destinationFilePath, true);

                        string serviceBusConnectionString = _configuration["ServiceBusConnectionString"];
                        string queueName = _configuration["QueueName"];

                        ServiceBusClient client;
                        ServiceBusSender sender;
                        const int numOfMessages = 1;
                        var clientOptions = new ServiceBusClientOptions()
                        {
                            TransportType = ServiceBusTransportType.AmqpWebSockets
                        };
                        client = new ServiceBusClient(serviceBusConnectionString, clientOptions);
                        sender = client.CreateSender(queueName);
                        using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();
                        for (int i = 1; i <= numOfMessages; i++)
                        {
                            if (!messageBatch.TryAddMessage(new ServiceBusMessage($"{containerName}:{uniqueFilename}")))
                            {
                                throw new Exception("The message is too large to fit in the batch.");
                            }
                        }

                        try
                        {
                            await sender.SendMessagesAsync(messageBatch);
                        }
                        finally
                        {
                            await sender.DisposeAsync();
                            await client.DisposeAsync();
                        }

                        return Ok(new Response { Success = true, Message = "File is being processed...", StatusCode = StatusCodes.Status200OK });
                    }
                }
                else
                {
                    return Ok(new Response { Success = false, Message = "Incorrect file format or file not uploaded", StatusCode = StatusCodes.Status400BadRequest });
                }
            }
            catch(Exception ex)
            {
                return Ok(new Response { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }
    }
}
