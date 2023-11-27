using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text.Json;
using Task7SQSLambda.Helpers;
using Task7SQSLambda.Models;
using Task7SQSLambda.Repositories;
using Task7SQSLambda.Helpers;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Task7SQSLambda;

public class Function
{
    private readonly IAmazonS3 _s3Client;
    private readonly IAmazonSQS _sqsClient;
    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    /// 
    public Function()
    {
        _s3Client = new AmazonS3Client();
        _sqsClient = new AmazonSQSClient();
    }


    /// <summary>
    /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
    /// to respond to SQS messages.
    /// </summary>
    /// <param name="evnt"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
    {
        foreach (var message in evnt.Records)
        {
            await ProcessMessageAsync(message, context);
        }
    }

    private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
    {
        context.Logger.LogInformation($"Processed message {message.Body}");

        // TODO: Do interesting work based on the new message
        try
        {


            var s3Info = message.Body.Split(":");
            var objectReq = new GetObjectRequest()
            {
                BucketName = s3Info[0],
                Key = s3Info[1]
            };
            if (s3Info.Length == 2)
            {


                using (var response = await _s3Client.GetObjectAsync(objectReq))
                {
                    using (var streamReader = new StreamReader(response.ResponseStream))
                    {
                        var jsonContent = await streamReader.ReadToEndAsync();
                        context.Logger.Log("S3 content: " + jsonContent.ToString());
                        var employee = JsonSerializer.Deserialize<EmployeeModel>(jsonContent) ?? throw new Exception("File data is incorrect");
                        var userRepo = new UserRepo();
                        await userRepo.AddEmployee(employee);
                    }
                }
                var creds = await CredentialsHelper.GetAWSCredentials();
                var credentials = new BasicAWSCredentials(creds.AccessKey, creds.AccessSecret);
                var client = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.EUNorth1);
                var request = new PublishRequest()
                {
                    TopicArn = "arn:aws:sns:eu-north-1:792835767734:task7-sns",
                    Message = Constants.FileSuccessfullyProcessed()
                };

                await client.PublishAsync(request);
                await Task.CompletedTask;
            }

            else
            {
                context.Logger.LogInformation(Constants.InvalidFilename());
            }
        }
        catch (Exception ex)
        {
            context.Logger.LogInformation(ex.Message);
        }
    }

}