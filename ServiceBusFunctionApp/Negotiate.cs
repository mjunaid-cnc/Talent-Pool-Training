using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusFunctionApp
{
    public class Negotiate
    {
        [FunctionName("negotiate")]
        public static SignalRConnectionInfo NegotiateSignalR(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "get", Route = "negotiate")] HttpRequest req,
            [SignalRConnectionInfo(HubName = "Task9Hub")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
}
