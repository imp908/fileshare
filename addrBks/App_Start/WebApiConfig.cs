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

            //enables all parametreless methods
            config.Routes.MapHttpRoute(
            name: "DefaultApiParam",
            routeTemplate: "api/{controller}/{action}"
            );



            //default Birthdays
            config.Routes.MapHttpRoute(
            name: "Persons_GET_paramed",
            routeTemplate: "api/Person/TrackedBirthdaysAccGET/{GUID}",
             defaults: new { controller = "Person", Action = "TrackedBirthdaysAccGET" }
            );
            config.Routes.MapHttpRoute(
            name: "Birthdays_GET_paramed",
            routeTemplate: "api/Birthdays/GetBirthdays/{GUID}",
            defaults: new { controller = "Birthdays", Action = "GetBirthdays" }
            );



            //birthdays Acc
            config.Routes.MapHttpRoute(
            name: "Birthdays_Acc_get",
            routeTemplate: "api/Birthdays/GetBirthdaysAcc/{GUID}",
            defaults: new { controller = "Birthdays", Action = "GetBirthdaysAcc" }
            );
            config.Routes.MapHttpRoute(
            name: "Birthdays_Acc_POST",
            routeTemplate: "api/Birthdays/GetBirthdaysAcc/{fromGUID}/{toGUID}",
            defaults: new { controller = "Birthdays", Action = "PostBirthdaysAcc" }
            );
            config.Routes.MapHttpRoute(
            name: "Birthdays_Acc_Delete",
            routeTemplate: "api/Birthdays/GetBirthdaysAcc/{fromGUID}/{toGUID}",
            defaults: new { controller = "Birthdays", Action = "DeleteBirthdaysAcc" }
            );
            //birthdays Authenticate
            config.Routes.MapHttpRoute(
            name: "Birthdays_noAcc_get",
            routeTemplate: "api/Birthdays/GetBirthdays/",
            defaults: new { controller = "Birthdays", Action = "GetBirthdays" }
            );
            config.Routes.MapHttpRoute(
            name: "Birthdays_noAcc_POST",
            routeTemplate: "api/Birthdays/PostBirthdays/{toGUID}",
            defaults: new { controller = "Birthdays", Action = "PostBirthdays" }
            );
            config.Routes.MapHttpRoute(
            name: "Birthdays_noAcc_Delete",
            routeTemplate: "api/Birthdays/DeleteBirthdays/{toGUID}",
            defaults: new { controller = "Birthdays", Action = "DeleteBirthdays" }
            );





            //placed first, enables all but disables
            //all controllers GET methods with single parameter
            config.Routes.MapHttpRoute(
            name: "Persons_GET",
            routeTemplate: "api/{controller}/{action}/{accountName}",
            defaults: new { accountName = RouteParameter.Optional, controller = "Person" }
            );
            //enables Birthdays Get and Persons Gettracked with GUID parameter
            config.Routes.MapHttpRoute(
            name: "Birthdays_GET",
            routeTemplate: "api/{controller}/{action}/{GUID}",
            defaults: new { GUID = RouteParameter.Optional, controller = "Birthdays" }
            );
            

            //enables whole bunch of Birthday, Person methods with fromGUID,toGUID signature
            config.Routes.MapHttpRoute(
             name: "Birthdays_Perosn_POST_DELETE",
             routeTemplate: "api/{controller}/{action}/{fromGUID}/{toGUID}"
            );

         

            /*            
            var cors = new EnableCorsAttribute("*", "X-Accept-Charset,X-Accept,Content-Type,Credentials", "POST, GET, PUT, OPTIONS, PATCH, DELETE") { SupportsCredentials = true, PreflightMaxAge = 10 };
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            config.EnableCors(cors);

            var autofacResolver = AutofacConfig.ConfigureContainer().Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(autofacResolver);
            */

        }
    }
}
