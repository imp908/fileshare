namespace Configs{

using System;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DbInstanceCheck;

using AutoMapper;
using MongoModel;
using EfModel;
using Checks_;
using Itnerfaces;

    public class ConfigInstances
    {
        public static void ConfigsRun(){
            AutoMapperConfig();
            AutoFacConfig();
        }        
        public static void AutoMapperConfig(){
             Mapper.Initialize(
                cfg => {
                    cfg.CreateMap<Post, PostMng>(MemberList.Source);
                        //.IgnoreAllUnmapped();
                        //.ConstructUsing(s => { return new PostMng(){PostId=s.PostId, Title=s.Title,Content=s.Content};} ).ForAllOtherMembers(s=>s.Ignore());
                    cfg.CreateMap<Blog, BlogMng>(MemberList.Source);
                        //.ConstructUsing(s => { return new BlogMng(){BlogId=s.BlogId,Url=s.Url,Rating=s.Rating,Posts=Mapper.Map<List<Post>,List<PostMng>>(s.Posts)};} ).ForAllOtherMembers(s=>s.Ignore());
                    cfg.CreateMap<User, UserMng>(MemberList.Source);
                        //.ConstructUsing(s => { return new UserMng(){UserId=s.UserId,Email=s.Email,Blogs=Mapper.Map<List<Blog>,List<BlogMng>>(s.Blogs)};} ).ForAllOtherMembers(s=>s.Ignore());

                        cfg.AllowNullDestinationValues = true;
                }   
            );
        }
        private static IContainer AutoFacConfig(){
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddAutoMapper();

            var containerBuilder = new ContainerBuilder();                        

            containerBuilder.Populate(serviceCollection);
            
            containerBuilder.RegisterType<DbCHeck>();
            containerBuilder.RegisterType<MongoCheck>().As<IGo>()
            .SingleInstance();

            IContainer container = containerBuilder.Build();
            IServiceProvider serviceProvider = new AutofacServiceProvider(container);
            return container;
        }      
    }
}