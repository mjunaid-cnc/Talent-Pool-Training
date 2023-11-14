using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task6_AWS.Helpers
{
    public class DataHelper
    {
        public static async Task<string?> GetConnectionString()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(assemblyLocation))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();

            string dbName = config["DbName"];
            string secretName = config["SecretName"];
            string region = config["Region"];

            using var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));
            var request = new GetSecretValueRequest()
            {
                SecretId = secretName
            };
            var response = await client.GetSecretValueAsync(request);
            var secretData = JsonConvert.DeserializeObject<SecretData>(response.SecretString);
            if (secretData != null)
            {
                string connectionString = $"Server={secretData.Host};Database={dbName};User ID={secretData.Username};Password={secretData.Password};";
                return connectionString;
            }
            return null;
        }
    }
}

