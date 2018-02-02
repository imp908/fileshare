
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

        //JsonToTypeList.GO();

        //test new quizes
        //Quizes.QuizUOWTest.GO();

        //Check LinqToContext
        //LinqToContextCheck.GO();

        //moove database
        UOWMooveDb();
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

      
      MooveDB.Migrate(mngTo, mngFrom1,true,true,false, false);
      MooveDB.Migrate(mngTo, mngFrom2,false,false,true, false);
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
  
    public static class MooveDB
    {
        static TypeConverter tc = new TypeConverter();
        static IOrientRepo targetRepo_,sourceRepo_;
        public static OrientDatabase Migrate(Managers.Manager to_,Managers.Manager from_,
        bool dropAndCreateIfExists=false, List<IOrientObjects.IOrientDefaultObject> mooveClasses,bool mooveObjects=false,bool generate=false)
        {
        OrientDatabase result=null;
 
        bool allreadyExists=false;
      
        if(to_==null){ throw new Exception("No from DB passed");}
        targetRepo_=to_.GetRepo();
        if(targetRepo_==null){ throw new Exception("No from repo exists");}
      
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
        if(mooveObjects){
            MooveObjectsOfClass<Person>(targetRepo_,sourceRepo_);
            MooveObjectsOfClass<Unit>(targetRepo_,sourceRepo_);       
         
            MooveObjectsOfClass<SubUnit>(targetRepo_,sourceRepo_);
            MooveObjectsOfClass<MainAssignment>(targetRepo_,sourceRepo_);
            MooveObjectsOfClass<OldMainAssignment>(targetRepo_,sourceRepo_); 
        }
        /*
        MooveObjectsOfClass<UserSettings>(targetRepo_,sourceRepo_);
        MooveObjectsOfClass<CommonSettings>(targetRepo_,sourceRepo_);
        */

        MooveObjectsOfClass<PersonRelation>(targetRepo_,sourceRepo_);

        }
        if(generate==true){
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
        static void MooveObject(List<IOrientObjects.IOrientDefaultObject> mooveObjects){
            if(mooveObjects!=null&&mooveObjects.Count()>0){
                foreach (OrientDefaultObject do_ in mooveObjects)
                {
                    
                }
            }
        }
    }

    //check Linq to context
    public static class LinqToContextCheck
  {
    static TestContext ts = new TestContext();
    public static void GO()
    {
      TestEntity te=new TestEntity() {Id=0,name=null};
      var a = te?.name;

      ts.ExpressionBuild();

      string st4=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.tp.isTrue==false);
      string st1=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.Id>=1);      

      string st2=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.name=="test name");
      string st3=ts.VisitLeftRightFromExpressionTypes<TestEntity>(s=>s.intrinsicIsTrue==true);
    
    }
  }

    public static class JsonToTypeList
    {
        static JSONManager jm = new JSONManager();
        public static void GO()
        {
            List<Type> types_=new List<Type>();
            List<IOrientObjects.IOrientEntity> objects_=new List<IOrientObjects.IOrientEntity>();

            Person p=new Person(){GUID="a"};
            News n=new News(){GUID="b"};

            //serialize object to string
            string pStr=jm.SerializeObject(p);
            string nStr=jm.SerializeObject(n);

            //deserialize static type
            Person pDes=jm.DeserializeFromParentNodeStringObj<Person>(pStr);
            News nDes=jm.DeserializeFromParentNodeStringObj<News>(nStr);

            objects_.Add(pDes);
            objects_.Add(nDes);

            foreach (IOrientObjects.IOrientDefaultObject oo_ in objects_)
            {
                string str_ = jm.SerializeObject(oo_);
            }
        }
    }

}
