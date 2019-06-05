using System;
using System.Linq;
using System.Reflection;
using DI_check;


namespace Console_host
{
    using AutofacConfig;

    public class InjectedGO : DI_check.Igo
    {
        public void GO()
        {
            Console.WriteLine("Injected GO");
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            ContainerResolve.RefStart(ContainerBuild.Configuration());
        }

        /// <summary>
        /// Starts run method from assembly load
        /// </summary>
        void AsmStart()
        {
            try
            {
                Assembly asm = Assembly.LoadFrom(@"G:\disk\Files\git\sharp\console\DI_check\DI_check\bin\Debug\DI_check.dll");
                if (asm != null)
                {
                    Type run = (from s in asm.GetTypes() where s.Name == "RUN" select s).FirstOrDefault();
                    var o = Activator.CreateInstance(run);
                    run.InvokeMember("GO", BindingFlags.InvokeMethod, null, o, null);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
        }
    }
}

namespace AutofacConfig
{
    using Autofac;
    using Console_host;
    using AutoMapper;
    using NLog;

    public class StartupMessageWriter : IStartable
    {
        public void Start()
        {
            Console.WriteLine("App is starting up!");
        }
    }

    /// <summary>
    /// Configuration modules
    /// </summary>
    public class GoConfigModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<InjectedGO>();
            //builder.RegisterType<DI_check.Text>();
            //builder.RegisterType<DI_check.BlogMap>();
        }
    }

    /// <summary>
    /// Container scope build
    /// </summary>
    public class ContainerBuild
    {
        public static IContainer Configuration()
        {
            ContainerBuilder builder = new ContainerBuilder();
                builder.RegisterModule(new GoConfigModule());
                builder.RegisterModule(new AutoMapperConfig());
                builder.RegisterModule(new NlogConfig());
            IContainer container = builder.Build();
            return container;
        }
    }

    /// <summary>
    /// Configuring Autofac module
    /// </summary>
    public class AutoMapperConfig : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InjectedGO>();
            builder.RegisterType<DI_check.Text>();
            builder.RegisterType<DI_check.BlogMap>();
            builder.Register(c => {
                return new DI_check.BlogMap(this.mapper(), NLog.LogManager.GetCurrentClassLogger());          
            }).As<DI_check.Igo>();

            base.Load(builder);
        }

        public MapperConfiguration config()
        {
            return new MapperConfiguration(x => {
                x.CreateMap<DI_check.Models.EfModel.User, DI_check.Models.MongoModel.User>();
                x.CreateMap<DI_check.Models.EfModel.Blog, DI_check.Models.MongoModel.Blog>();
            });
        }
        public IMapper mapper()
        {
            IMapper mapper_ = this.config().CreateMapper();
            return mapper_;
        }
    }

    public class NlogConfig : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            NLog.LogManager.Configuration = config;
            
            //builder.Register(c=> {
            //    var logger = NLog.LogManager.GetCurrentClassLogger();
            //    return new Nlogger(NLog.LogManager.GetCurrentClassLogger());
            //}).As<Ilog>();

            base.Load(builder);
        }
    }

    /// <summary>
    /// Container scope resolve
    /// </summary>
    public class ContainerResolve
    {
        public static void RefStart(IContainer container)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                var reader = scope.Resolve<Igo>();
                reader.GO();
            }
        }

    }
}
