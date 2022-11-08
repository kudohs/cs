using System.Threading.Tasks;
using Intacct.SDK;
using Intacct.SDK.Functions.Common;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Response;
using Newtonsoft.Json;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Intacct.SDK.Functions.Common.NewQuery.QuerySelect;
using System.Text.RegularExpressions;

namespace Intacct.Examples
{
    public static class List_GLDETAIL0_0    {
       
        public static string Run(ILogger logger)       
        {
            OnlineClient client = Bootstrap.Client(logger);                  

            SelectBuilder selectBuilder = new SelectBuilder();
            ISelect[] fields = selectBuilder.
                Fields(new[] { "*" }).
                GetFields();

            int offset = 0;            
            int pagesize = 100; 

            string resultJsonString = "";
               var ReadByQuery = new ReadByQuery()
            {
                ObjectName = "APBILLITEM",                
                PageSize = 20,
            };                     

            do
            {
                ReadByQuery.PageSize = pagesize;
                Task<OnlineResponse> task = client.Execute(ReadByQuery);
                OnlineResponse response = task.Result;
                Result result = response.Results[0];
                dynamic resultJson =
                    JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result.Data));
                offset += pagesize;                              
                var x = Regex.Replace(resultJson.ToString(), @"[\[\]']+", "");
                resultJsonString += x + ",";      

               } while (offset < 1000);                
            return "[" + resultJsonString + "]";

       }
    }
}
