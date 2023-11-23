using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Task7SQSLambda.Models;

namespace Task7SQSLambda.Helpers
{
    public class CredentialsHelper
    {
        public static async Task<CredentialsModel> GetAWSCredentials()
        {
            var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName("eu-north-1"));
            var request = new GetSecretValueRequest()
            {
                SecretId = "talentpoolUserSecret",
            };
            var response = await client.GetSecretValueAsync(request);
            var creds = JsonSerializer.Deserialize<CredentialsModel>(response.SecretString) ?? throw new Exception("Secret is incorrect");
            return creds;
        }
    }
}
