using Autofac.Integration.WebApi;
using NewsAPI.App_Start;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

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

            //var cors = new EnableCorsAttribute("*", "Content-Type, Accept, Vary", "GET, POST, PUT, DELETE, OPTIONS, HEAD") { SupportsCredentials = true };
            ////  var cors = new EnableCorsAttribute("http://static.nspk.ru/*", "*", "*");
            //config.EnableCors(cors);

            var autofacResolver = AutofacConfig.ConfigureContainer().Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(autofacResolver);
            
        }
    }
}
