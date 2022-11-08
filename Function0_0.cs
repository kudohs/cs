using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Intacct.Examples;

namespace DurableHttp_0120
{
    public static class Function0_0
    {

        [FunctionName("Orch")]
        public static async Task<List<string>> RunOrchestrator(   
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();
            var x = await context.CallActivityAsync<string>("GLDETAILAct", null);
            outputs.Add(x);
            return outputs;
        }

        [FunctionName("GLDETAILAct")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            ILogger logger = Bootstrap.Logger("Program");
            var result = List_GLDETAIL0_0.Run(log);
            return result;            
        }

        [FunctionName("Function1_1")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string instanceId = await starter.StartNewAsync("Orch", null);
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
