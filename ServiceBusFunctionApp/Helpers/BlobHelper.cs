using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusFunctionApp.Helpers
{
    public class BlobHelper
    {
        public static async Task<string?> GetBlobConnectionStringAsync(Microsoft.Azure.WebJobs.ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
             .SetBasePath(context.FunctionAppDirectory)
             .AddJsonFile("appsettings.json", true, true)
             .Build();

            string secretName = config["SecretName"] ?? throw new Exception("secret name is null");
            string kvUri = config["KVUri"];

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var secret = await client.GetSecretAsync(secretName);

            if (secret != null)
            {
                return secret.Value.Value;
            }
            return null;
        }
    }
}
