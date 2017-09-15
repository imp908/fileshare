using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;


using Orient.Client;

namespace OrientDbApi.Controllers
{
    public class OrientController : ApiController
    {
        protected static string OSESSIONID;

        [HttpGet]
        public void GetByID(int id)
        {
            AuthorizeOrientDB();
            Get();
            GetFunc();
            
            OServer server = new OServer(@"http://msk1-vm-ovisp02",2480, "root", "I9grekVmk5g");
            var res = server.DatabaseExist(@"news_test1",OStorageType.Local);

            ODatabase db = new ODatabase(@"http://msk1-vm-ovisp02",2480, @"news_test1", ODatabaseType.Graph, "root","I9grekVmk5g");
            long a = db.Size;

        }

        public void AuthorizeOrientDB()
        {
            string url = @"http://msk1-vm-ovisp02:2480/connect/news_test1";
            WebRequest AuthRequest = WebRequest.Create(url);
            AuthRequest.Credentials = new NetworkCredential("root", "I9grekVmk5g");
            AuthRequest.Method = "GET";
            AuthRequest.ContentType = "application/json; charset=utf-8";
            WebResponse response = AuthRequest.GetResponse();
            OSESSIONID = response.Headers.Get("Set-Cookie");  
            int status = (int)((HttpWebResponse)response).StatusCode;
            response.Close();
        }

        public void Get()
        {
            string url = @"http://msk1-vm-ovisp02:2480/command/news_test1/sql/select from Entity";
            WebRequest wr = WebRequest.Create(url);
            wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
            wr.Method = "GET";
            wr.ContentType = "application/json; charset=utf-8";
            var resp = new HttpResponseMessage(HttpStatusCode.OK);

            string res = WebRequestToString(wr);
        }
        public void GetFunc()
        {
            string url = @"http://msk1-vm-ovisp02:2480/function/news_test1/GetEntity/100";
            WebRequest wr = WebRequest.Create(url);
            wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
            wr.Method = "GET";
            wr.ContentType = "application/json; charset=utf-8";
            var resp = new HttpResponseMessage(HttpStatusCode.OK);

            string res = WebRequestToString(wr);
        }
        public string  WebRequestToString(WebRequest wr)
        {
            using (var responseApi = (HttpWebResponse)wr.GetResponse())
            {
                using (var reader = new StreamReader(responseApi.GetResponseStream()))
                {
                    var objText = reader.ReadToEnd();
                    var objClasses = JsonConvert.DeserializeObject(objText);
                    var jsonResult = objClasses.ToString();

                    return jsonResult;

                }
            }
        }

    }
}
