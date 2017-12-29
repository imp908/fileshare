
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using NewsAPI.Jobs;
using NewsAPI.App_Start;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace NewsAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          AreaRegistration.RegisterAllAreas();
          GlobalConfiguration.Configure(WebApiConfig.Register);

            // запуск выполнения работы по отправке ежедневного оповещения о днях рождения избранных
            DailyBirthdayNotificationScheduler.Start();
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


}
