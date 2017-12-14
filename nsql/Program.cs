
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

namespace NSQLManager
{

    class OrientDriverConnnect
    {

        static void Main(string[] args)
        {
            Trash.FormatRearrange.StringsCheck();

            RepoCheck rc=new RepoCheck();

            rc.ManagerCheck(false);
            rc.UOWRandomcheck();
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
new Person() {Name="0", GUID="0", Changed=new DateTime(2017, 01, 01, 00, 00, 00), Created=new DateTime(2017, 01, 01, 00, 00, 00)};

            u =
new Unit() {Name="0", GUID="0", Changed=new DateTime(2017, 01, 01, 00, 00, 00), Created=new DateTime(2017, 01, 01, 00, 00, 00)};

            m =
new MainAssignment() {Name="0", GUID="0", Changed=new DateTime(2017, 01, 01, 00, 00, 00), Created=new DateTime(2017, 01, 01, 00, 00, 00)};
            
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

        /// <summary>
        /// Database boilerplate fire
        /// </summary>
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
            WebResponseReader webResponseReader = new WebResponseReader();
            WebRequestManager webRequestManager = new WebRequestManager();
            webRequestManager.SetCredentials(new NetworkCredential(login,password));
            CommandFactory commandFactory = new CommandFactory();
            FormatFactory formatFactory = new FormatFactory();
            OrientQueryFactory orientQueryFactory = new OrientQueryFactory();
            OrientCLRconverter orientCLRconverter = new OrientCLRconverter();

            Manager manager = new Manager(typeConverter, jsonMnager,tokenFactory,UrlShema, bodyShema, webRequestManager
                ,webResponseReader,commandFactory,formatFactory,orientQueryFactory,orientCLRconverter);            

            //node objects for insertion
            Person personOne =
new Person(){Seed=123,Name="0",GUID="000",Changed=new DateTime(2017,01,01,00,00,00),Created=new DateTime(2017,01,01,00,00,00)};
            Person personTwo=
new Person(){Seed=456,Name="0",GUID="001",Changed=new DateTime(2017,01,01,00,00,00),Created=new DateTime(2017,01,01,00,00,00)};
            MainAssignment mainAssignment=new MainAssignment();

            List<Person> personsToAdd = new List<Person>() {
new Person(){
Seed =123,Name="Neprintsevia",sAMAccountName="Neprintsevia",GUID="000"
,Changed=new DateTime(2017,01,01,00,00,00),Created=new DateTime(2017,01,01,00,00,00)
}       
    };
            for(int i=0;i<=10;i++)
            {
                personsToAdd.Add(
                    new Person() { sAMAccountName = "Person"+i, Name = "Person"+i }
                    );
            }
            //Type tpp = manager.CreateClass<Note, V>("news_test5");

            //db delete
            manager.DeleteDb(dbName, dbHost);

            //db crete
            manager.CreateDb(dbName,dbHost);

            //create class
            Type tp=manager.CreateClass<Unit,V>(dbName);
            Type nt=manager.CreateClass<Note, V>(dbName);
            Type obc=manager.CreateClass<Object_SC, V>(dbName);
            Type auCl=manager.CreateClass<Authorship, E>(dbName);
            Type cmCl=manager.CreateClass<Comment, E>(dbName);
            Type maCl=manager.CreateClass<MainAssignment, E>(dbName);

            Note ntCl = new Note();
            Note ntCl0 = new Note() { name = "test name", content = "test content" };
            Object_SC obs = new Object_SC() { GUID = "1", changed = DateTime.Now, created = DateTime.Now, disabled = DateTime.Now };
            manager.CreateVertex<Note>(ntCl, dbName);
            manager.CreateVertex<Object_SC>(obs, dbName);

            manager.CreateClass("Person","V",dbName);
            
            //create property
            manager.CreateProperty<Person>(personOne, null);
            manager.CreateProperty("Unit", "Name", typeof(string), false, false);

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
            IEnumerable<MainAssignment> a = manager.Select<MainAssignment>("1=1", dbName);

            Note ntCr=manager.CreateVertex<Note>(ntCl0, dbName);
            Authorship aut=new Authorship();
            Authorship aCr=manager.CreateEdge<Authorship>(aut,p0,ntCr,dbName);

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
            string res = UserAuthenticationMultiple.UserAcc();
        }
        public void UOWRandomcheck()
        {

List<string> idioticNews = new List<string> {
"Fire on the sun!","Internet trolls are jerks","Bugs flying around with wings are flying bugs","Chuck norris facts are they true?"
,"Health officials: Pools and diarea not good mix","Tiger wood He's goo at Golf","A nuclear explosion would be adisaster",
"Russians are comming!","Rain creates wet roads"
};

List<string> bullshitComments = new List<string>
{
"Supper!","Awasome!","How could thishappen!","i am stonned...","Ggggg "
};

//select expand(outE('Authorship').inV('Note')) from Person
//traverse both() from (select expand(outE('Authorship').inV('Note')) from Person)
    
NewsUOWs.NewsUow newsUOW = new NewsUOWs.NewsUow(ConfigurationManager.AppSettings["TestDBname"]);

            List<Note> newsCreated = new List<Note>();
            List<Note> commentsCreated = new List<Note>();
            List<Person> personsAdded = new List<Person>();       

            List<Note> news = new List<Note>();
            foreach(string noteCont in idioticNews)
            {
news.Add(new Note() { name = noteCont, content = "here goes text of really fucking interesting news" });
            }

            List<Note> comments = new List<Note>();
            foreach(string comment in bullshitComments)
            {
comments.Add(new Note() { name = comment, content = "here goes text of really fucking interesting news" });
            }

            personsAdded = newsUOW.GetOrientObjects<Person>(null).ToList();
            
            Random rnd=new Random();
            int newsCount = idioticNews.Count() - 1;
            int commentsCount = bullshitComments.Count() - 1;

            //generate news 
            foreach (Person p in personsAdded)
            {
                //news published count
                int gap=(int)rnd.Next(0,5);
                
                if (gap > 0)
                {                    
                    for (int i = 0; i <= gap;i++)
                    {
                        //news index
                        int newsRndInd=(int)rnd.Next(0, newsCount-1);
                        newsCreated.Add(newsUOW.CreateNews(p, news[newsRndInd]));
                    }
                }                
            }

            //Generate commentaries
            foreach (Person p in personsAdded)
            {
                //comments count
                int gap = (int)rnd.Next(0, 5);

                if (gap > 0)
                {
                    for (int i = 0; i <= gap;i++)
                    {
                        //created news gap
                        newsCount = newsCreated.Count();
                        //news index
                        int newsRndInd = (int)rnd.Next(0, newsCount - 1);
                        int commentRndInd = (int)rnd.Next(0, commentsCount - 1);
                        commentsCreated.Add(newsUOW.CreateCommentary(p, newsCreated[newsRndInd], comments[commentRndInd]));
                        
                    }
                }               

            }

            //traverse both() from (select from Person)
            foreach (Note ntd in news)
            {
                newsUOW.DeleteNews(p, ntd.id);
            }
        

        }
        public void UOWcheck()
        {

        }
    }

}

