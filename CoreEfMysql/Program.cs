using System;
using MySqlContext;
using Checks_;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DbInstanceCheck;

using AutoMapper;

using Itnerfaces;

using EfModel;
using MongoModel;

using MongoRepo;

using Configs;

namespace SBcrSc
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
    class Program
    {
        private const string PressKeyMessage = "Press any key to continue...";

        public static IContainer container {get;set;}
        static void Main(string[] args)
        {              
            MongoRepoTest.Test();
            ConfigInstances.ConfigsRun();
            //AutoMapperConfig();
            //container=AutoFacConfig();
            //DbCHeck dc = new DbCHeck();dc.GO();

            MysqlContextCheck msql = new MysqlContextCheck();
            msql.GO();
           
            List<User> users = msql.users;
            List<UserMng> usersMng=null;
            try{
                usersMng = Mapper.Map<List<User>,List<UserMng>>(users);
            }catch(Exception e){ 
                Console.WriteLine(e.Message); throw; 
            }

            MongoCheck mng = new MongoCheck();
            mng.usersMng=usersMng;
            mng.GO();

            //ServiceLocator();
            Console.WriteLine("Hello World!");
            Console.WriteLine(PressKeyMessage);
            Console.ReadLine();
        }

        private static void ServiceLocator(){
            using (var scope = container.BeginLifetimeScope())
            {
                var go = scope.Resolve<Itnerfaces.IGo>();
                go.GO();
            }
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
