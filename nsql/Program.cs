
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
using PersonUOWs;
using POCO;

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
          Trash.FormatRearrange.StringsCheck();

          RepoCheck rc=new RepoCheck();
          RepoCheck.startcond sc=RepoCheck.startcond.MNL;

      //DELETE and regenerate DB from scratch
      //rc.ManagerCheck(false);
      //generate News, and commentaries
      //rc.UOWRealCheck(true);
      //check absent person insert
      //rc.UOWCheckPersonCreation();

      //functional check
      rc.UOWFunctionalCheck();

//check structural or generated obj createion
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

        public Manager DefaultManagerInit(string databaseName=null,string hostPort_=null)
        {
          string dbName;
          string login = ConfigurationManager.AppSettings["orient_login"];
          string password = ConfigurationManager.AppSettings["orient_pswd"];
          string dbHost = string.Format("{0}:{1}"
              , ConfigurationManager.AppSettings["ParentHost"]
              , ConfigurationManager.AppSettings["ParentPort"]);
          if (databaseName == null)
          {
              dbName = ConfigurationManager.AppSettings["ParentDB"];
          }
          else { dbName = databaseName; }
          if (hostPort_ == null)
          {
              dbHost = string.Format("{0}:{1}"
              , ConfigurationManager.AppSettings["ParentHost"]
              , ConfigurationManager.AppSettings["ParentPort"]);
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

          return new Manager(typeConverter, jsonMnager, tokenFactory, UrlShema, bodyShema, commandShema_
          , webRequestManager, webResponseReader, commandFactory, formatFactory, orientQueryFactory, orientCLRconverter);

        }
        public NewsUOWs.NewsRealUow ActualNewsUOW()
        {
          NewsUOWs.NewsRealUow newsUow = new NewsUOWs.NewsRealUow("test_db");
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
            string holidaysResp =
"{ \"GUID\": \"542ceb48-8454-11e4-acb0-00c2c66d13b0\", \"Holidays\": [{ \"Position\": \"Главный специалист\", \"Holidays\": [{ \"LeaveType\": \"Основной\", \"Days\": 13 }] }, { \"Position\": \"Ведущий специалист\", \"Holidays\": [{ \"LeaveType\": \"Основной\", \"Days\": 13 }] }] } ";
            string hs =
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
            catch (Exception e) {}

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

            PersonUOWold pUOW=new PersonUOWold();
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

            PersonUOWold pUOW=new PersonUOWold();
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

            PersonUOWold pUOW=new PersonUOWold();
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
    
NewsUOWs.NewsUow newsUOW = new NewsUOWs.NewsUow(ConfigurationManager.AppSettings["TestDBname"]);

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
        public void UOWstringobjectCheck()
        {
            NewsUOWs.NewsRealUow uow=new NewsUOWs.NewsRealUow("test_db");
            PersonUOW pUow=new PersonUOW("test_db");
            List<News> addedNews=new List<News>();

             string pStr1=
            "{\"Seed\":0,\"FirstName\":null,\"LastName\":null,\"MiddleName\":null,\"Birthday\":null,\"mail\":null,\"telephoneNumber\":null,\"userAccountControl\":null,\"objectGUID\":null,\"sAMAccountName\":\"acc1\",\"OneSHash\":null,\"Hash\":null,\"fieldTypes\":null,\"@class\":null,\"Name\":\"N10\",\"GUID\":\"g10\",\"Created\":\"2017-12-17 03:18:12\",\"Changed\":null,\"Disabled\":null}";
            Person p2=new Person() {GUID="g10",Name="N10",sAMAccountName="acc1",id="id1"};
            
            //SERIALIZATIONs
            string pStr2 = JsonConvert.SerializeObject(p2);

            Person pGen2=JsonConvert.DeserializeObject<Person>(pStr2);
            Person pGen1=JsonConvert.DeserializeObject<Person>(pStr1);

            Person testPerson = pUow.GetPersonByAccount("YablokovAV");

            Person person1=uow.UOWdeserialize<Person>(pStr1);
            string strperson1=uow.UOWserialize<Person>(person1);


            //FIELD HIDING CHECK
            OrientDefaultObject v1=new OrientDefaultObject() {GUID="g10",id="id1"};

            string v1Str = JsonConvert.SerializeObject(v1);

            OrientDefaultObject v2 = JsonConvert.DeserializeObject<OrientDefaultObject>(v1Str);
            OrientDefaultObject v3 =JsonConvert.DeserializeObject<OrientDefaultObject>(pStr1);

            TestPersonPOCO tp1 = new TestPersonPOCO() { Name="person1", Acc="acc1",rid="#54:7", version="v1"};
            string tpStr=JsonConvert.SerializeObject(tp1);
            TestPersonPOCO tp1Gen=JsonConvert.DeserializeObject<TestPersonPOCO>(tpStr);
            string tpStr2="{\"Name\":\"person1\",\"Acc\":\"acc1\",\"class\":null,\"type\":null,\"version\":\"v1\"}";
            TestPersonPOCO tp2Gen=JsonConvert.DeserializeObject<TestPersonPOCO>(tpStr2);



            //GET
            //content byte convert check
            string contenstr ="<p style=\"margin-left:0cm; margin-right:0cm\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\">С понедельника планируется работа по переводу сервисов RFC на новые типы заявок и бизнес-процессы, работа продлится до окончания фриза по изменениям в ОПКЦ системах.</span></span></p>↵↵<p style=\"margin-left:0cm; margin-right:0cm\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\">На сколько мне известно, у нас есть ряд доработок работающих в этих сервисах (например почтовое уведомление о недоступности сервиса при установке соответствующего чекбокса).</span></span></p>↵↵<p style=\"margin-left:0cm; margin-right:0cm\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\">Пришлите мне пожалуйста список этих доработок и что с моей стороны необходимо делать при переводе сервисов на другие типы заявок и БП, чтобы у нас ничего не отвалилось и весь дополнительный функционал продолжал работать в полном объеме без прерываний.</span></span></p>↵↵<p><img alt=\"\" height=\"223\" src=\"http://static.nspk.ru/files/a260252d4ed0b5c9fd29fdedab3ab817910c076f2b9cfc7d774ecda33369857d18736e48f6adfaf4b57332ac7329d46aa91488a63e8ddad43a831ddfb96c6fa9/1.jpg\" width=\"223\" /></p>↵↵<p>&nbsp;</p>↵↵<p>&nbsp;</p>↵";
            //contenstr = "<p>1</p>\n\n<p>gugjhgch</p>\n";
            //contenstr="<p style=\\\"margin-left:0cm; margin-right:0cm\\\">";
            contenstr="<p style=\"margin-left:0cm; margin-right:0cm\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\">С понедельника планируется работа по переводу сервисов RFC на новые типы заявок и бизнес-процессы, работа продлится до окончания фриза по изменениям в ОПКЦ системах.</span></span></p>↵↵<p style=\"margin-left:0cm; margin-right:0cm\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\">На сколько мне известно, у нас есть ряд доработок работающих в этих сервисах (например почтовое уведомление о недоступности сервиса при установке соответствующего чекбокса).</span></span></p>↵↵<p style=\"margin-left:0cm; margin-right:0cm\"><span style=\"font-size:11pt\"><span style=\"font-family:Calibri,sans-serif\">Пришлите мне пожалуйста список этих доработок и что с моей стороны необходимо делать при переводе сервисов на другие типы заявок и БП, чтобы у нас ничего не отвалилось и весь дополнительный функционал продолжал работать в полном объеме без прерываний.</span></span></p>↵↵<p><img alt=\"\" height=\"223\" src=\"http://static.nspk.ru/files/a260252d4ed0b5c9fd29fdedab3ab817910c076f2b9cfc7d774ecda33369857d18736e48f6adfaf4b57332ac7329d46aa91488a63e8ddad43a831ddfb96c6fa9/1.jpg\" width=\"223\" /></p>↵↵<p>&nbsp;</p>↵↵<p>&nbsp;</p>↵";


            string enc = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(contenstr));


            //CHECK CONTENT
            News ns=new News();
            ns.contentBase64 = contenstr;
            string nsCont = uow.UOWserialize<News>(ns);
            News nesAdd2=uow.CreateNews(person1, ns);
            string cntDes = nesAdd2.contentBase64;

            string newsStr="{\"PGUID\":\"\",\"authAcc\":\"\",\"authGUID\":\"\",\"authName\":\"\",\"pic\":\"\",\"name\":\"\",\"content_\":\"cnt1\",\"description\":\"\",\"commentDepth\":0,\"hasComments\":false,\"likes\":0,\"liked\":false,\"Created\":\"2017-12-19 13:43:04\"}";
            News newsGen0 = uow.UOWdeserialize<News>(newsStr);
            News nesAdd=uow.CreateNews(person1, newsGen0);
            addedNews.Add(nesAdd);

            //UPDATEs        
            addedNews[0].contentBase64="updated content";
            string strDes = uow.UOWserialize<News>(addedNews[0]);
            Note updatedNews=uow.UpdateNews(addedNews[0]);
            Note updatedNewsGet = uow.GetNewsByGUID(updatedNews.GUID);

            //CHECK BASE64
            string ctr = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(contenstr));         
            byte[] bts = System.Convert.FromBase64String(ctr);
            string contenstrDec=System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(ctr));

        }
        public void UOWRealCheck(bool newsGen=false)
        {
          
          List<Person> personsAdded = new List<Person>();          
          List<News> newsAdded = new List<News>();          
          List<Commentary> commentaryAdded = new List<Commentary>();          

          List<V> nodes = new List<V>();

          NewsUOWs.NewsRealUow uow = new NewsUOWs.NewsRealUow("test_db");

          string dtStr="{\"transaction\":true,\"operations\":[{\"type\":\"script\",\"language\":\"sql\",\"script\":[\" create Vertex Person content {\"Seed\":1005911,\"FirstName\":\"Илья\",\"LastName\":\"Непринцев\",\"MiddleName\":\"Александрович\",\"Birthday\":\"1987-03-09 00:00:00\",\"mail\":\"Neprintsevia@nspk.ru\",\"telephoneNumber\":1312,\"userAccountControl\":512,\"objectGUID\":\"7E3E2C99-E53B-4265-B959-95B26CA939C8\",\"sAMAccountName\":\"Neprintsevia\",\"OneSHash\":\"5de9dfe2c74b5eef8e13f4f43163f445\",\"Hash\":\"f202a15b6dac384aecbd2424dcca10a7\",\"@class\":\"Person\",\"Name\":\"Непринцев Илья Александрович\",\"GUID\":\"ba124b8e-9857-11e7-8119-005056813668\",\"Created\":\"2017-09-13 07:15:29\",\"Changed\":\"2017-12-05 10:07:19\"} \"]}]}";
          byte[] bt=Encoding.UTF8.GetBytes(dtStr);
          string dtStrRegen = Encoding.UTF8.GetString(bt, 0, bt.Count());
          bool res = dtStr.Equals(dtStrRegen);
        
          PersonUOWs.PersonUOW puow = new PersonUOW("news_test5","http://msk1-vm-ovisp02:2480");
          Person person_ = puow.GetPersonByAccount("Neprintsevia");
          string str=uow.UOWserialize<Person>(person_);
          Manager mng = DefaultManagerInit();
          Person pr=mng.CreateVertex<Person>(str,"test_db");

          personsAdded=uow.GetOrientObjects<Person>(null).ToList();
         
       
          Random rnd=new Random();
          int persCnt=(int)rnd.Next(5,10);
          int newsCnt=(int)rnd.Next(5,10);
          int commentaryCnt=(int)rnd.Next(5,10);

          //get news with commentaries
          //news + comments
          //comment + coments
          //IEnumerable<Note> notes_=uow.GetByOffset("82f83601-d5cd-4108-b1b7-d27ac5a3933a",3);

          IEnumerable<News> news_=uow.GetNews(10);
          
          if(newsGen) {
            //news add
            for(int i=0;i<personsAdded.Count()-1;i++)
            {
              
              newsCnt=(int)rnd.Next(0,3);
              for(int i2=0;i2<newsCnt;i2++)
              {
                newsAdded.Add(
                  uow.CreateNews(personsAdded[i],new News(){name="News"+i2,content="fucking interesting news"})
                );
              }
            }
          }
          
          nodes.AddRange(
            uow.GetOrientObjects<News>(null).ToList()
          );

          //rand, comments gen
          for(int i=0;i<personsAdded.Count()-1;i++)
          {
            commentaryCnt=(int)rnd.Next(8,15);
            for(int i2=0;i2<commentaryCnt;i2++){
              int nodeToCommentId=(int)rnd.Next(0,nodes.Count()-1);
              Note nodeToComment=uow.GetNoteByID(nodes[nodeToCommentId].id);
              nodes.Add(
                uow.CreateCommentary(personsAdded[i],new Commentary(){name="Commentary"+i2,content="fucking bullshit comentary"},nodeToComment)
              );
            }

          }

          string location_ = Assembly.GetExecutingAssembly().Location;
          string path_ = Directory.GetParent(location_).ToString() + "\\nodes.json";
          File.WriteAllText(path_, JsonConvert.SerializeObject(nodes,Formatting.Indented));
        
        }
        
        //DATABASE BOILERPLATE
        public void ManagerCheck(bool cleanUpAter = true)
        {
            
            string login = ConfigurationManager.AppSettings["orient_login"];
            string password = ConfigurationManager.AppSettings["orient_pswd"];
            string dbHost = string.Format("{0}:{1}" 
                ,ConfigurationManager.AppSettings["ParentHost"]
                , ConfigurationManager.AppSettings["ParentPort"]);
            string dbName = ConfigurationManager.AppSettings["TestDBname"];

            TypeConverter typeConverter = new TypeConverter();
            JsonManagers.JSONManager jsonMnager = new JSONManager();
            TokenMiniFactory tokenFactory = new TokenMiniFactory();
            UrlShemasExplicit UrlShema = new UrlShemasExplicit(
                new CommandBuilder(tokenFactory,new FormatFactory()) 
                ,new FormatFromListGenerator(new TokenMiniFactory())
                , tokenFactory, new OrientBodyFactory());
            BodyShemas bodyShema = new BodyShemas(new CommandFactory(),new FormatFactory(),new TokenMiniFactory(),
                new OrientBodyFactory());
         
            UrlShema.AddHost(dbHost);
            WebResponseReader webResponseReader=new WebResponseReader();
            WebRequestManager webRequestManager=new WebRequestManager();
            webRequestManager.SetCredentials(new NetworkCredential(login,password));
            CommandFactory commandFactory=new CommandFactory();
            FormatFactory formatFactory=new FormatFactory();
            OrientQueryFactory orientQueryFactory=new OrientQueryFactory();
            OrientCLRconverter orientCLRconverter=new OrientCLRconverter();

            CommandShemasExplicit commandShema_ = new CommandShemasExplicit(commandFactory, formatFactory,
            new TokenMiniFactory(), new OrientQueryFactory());

            Manager manager = new Manager(typeConverter, jsonMnager,tokenFactory,UrlShema,bodyShema, commandShema_
            ,webRequestManager,webResponseReader,commandFactory,formatFactory,orientQueryFactory,orientCLRconverter);            

            //node objects for insertion
            Person personOne =
new Person(){Seed=123,Name="0",GUID="000",changed=new DateTime(2017,01,01,00,00,00),created=new DateTime(2017,01,01,00,00,00)};
            Person personTwo=
new Person(){Seed=456,Name="0",GUID="001",changed=new DateTime(2017,01,01,00,00,00),created=new DateTime(2017,01,01,00,00,00)};
            MainAssignment mainAssignment=new MainAssignment();
            string pone = manager.ObjectToContentString<Person>(personOne);
            List<Person> personsToAdd = new List<Person>() {
new Person(){
Seed =123,Name="Neprintsevia",sAMAccountName="Neprintsevia",GUID="000"
,changed=new DateTime(2017,01,01,00,00,00),created=new DateTime(2017,01,01,00,00,00)
},
new Person(){
Seed =123,Name="YablokovAE",sAMAccountName="YablokovAE",GUID="000"
,changed=new DateTime(2017,01,01,00,00,00),created=new DateTime(2017,01,01,00,00,00)
}      
    };            

            for(int i=0;i<=10;i++)
            {
                personsToAdd.Add(
                    new Person() { sAMAccountName="Person"+i, Name="Person"+i, GUID="GUID"+i }
                    );
            }

            manager.BindDbName("test_db");            

            //db delete
            manager.DeleteDb(dbName, dbHost);

            //db crete
            manager.CreateDb(dbName,dbHost);

            manager.DbPredefinedParameters();

            //create class
            Type oE=manager.CreateClass<OrientEdge,E>(dbName);
            Type maCl=manager.CreateClass<MainAssignment,E>(dbName);
            Type obc=manager.CreateClass<Object_SC, V>(dbName);
            Type tp=manager.CreateClass<Unit, V>(dbName);
            //Type tpp = manager.CreateClass<Note, V>("news_test5");
            Type nt=manager.CreateClass<Note, V>(dbName);
            
            Type cmt=manager.CreateClass<Commentary,Note>(dbName);
            Type nws=manager.CreateClass<News,Note>(dbName);
            Type auCl=manager.CreateClass<Authorship,E>(dbName);
            Type cmCl=manager.CreateClass<Comment,E>(dbName);

            Note ntCl=new Note();
            Note ntCl0=new Note(){name = "test name",content = "test content"};
            Object_SC obs = new Object_SC() { GUID = "1", changed = DateTime.Now, created = DateTime.Now, disabled = DateTime.Now };
            News ns = new News() {name ="Real news"};
            Commentary cm = new Commentary() {name ="Real comment"};         

            manager.CreateClass("Person","V",dbName);
            MainAssignment ma = new MainAssignment() { };

            //create property
            //will not create properties - not initialized object all property types anonimous.
            manager.CreateProperty<OrientEdge>(null, null);
            //create all properties even if all null.
            manager.CreateProperty<MainAssignment>( new MainAssignment(), null);
            manager.CreateProperty<Unit>( new Unit(), null);
            manager.CreateProperty<Note>( new Note(), null);
            manager.CreateProperty<Authorship>( new Authorship(), null);
            manager.CreateProperty<Comment>( new Comment(), null);
            manager.CreateProperty<Commentary>( new Commentary(), null);
            manager.CreateProperty<News>( new News(), null);
            manager.CreateProperty<Person>(personOne, null);
            //create single property from names
            //manager.CreateProperty("Unit", "Name", typeof(string), false, false);

            manager.CreateVertex<Note>(ntCl, dbName);
            manager.CreateVertex<Object_SC>(obs, dbName);

            manager.CreateVertex<News>(ns,dbName);
            manager.CreateVertex<Commentary>(cm,dbName);

            //add node
            Person p0 = manager.CreateVertex<Person>(personTwo, dbName);        
            manager.CreateVertex("Unit", "{\"Name\":\"TestName\"}",null);
            Unit u0 = manager.CreateVertex<Unit>(u, dbName);

            //add test person
            foreach (Person prs in personsToAdd)
            {
                Person p = manager.CreateVertex<Person>(prs, null);                
            }


            //add relation
            MainAssignment maA = manager.CreateEdge<MainAssignment>(mainAssignment,p0, u0, dbName);
            
            //select from relation
            IEnumerable<MainAssignment> a = manager.SelectFromType<MainAssignment>("1=1", dbName);

            Note ntCr=manager.CreateVertex<Note>(ntCl0, dbName);            
            Authorship aut=new Authorship();
            Authorship aCr=manager.CreateEdge<Authorship>(aut,p0,ntCr,dbName);         

            IEnumerable<Note> notes=manager.SelectFromTraverseWithOffset<Note, Comment, Commentary, Authorship, Comment>("26:2","commentDepth",0,2, "test_db");

            if (cleanUpAter)
            {
                //delete edge
                string res = manager.DeleteEdge<Authorship, Person, Note>(p0, ntCr).GetResult();
                //Delete concrete node
                res = manager.Delete<Unit>(u0).GetResult();
                //delete all nodes of type
                res = manager.Delete<Person>().GetResult();

                //db delete
                manager.DeleteDb(dbName, dbHost);
            }

        }
        //FUNCTIONAL TESTS
        public void UOWFunctionalCheck()
        {

          NewsUOWs.NewsRealUow nu = new NewsUOWs.NewsRealUow("test_db");
          PersonUOWs.PersonUOW pu = new PersonUOWs.PersonUOW("news_test5");
          
          //ABSENT PERSON CHECK
          Random rnd = new Random();
          int acc = (int)rnd.Next(0, 10000);
          
          Person personAbsent = new Person() { Name = "PersonAbsent", sAMAccountName = "absent"+acc };
          string newsContent = "{\"conntent_\":\"news text\",\"name\":\"News name\"}";
          News newsToinsert = nu.UOWdeserialize<News>(newsContent);
          News newsAdded=nu.CreateNews(personAbsent, newsToinsert);

          Person personFromTest=pu.GetPersonByAccount("Neprintsevia");
          News newsAdded2=nu.CreateNews(personFromTest, newsToinsert);

          //GET check
          IEnumerable<Note> notes=nu.GetByOffset("3238d02d-6486-4448-8bc4-b1cb8a659dcd", null);
          IEnumerable<Note> news=nu.GetNews(null);
        
        }
        
        public void UOWfromStringObjectsCreate()
        {
          NewsUOWs.NewsRealUow nu = ActualNewsUOW();
                    
        }
     
    
    }

}

