using Autofac.Integration.WebApi;
using NewsAPI.App_Start;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Routing;
using System;
using System.Web.Http.Cors;

namespace NewsAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            // Web API routes
            config.MapHttpAttributeRoutes();


           // config.Routes.MapHttpRoute(
           //     name: "DefaultApi",
           //     routeTemplate: "api/{controller}/{id}",
           //     defaults: new { id = RouteParameter.Optional }
           // );
           // config.Routes.MapHttpRoute(
           //     "ApiByType",
           //     "api/{controller}/{type}/{start}/{count}",
           //      new { action = "Get", type = RouteParameter.Optional, start = RouteParameter.Optional, count = RouteParameter.Optional },
           //      new { httpMethod = new HttpMethodConstraint(HttpMethod.Get), type = @"^[a-zA-Z]*$" }

           // );
           // config.Routes.MapHttpRoute(
           //     "ApiById",
           //    "api/{controller}/{id}/{start}/{count}",
           //     new { action = "Get", start = RouteParameter.Optional, end = RouteParameter.Optional },
           //     new { httpMethod = new HttpMethodConstraint(HttpMethod.Get), id = @"\d+" }
           //);

            
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



      //placed first, enables all but disables
      //all controllers GET methods with single parameter
      config.Routes.MapHttpRoute(
      name: "Persons_GET",
      routeTemplate: "api/{controller}/{action}/{accountName}",
      defaults: new { accountName = RouteParameter.Optional, controller = "Person" }
      );           


      //enables whole bunch of Birthday, Person methods with fromGUID,toGUID signature
      config.Routes.MapHttpRoute(
       name: "Birthdays_Perosn_POST_DELETE",
       routeTemplate: "api/{controller}/{action}/{fromGUID}/{toGUID}"
      );


            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

            var cors = new EnableCorsAttribute("*", "X-Accept-Charset,X-Requested-With,X-Accept,Content-Type,Credentials", "POST, GET, PUT, OPTIONS, PATCH, DELETE") { SupportsCredentials = true, PreflightMaxAge = 10 };

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

            config.EnableCors(cors);

            var autofacResolver = AutofacConfig.ConfigureContainer().Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(autofacResolver);
        }
    }
}
