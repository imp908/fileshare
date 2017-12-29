using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace NewsAPI.Extensions
{
    public static class ActionResultExtensions
    {
        public static string ExtractEntityId(this IHttpActionResult ar, string idName)
        {
            var responseWithId = ar.ExecuteAsync(new CancellationToken());

            string id_str;
            using (var contentStream = responseWithId.Result.Content.ReadAsStreamAsync().Result)
            {
                using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                {
                    id_str = reader.ReadToEnd();
                }
            }

            return id_str;
        }
    }
}