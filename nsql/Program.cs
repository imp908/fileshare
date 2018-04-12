
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
            ManagerCheck.APItester_sngltnCheck();
      
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
            Quizes.QuizNewUOW quizUOW=new Quizes.QuizNewUOW(repo);

            quizUOW.InitClasses();
          
          List<QuizNewGet> qzReceive = new List<QuizNewGet>();

          List<QuizNewGet> qzSend = new List<QuizNewGet>(){
                    new QuizNewGet(){key=0,value="quiz 1", dateFrom=DateTime.Now,dateTo=DateTime.Now,
                      questions= new List<Question>(){

                        new Question(){key=0,value="quiestion 1",toStore=true,type="checkbox",answers=new List<Answer>(){
                          new Answer(){key=0,value="answer 1"}
                          ,new Answer(){key=1,value="answer 2"}}}
                        
                        ,new Question(){key=0,value="quiestion 2",toStore=true,type="checkbox",answers=new List<Answer>(){
                        new Answer(){key=0,value="answer 1"}
                        ,new Answer(){key=1,value="answer 2"}
                        ,new Answer(){key=2,value="answer 3"}}}

                    }
                }
                , new QuizNewGet(){key=0,value="quiz 2", dateFrom=DateTime.Now,dateTo=DateTime.Now,
                      questions= new List<Question>(){

                        new Question(){key=0,value="quiestion 1",toStore=true,type="text"}
                        
                        ,new Question(){key=0,value="quiestion 2",toStore=true,type="checkbox",answers=new List<Answer>(){
                        new Answer(){key=0,value="answer 1"}
                        ,new Answer(){key=1,value="answer 2"}
                        ,new Answer(){key=2,value="answer 3"}}}

                    }
                }
            };

            string snd=jm.SerializeObject(qzSend);

            quizUOW.QuizPost(qzSend);

            qzReceive = quizUOW.QuizGet().ToList();

            //quizUOW.QuizDelete(qzReceive);

        }
        public static void AdinTceCheck(){
            AdinTce.AdinTceRepo adinTceRepo = new AdinTce.AdinTceRepo();
            string res=adinTceRepo.HoliVation("ba124b8e-9857-11e7-8119-005056813668");
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
            MooveDB.Migrate(mngTo,mngFrom2,classes_,null,true,false);
            //migrate ral persons from actual person DB
            MooveDB.Migrate(mngTo,mngFrom1,null, classes_, false,false);
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
    
    /// <summary>
    /// Mooves orient database (Classes with properties, vertices and edges with GUID)  
    /// from one Manager to another with some options.
    /// </summary>
    public static class MooveDB
    {
        static TypeConverter tc = new TypeConverter();
        static IOrientRepo targetRepo_,sourceRepo_;
        static List<NodeReferenceConditional> conditionalItems;

        static void ConditionalItemsInit(List<IOrientObjects.IOrientDefaultObject> mooveClasses)
        {
            conditionalItems=new List<NodeReferenceConditional>();
            foreach (OrientDefaultObject tp_ in mooveClasses.Where(s=>s.GetType().BaseType==typeof(V)))
            {
                if(tp_!=null){conditionalItems.Add(new NodeReferenceConditional(){orientItem=tp_,processed=false});}
            }
            foreach (OrientDefaultObject tp_ in mooveClasses.Where(s => s.GetType().BaseType==typeof(E)))
            {
                if(tp_!=null){conditionalItems.Add(new NodeReferenceConditional(){orientItem=tp_,processed=false});}
            }
        }
        public static List<IOrientObjects.IOrientDefaultObject> GetClasses(List<IOrientObjects.IOrientDefaultObject>  list_)
        {           
            return list_;
        }
        public static OrientDatabase Migrate(Managers.Manager to_,Managers.Manager from_,List<IOrientObjects.IOrientDefaultObject> mooveClasses
        ,List<IOrientObjects.IOrientDefaultObject> mooveObjects, bool dropAndCreateIfExists = false,bool generate=false)
        {
        OrientDatabase result=null;
 
        bool allreadyExists=false;
      
        if(to_==null){throw new Exception("No from DB passed");}
        targetRepo_=to_.GetRepo();
        if(targetRepo_==null){throw new Exception("No from repo exists");}
      
        OrientDatabase dbTo=targetRepo_.GetDb();
		    		  
        if(dropAndCreateIfExists==true){
	        //drop and create db
	        if(dbTo!=null){targetRepo_.DeleteDb();}
        targetRepo_.CreateDb();
        if(targetRepo_.GetDb()==null){throw new Exception("Db was not recreated");}
        }
        if(from_!=null){
	        //moove db
        sourceRepo_=from_.GetRepo();
        if(sourceRepo_==null){throw new Exception("No from repo exists");}
	    OrientDatabase dbFrom=sourceRepo_.GetDb();
        dbFrom=sourceRepo_.GetDb();
        dbTo=targetRepo_.GetDb();
        if(dbTo==null)
        {throw new Exception("No target database exists");}
        if(dbFrom==null)
        {throw new Exception("No source database exists");}
        
        if(mooveClasses!=null&& mooveClasses.Count()>0){
            MooveClasses(targetRepo_,sourceRepo_, mooveClasses);

            foreach (OrientDefaultObject oL_ in mooveClasses){
                targetRepo_.CreateProperty<OrientDefaultObject>(oL_,null);
            }
        }
        if(mooveObjects!=null&&mooveObjects.Count()>0)
        {
            ConditionalItemsInit(mooveObjects);
            MooveObject();
            
            /*
            MooveObjectsOfClass<Person>(targetRepo_,sourceRepo_);
            MooveObjectsOfClass<Unit>(targetRepo_,sourceRepo_);       

            MooveObjectsOfClass<SubUnit>(targetRepo_,sourceRepo_);
            MooveObjectsOfClass<MainAssignment>(targetRepo_,sourceRepo_);
            MooveObjectsOfClass<OldMainAssignment>(targetRepo_,sourceRepo_);
            */
        }
            /*
            MooveObjectsOfClass<UserSettings>(targetRepo_,sourceRepo_);
            MooveObjectsOfClass<CommonSettings>(targetRepo_,sourceRepo_);


            MooveObjectsOfClass<PersonRelation>(targetRepo_,sourceRepo_);
            */
        }
        if (generate==true){
            //generate scenery to existing
            to_.GenDB(false,false);
            to_.GenNewsComments(null,null);
        }
      
            targetRepo_.StoreDbStatistic(null,null);
            return result;
        }
   
        static void MooveClasses(IOrientRepo targetRepo,IOrientRepo sourceRepo, List<IOrientObjects.IOrientDefaultObject> mooveClasses){
        TypeConverter tc = new TypeConverter();
        OrientDatabase dbFrom=sourceRepo.GetDb(null,null);

            foreach (OrientDefaultObject do_ in mooveClasses){
                OrientClass oc = (from s in dbFrom.classes where s.name==do_.GetType().Name select s).FirstOrDefault();
                if(oc!=null)
                {
                    CreateClassRec(oc);
                }
            }          
        }
        static void CreateClassRec(OrientClass class_){
        OrientClass _class=targetRepo_.GetClass(class_.name,null, null);    
            if(_class==null){
                if(class_.superClass!=null){
                    OrientClass superClass=targetRepo_.GetClass(class_.superClass,null, null);
                    if(superClass==null){
                    superClass=sourceRepo_.GetClass(class_.superClass,null, null);
                        if(superClass==null){throw new Exception("no superclass in sourcedb found");}
                    CreateClassRec(superClass);
                    }
                    class_=targetRepo_.CreateClass(class_.name,superClass.name,null).GetClass(class_.name,null,null);
                }else{
                    class_=targetRepo_.CreateClass(class_.name,null,null).GetClass(class_.name,null,null);
                }
                if(class_==null){throw new Exception("failed to create class");}
            }
        }
    
        static void MooveObjectsOfClass<T>(IOrientRepo targetRepo,IOrientRepo sourceRepo) 
            where T:class, IOrientObjects.IOrientDefaultObject
        {
            List<T> arbitraryObjects = new List<T>();
            foreach(T p in sourceRepo.SelectFromType<T>(null,null)){
            T pc = null;
                try{
                    if(p.GetType().BaseType==typeof(V)){
                        pc=targetRepo.CreateVertex<T>(p, null);
                    }
                    if(p.GetType().BaseType==typeof(E)){
                        POCO.OrientEdge io=p as POCO.OrientEdge;
                        POCO.V vFrom=
                        sourceRepo.SelectByIDWithCondition<POCO.V>(io.In,null,null).FirstOrDefault();
                        POCO.V vTo=
                        sourceRepo.SelectByIDWithCondition<POCO.V>(io.Out,null,null).FirstOrDefault();

                        vTo=targetRepo.SelectFromType<POCO.V>("GUID='"+vTo.GUID+"'",null).FirstOrDefault();

                        pc=targetRepo.CreateEdge<T>(p,vFrom,vTo, null) as T;
                    }
                    if(pc==null){ if(arbitraryObjects!=null ){arbitraryObjects.Add(p);}}
                }catch(Exception e){ System.Diagnostics.Trace.WriteLine(e.Message);}
            }
            if(arbitraryObjects.Count()>0){
                CheckListOfObjects(targetRepo, arbitraryObjects);
            }
        }
        static void CheckListOfObjects<T>(IOrientRepo targetRepo, List<T> unaddedObjects) where T:class, IOrientObjects.IOrientDefaultObject
        {
            T pc=null;
            foreach(T p in unaddedObjects){
            pc=targetRepo.SelectFromType<T>("GUID='"+p.GUID+"'",null).FirstOrDefault();
            if(pc==null){
                try{
                    if(p.GetType().BaseType==typeof(V)){
                        pc=targetRepo.CreateVertex<T>(p, null);
                    }
                    if(p.GetType().BaseType==typeof(E)){
                        IOrientObjects.IOrientEdge io=p as IOrientObjects.IOrientEdge;
                        IOrientObjects.IOrientVertex vFrom=targetRepo.SelectByIDWithCondition<T>(io.In,null,null).FirstOrDefault() as IOrientObjects.IOrientVertex;
                        IOrientObjects.IOrientVertex vTo=targetRepo.SelectByIDWithCondition<T>(io.Out,null,null).FirstOrDefault() as IOrientObjects.IOrientVertex;
                        pc=targetRepo.CreateEdge<IOrientObjects.IOrientDefaultObject>(p,vFrom,vTo, null) as T;
                    }
                }catch(Exception e){ System.Diagnostics.Trace.WriteLine(e.Message);}
            }
            }
        }

        //If object classes list passed, handles object movement from source to target
        static void MooveObject(){
            if(conditionalItems != null&& conditionalItems.Count()>0){
                //Loop throught every assed class
                foreach(NodeReferenceConditional conditionalreference_ in conditionalItems)
                {
                    IOrientObjects.IOrientDefaultObject objectClass_ = conditionalreference_.orientItem;
                   
                    //for Nodes
                    if (objectClass_.GetType().BaseType.Equals(typeof(V)))
                    {
                        //get all objects of class from base
                        IEnumerable<IOrientObjects.IOrientDefaultObject> tempVert = sourceRepo_.SelectRefl(objectClass_.GetType(), null);
                        if (tempVert != null && tempVert.Count() > 0)
                        {
                            //for every Node just simple create
                            foreach (V vToInsert in tempVert)
                            {
                                Type tp = vToInsert.GetType();
                                IOrientObjects.IOrientDefaultObject vInserted =targetRepo_.CreateVertexTp(vToInsert, null);
                                if (vInserted==null){throw new Exception("vertex was not mooved");}
                            }
                        }
                    }

                    //for References
                    if (objectClass_.GetType().BaseType.Equals(typeof(E)))
                    {
                        //get all objects of class from base
                        IEnumerable<IOrientObjects.IOrientDefaultObject> tempVert = sourceRepo_.SelectRefl(objectClass_.GetType(), null);
                        //for every Node just simple create
                        foreach (E eToInsert in tempVert)
                        {
                            //get related Nodes from source DB by referenced rIDs
                            V vFrom=sourceRepo_.SelectByIDWithCondition<V>(eToInsert.In, null, null).FirstOrDefault();
                            V vTo= sourceRepo_.SelectByIDWithCondition<V>(eToInsert.Out, null, null).FirstOrDefault();
                            if (vFrom == null || vTo == null) { throw new Exception("no in or out Node in source db"); }

                            //get related Nodes in target DB by GUIDs of source DB Nodes
                            V vFromToIns=targetRepo_.SelectByCondFromType<V>(typeof(V),"GUID='"+vFrom.GUID+"'",null).FirstOrDefault();
                            V vToToIns=targetRepo_.SelectByCondFromType<V>(typeof(V),"GUID='"+vTo.GUID+"'",null).FirstOrDefault();
                            if (vFromToIns == null || vToToIns == null) {throw new Exception("no in or out Node in target db");}

                            //create relation inn target DB between Nodes related in source DB
                            E eInserted = targetRepo_.CreateEdge<E>(eToInsert, vFromToIns, vToToIns, null);
                            if (eInserted==null){throw new Exception("no in or out Node in target db");}
                        }
                    }
                  
                   
                }

            }
        }
    }   

}
