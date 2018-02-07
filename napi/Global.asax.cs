
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using NewsAPI.App_Start;
using System.Web.Mvc;
using System.Web.Http.Cors;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;

namespace NewsAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);           
            EnableCorsAttribute ep = new EnableCorsAttribute("*", "*", "*");
            GlobalConfiguration.Configuration.Formatters.Insert(0, new TextMediaTypeFormatter());


        }

        //protected void Application_BeginRequest()
        //{
        //    string[] allowedOrigin = new string[] { "http://my.nspk.ru" };
        //    var origin = HttpContext.Current.Request.Headers["Origin"];
        //    if (origin != null && allowedOrigin.Contains(origin))
        //    {
        //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", origin);
        //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST");
        //        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");
        //        //Need to add more later , will see when required
        //    }
        //}

    }

    public class TextMediaTypeFormatter : MediaTypeFormatter
    {
        public TextMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));
        }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var taskCompletionSource = new TaskCompletionSource<object>();
            try
            {
                var memoryStream = new MemoryStream();
                readStream.CopyTo(memoryStream);
                var s = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                taskCompletionSource.SetResult(s);
            }
            catch (Exception e)
            {
                taskCompletionSource.SetException(e);
            }
            return taskCompletionSource.Task;
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(string);
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }
    }


}
