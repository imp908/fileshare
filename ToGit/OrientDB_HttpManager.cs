using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace NewsAPI.Helpers
{
    public class OrientDB_HttpManager
    {
        protected static string OSESSIONID;

        public static void AuthorizeOrientDB(string Url, string login, string password)
        {
            WebRequest AuthRequest = WebRequest.Create(Url);
            AuthRequest.Credentials = new NetworkCredential(login, password);
            AuthRequest.Method = "GET";
            AuthRequest.ContentType = "application/json; charset=utf-8";
            WebResponse response = AuthRequest.GetResponse();
            OSESSIONID = response.Headers.Get("Set-Cookie");
            response.Close();
        }
            

        public class PostBatch : IHttpActionResult
        {
            Uri _uri;
            string _jsonBatch;

            public PostBatch(string Uri, string JsonBatch)
            {
                _uri = new Uri(Uri);
                _jsonBatch = JsonBatch;
            }

            async Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_uri);
                request.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
                request.Method = "POST";
                request.ContentType = "application/json; charset=utf-8";

                byte[] byteArray = Encoding.UTF8.GetBytes(_jsonBatch);
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(byteArray, 0, byteArray.Length);
                }

                var resp = new HttpResponseMessage(HttpStatusCode.OK);


                using (var responseApi = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(responseApi.GetResponseStream()))
                    {
                        var responseString = reader.ReadToEnd();

                        resp.Content = new StringContent(responseString, Encoding.UTF8, "application/json");

                    }
                }
                return await Task.FromResult(resp);
            }
        }


        public class GetBatchResult : IHttpActionResult
        {
            Uri _uri;

            public GetBatchResult(string Uri)
            {
                _uri = new Uri(Uri);
            }

            async Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_uri);
                request.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);

                request.Method = "GET";
                request.ContentType = "application/json; charset=utf-8";

                var resp = new HttpResponseMessage(HttpStatusCode.OK);

                using (var responseApi = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(responseApi.GetResponseStream()))
                    {
                        var objText = reader.ReadToEnd();
                        var objClasses = JObject.Parse(objText);
                        var jsonResult = objClasses.ToString();

                        resp.Content = new StringContent(jsonResult, Encoding.UTF8, "application/json");
                    }
                }
                return await Task.FromResult(resp);
            }

        }

    }
}