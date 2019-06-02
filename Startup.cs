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

using AutoMapper;

namespace mvccoresb
{
    using mvccoresb.Domain.Interfaces;
    using mvccoresb.Domain.TestModels;

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

            //var connectionStringSQL = "Server=HP-HP000114\\SQLEXPRESS02;Database=EFdb;Trusted_Connection=True;";
            var connectionStringSQL = "Server=AAAPC;Database=testdb;User Id=tl;Password=QwErT123;";
            services.AddDbContext<TestContext>(o => o.UseSqlServer(connectionStringSQL));



            /*Autofac autofacContainer */
            var autofacContainer = new ContainerBuilder();
           
         

            /*Automapper Register */
            services.AddAutoMapper(typeof(Startup));
                   
            //AutoMapperStaticConfiguration.Configure();
            /*Mapper initialize with Static initialization*/
            // Mapper.Initialize(cfg =>
            // {
            //     cfg.CreateMap<BlogEF, BlogBLL>(MemberList.None);
            //     cfg.CreateMap<PostEF, PostBLL>(MemberList.None);                
            // });
            // try{  
            //     Mapper.AssertConfigurationIsValid();
            // }catch(Exception e)
            // {

            // }

            /*Mapper initialize with Instance API initialization */
            var config = ConfigureAutoMapper();
            IMapper mapper = new Mapper(config);                      
  

            /*Autofac registrations */
            autofacContainer.Populate(services);        
            ConfigureAutofac(services,autofacContainer);

            /*Registration of automapper with autofac Instance API */
            autofacContainer.RegisterInstance(mapper).As<IMapper>();    

            this.ApplicationContainer = autofacContainer.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        public ContainerBuilder ConfigureAutofac(IServiceCollection services,ContainerBuilder autofacContainer)
        {            
          
            /**EF,repo and UOW reg */
            autofacContainer.RegisterType<TestContext>().As<DbContext>()
                .InstancePerLifetimeScope();

            autofacContainer.RegisterType<RepositoryEF>()
                .As<IRepository>().InstancePerLifetimeScope();

            autofacContainer.RegisterType<CQRSBloggingWrite>()
                .As<ICQRSBloggingWrite>().InstancePerLifetimeScope();
            autofacContainer.RegisterType<CQRSBloggingRead>()
                .As<ICQRSBloggingRead>().InstancePerLifetimeScope();

            //*DAL->BLL reg */
            autofacContainer.RegisterType<BlogEF>()
                .As<IBlogEF>().InstancePerLifetimeScope();
            autofacContainer.RegisterType<BlogBLL>()
                .As<IBlogBLL>().InstancePerLifetimeScope();
            autofacContainer.RegisterType<PostBLL>()
                .As<IPostBLL>().InstancePerLifetimeScope();

            return autofacContainer;
        }

        public MapperConfiguration ConfigureAutoMapper(){
            return new MapperConfiguration(cfg => {
                //cfg.AddProfiles(typeof(BlogEF), typeof(BlogBLL));
                cfg.CreateMap<BlogEF, BlogBLL>()
                    .ForMember(dest => dest.Id, m => m.MapFrom(src => src.BlogId))
                    .ForMember(dest => dest.Posts, m => m.Ignore());

                cfg.CreateMap<PostEF, PostBLL>(MemberList.None).ReverseMap();

                cfg.CreateMap<PersonAdsPostCommand, PostEF>()
                    .ForMember(dest => dest.AuthorId, m => m.MapFrom(src => src.PersonId));

                cfg.CreateMap<AddPostAPI, PostEF>()
                    .ForMember(dest => dest.AuthorId, m => m.MapFrom(src => src.PersonId))
                    .ForMember(dest => dest.BlogId, m => m.MapFrom(src => src.BlogId));

                cfg.CreateMap<PersonEF, PersonAPI>();
                cfg.CreateMap<BlogEF, BlogAPI>();
                cfg.CreateMap<PostEF, PostAPI>();
            });
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

    public static class AutoMapperStaticConfiguration
    {
        public static void Configure(){
            /*Mapper initialize with Static initialization*/
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<BlogEF, BlogBLL>();
                cfg.CreateMap<PostEF, PostBLL>();                
            });
        }
    }
}
