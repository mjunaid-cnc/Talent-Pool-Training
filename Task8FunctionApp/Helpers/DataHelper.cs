using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Threading.Tasks;

namespace Task8FunctionApp.Helpers
{
    public class DataHelper
    {
        public static async Task<string?> GetConnectionString()
        {
            string secretName = Environment.GetEnvironmentVariable("SecretName");
            string kvUri = Environment.GetEnvironmentVariable("KVUri");

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
