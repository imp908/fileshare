
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

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

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
      
      //START API TEST
      ManagerCheck.APItester_sngltnCheck();

      //QUIZ CHECK
      //ManagerCheck.QuizCheck();

    }

  }

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
      NewsUOWs.NewsRealUow newsUow = new NewsUOWs.NewsRealUow(DefaultManagerInit("test_db"));
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

Managers.Manager mng = new Managers.Manager("dev_db");
//CREATE DB
mng.GenDB(cleanUpAter);
//GENERATE NEWS,COMMENTS
mng.GenNewsComments(newsGen);

    }
    public static void GenTestDB(bool cleanUpAter=false,bool newsGen=true)
    {

Managers.Manager mng = new Managers.Manager("test_db");
//CREATE DB
mng.GenDB(cleanUpAter);
//GENERATE NEWS,COMMENTS
mng.GenNewsComments(newsGen);

    }    
      
    //FUNCTIONAL TESTS
    public static void UOWFunctionalCheck()
    {
      
      Managers.Manager mng = new Managers.Manager(ConfigurationManager.AppSettings["OrientDevDB"],null);
      //Managers.Manager mng = new Managers.Manager(ConfigurationManager.AppSettings["OrientUnitTestDB"],null);
      PersonUOWs.PersonUOW pu=mng.GetPersonUOW();
      NewsUOWs.NewsRealUow nu=mng.GetNewsUOW();

      Managers.Manager mngPerson=new Managers.Manager(ConfigurationManager.AppSettings["OrientSourceDB"],null);
      PersonUOWs.PersonUOW personToGetUOW=mngPerson.GetPersonUOW();
      
      POCO.News newsToAdd0 = new News() { GUID = "119", content = "s \"a \"a  t " };
      POCO.Person newsMaker = nu.SearchByName("Neprintsevia").FirstOrDefault();
      
      GETparameters gp = new GETparameters() {offest=5,published=true,pinned=true,asc=true,author=newsMaker };
      JSONManager jm = new JSONManager();

      List<Note> notesCreated=(from s in nu.GetOrientObjects<Note>() where s.authGUID!=null select s).ToList();
      if(notesCreated.Count()>0)
      {
        Note noteToLike=notesCreated[0];
        Liked lk=nu.LikeNote(noteToLike,newsMaker);
        IEnumerable<Note> notesLiked=nu.GetPersonNewsHCSelectCond(5, null, null,true, true, null);
      }

      //PARAMS OBJ CHECK
      string gps = jm.SerializeObject(gp);
      string res =mng.GetNewsHC(gp);
      gp = new GETparameters() {offest=5,published=true};
      res =mng.GetNewsHC(gp);

      //NEWS POSTE CHECK
      string newsToAdded0=mng.PostNews(newsToAdd0, null);

      //GET BY OFFSET CHECK
      List<Note> notes0 = nu.GetByOffset("153c2d01-6def-4bcc-97fe-85b051fd8532", 50).ToList();


      Person commenter_ = pu.GetPersonByGUID("88906e68-e697-11e5-80d4-005056813668");
      News newsTocomment = nu.GetNewsByGUID("153c2d01-6def-4bcc-97fe-85b051fd8532");
      Commentary comment = new Commentary() { content = "nt_0" };
      comment=nu.CreateCommentary(commenter_, comment, newsTocomment);    

      //GET check
      IEnumerable <Note> notes=nu.GetByOffset("558d95f3-964e-45f3-9708-2ee964aa2854", 2);
      IEnumerable<Note> news=nu.GetNews(null,null,null);

      //PersonsWith news
      List<Person> persons=nu.GetOrientObjects<Person>().ToList();
      //get by date check          
      Person personUpdatesNews=persons[0];
      Person personNewsUpdated=persons[1];

      News newToupdate=nu.CreateNews(personNewsUpdated,new News(){content="content created"});
          
      if(newToupdate!=null)
      {
        newToupdate.content="Updated";
        nu.UpdateNote(personUpdatesNews, newToupdate);
        newToupdate=nu.GetNewsByGUID(newToupdate.GUID);
        Note updatedNote=nu.UpdateNotePersonal(personUpdatesNews,newToupdate);
        IEnumerable<News> ns=nu.GetNews(5,null,null);
      }

      Person pers = nu.SearchByName("neprintsev").FirstOrDefault();
      Note netoupdate2 = nu.GetNoteByGUID("34b0cd78-2931-4e7b-8533-5d9ea6ee982b");
      Note updatedNote2=nu.UpdateNote(pers,netoupdate2);

      //test author news update comment not update
      Person someGuy=nu.GetOrientObjects<Person>(null).ToList()[0];
      News newsToAdd=new News() { name="TestNews",content="content" };
      News newsTestTime=nu.CreateNews(someGuy,newsToAdd);            
         
      //test personal update        

      //authored commentaries
      IEnumerable<Commentary> commentaries = from s in nu.GetOrientObjects<Commentary>() where s.author_ != null select s;


      //ABSENT PERSON CHECK
      Random rnd = new Random();
      int acc = (int)rnd.Next(0, 10000);
      //News ns = nu.GetNewsByGUID("2370b972-48d4-4e49-95ad-b99ba5382042");
      //News ns = nu.GetNewsByGUID("e7bc87ec-f649-4748-b4cb-d2863f780f1c");
      //nu.GetNewsByGUID("f7557c27-f889-4aab-91ce-ba15e34e3981");
      //News ns = nu.GetNewsByGUID("f7557c27-f889-4aab-91ce-ba15e34e3981");

      var a=nu.GetNews(5,null,null);

      Person personAbsent = new Person() { Name = "PersonAbsent", sAMAccountName = "absent"+acc };
      string newsContent = "{\"conntent_\":\"news text\",\"name\":\"News name\"}";
      News newsToinsert = nu.UOWdeserialize<News>(newsContent);
      News newsAdded=nu.CreateNews(personAbsent, newsToinsert);

      //NEWS CREATION CHECK
      Person personFromTest=personToGetUOW.GetPersonByAccount("Neprintsevia");
      News newsAdded2=nu.CreateNews(personFromTest, newsToinsert);
      string personFromStr = nu.UOWserialize<Person>(personFromTest);
      string newsCreatedStr = nu.UOWserialize<News>(newsAdded2);

      //COMMENTARY CREATION CHECK
      Commentary commentToAdd = nu.UOWdeserialize<Commentary>(newsContent);
      Commentary commentAdded = nu.CreateCommentary(personFromTest, commentToAdd, newsAdded2);
      string CommentaryCreatedStr = nu.UOWserialize<Commentary>(commentAdded);

      //NEWS UPDATE CHECK
      newsAdded.content="Updated news";
      News commentUpdated1=nu.UpdateNews(personFromTest,newsAdded);
      //COMMENT UPDATE CHECK
      commentAdded.content="Updated comment";
      Commentary commentUpdated2=nu.UpdateCommentary(personFromTest,commentAdded);
            
    }            
    
  }  
  
  
  public static class EFcheck
  {
    public enum Genre
    {
      Action,
      Humor,
      Fantasy,
    }

    public class Book
    {
      public int Id { get; set; }
      public string Title { get; set; }
      public Genre? Genre { get; set; }
      public AuthorName Author { get; set; }
    }

    public class AuthorName
    {
      public string First { get; set; }
      public string Last { get; set; }
    }

    public class UnicodeContext : DbContext
    {
      public DbSet<Book> Books { get; set; }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
        modelBuilder.Entity<Book>().Property(p => p.Title).IsUnicode(true);
      }
    }

    public class NonUnicodeContext : DbContext
    {
      public DbSet<Book> Books { get; set; }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
        modelBuilder.Entity<Book>().Property(p => p.Title).IsUnicode(false);
      }
    }
       
    public static string EFqueryCheck()
    {
      string str=null;

      const string expectedSql =
@"SELECT 
[Extent1].[Id] AS [Id]
FROM [dbo].[Books] AS [Extent1]
WHERE ([Extent1].[Title] IN (N'Title1', N'Title2')) 
OR ([Extent1].[Title] IS NULL)";

      var array = new[] { "Title1", "Title2", null };

      using (var context = new UnicodeContext())
      {
        ((IObjectContextAdapter)context).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior=false;

        var query = from book in context.Books
        where array.Contains(book.Title)
        select book.Id;

        str = Regex.Replace(query.ToString(), @"\s", string.Empty);

        bool res = Regex.Replace(query.ToString(), @"\s", string.Empty)==expectedSql;
      }
      return str;
    }
  
  }

}
