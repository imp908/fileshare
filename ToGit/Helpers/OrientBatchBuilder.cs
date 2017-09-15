using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAPI.Helpers
{
    public static class OrientBatchBuilder
    {
        public static string CreateBatch(string query)
        {
            OrientBatch batch = new OrientBatch(true, "script", "sql", new List<string>() { query });
            return JsonConvert.SerializeObject(batch);
        }
    }
}