using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using NewsAPI.Implements;
using NewsAPI.Interfaces;

namespace NewsAPI.App_Start
{
    public class AutofacConfig
    {
        public static ContainerBuilder ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<OrientNews>().As<INews>().SingleInstance();
            builder.RegisterType<OrientProxy>().As<IAddressBookProxy>().SingleInstance();
            builder.RegisterType<OrientNewsJsonValidator>().As<IJsonValidator>().SingleInstance();
            builder.RegisterType<UserAuthenticator>().As<IUserAuthenticator>().SingleInstance();
            builder.RegisterType<IntranetAccount>().As<IAccount>().SingleInstance();

            //builder.RegisterType<JSONProxy>().As<IJSONProxy>();
            //builder.RegisterType<FunctionsToString>().As<IFunctionToString>();
            //builder.RegisterType<OrientPersons>().As<IPersonFunctions>();
            //builder.RegisterType<NSQLManager.IPersonUOW>().As<NSQLManager.PersonUOW>();
           

            return builder;
        }



    }
}