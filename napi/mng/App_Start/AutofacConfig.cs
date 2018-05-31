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
            builder.RegisterType<IntranetUserSettings>().As<IUserSettings>().SingleInstance(); 
            builder.RegisterType<IntranetPersonBirthdays>().As<IPersonBirhtdays>().SingleInstance();
            builder.RegisterType<IntranetPersonRelation>().As<IPersonRelation>().SingleInstance();

            return builder;
        }        

    }
}