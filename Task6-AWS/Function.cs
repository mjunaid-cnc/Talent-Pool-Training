using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.Data.SqlClient;
using Models;
using Newtonsoft.Json;
using System.Xml.Linq;
using Task6_AWS.Entities;
using Task6_AWS.Helpers;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Task6_AWS;

public class Function
{
    IAmazonS3 S3Client { get; set; }

    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Function()
    {
        S3Client = new AmazonS3Client();
    }

    /// <summary>
    /// Constructs an instance with a preconfigured S3 client. This can be used for testing outside of the Lambda environment.
    /// </summary>
    /// <param name="s3Client"></param>
    public Function(IAmazonS3 s3Client)
    {
        this.S3Client = s3Client;
    }

    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an S3 event object and can be used 
    /// to respond to S3 notifications.
    /// </summary>
    /// <param name="evnt"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionHandler(S3Event evnt, ILambdaContext context)
    {
        var eventRecords = evnt.Records ?? new List<S3Event.S3EventNotificationRecord>();
        foreach (var record in eventRecords)
        {
            var s3Event = record.S3;
            if (s3Event == null)
            {
                continue;
            }

            try
            {
                var response = await this.S3Client.GetObjectMetadataAsync(s3Event.Bucket.Name, s3Event.Object.Key);
                var file = await this.S3Client.GetObjectAsync(s3Event.Bucket.Name, s3Event.Object.Key);
                if (!file.Key.StartsWith("processed_"))
                {
                    using var reader = new StreamReader(file.ResponseStream);
                    var fileContents = await reader.ReadToEndAsync();
                    LambdaLogger.Log(fileContents);
                    var user = JsonConvert.DeserializeObject<User>(fileContents);
                    if (user != null)
                    {
                        var connectionString = $"Server=database-1.c1jusiebdoer.eu-north-1.rds.amazonaws.com;Database=Task6;User ID=admin;Password=everton12;";
                        LambdaLogger.Log(connectionString);
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string insertQuery = "INSERT INTO Users(Username, Email, Company) VALUES (@Username, @Email, @Company)";

                            using (SqlCommand command = new SqlCommand(insertQuery, connection))
                            {
                                command.Parameters.AddWithValue("@Username", user.Username);
                                command.Parameters.AddWithValue("@Email", user.Email);
                                command.Parameters.AddWithValue("@Company", user.Company);

                                command.ExecuteNonQuery();
                            }
                        }
                        user.Company = "Netsol";
                        string modifiedJson = JsonConvert.SerializeObject(user);
                        string processedKey = "processed_" + s3Event.Object.Key;
                        var putObjectReq = new PutObjectRequest
                        {
                            BucketName = s3Event.Bucket.Name,
                            Key = processedKey,
                            ContentBody = modifiedJson
                        };
                        await S3Client.PutObjectAsync(putObjectReq);
                    }
                }
            }
            catch (Exception e)
            {
                context.Logger.LogError($"Error getting object {s3Event.Object.Key} from bucket {s3Event.Bucket.Name}. Make sure they exist and your bucket is in the same region as this function.");
                context.Logger.LogError(e.Message);
                context.Logger.LogError(e.StackTrace);
                throw;
            }
        }
    }
}