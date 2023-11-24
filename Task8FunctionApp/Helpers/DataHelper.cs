using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task8FunctionApp.Helpers
{
    public class DataHelper
    {
        public static async Task<string?> GetConnectionString()
        {
            var config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
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
