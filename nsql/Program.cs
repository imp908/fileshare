
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
using Repos;


using System.Net;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Reflection;

using System.IO;

namespace NSQLManager
{

  class OrientDriverConnnect
  {

    static void Main(string[] args)
    {
          
      //mng.GenDB();
      //mng.GenNewsComments();
          
      RepoCheck rc=new RepoCheck();
      //rc.UOWFunctionalCheck();
          
      rc.GenTestDB();

  //QUIZ CHECK
  //rc.QuizCheck();

  //check absent person insert
  //rc.UOWCheckPersonCreation();

  //MIGRATING PERSON, UPDATING NOTES
  //rc.UOWFunctionalCheck();

//check structural or generated obj createion
  //RepoCheck.startcond sc=RepoCheck.startcond.MNL;
  //rc.UOWRandomcheck(sc);
//check manual object behaviour
  //rc.UOWstringobjectCheck();

    }

  }

  public class RepoCheck
  {

    JSONManager jm;
    OrientTokenBuilder tb;
    TypeConverter tc;
    CommandBuilder ocb;
    OrientWebManager wm ;
    WebResponseReader wr;

    Repo repo;
    Person p;
    Unit u;
    SubUnit s;

    MainAssignment m;
    List<string> lp,lu;

    UserSettings us;
    CommonSettings cs;
    string guid_;

    public enum startcond {RNDGEN,MNL};

    public RepoCheck()
    {           
            
        jm=new JSONManager();
        tb=new OrientTokenBuilder();
        tc=new TypeConverter();
        ocb=new OrientCommandBuilder(new TokenMiniFactory(), new FormatFactory());
        wm=new OrientWebManager();
        wr=new WebResponseReader();

        us=new UserSettings() {showBirthday=true};
        cs=new CommonSettings();
            
        repo=new Repo(jm, tb, tc, ocb, wm, wr);
         
        s=new SubUnit();

        p =
new Person() {Name="0", GUID="0", changed=new DateTime(2017, 01, 01, 00, 00, 00), created=new DateTime(2017, 01, 01, 00, 00, 00)};

        u =
new Unit() {Name="0", GUID="0", changed=new DateTime(2017, 01, 01, 00, 00, 00), created=new DateTime(2017, 01, 01, 00, 00, 00)};

        m =
new MainAssignment() { GUID="0", changed=new DateTime(2017, 01, 01, 00, 00, 00), created=new DateTime(2017, 01, 01, 00, 00, 00)};
            
        lp=new List<string>();
        lu=new List<string>();

        guid_="ba124b8e-9857-11e7-8119-005056813668";

    }


    void GO()
    {            

        BatchBodyContentCheck();

        TrackBirthdaysPtP();
        DeleteBirthdays();
        DbCreateDeleteCheck();
        AddCheck();
        APItester_sngltnCheck();
                        
        TrackBirthdaysOneToAll();
            
        DeleteCheck();
        ExplicitCommandsCheck();
        BirthdayConditionAdd();

    }

    public OrientRepo DefaultManagerInit(string databaseName=null,string hostPort_=null)
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
    public NewsUOWs.NewsRealUow ActualNewsUOW()
    {
      NewsUOWs.NewsRealUow newsUow = new NewsUOWs.NewsRealUow(DefaultManagerInit("test_db"));
      return newsUow;
    }
        
    public void PropCheck()
    {
      propSearch<Person>(new Person() { Seed = 123  });
    }
    public void propSearch<T>(T item)
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


    public void JsonManagerCheck()
    {
        string hs ="{ \"GUID\": \"542ceb48-8454-11e4-acb0-00c2c66d13b0\", \"Holidays\": [{ \"Position\": \"Главный специалист\", \"Holidays\": [{ \"LeaveType\": \"Основной\", \"Days\": 13 }] }, { \"Position\": \"Ведущий специалист\", \"Holidays\": [{ \"LeaveType\": \"Основной\", \"Days\": 13 }] }] } ";
        hs =
"[ { \"GUID\": \"542ceb48-8454-11e4-acb0-00c2c66d13b0\", \"Position\": \"Главный специалист\", \"Holidays\": [ { \"LeaveType\": \"Основной\", \"Days\": 13 } ] }, { \"GUID\": \"542ceb48-8454-11e4-acb0-00c2c66d13b0\", \"Position\": \"Ведущий специалист\", \"Holidays\": [ { \"LeaveType\": \"Основной\", \"Days\": 0 } ] } ] ";
        JSONManager jm = new JSONManager();

        IEnumerable<List<AdinTce.Holiday>> a = jm.DeserializeFromParentChildren<List<AdinTce.Holiday>>(hs, "Holidays");
    }
    public void QuizCheck()
    {
        Quizes.QuizRepo qr=new Quizes.QuizRepo();
        qr.Quiz();
    }
    public void BatchBodyContentCheck()
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
    public void APItester_sngltnCheck()
    {
        APItester_sngltn at=new APItester_sngltn();
        at.Initialize();
        at.GO();
    }
    public void AddCheck()
    {
        int lim=10;

        for (int i=0; i <= lim; i++)
        {
            lp.Add(jm.DeserializeFromParentNode<Person>(repo.Add(p), new RESULT().Text).Select(s => s.id.Replace(@"#","")).FirstOrDefault());
            lu.Add(jm.DeserializeFromParentNode<Unit>(repo.Add(u), new RESULT().Text).Select(s=>s.id.Replace(@"#", "")).FirstOrDefault());              
        }
        for (int i=0; i <= lim/2; i++)
        {             
            repo.Add(m, new TextToken() {Text=lu[i]}, new TextToken() {Text=lp[i + 1]});                
        }
        for (int i=0; i <= lim / 2; i++)
        {
            repo.Add(s, new TextToken() {Text=lu[i]}, new TextToken() {Text=lu[i + 1]});
        }

    }
    public void DeleteCheck()
    {
        string str;
        str=repo.Delete(typeof(Person), new TextToken() {Text=@"Name =0"});
        str=repo.Delete(typeof(Unit), new TextToken() {Text=@"Name =0"});
        str=repo.Delete(typeof(MainAssignment), new TextToken() {Text=@"Name =0"});
        str=repo.Delete(typeof(SubUnit), new TextToken() {Text=@"Name =0"});
    }
    public void ExplicitCommandsCheck()
    {

        OrientCommandBuilder cb=new OrientCommandBuilder(new TokenMiniFactory(), new FormatFactory());
        OrientTokenBuilderExplicit eb=new OrientTokenBuilderExplicit();
        ITypeTokenConverter tc=new TypeConverter();

        List<IQueryManagers.ITypeToken> lt=new List<IQueryManagers.ITypeToken>();
        List<string> ls=new List<string>();

      
lt=eb.Create(new OrientClassToken() {Text="VSCN"}, new OrientClassToken() {Text="V"});           
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt=eb.Create(new OrientClassToken() {Text="VSCN"}, new OrientPropertyToken() {Text="Name"}, new OrientSTRINGToken(), true,true);
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt=eb.Create(new OrientClassToken() {Text="VSCN"}, new OrientPropertyToken() {Text="Created"}, new OrientDATEToken(), true, true);
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);



lt=eb.Create(new OrientClassToken() {Text="ESCN"}, new OrientClassToken() {Text="E"});
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt=eb.Create(new OrientClassToken() {Text="ESCN"}, new OrientPropertyToken() {Text="Name"}, new OrientSTRINGToken(), true, true);
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt=eb.Create(new OrientClassToken() {Text="ESCN"}, new OrientPropertyToken() {Text="Created"}, new OrientDATEToken(), true, true);
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);



lt=eb.Create(new OrientClassToken() {Text="VSCN"}, new OrientClassToken() {Text="VSCN"});
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt=eb.Create(new OrientClassToken() {Text="Beer"}, new OrientClassToken() {Text="VSCN"});
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);

lt=eb.Create(new OrientClassToken() {Text="Produces"}, new OrientClassToken() {Text="ESCN"});
ls.Add(new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), lt, new TextFormatGenerate(lt)).Text.Text);



    }
    public void BirthdayConditionAdd()
    {

        List<string> persIds=new List<string>();
        List<string> edgeIds=new List<string>();

        var PersList=jm.DeserializeFromParentNode<Person>(
    repo.Select(
        typeof(Person),
    new TextToken() {Text="1=1 and outE(\"CommonSettings\").inv(\"UserSettings\").showBirthday[0] is null"})
    , new RESULT().Text);

        foreach(Person pers in PersList)
        {
            persIds.Add(pers.id.Replace(@"#", ""));
        }          

        for (int i=0; i < persIds.Count(); i++)
        {
            string id=jm.DeserializeFromParentNode<UserSettings>(repo.Add(us), new RESULT().Text).Select(s => s.id.Replace(@"#", "")).FirstOrDefault();

            repo.Add(cs, new TextToken() {Text=persIds[i]}, new TextToken() {Text=id});
        }


        repo.Delete(typeof(UserSettings), new TextToken() {Text=@"1 =1"});
        repo.Delete(typeof(CommonSettings), new TextToken() {Text=@"1 =1"});

    }
    public void DbCreateDeleteCheck()
    {

        repo.Add(new TextToken() {Text="test_db"}, new DELETE());
        repo.Add(new TextToken() {Text="test_db"}, new POST());

    }
    public void TrackBirthdaysOneToAll()
    {
        repo.changeAuthCredentials(
            ConfigurationManager.AppSettings["ParentLogin"]
            , ConfigurationManager.AppSettings["ParentPassword"]
            );

        PersonUOWs.PersonUOWold pUOW=new PersonUOWs.PersonUOWold();
        TrackBirthdays tb=new TrackBirthdays();

        Person fromPerson=pUOW.GetObjByGUID(guid_).FirstOrDefault();
        List<Person> personsTo=pUOW.GetAll().ToList();
        List<string> ids=new List<string>() {};
            
        foreach (Person pt in personsTo)
        {
            ids.Add(repo.Add(tb, fromPerson, pt));
        }

        repo.Delete(typeof(TrackBirthdays), new TextToken() {Text= "1=1"} );

    }
    public void TrackBirthdaysPtP()
    {
        repo.changeAuthCredentials(
            ConfigurationManager.AppSettings["orient_login"]
            , ConfigurationManager.AppSettings["orient_pswd"]
            );

        PersonUOWs.PersonUOWold pUOW=new PersonUOWs.PersonUOWold();
        TrackBirthdays tb=new TrackBirthdays();
        List<Person> personsTo=pUOW.GetAll().Take(3).ToList();
        string personsfrom=null;

        List<string> ids=new List<string>() {};

        repo.Delete(typeof(TrackBirthdays), new TextToken() {Text="1=1"});

        foreach (Person pf in personsTo)
        {
            foreach (Person pt in personsTo)
            {
                if (pf.GUID != pt.GUID)
                {
                    ids.Add(pUOW.AddTrackBirthday(tb, pf.GUID, pt.GUID));
                }
            }               
        }

        personsfrom=pUOW.GetTrackedBirthday(personsTo.FirstOrDefault().GUID);

        foreach (Person pf in personsTo)
        {
            foreach (Person pt in personsTo)
            {
                if (pf.GUID != pt.GUID)
                {
                    ids.Add(pUOW.DeleteTrackedBirthday(tb, pf.GUID, pt.GUID));
                }
            }
        }

           
    }
    public void DeleteBirthdays()
    {
        repo.changeAuthCredentials(
          ConfigurationManager.AppSettings["ParentLogin"]
          , ConfigurationManager.AppSettings["ParentPassword"]
          );

        PersonUOWs.PersonUOWold pUOW=new PersonUOWs.PersonUOWold();
        TrackBirthdays tb=new TrackBirthdays();

    }
    public void AuthCheck()
    {
        string res=UserAuthenticationMultiple.UserAcc();
    }
    public void UOWRandomcheck(startcond sc_)
    {
           
List<string> idioticNews = new List<string> {
"Fire on the sun!","Internet trolls are jerks","Bugs flying around with wings are flying bugs","Chuck norris facts are they true?"
,"Health officials: Pools and diarea not good mix","Tiger wood He's goo at Golf","A nuclear explosion would be adisaster",
"Russians are comming!","Rain creates wet roads"
};

List<string> bullshitComments = new List<string>
{
"Supper!","Awasome!","How could this happen!","i am stonned...","Ggggg...","Summer is my favorite month","aaa","bbb"
,"c","d","e","f","g","h","i","h","j"
};

//select expand(outE('Authorship').inV('Note')) from Person
//traverse both() from (select expand(outE('Authorship').inV('Note')) from Person)
    
NewsUOWs.NewsUowOld newsUOW = new NewsUOWs.NewsUowOld(ConfigurationManager.AppSettings["OrientUnitTestDB"]);

        List<Note> newsCreated = new List<Note>();
        List<Note> commentsCreated = new List<Note>();
        List<Person> personsCreated = new List<Person>();
        List<Person> personsToAdd = new List<Person>();         

        List<Note> newsToAdd = new List<Note>();
        foreach(string noteCont in idioticNews)
        {

            newsToAdd.Add(new Note(){name=noteCont,content="here goes text of really fucking interesting news"});               
        }

           
        List<Note> commentsToAdd=new List<Note>();
        foreach(string comment in bullshitComments)
        {             
            commentsToAdd.Add(new Note(){name=comment,content="bullshit comment"});
        }

        personsToAdd=newsUOW.GetOrientObjects<Person>(null).ToList();            
        Random rnd=new Random();
        int newsCount=idioticNews.Count()-1;
        int commentsCount=bullshitComments.Count()-1;

        Note newsNewToAdd=new Note(){ pic="", GUID = "ABC",name="name1",content="news cont"};
        Note newsNewAdded=newsUOW.CreateNews(personsToAdd[0],newsToAdd[0]);
        string newsAdd=newsUOW.NoteToString(newsNewAdded);
        string str
= "{\"GUID\":\"abc\",\"pic\":\"acs\",\"name\":\"test name\",\"content\":\"test content\",\"description\":\"\",\"commentDepth\":0,\"hasComments\":false,\"@type\":\"d\",\"@rid\":\"#16:0\",\"@version\":\"1\",\"@class\":\"Note\"}";

        Note newsRec = newsUOW.StringToNote(str);

        //Note newsRec=newsUOW.StringToNote(str);

        if (sc_==startcond.MNL)
        {

            //NEWS
            //p0-[atrsh]->news0
            newsCreated.Add(newsUOW.CreateNews(personsToAdd[0],newsToAdd[0]));
            //p0-[atrsh]->news1
            newsCreated.Add(newsUOW.CreateNews(personsToAdd[0],newsToAdd[1]));
            //p1-[atrsh]->news2
            newsCreated.Add(newsUOW.CreateNews(personsToAdd[1],newsToAdd[2]));

            //COMMENTS
            //p1-[atrsh]->(commentary)<-[comment0]-news0
            commentsCreated.Add(newsUOW.CreateCommentary(personsToAdd[1], 
                new Note(){name="Comment",content="bullshit comment"}, newsCreated[0]));
            //p3-[atrsh]->(commentary)<-[comment1]-news0
            commentsCreated.Add(newsUOW.CreateCommentary(personsToAdd[3], commentsToAdd[1],newsCreated[0]));
            //p0-[atrsh]->(commentary)<-[comment2]-news2
            commentsCreated.Add(newsUOW.CreateCommentary(personsToAdd[0], commentsToAdd[2],newsCreated[2]));
            //p1-[atrsh]->(commentary)<-[comment3]-comment1
            commentsCreated.Add(newsUOW.CreateCommentary(personsToAdd[1], commentsToAdd[3], commentsCreated[1]));

            //p3-[atrsh]->(commentary)<-[comment4]-comment3
            commentsCreated.Add(newsUOW.CreateCommentary(personsToAdd[3], commentsToAdd[4], commentsCreated[3]));
            //p0-[atrsh]->(commentary)<-[comment5]-comment4
            commentsCreated.Add(newsUOW.CreateCommentary(personsToAdd[0], commentsToAdd[5], commentsCreated[4]));

            //p2-[atrsh]->(commentary)<-[comment6]-news0
            commentsCreated.Add(newsUOW.CreateCommentary(personsToAdd[2], commentsToAdd[6], newsCreated[0]));

            //p1-[atrsh]->(commentary)<-[comment7]-comment6
            commentsCreated.Add(newsUOW.CreateCommentary(personsToAdd[1], commentsToAdd[7], commentsCreated[6]));
            //p3-[atrsh]->(commentary)<-[comment8]-comment6
            commentsCreated.Add(newsUOW.CreateCommentary(personsToAdd[3], commentsToAdd[8], commentsCreated[6]));

            //UPDATE NEWS
            //change news 0                  
            Note addedNews=newsUOW.GetNewsByGUID("2eb7ec8c-ddcb-4149-994d-aa5517a0b078");
            addedNews.content="updated content 10";
            Note updatedNews=newsUOW.UpdateNews(addedNews);

        }

        if (sc_ == startcond.RNDGEN)
        {
            //generate news 
            foreach (Person p in personsToAdd)
            {
                //news published count
                int gap = (int)rnd.Next(0, 5);

                if (gap > 0)
                {
                    for (int i = 0; i <= gap; i++)
                    {
                        //news index
                        int newsRndInd = (int)rnd.Next(0, newsCount - 1);
                        newsCreated.Add(newsUOW.CreateNews(p, newsCreated[newsRndInd]));
                    }
                }
            }

            List<int> cnt = new List<int>();
            //Generate commentaries
            foreach (Person p in personsToAdd)
            {
                //comments count
                int gap = (int)rnd.Next(0, 5);

                if (gap > 0)
                {
                    for (int i = 0; i <= gap; i++)
                    {
                        //created news gap
                        newsCount = newsCreated.Count();
                        //news index
                        int newsRndInd = (int)rnd.Next(0, newsCount - 1);
                        int commentRndInd = (int)rnd.Next(0, commentsCount - 1);
                        commentsCreated.Add(newsUOW.CreateCommentary(p, newsCreated[newsRndInd], commentsCreated[commentRndInd]));

                    }
                }


                cnt.Add(newsUOW.GetPersonNews(p).Count());
            }
        }

        //traverse both() from (select from Person)
        foreach (Note ntd in newsCreated)
        {
            newsUOW.DeleteNews(p, ntd.id);
        }        

    }
                     
        
    //DATABASE BOILERPLATE
    public void GenDevDB(bool cleanUpAter=false,bool newsGen=true)
    {

Managers.Manager mng = new Managers.Manager("dev_db");
//CREATE DB
mng.GenDB(cleanUpAter);
//GENERATE NEWS,COMMENTS
mng.GenNewsComments(newsGen);

    }
    public void GenTestDB(bool cleanUpAter=false,bool newsGen=true)
    {

Managers.Manager mng = new Managers.Manager("test_db");
//CREATE DB
mng.GenDB(cleanUpAter);
//GENERATE NEWS,COMMENTS
mng.GenNewsComments(newsGen);

    }
      
    //FUNCTIONAL TESTS
    public void UOWFunctionalCheck()
    {
           
      //Managers.Manager mng = new Managers.Manager(ConfigurationManager.AppSettings["OrientUnitTestDB"],null);
      Managers.Manager mng = new Managers.Manager(ConfigurationManager.AppSettings["OrientDevDB"],null);
      PersonUOWs.PersonUOW pu=mng.GetPersonUOW();
      NewsUOWs.NewsRealUow nu=mng.GetNewsUOW();

      Managers.Manager mngPerson=new Managers.Manager(ConfigurationManager.AppSettings["OrientSourceDB"],null);
      PersonUOWs.PersonUOW personToGetUOW=mngPerson.GetPersonUOW();

      List<Note> notes0 = nu.GetByOffset("153c2d01-6def-4bcc-97fe-85b051fd8532", 50).ToList();

      Person commenter_ = pu.GetPersonByGUID("88906e68-e697-11e5-80d4-005056813668");
      News newsTocomment = nu.GetNewsByGUID("153c2d01-6def-4bcc-97fe-85b051fd8532");
      Commentary comment = new Commentary() { content = "nt_0" };
      comment=nu.CreateCommentary(commenter_, comment, newsTocomment);


      //GET check
      IEnumerable <Note> notes=nu.GetByOffset("558d95f3-964e-45f3-9708-2ee964aa2854", 2);
      IEnumerable<Note> news=nu.GetNews(null);

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
        IEnumerable<News> ns=nu.GetNews(5);
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

      var a=nu.GetNews(5);

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

      //COMMENTARY UPDATE CHECK
      newsAdded.content="Updated news";
      News commentUpdated1=nu.UpdateNews(personFromTest,newsAdded);
      commentAdded.content="Updated comment";
      Commentary commentUpdated2=nu.UpdateCommentary(personFromTest,commentAdded);
            
    }            
    
  }

}

