
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

      //check linq to context
      //LinqToContextCheck.GO();

      //FUCNTIONAL CHECK
      ManagerCheck.UOWFunctionalCheck();
      
      //START API TEST
      ManagerCheck.APItester_sngltnCheck();

      //QUIZ CHECK
      //ManagerCheck.QuizCheck();

    }

  }

  //move to tests except DB generating
  public static class ManagerCheck
  {    
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

    static OrientRepo DefaultManagerInit(string databaseName=null,string hostPort_=null)
    {
      string dbName;
      string login = ConfigurationManager.AppSettings["orient_login"];
      string password = ConfigurationManager.AppSettings["orient_pswd"];
      string dbHost = string.Format("{0}:{1}"
          , ConfigurationManager.AppSettings["OrientDevHost"]
          , ConfigurationManager.AppSettings["OrientPort"]);
      if (databaseName == null)
      {
        dbName = ConfigurationManager.AppSettings["OrientUnitTestDB"];
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

      return new OrientRepo(typeConverter, jsonMnager, tokenFactory, UrlShema, bodyShema, commandShema_
      , webRequestManager, webResponseReader, commandFactory, formatFactory, orientQueryFactory, orientCLRconverter);

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
    public static void QuizCheck()
    {
      Quizes.QuizRepo qr=new Quizes.QuizRepo();
      qr.Quiz();
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

    //API testing mehod
    public static void APItester_sngltnCheck()
    {
      APItester_sngltn at=new APItester_sngltn();
      at.Initialize();
      at.GO();
    }
    //DATABASE BOILERPLATE
    public static void GenDevDB(bool cleanUpAter=false,bool newsGen=true)
    {

List<News> news_=new List<News>(){};
List<Commentary> comments_=new List<Commentary>() { };

Managers.Manager mng=new Managers.Manager("dev_db");
//CREATE DB
mng.GenDB(cleanUpAter);
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
      
      //GET CLASS
      Managers.Manager mngCl = new Managers.Manager("dev_db",null);
      IOrientRepo rp = mngCl.GetRepo();
      GetClass gc = rp.GetClass<Person>("dev_db", null);



      //Managers.Manager mng = new Managers.Manager(ConfigurationManager.AppSettings["OrientDevDB"],null);
      Managers.Manager mng = new Managers.Manager(ConfigurationManager.AppSettings["OrientUnitTestDB"],null);
      Managers.Manager mngSource = new Managers.Manager(ConfigurationManager.AppSettings["OrientSourceDB"],null);
      PersonUOWs.PersonUOW pu=mng.GetPersonUOW();
      NewsUOWs.NewsRealUow nu=mng.GetNewsUOW();

      Managers.Manager mngPerson=new Managers.Manager(ConfigurationManager.AppSettings["OrientSourceDB"]);

      PersonUOWs.PersonUOW personToGetUOW=mngPerson.GetPersonUOW();
      
      POCO.News newsToAdd0 = new News() { GUID = "119", content = "s \"a \"a  t " };
      POCO.Person newsMaker = pu.SearchByName("Neprintsevia").FirstOrDefault();
      POCO.Person likeMaker = pu.SearchByName("Person1").FirstOrDefault();
      POCO.Person troubleMaker = pu.SearchByName("Person0").FirstOrDefault();

      GETparameters gp = new GETparameters() {offest=5,published=true,pinned=true,asc=true,author=newsMaker };
      JSONManager jm = new JSONManager();
      

      //ABSENT PERSON CHECK
      Random rnd = new Random();
      
      //News ns = nu.GetNewsByGUID("2370b972-48d4-4e49-95ad-b99ba5382042");
      //News ns = nu.GetNewsByGUID("e7bc87ec-f649-4748-b4cb-d2863f780f1c");
      //nu.GetNewsByGUID("f7557c27-f889-4aab-91ce-ba15e34e3981");
      //News ns = nu.GetNewsByGUID("f7557c27-f889-4aab-91ce-ba15e34e3981");

      var a=nu.GetNews(5,null,null);
      int acc = (int)rnd.Next(0, 10000);

      Person personAbsent = new Person() { Name = "PersonAbsent", sAMAccountName = "absent"+acc };
      string newsContent = "{\"conntent_\":\"news text\",\"name\":\"News name\"}";
            

    }

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
  
  //check Linq to context
  public static class LinqToContextCheck
  {
    static TestContext ts = new TestContext();
    public static void GO()
    {
      string st1=ts.ConditionFromExpressionTypes<TestEntity>(s=>s.Id>=1);
      string st2=ts.ConditionFromExpressionTypes<TestEntity>(s=>s.name=="test name");
      string st3=ts.ConditionFromExpressionTypes<TestEntity>(s=>s.intrinsicIsTrue==true);
      string st4=ts.ConditionFromExpressionTypes<TestEntity>(s=>s.tp.isTrue==true);
    }
  }
}
