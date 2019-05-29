using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.AspNetCore.Mvc.Razor;

using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace mvccoresb
{
    using mvccoresb.Domain.Interfaces;
    using mvccoresb.Infrastructure.EF;
    using Microsoft.EntityFrameworkCore;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;                
            });

            /** Register route to move Areas default MVC folder to custom location */
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/API/Areas/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/API/Areas/{2}/Views/Shared/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            /** Renames all defalt Views including Areas/Area/Views folders to custom name */
            services.Configure<RazorViewEngineOptions>(
                options => options.ViewLocationExpanders.Add(
            new CustomViewLocation()));

            var connectionStringSQL = "Server=HP-HP000114\\SQLEXPRESS02;Database=EFdb;Trusted_Connection=True;";
            //var connectionStringSQL = "Server=AAAPC;Database=testdb;User Id=tl;Password=QwErT123;";
            services.AddDbContext<TestContext>(o => o.UseSqlServer(connectionStringSQL));

            /*Autofac builder */
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<TestContext>().As<DbContext>()
                .InstancePerLifetimeScope();
            builder.RegisterType<RepositoryEF>()
                .As<IRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UOW>()
                .As<IUOW>().InstancePerLifetimeScope();
            builder.RegisterType<UOWblogs>()
                .As<IUOWBlogging>().InstancePerLifetimeScope();
            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {            
            // builder.RegisterType<RepositoryEF>()
            //     .As<IRepository>().InstancePerLifetimeScope();
            // builder.RegisterType<UOW>()
            //     .As<IUOW>().InstancePerLifetimeScope();
            // builder.RegisterType<UOWblogs>()
            //     .As<IUOWBlogging>().InstancePerLifetimeScope();
            var container = builder.Build();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                /**mapping for default scaffolded area */
                routes.MapRoute(
                    name:"areas",
                    template:"{area}/{controller}/{action}"
                );

                /** mapping for custom testarea */
                routes.MapAreaRoute(
                    name: "TestArea",
                    areaName: "TestArea",
                    template: "TestArea/{controller=Home}/{action=Index}"
                );

            });
        }
    }
}
