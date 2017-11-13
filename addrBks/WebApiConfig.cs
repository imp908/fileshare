using Autofac.Integration.WebApi;
using NewsAPI.App_Start;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

using System.Web.Http.Routing;

using System.Web.Http.Cors;


namespace NewsAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
           config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                 "DefaultForAll",
                 "api/{controller}",                 
                 new { httpmethod = new HttpMethodConstraint(HttpMethod.Get) }
             );

            config.Routes.MapHttpRoute(
               "PersonAccount",
               "api/{controller}/{action}",
               new { controller = "person", Action = "Acc", httpmethod = new HttpMethodConstraint(HttpMethod.Get)
               , name = @"^[a-zA-z]*$" }
            );

            config.Routes.MapHttpRoute(
              "PersonHolidaysByAccount",
              "api/{controller}/{action}",
              new { controller = "person", Action = "HoliVationAcc", httpmethod = new HttpMethodConstraint(HttpMethod.Get)
              ,
                  name = @"^[a-zA-z]*$"
              }
            );

            config.Routes.MapHttpRoute(
              "NewsID",
              "api/{controller}/{type}",
              new { httpmethod = new HttpMethodConstraint(HttpMethod.Get), type = @"^[a-zA-Z]*$" }
            );

            config.Routes.MapHttpRoute(
              "AccountId",
              "api/{controller}/{count}",
              new { httpmethod = new HttpMethodConstraint(HttpMethod.Get), count = @"\d+" }
            );


            config.Routes.MapHttpRoute(
               "ApiByTypeCnt",
               "api/{controller}/{type}/{count}",
                new { action = "Get" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get), type = @"^[a-zA-Z]*$", count = @"\d+" }

            );
            config.Routes.MapHttpRoute(
                "ApiByTypeOfs",
                "api/{controller}/{type}/{start}/{count}",
                 new { action = "Get" },
                 new { httpMethod = new HttpMethodConstraint(HttpMethod.Get), type = @"^[a-zA-Z]*$", start = @"\d+", count = @"\d+" }

            );
            config.Routes.MapHttpRoute(
                "ApiById",
               "api/{controller}/{type}/{start}/{count}",
                new { action = "Get" },
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get), type = @"\d+", start = @"\d+", count = @"\d+" }
            );

            config.Routes.MapHttpRoute(
                "Persons",
                "api/{controller}/{action}/{accountName}",
                new { action = "Get", accountName = RouteParameter.Optional }                
            );

			var cors = new EnableCorsAttribute("*", "X-Accept-Charset,X-Accept,Content-Type,Credentials", "POST, GET, PUT, OPTIONS, PATCH, DELETE") { SupportsCredentials = true, PreflightMaxAge = 10 };
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            config.EnableCors(cors);

            var autofacResolver = AutofacConfig.ConfigureContainer().Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(autofacResolver);
            
        }
    }
}
