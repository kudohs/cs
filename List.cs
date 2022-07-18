using System;
using System.Threading.Tasks;
using Intacct.SDK;
using Intacct.SDK.Functions.Common;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Newtonsoft.Json.Linq;

using Intacct.SDK.Functions.Common.NewQuery;
using Intacct.SDK.Functions.Common.NewQuery.QueryFilter;
using Intacct.SDK.Functions.Common.NewQuery.QueryOrderBy;
using Intacct.SDK.Functions.Common.NewQuery.QuerySelect;

using System.Text.RegularExpressions;

namespace Intacct.Examples
{
    public static class List_APBILL
    {       
        public static string Run(ILogger logger)       
        {
            OnlineClient client = Bootstrap.Client(logger);

            SelectBuilder selectBuilder = new SelectBuilder();
            ISelect[] fields = selectBuilder.
                //Fields(new[] { "RECORDNO", "STATUS", "OPEN" }). // APBILLBATCH
                Fields(new[]  { "*" } ). // APBILLBATCH
                GetFields();

            int offset = 0;
            int pagesize = 100;
            string resultJsonString = "";

            //var query = new QueryFunction()
            var ReadByQuery = new QueryFunction()
            {
                FromObject = "APBILL",
                Offset = 0,                    
                PageSize = 20,
                SelectFields = fields
            };
           
            do
            {
                //query.Offset = offset;
                //query.PageSize = pagesize;
                ReadByQuery.Offset = offset;
                ReadByQuery.PageSize = pagesize;

                //Task<OnlineResponse> task = client.Execute(query);
                Task<OnlineResponse> task = client.Execute(ReadByQuery);
                OnlineResponse response = task.Result;
                Result result = response.Results[0];

                dynamic resultJson =
                           JsonConvert.DeserializeObject(JsonConvert.SerializeObject(result.Data));
                offset += pagesize;
                var x = Regex.Replace(resultJson.ToString(), @"[\[\]']+", "");
                resultJsonString += x + ",";

            } while (offset < 2001);  
            return "[" + resultJsonString + "]";
        }
    }
}
