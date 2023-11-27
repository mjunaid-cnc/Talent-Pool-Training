using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text.Json;
using Task7.API.Helpers;
using Task7.API.Models;
using Task7.API.Repositories;

namespace Task7.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
        private readonly IConfiguration _configuration;
        private readonly IHubContext<WebSocketHub> _hubContext;

        public EmployeeController(IAmazonS3 s3Client, IConfiguration configuration, IHubContext<WebSocketHub> hubContext)
        {
            _s3Client = s3Client;
            _configuration = configuration;
            _hubContext = hubContext;
             
        }
        [HttpPost("post")]
        public async Task<IActionResult> PostEmployeeData(IFormFile file)
        {
            try
            {
                var fileExt = Path.GetExtension(file.FileName);
                if (fileExt == "." + FileType.json.ToString() && file != null && file.Length > 0)
                {
                    using var memoryStr = new MemoryStream();
                    await file.CopyToAsync(memoryStr);

                    memoryStr.Position = 0;
                    string fileName = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName);

                    string uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), Folders.uploads.ToString());
                    string filePath = Path.Combine(uploadFolderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    var jsonData = System.IO.File.ReadAllText(filePath);
                    var employeeData = System.Text.Json.JsonSerializer.Deserialize<EmployeeModel>(jsonData);

                    await UploadFileToS3(memoryStr, _configuration["BucketName"], fileName);
                    
                    string accessKey = _configuration["AccessKey"];
                    string accessSecret = _configuration["AccessSecret"];
                    var client = new AmazonSQSClient(accessKey, accessSecret, Amazon.RegionEndpoint.EUNorth1);
                    var request = new SendMessageRequest()
                    {
                        QueueUrl = _configuration["QueueUrl"],
                        MessageBody = _configuration["BucketName"] + ":" + fileName
                    };
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", "File successfully processed");
                    await client.SendMessageAsync(request);
                    return Ok(new Response { Success = true, Message = "The file is being processed...", StatusCode = StatusCodes.Status200OK  });
                }
                else
                {
                    return Ok(new Response { Success = false, Message = "Invalid file type. Please upload a .json file", StatusCode = StatusCodes.Status400BadRequest });
                }
            }
            catch (Exception ex)
            {
                return Ok(new Response { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var userRepo = new UserRepo();
                var employees = await userRepo.GetEmployees();
                return Ok(new Response { Success = true, Content= employees, StatusCode= StatusCodes.Status200OK });
            }
            catch (Exception ex)
            {
                return Ok(new Response { Success = false, Message = ex.Message, StatusCode = StatusCodes.Status500InternalServerError });
            }
        }

        private async Task UploadFileToS3(Stream fileStream, string bucketName, string key)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(fileStream, bucketName, key);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine($"Error: {amazonS3Exception.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
