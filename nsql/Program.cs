
using System;
using System.Collections.Generic;
using System.Configuration;

using System.Linq;

using JsonManagers;
using WebManagers;
using QueryManagers;
using POCO;

using APItesting;
using IQueryManagers;
using OrientRealization;

using System.Net;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Reflection;

using System.IO;

using System.Text.RegularExpressions;


namespace NSQLManager
{

    class OrientDriverConnnect
    {

        static void Main(string[] args)
        {

            //EFcheck.EFqueryCheck();

            //GENERATING DATABASES
            //ManagerCheck.GenTestDB();
            //ManagerCheck.GenDevDB();

            //FUCNTIONAL CHECK
            ManagerCheck.UOWFunctionalCheck();

            //check linq to context
            LinqToContextPOC.LinqToContextCheck.GO();
      
            //START API TEST
            //ManagerCheck.APItester_sngltnCheck();
      
            //QUIZ CHECK
            //ManagerCheck.QuizCheck();

        }

    }

    //move to tests except DB generating
    public static class ManagerCheck
    {
        static JSONManager jm = new JSONManager();

        static void propSearch<T>(T item)
        {
          var pc = item.GetType().GetProperties();
          var pc2 = typeof(T).GetProperties();
     
          foreach (PropertyInfo ps in pc)
          {
            MethodInfo[] mi = ps.GetAccessors(true);
            Type pt = ps.PropertyType.GetType();
            Type t = ps.PropertyType;
            TypeInfo ti = ps.PropertyType.GetTypeInfo();
            Type ptt = item.GetType().GetProperty(ps.Name).GetType();
            var a = typeof(T).GetProperty(ps.Name).GetValue(item).GetType();
            Type tt = a.GetType();
          }
        }

        static OrientRepo DefaultManagerInit(string databaseName=null,string hostPort_=null,string login_=null,string password_=null)
        {
          string dbName;
          string login = ConfigurationManager.AppSettings["orient_login"];
          string password = ConfigurationManager.AppSettings["orient_pswd"];

            if (login_ == null) { login = ConfigurationManager.AppSettings["orient_login"]; } else { login = login_; }
            if (password_ == null) { password = ConfigurationManager.AppSettings["orient_pswd"]; } else { password = password_; }

            string dbHost = string.Format("{0}:{1}"
              , ConfigurationManager.AppSettings["OrientDevHost"]
              , ConfigurationManager.AppSettings["OrientPort"]);

          if (databaseName == null)
          {
            dbName = ConfigurationManager.AppSettings["OrientDevDB"];
          }
          else { dbName = databaseName; }
          if (hostPort_ == null)
          {
            dbHost = string.Format("{0}:{1}"
            , ConfigurationManager.AppSettings["OrientDevHost"]
            , ConfigurationManager.AppSettings["OrientPort"]);
          }
          else { dbName = hostPort_; }

          TypeConverter typeConverter = new TypeConverter();
          JsonManagers.JSONManager jsonMnager = new JSONManager();
          TokenMiniFactory tokenFactory = new TokenMiniFactory();
          UrlShemasExplicit UrlShema = new UrlShemasExplicit(
              new CommandBuilder(tokenFactory, new FormatFactory())
              , new FormatFromListGenerator(new TokenMiniFactory())
              , tokenFactory, new OrientBodyFactory());

          BodyShemas bodyShema = new BodyShemas(new CommandFactory(), new FormatFactory(), new TokenMiniFactory(),
              new OrientBodyFactory());

          UrlShema.AddHost(dbHost);
          WebResponseReader webResponseReader = new WebResponseReader();
          WebRequestManager webRequestManager = new WebRequestManager();
          webRequestManager.SetCredentials(new NetworkCredential(login, password));
          CommandFactory commandFactory = new CommandFactory();
          FormatFactory formatFactory = new FormatFactory();
          OrientQueryFactory orientQueryFactory = new OrientQueryFactory();
          OrientCLRconverter orientCLRconverter = new OrientCLRconverter();

          CommandShemasExplicit commandShema_ = new CommandShemasExplicit(commandFactory, formatFactory,
          new TokenMiniFactory(), new OrientQueryFactory());

            OrientRepo or = new OrientRepo(typeConverter, jsonMnager, tokenFactory, UrlShema, bodyShema, commandShema_
            , webRequestManager, webResponseReader, commandFactory, formatFactory, orientQueryFactory, orientCLRconverter);
            
            or.BindDbName(dbName);

            return or;

        }
        static OrientRepo ManagerInit(string host_,string hostPort_,string databaseName,string login_,string password_)
        {
            string dbName;
            string login = login_;
            string password = password_;
            string dbHost = string.Format("{0}:{1}"
                , host_
                ,hostPort_);
            dbName =databaseName;          

            TypeConverter typeConverter = new TypeConverter();
            JsonManagers.JSONManager jsonMnager = new JSONManager();
            TokenMiniFactory tokenFactory = new TokenMiniFactory();
            UrlShemasExplicit UrlShema = new UrlShemasExplicit(
                new CommandBuilder(tokenFactory, new FormatFactory())
                , new FormatFromListGenerator(new TokenMiniFactory())
                , tokenFactory, new OrientBodyFactory());

            BodyShemas bodyShema = new BodyShemas(new CommandFactory(), new FormatFactory(), new TokenMiniFactory(),
                new OrientBodyFactory());

            UrlShema.AddHost(dbHost);
            WebResponseReader webResponseReader = new WebResponseReader();
            WebRequestManager webRequestManager = new WebRequestManager();
            webRequestManager.SetCredentials(new NetworkCredential(login, password));
            CommandFactory commandFactory = new CommandFactory();
            FormatFactory formatFactory = new FormatFactory();
            OrientQueryFactory orientQueryFactory = new OrientQueryFactory();
            OrientCLRconverter orientCLRconverter = new OrientCLRconverter();

            CommandShemasExplicit commandShema_ = new CommandShemasExplicit(commandFactory, formatFactory,
            new TokenMiniFactory(), new OrientQueryFactory());
            OrientRepo or = new OrientRepo(typeConverter, jsonMnager, tokenFactory, UrlShema, bodyShema, commandShema_
            , webRequestManager, webResponseReader, commandFactory, formatFactory, orientQueryFactory, orientCLRconverter);
            
            or.BindDbName(dbName);

            return or;

        }
    
        static NewsUOWs.NewsRealUow ActualNewsUOW()
        {
          NewsUOWs.NewsRealUow newsUow = new NewsUOWs.NewsRealUow(DefaultManagerInit(ConfigurationManager.AppSettings["OrientUnitTestDB"]));
          return newsUow;
        }
        public static void JsonManagerCheck()
        {
          string hs ="{ \"GUID\": \"542ceb48-8454-11e4-acb0-00c2c66d13b0\", \"Holidays\": [{ \"Position\": \"Главный специалист\", \"Holidays\": [{ \"LeaveType\": \"Основной\", \"Days\": 13 }] }, { \"Position\": \"Ведущий специалист\", \"Holidays\": [{ \"LeaveType\": \"Основной\", \"Days\": 13 }] }] } ";
          hs =
    "[ { \"GUID\": \"542ceb48-8454-11e4-acb0-00c2c66d13b0\", \"Position\": \"Главный специалист\", \"Holidays\": [ { \"LeaveType\": \"Основной\", \"Days\": 13 } ] }, { \"GUID\": \"542ceb48-8454-11e4-acb0-00c2c66d13b0\", \"Position\": \"Ведущий специалист\", \"Holidays\": [ { \"LeaveType\": \"Основной\", \"Days\": 0 } ] } ] ";
          JSONManager jm = new JSONManager();

          IEnumerable<List<AdinTce.Holiday>> a = jm.DeserializeFromParentChildren<List<AdinTce.Holiday>>(hs, "Holidays");
        }
        
        public static void BatchBodyContentCheck()
        {
            WebRequest request=WebRequest.Create("http://localhost:2480/batch/test_db");

            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + System.Convert.ToBase64String(
              Encoding.ASCII.GetBytes("root:root")
              ));

            string stringData="{\"transaction\":true,\"operations\":[   {\"type\":\"script\",\"language\":\"sql\",\"script\":[   \"Create Vertex Person content {\"Name\":\"0\",\"GUID\":\"1\",\"Created\":\"2017-01-01 00:00:00\",\"Changed\":\"2017-01-01 00:00:00\"}\"   ]}]}"; //place body here
            var data=Encoding.ASCII.GetBytes(stringData); // or UTF8

            request.Method="POST";
            request.ContentType=""; //place MIME type here
            request.ContentLength=data.Length;

            var newStream=request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
           

            try
            {
                var a=(HttpWebResponse)request.GetResponse();
            }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

        }     
        public static void AuthCheck()
        {
            string res=UserAuthenticationMultiple.UserAcc();
        }
        public static void QuizNewCheck(){            

            OrientRepo repo=DefaultManagerInit();
            JSONManager jm = new JSONManager();
            POCO.QuizItemNew quizItemAdd = null;            
            Quizes.QuizNewUOW quizUOW=new Quizes.QuizNewUOW(repo);
            
            //generate quizes from C# code to send to Angular IO test only
            //POCO.QuizItemNew qitm = quizUOW.QuizGenerate();

            //init quizes from json exported from Angular for test purposes only
            string str = File.ReadAllText(@"C:\111\quizes_ang.json");
            quizItemAdd = quizUOW.UOWdeserialize<POCO.QuizItemNew>(str);

            //delete all quiz objects and classes and recreate Quiz classes
            quizUOW.ReInitClasses();
            
            //create quiz item
            quizUOW.QuizItemAdd(quizItemAdd);

            POCO.QuizItemNew quizItemGet = null;
            quizItemGet = quizUOW.QuizItemGet();
            string snd=jm.SerializeObject(quizItemGet);
            File.WriteAllText(@"C:\111\quizes_sharp.json",snd);

        }
        public static void AdinTceCheck(){
            AdinTce.AdinTceRepo adinTceRepo = new AdinTce.AdinTceRepo();
            string res=adinTceRepo.HoliVation("ba124b8e-9857-11e7-8119-005056813668");
        }
        public static void AdinTceTest()
        {
            AdinTce.AdinTceRepo adinTceRepo = new AdinTce.AdinTceRepo();

            IOrientRepo repo = RepoFactory.NewOrientRepo("Orgchart_prod_2", "http://msk1-vm-indb01:2480","root", "mR%mzJUGq1E");
            
            IUOWs.UOW uow = new IUOWs.UOW(repo);

            IEnumerable<Person> persons = uow.GetItems<Person>();
            string res="";
            foreach (Person i in persons)
            {
                res = adinTceRepo.HoliVation(i.GUID);
            }

            //"0629685d-7e6b-11e7-8119-005056813668"
            //"f58c67e4-ab78-11e4-b308-f80f41d3dd35"
            

        }


        //API testing mehod
        public static void APItester_sngltnCheck()
        {
            APItester_sngltn at=new APItester_sngltn();
            at.Initialize();
            at.GO();
        }
        //DATABASE BOILERPLATE
        public static void GenDevDB(bool recreate = false,bool cleanUpAter=false,bool newsGen=true)
        {

    List<News> news_=new List<News>(){};
    List<Commentary> comments_=new List<Commentary>() { };

    Managers.Manager mng=new Managers.Manager("dev_db");
    //CREATE DB
    mng.GenDB(true, cleanUpAter);
    //GENERATE NEWS,COMMENTS
    mng.GenNewsComments(newsGen,true);

        }
        public static void GenTestDB(bool cleanUpAter=false,bool newsGen=true)
        {

    List<News> news_ = new List<News>() { };
    List<Commentary> comments_ = new List<Commentary>() { };

    Managers.Manager mng = new Managers.Manager("test_db");
    //CREATE DB
    mng.GenDB(cleanUpAter);
    //GENERATE NEWS,COMMENTS
    mng.GenNewsComments(newsGen,true);

        }    

        //FUNCTIONAL TESTS
        public static void UOWFunctionalCheck()
        {
            //AdinTceTest();

            //AdinTceCheck();

            QuizNewCheck();

            //GetPersonCheck();

            //JsonToTypeList.GO();

            //test new quizes
            //Quizes.QuizUOWTest.GO();

            //Check LinqToContext
            //LinqToContextCheck.GO();

            //moove database
            //UOWMooveDb();
        }

        //MOOVE DB
        public static void UOWMooveDb()
        {
            Managers.Manager mngFrom1=new Managers.Manager("dev_db","http://msk1-vm-ovisp02:2480","root","I9grekVmk5g");
            Managers.Manager mngFrom2=new Managers.Manager("news_test5","http://msk1-vm-ovisp02:2480","root","I9grekVmk5g");

            //msk1-vm-indb01.nspk.ru
            //mR%mzJUGq1E
            Managers.Manager mngTo=new Managers.Manager("news_test_for_prod","http://msk1-vm-ovisp02:2480","root","I9grekVmk5g");
                //Managers.Manager mngTo=new Managers.Manager("news_prod","http://msk1-vm-indb01.nspk.ru:2480","root","mR%mzJUGq1E");

            List<IOrientObjects.IOrientDefaultObject> classes_=new List<IOrientObjects.IOrientDefaultObject>();
            classes_.Add(new Note());
            classes_.Add(new Authorship());
           
            //migrate class names and shemas from actual DB
            DbSync.MooveDB.Migrate(mngTo,mngFrom2,classes_,null,true,false);
            //migrate ral persons from actual person DB
            DbSync.MooveDB.Migrate(mngTo,mngFrom1,null, classes_, false,false);
        }
        //Exclusive person moove
        public static void UOWMovePersonFromProd()
        {    
          //!!! PROD DATABASE FOR PERSON SYNC !!!
          //!!!
          //Managers.Manager mngPerson=new Managers.Manager("Orgchart_prod","http://msk1-vm-indb01:2480","root","mR%mzJUGq1E");
          //!!!

          /*
          testing Chilinyak
          Чили
          13da7c6ca09a755dc45553bce03723f7
          a.chilinyak
          */
        }
      
    }
  
}
