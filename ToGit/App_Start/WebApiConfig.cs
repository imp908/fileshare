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
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                "ApiByType",
                "api/{controller}/{type}/{id}/{count}",
                 new { action = "Get",  id = RouteParameter.Optional, count = RouteParameter.Optional },
                 new { httpMethod = new HttpMethodConstraint(HttpMethod.Get), type =@"^[a-zA-Z]*$"  }
                 
            );
            config.Routes.MapHttpRoute(
                "ApiById",
               "api/{controller}/{id}/{start}/{end}",
                new { action = "Get", start = RouteParameter.Optional, end = RouteParameter.Optional }, 
                new { httpMethod = new HttpMethodConstraint(HttpMethod.Get), id = @"\d+" } //@"^\d$"
           );

          

            var autofacResolver = AutofacConfig.ConfigureContainer().Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(autofacResolver);


        }
    }
}
