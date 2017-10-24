using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Orient.Client;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

using System.IO;

using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ConsoleApp1
{
    public class Record
    {
        List<string> Name { get; set; }
    }
    class OrientDriverConnnect
    {


        static void Main(string[] args)
        {

            PersonApiCheck();

            JScheck();

            BuilderCheck();
            ManagersCheck();
                      
            HTTPcheck();
            
        }

        public static void JScheck()
        {
            JSONmanager jm = new JSONmanager();

            
            string input = "{\"result\":[{\"@type\":\"d\",\"@rid\":\"#-2:0\",\"@version\":0,\"Name\":\"kapkaev\"},{\"@type\":\"d\",\"@rid\":\"#-2:1\",\"@version\":0,\"Name\":\"kokuevol\"}]}";
            string input2 = "[{\"@type\":\"d\",\"@rid\":\"#-2:13\",\"@version\":0,\"Name\":[\"tishakovoi\"]}]";
            string input3 = "[{\"@type\":\"d\",\"@rid\":\"#-2:13\",\"@version\":0,\"Name\":[\"tishakovoi\"]},{\"@type\":\"d\",\"@rid\":\"#-2:13\",\"@version\":0,\"Name\":[\"kapkaev\"]}]";

            string rk = @"result";
            string nk = @"Name";

            var des1 = JsonConvert.DeserializeObject(input);
            var des2 = JsonConvert.DeserializeObject(input2);
            var des3 = JsonConvert.DeserializeObject(input3);

            JToken jt1 = null, jt2 = null;
            Record jt3 =new Record();
            JToken r1,r0 = null;
            IEnumerable<JToken> r2 = null, r3 = null;

            try{
                jt1 = JsonConvert.DeserializeObject<JToken>(input);
            }catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try{
                jt2 = JsonConvert.DeserializeObject<JToken>(input2);
            }catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try{
                jt3 = JsonConvert.DeserializeObject<Record>(input3);
            }catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            //Json has parent node
            try { JToken e1 = jt1[rk]; } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try { IEnumerable<JToken> e1 = jt1[rk].Children(); } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try { IEnumerable<JToken> e1 = jt1[rk].Children()[nk]; } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            //Json has parent as collection
                
            //not OK
            try { JToken e1 = jt2[rk];} catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try { JToken e1 = jt2[nk];} catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            //OK
            try { IEnumerable<JToken> e2 = jt2.Children();}catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try { IEnumerable<JToken> e3 = jt2.Children()[nk];}catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            //try { IEnumerable<JToken> e4 = jt3.Children()[nk];}catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            try {
                r1 = JToken.Parse(input);
                r0 = JToken.Parse(input)[rk];
                r2 = JToken.Parse(input2).Children()[nk];
                r3 = JToken.Parse(input3).Children()[nk];
            } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            try
            {
                IEnumerable<Person> z = jm.DeserializeFromNode<Person>(input, "result");
                IEnumerable<JToken> a = jm.ExtractTokens(input3, "Name");

                var r_0 = JsonConvert.SerializeObject(a);
                var r_1 = JsonConvert.SerializeObject(z);
          
                string c = jm.CollectionToStringFormat<JToken>(a, null);

                string r5 = jm.CollectionToStringFormat<Person>(z, new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
                string r6 = jm.CollectionToStringFormat<JToken>(a,null);
                
                var t2 = JsonConvert.DeserializeObject(c);
                var t3 = JsonConvert.DeserializeObject(r5);
                var t4 = JsonConvert.DeserializeObject(r6);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            try {
                JSONorient jo = new JSONorient();
                string x3 = jo.FilterChildren(input3, "Name");
            } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

        }

        public static void OrientSpagettyCheck()
        {

            OrientSpagettyCheck oc = new OrientSpagettyCheck();
            oc.Initialize();
            oc.DBmigrate();
            oc.MigrateEdgeData<MainAssignment>();
            oc.MigrateEdgeData<OldMainAssignment>();
            oc.MigrateEdgeData<OutExtAssignment>();
            oc.MigrateEdgeData<SubUnit>();

        }

        public static void ManagersCheck()
        {

            string url = @"http://10.31.14.76/cleverence_ui/hs/IntraService/location/full";

            OrientWebManager owm = new OrientWebManager();
            JSONmanager jm = new JSONmanager();
            WebResponseReader wr = new WebResponseReader();

            //sample JSON [{},{}] http get from url

            string jres = wr.ReadResponse(owm.GetResponse(url, "GET"));
            IEnumerable<J_Address> addresses = jm.DeseializeSample<J_Address>(jres);

            //authenticate Orient
            owm.Authenticate(@"http://msk1-vm-ovisp02:2480/connect/news_test3", new NetworkCredential("root", "I9grekVmk5g"));

            //authenticated htp response from command
            url = @"http://msk1-vm-ovisp02:2480/command/news_test3/sql/select from Person";
            jres = wr.ReadResponse(owm.GetResponse(url, "GET"));
            //в коллекцию объектов из модели {node:[{},{}]} -> List<model>({}) для работы в коде
            IEnumerable<Person> persons = jm.DeserializeFromNode<Person>(jres, @"result");
            //в JSON строку List<{}> -> [{},{}] для передачи в API
            string str = jm.CollectionToStringFormat<Person>(persons, null);

            //из функции
            url = @"http://msk1-vm-ovisp02:2480/function/news_test3/SearchByLastNameTst/сав";
            jres = wr.ReadResponse(owm.GetResponse(url, "GET"));
            //{node:[{},{}]} -> List<model>({}) 
            persons = jm.DeserializeFromNode<Person>(jres, @"result");
            str = jm.CollectionToStringFormat<Person>(persons, null);

            //из комманды другой класс
            //authenticated htp response from command
            url = @"http://msk1-vm-ovisp02:2480/command/news_test3/sql/select from Unit";
            jres = wr.ReadResponse(owm.GetResponse(url, "GET"));
            //{node:[{},{}]} -> List<model>({})
            IEnumerable<Unit> units = jm.DeserializeFromNode<Unit>(jres, @"result");
            //в строку [{"name":"a"},..,{"name":"b"}]
            str = jm.CollectionToStringFormat<Unit>(units, null);
            //в строку ["a",..,"b"]
            str = jm.CollectionToStringFormat<JToken>(jm.ExtractTokens(jres, "result", "Name"), null);

        }

        public static void BuilderCheck()
        {

            string login = "root";
            string password = "I9grekVmk5g";
            string authUrl = @"http://msk1-vm-ovisp02:2480/connect/news_test3";
            string commandUrl = @"http://msk1-vm-ovisp02:2480/command/news_test3/sql/";
            string functionUrl = @"http://msk1-vm-ovisp02:2480/function/news_test3/";

            OrientApiUrlBuilder ob = new OrientApiUrlBuilder();

            //<--Out
            //HTTPmanager hm = new HTTPmanager();

            //-->In
            OrientWebManager owm = new OrientWebManager();
            NetworkCredential nc = new NetworkCredential(login, password);
            WebResponseReader wrr = new WebResponseReader();

            JSONmanager jm = new JSONmanager();


            owm.Authenticate(authUrl, nc);

            //задаем параметры комманды, тело селекта и тело where. where (можно оставить пустым)
            ob.SelectCommandSet("Person", @"1=1");
            //авторизуемся
            //old
            //hm.HTTPauthProxy(@"http://msk1-vm-ovisp02:2480/connect/news_test3", login, password);
            //new
            owm.Authenticate(authUrl, nc);
            //получам ответ форматированный в JSON сторку
            //old
            //string jsonStringResponse = hm.GetAuthProx(@"http://msk1-vm-ovisp02:2480/command/news_test3/sql/" + ob.SelectCommandGet());
            //new
            WebResponse wr = owm.GetResponse(commandUrl + ob.SelectCommandGet(), "GET");
            string responseRaw = wrr.ReadResponse(wr);
            //десериализуем в модель, проксирование управляется полями POCO класса модели и атрибутами
            IEnumerable<Person> persons = jm.DeserializeFromNode<Person>(responseRaw, @"result");


            //Аналогично для Unit
            //задаем имя класса и фильтр where
            ob.SelectCommandSet("Unit", @"1=1");
            responseRaw = wrr.ReadResponse(owm.GetResponse(commandUrl + ob.SelectCommandGet(), "GET"));
            IEnumerable<Unit> units = jm.DeserializeFromNode<Unit>(responseRaw, @"result");


            //Аналогично для функции
            //имя функции и параметр
            //Так как функция возвразает только одно поле, коллекция заполнится объектами со значением только в 1 поле
            //остальные null
            ob.FucntionSet("SearchByLastNameTst", @"сав");
            responseRaw = wrr.ReadResponse(owm.GetResponse(functionUrl + ob.FunctionCommandGet(), "GET"));
            IEnumerable<Person> persons2 = jm.DeserializeFromNode<Person>(responseRaw, @"result");
            //Кручу-верчу для получения "чистых" коллекицй, без полей с null.
            //(когда возращается только часть полей, null values игнорятся, без изменения модели и залезания в строку ответа)
            string jSer = jm.CollectionToStringFormat<Person>(persons2, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            //Промежуточная коллекция с экстракцией нужного токена, для понимания типа
            IJEnumerable<JToken> jt = jm.ExtractTokens(jSer, "LastName");
            //Формирование "чистой" строки из коллекции токенов
            string res0 = jm.CollectionToStringFormat<IJEnumerable<JToken>>(jt, null);

            
            //Аналогично, без иключения null, oneline          
            res0 = jm.CollectionToStringFormat<IJEnumerable<JToken>>(jm.ExtractTokens(
                jm.CollectionToStringFormat<Person>(
                    jm.DeserializeFromNode<Person>(responseRaw, @"result")
                , null)
                , "LastName"), null);

        }

        public static void HTTPcheck()
        {

            WebResponseReader reader = new WebResponseReader();

            //read basic GET response
            string url = @"http://msk1-vm-ovisp01:8083/api/Person/GetCollegesLower/bs";
            WebManager hm = new WebManager();
            WebResponse response = hm.GetResponse(url, "GET");
            string sampleResult = reader.ReadResponse(response);


            //Authentication
            string authUrl = @"http://msk1-vm-ovisp02:2480/connect/news_test3";
            NetworkCredential nc = new NetworkCredential("root", "I9grekVmk5g");

            //read Orient fucntion GET with authentication
            url = @"http://msk1-vm-ovisp02:2480/function/news_test3/GetCollegesLowerByAccount/bs";
            OrientWebManager orm = new OrientWebManager();
            orm.Authenticate(authUrl, nc);
            response = orm.GetResponse(url, "GET");
            string resultOrient = reader.ReadResponse(response);


            //deserializing string request
            JSONmanager mng = new JSONmanager();
            IEnumerable<string> a = mng.DeseializeSample<string>(sampleResult);
            IEnumerable<string> b = mng.DeserializeFromNode<string>(resultOrient, "result", "Name");


            //change equality check
            //they are equal
            bool res = a.Equals(b);

        }
        
        public static void PersonApiCheck()
        {

            WebManager wm = new WebManager();
            APItester_sngltn ut = new APItester_sngltn();
            WebResponseReader wr = new WebResponseReader();
            
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json"))
            {
                string res = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json");
                APItester_sngltn.TestCases = JsonConvert.DeserializeObject<List<APItester_sngltn>>(res);
            }
            else
            {               
                APItester_sngltn.TestCases.Add(new APItester_sngltn()
                {
                    URI = "http://msk1-vm-ovisp01:8083/api/Person/GetManager/DegterevaSV"
                    ,
                    Expected = "[\"filimonovats\",\"kvv\"]"
                    ,
                    OK = false
                });
                APItester_sngltn.TestCases.Add(new APItester_sngltn()
                {
                    URI = "http://msk1-vm-ovisp01:8083/api/Person/GetColleges/lobanovamg"
                   ,
                    Expected = "[\"stalmakovsm\",\"Bagirovaev\",\"kotovaen\",\"iku\",\"SergiecIG\",\"a.vagin\",\"lobanovamg\",\"Tikhomirovaa\",\"popovalb\"]"
                   ,
                    OK = false
                });
            }
        

            foreach (APItester_sngltn tc in APItester_sngltn.TestCases)
            {
                try
                {
                    var resp = wm.GetResponse(tc.URI, "GET");
                    string rR = wr.ReadResponse(resp);

                    if (tc.Expected == rR)
                    {
                        tc.OK_(rR);
                    }
                    else
                    {
                        tc.NotOK_(rR);
                    }
                }catch (Exception e)
                {

                    if (tc.Expected == e.GetType().ToString())
                    {
                        tc.OK_(e.GetType().ToString(), e.Message);                      
                    }
                    else
                    {
                        tc.NotOK_(e.GetType().ToString(), e.Message);
                    }
                }
            }

            string jStr = JsonConvert.SerializeObject(APItester_sngltn.TestCases,Formatting.Indented);
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json", jStr);

        }
      
    }

  

    //DB CREATE, Objects from existing DB move, AND DROP. no class segregation
    public class OrientSpagettyCheck
    {

        protected static string OSESSIONID;

        string Authurl;

        OrientDB_Net.binary.Innov8tive.API.ConnectionOptions childBasePool;
        OrientDB_Net.binary.Innov8tive.API.ConnectionOptions parentBasePool;

        OServer os;

        ODatabase childBase;
        ODatabase parentBase;

        List<ODocument> childClasses;
        List<ODocument> parentClasses;

        public ODatabase openDatabase(string _host, int _port,
        string _dbName, string _user, string _passwd)
        {

            // CONSOLE LOG
            Console.WriteLine("Opening Database: {0}", _dbName);

            // OPEN DATABASE
            ODatabase database = new ODatabase(_host, _port, _dbName,
                ODatabaseType.Graph, _user, _passwd);

            // RETURN ODATABASE INSTANCE
            return database;
        }

        public void Initialize()
        {
            Authurl = string.Format(@"{0}{1}:{2}/connect/{3}",
            @"http://",
            ConfigurationManager.AppSettings["ParentHost"],
            "2480",
            ConfigurationManager.AppSettings["ParentDBname"]
            );

            childBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();
            parentBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();

            childBasePool.HostName = ConfigurationManager.AppSettings["ChildHost"];
            childBasePool.UserName = ConfigurationManager.AppSettings["ChildLogin"];
            childBasePool.Password = ConfigurationManager.AppSettings["ChildPassword"];
            childBasePool.Port = 2424;
            childBasePool.DatabaseName = ConfigurationManager.AppSettings["ChildDBname"];
            childBasePool.DatabaseType = ODatabaseType.Graph;

            parentBasePool.HostName = ConfigurationManager.AppSettings["ParentHost"];
            parentBasePool.UserName = ConfigurationManager.AppSettings["ParentLogin"]; ;
            parentBasePool.Password = ConfigurationManager.AppSettings["ParentPassword"];
            parentBasePool.Port = 2424;
            parentBasePool.DatabaseName = ConfigurationManager.AppSettings["ParentDBname"];
            parentBasePool.DatabaseType = ODatabaseType.Graph;

            //Create DB
            //http://orientdb.com/docs/last/NET-Server-CreateDatabase.html
            os = new OServer(childBasePool.HostName, childBasePool.Port, childBasePool.UserName, childBasePool.Password);

            childBase = openDatabase(childBasePool);
            parentBase = openDatabase(parentBasePool);
        }
        public void DBmigrate()
        {

            //OServer os = new OServer("msk1-vm-ovisp02",2424, "root", "I9grekVmk5g");
            if (os.DatabaseExist("news_test3", OStorageType.PLocal))
            {
                os.DropDatabase("news_test3", OStorageType.PLocal);
            }

            os.CreateDatabase("news_test3", childBasePool.DatabaseType, OStorageType.PLocal);

            //Get list of classes
            childClasses = GetDbClasses(childBase, ConfigurationManager.AppSettings["GetAllClasses"]);
            parentClasses = GetDbClasses(parentBase, ConfigurationManager.AppSettings["GetAllClasses"]);

            //Exort to JSON txt
            JsonSerialize(childClasses, @"child");
            JsonSerialize(parentClasses, @"parent");

            List<ODocument> uniqueParentKeys = FilterUniqueKeys(parentClasses, childClasses);

            JsonSerialize(uniqueParentKeys, @"unique");

            //Add properties to Vertix and Edge classes
            List<ODocument> ParentEdges = FilterClasses(uniqueParentKeys, "E");
            List<ODocument> NestedEdges = FilterClassesBySuperclass(uniqueParentKeys, ParentEdges);

            AddClassesNested(ParentEdges, childBase);
            AddProperties(ParentEdges, parentBase, childBase);
            AddClassesNested(NestedEdges, childBase);
            AddProperties(NestedEdges, parentBase, childBase);

            List<ODocument> ParentVertices = FilterClasses(uniqueParentKeys, "V");
            List<ODocument> NestedVertices = FilterClassesBySuperclass(uniqueParentKeys, ParentVertices);

            AddClassesNested(ParentVertices, childBase);
            AddProperties(ParentVertices, parentBase, childBase);
            AddClassesNested(NestedVertices, childBase);
            AddProperties(NestedVertices, parentBase, childBase);

            //Migrate Data from parent db in config with select from class to API
            AuthorizeOrientDB(Authurl);
            MigrateEntity<Person>(childBase);
            MigrateEntity<Unit>(childBase);

            //Drop Db
            childBase.Close();
            childBase.Dispose();
            os.Close();
            os.Dispose();
        }
        public void MigrateEdgeData<T>() where T : class, Edge
        {
            AuthorizeOrientDB(Authurl);
            string SelectCommand = string.Format(ConfigurationManager.AppSettings["SelectCommand"], "in,out", typeof(T).Name);
            List<T> MainAssignmentCollection = GetEntityCollectionFromParentBase<T>(SelectCommand);

            List<MigrateCollection> parentInGUIDs, parentOutGUIDs;
            List<MigrateCollection> childInIDs = new List<MigrateCollection>();
            List<MigrateCollection> childOutIDs = new List<MigrateCollection>();

            foreach (T mAssign in MainAssignmentCollection)
            {
                parentInGUIDs = JSONstringToCollection<MigrateCollection>(GetGUIDFromIDParentDB(mAssign.In.Replace(@"#", "")));
                parentOutGUIDs = JSONstringToCollection<MigrateCollection>(GetGUIDFromIDParentDB(mAssign.Out.Replace(@"#", "")));
                childInIDs = new List<MigrateCollection>();
                childOutIDs = new List<MigrateCollection>();

                foreach (MigrateCollection mA in parentInGUIDs)
                {
                    childInIDs.Add(new MigrateCollection() { rid = GetChildId(mA.GUID, mA.@class) });
                }
                foreach (MigrateCollection mA in parentOutGUIDs)
                {
                    childOutIDs.Add(new MigrateCollection() { rid = GetChildId(mA.GUID, mA.@class) });
                }

                foreach (MigrateCollection in_ in childInIDs)
                {
                    foreach (MigrateCollection out_ in childOutIDs)
                    {
                        CreateEdge<T>(out_, in_);
                    }
                }
            }
        }

        public ODatabase openDatabase(OrientDB_Net.binary.Innov8tive.API.ConnectionOptions connectionOption_)
        {
            ODatabase odb = new ODatabase(connectionOption_);
            return odb;
        }
        public void SpagettyCommands()
        {

            string Authurl = string.Format(@"{0}{1}:{2}/connect/{3}",
            @"http://",
            ConfigurationManager.AppSettings["ParentHost"],
            "2480",
            ConfigurationManager.AppSettings["ParentDBname"]
            );

            //OrientDB_Net.binary.Innov8tive.API.ConnectionOptions

            OrientDB_Net.binary.Innov8tive.API.ConnectionOptions childBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();
            OrientDB_Net.binary.Innov8tive.API.ConnectionOptions parentBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();

            childBasePool.HostName = ConfigurationManager.AppSettings["ChildHost"];
            childBasePool.UserName = ConfigurationManager.AppSettings["ChildLogin"];
            childBasePool.Password = ConfigurationManager.AppSettings["ChildPassword"];
            childBasePool.Port = 2424;
            childBasePool.DatabaseName = ConfigurationManager.AppSettings["ChildDBname"];
            childBasePool.DatabaseType = ODatabaseType.Document;

            parentBasePool.HostName = ConfigurationManager.AppSettings["ParentHost"];
            parentBasePool.UserName = ConfigurationManager.AppSettings["ParentLogin"]; ;
            parentBasePool.Password = ConfigurationManager.AppSettings["ParentPassword"];
            parentBasePool.Port = 2424;
            parentBasePool.DatabaseName = ConfigurationManager.AppSettings["ParentDBname"];
            parentBasePool.DatabaseType = ODatabaseType.Graph;


            string addVertex = string.Format(@"var d = orient.getGraph(); var vertex = db.addVertex({0} EXTENDS V)", @"Test_vertex");
            string addClassV = string.Format(@"var gdb = orient.getGraph(); var newClass = db.createVertexType({0},{1})", "Friend", "V");

            //http://orientdb.com/docs/last/NET-Server-CreateDatabase.html
            OServer os = new OServer(childBasePool.HostName, childBasePool.Port, childBasePool.UserName, childBasePool.Password);
            //OServer os = new OServer("msk1-vm-ovisp02",2424, "root", "I9grekVmk5g");
            if (!os.DatabaseExist(childBasePool.DatabaseName, OStorageType.PLocal))
            {
                os.CreateDatabase(childBasePool.DatabaseName, childBasePool.DatabaseType, OStorageType.PLocal);
            }

            var a = typeof(OServer);
            var b = os.GetType();

            Dictionary<string, string> serverConfig = os.ConfigList();
            var jsonConfig = JsonConvert.SerializeObject(serverConfig);

            ODatabase childBase = openDatabase(childBasePool);
            ODatabase parentBase = openDatabase(parentBasePool);



        }

        public void ChilldBaseDrop()
        {
            if (os.DatabaseExist(childBasePool.DatabaseName, OStorageType.PLocal))
            {
                os.DropDatabase(childBasePool.DatabaseName, OStorageType.PLocal);
            }
        }
        public List<ODocument> GetDbClasses(ODatabase db, string command)
        {
            return db.Query(command);
        }

        public List<ODocument> FilterUniqueKeys(List<ODocument> parent_, List<ODocument> child_)
        {
            List<ODocument> uniqueParentKeys =
                parent_.Except(
                from z in (from s in child_ where s.Keys.Contains("name") select s)
                join x in (from s in child_ where s.Keys.Contains("name") select s) on z["name"] equals x["name"]
                select z).ToList();
            return uniqueParentKeys;
        }

        public void JsonSerialize(object object_, string name_)
        {
            string object_str = JsonConvert.SerializeObject(object_, Formatting.Indented);
            File.WriteAllText(string.Format(@"C:\workflow\temp\serialized_{0}.txt", name_), object_str);
        }

        public void AddClasses(List<ODocument> collection, ODatabase database_)
        {
            foreach (ODocument class_ in collection)
            {
                var props = class_["properties"];
                string name = (string)class_["name"];
                string superclass = (string)class_["superClass"];
                database_.Command(
                    string.Format(ConfigurationManager.AppSettings["CreateClass"], name)
                    );
            }
        }
        public void AddClassesNested(List<ODocument> collection, ODatabase database_)
        {
            foreach (ODocument class_ in collection)
            {
                var props = class_["properties"];
                string name = (string)class_["name"];
                string superclass = (string)class_["superClass"];
                database_.Command(
                    string.Format(ConfigurationManager.AppSettings["CreateClassNested"], name, superclass)
                    );
            }
        }
        public void AddProperties(IEnumerable<ODocument> classes, ODatabase parentBase_, ODatabase childBase_)
        {
            foreach (ODocument class_ in classes)
            {
                IEnumerable<ODocument> properties = GetProperties(class_, parentBase_);
                if (properties != null)
                {
                    foreach (ODocument property in properties)
                    {
                        AddPropertyToClass(property, class_, childBase_);
                    }
                }


            }
        }

        public IEnumerable<ODocument> GetProperties(ODocument class_, ODatabase parentBase_)
        {
            IEnumerable<ODocument> result = null;
            Orient.Client.API.Query.OSqlSchema schema_ = parentBase_.Schema;
            string className = (string)class_["name"];
            if (className != "Object_SC" && (string)class_["superClass"] != "Object_SC")
            {
                if ((string)class_["superClass"] != "V" || (string)class_["superClass"] != "E")
                {
                    result = schema_.Properties((string)class_["superClass"]);
                    IEnumerable<ODocument> result2 = schema_.Properties((string)class_["name"]);
                    result = result.Concat(result2);
                }
                else
                {
                    result = schema_.Properties((string)class_["name"]);
                }

            }
            return result;
        }
        public void AddPropertyToClass(ODocument property_, ODocument class_, ODatabase childDatabase_)
        {
            string className = (string)class_["name"];
            string name = (string)property_["name"];
            int type = (int)property_["type"];
            string typeStr = OrientNumToCLRType.ValuetoString(type);
            string comm = string.Format(ConfigurationManager.AppSettings["CreateProperty"], className, name, typeStr);
            childDatabase_.Command(comm);
        }
        public List<ODocument> FilterClasses(List<ODocument> classes_, string type_)
        {
            return (from s in classes_ where (string)s["superClass"] == type_ select s).ToList();
        }
        public List<ODocument> FilterClassesBySuperclass(List<ODocument> classes_, List<ODocument> filter_)
        {
            return (from s in classes_
                    join c in filter_ on s["superClass"] equals c["name"]
                    select s).ToList();
        }

        public void AuthorizeOrientDB(string url)
        {
            WebRequest AuthRequest = WebRequest.Create(url);
            AuthRequest.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ParentLogin"], ConfigurationManager.AppSettings["ParentPassword"]);
            AuthRequest.Method = "GET";
            AuthRequest.ContentType = "application/json; charset=utf-8";
            WebResponse response = AuthRequest.GetResponse();
            OSESSIONID = response.Headers.Get("Set-Cookie");
            int status = (int)((HttpWebResponse)response).StatusCode;
            response.Close();
        }
        public string GetFromParentDB(string url)
        {
            WebRequest wr = WebRequest.Create(url);
            wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
            wr.Method = "GET";
            wr.ContentType = "application/json; charset=utf-8";
            var resp = new HttpResponseMessage(HttpStatusCode.OK);

            string res = WebRequestToString(wr);
            return res;
        }
        public object GetObj(string url)
        {
            WebRequest wr = WebRequest.Create(url);
            wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
            wr.Method = "GET";
            wr.ContentType = "application/json; charset=utf-8";
            var resp = new HttpResponseMessage(HttpStatusCode.OK);

            object res = WebRequestToObj(wr);
            return res;
        }
        public string WebRequestToString(WebRequest wr)
        {
            using (var responseApi = (HttpWebResponse)wr.GetResponse())
            {
                using (var reader = new StreamReader(responseApi.GetResponseStream()))
                {
                    string objText = reader.ReadToEnd();
                    var objClasses = JsonConvert.DeserializeObject(objText);
                    string jsonResult = objClasses.ToString();
                    return jsonResult;
                }
            }
        }
        public object WebRequestToObj(WebRequest wr)
        {
            using (var responseApi = (HttpWebResponse)wr.GetResponse())
            {
                using (var reader = new StreamReader(responseApi.GetResponseStream()))
                {
                    var objText = reader.ReadToEnd();
                    var objClasses = JsonConvert.DeserializeObject(objText);


                    return objClasses;

                }
            }
        }

        public List<T> JSONstringToCollection<T>(string input_) where T : class
        {
            JObject jarr = JObject.Parse(input_);
            List<JToken> results = jarr["result"].Children().ToList();
            List<T> persList = new List<T>();

            foreach (JToken jt in results)
            {
                persList.Add(jt.ToObject<T>());
            }
            return persList;
        }
        public string ObjectToJSONString<T>(T Object) where T : class
        {
            string res = JsonConvert.SerializeObject(Object, new IsoDateTimeConverter() { DateTimeFormat = @"yyyy-MM-dd HH:mm:ss" }).ToString();
            return res;
        }

        public string JsonToCommand(string ApiGetRes)
        {
            ApiGetRes = ApiGetRes.Replace("[", "").Replace("]", "");
            return ApiGetRes;
        }
        public string SelectCommand(string command_)
        {
            return string.Format(@"{0}{1}:{2}/command/{3}/sql/{4}",
            @"http://",
            ConfigurationManager.AppSettings["ParentHost"],
            "2480",
            ConfigurationManager.AppSettings["ParentDBname"],
            //@"select Seed,Created,GUID,Changed,FirstName,LastName,MiddleName,Birthday,mail,telephoneNumber,userAccountControl,objectGUID,sAMAccountName,Name,Hash,OneSHash from person LIMIT 5");         
            command_);
        }

        public List<T> GetEntityCollectionFromParentBase<T>(string command_) where T : class
        {
            List<T> list = JSONstringToCollection<T>(GetFromParentDB(SelectCommand(command_)));
            return list;
        }
        public void MigrateEntity<T>(ODatabase childBase_) where T : class
        {
            string SelectCommand = string.Format(ConfigurationManager.AppSettings["SelectCommand"], "*", typeof(T).Name);
            List<T> entities = GetEntityCollectionFromParentBase<T>(SelectCommand);
            //Person pers = JsonConvert.DeserializeObject<Person>(ApiGetRes);
            //GetObj(commUrl);
            //Get(commUrl);
            foreach (T item in entities)
            {
                string command = JsonToCommand(ObjectToJSONString<T>(item));
                //valid insert command
                //command = @"{""Seed"":""1005821"",""Created"":""2014-12-15 12:21:24"",""GUID"":""4e28f31a-89b8-11e4-bab2-00c2c66d13b0"",""Changed"":""2017-08-23 10:06:45"",""DeparmentColorRGB"":null,""DeparmentColorClass"":null,""Disabled"":null}";
                command = string.Format(
                    ConfigurationManager.AppSettings["InsertEntityCommand"],
                     item.GetType().Name
                    , command.Replace(@"""0001-01-01 00:00:00""", "null"));

                childBase_.Command(command);
            }

        }

        public string GetGUIDFromIDParentDB(string ID)
        {
            string result = null;
            string command_ = string.Format(ConfigurationManager.AppSettings["SelectCommand"], "@rid,@class,GUID", ID);
            result = GetFromParentDB(SelectCommand(command_));
            return result;
        }
        public string GetChildId(string GUID, string class_)
        {
            string result = null;
            string command_ = string.Format(ConfigurationManager.AppSettings["SelectWhereCommand"], "@rid", class_, @"GUID = '" + GUID + "'");
            OCommandResult Oresult = childBase.Command(command_);
            result = (from s in Oresult.ToList() select s).FirstOrDefault()["rid"].ToString();
            return result;
        }
        public void CreateEdge<T>(MigrateCollection From, MigrateCollection To) where T : class, Edge
        {
            string command_ = string.Format(ConfigurationManager.AppSettings["CreateEdge"], typeof(T).Name, From.rid, To.rid);
            childBase.Command(command_);
        }

    }

    //MODEL scope
    #region POCOs
    //@JsonInclude(Include.NON_NULL)

    public class Person
    {
        [JsonProperty("type")]
        public string @type { get; set; }
        public string @rid { get; set; }
        public string @version { get; set; }
        [JsonProperty("class")]
        public string @class { get; set; }

        public long? Seed { get; set; }
        public DateTime? Created { get; set; }
        public string GUID { get; set; }
        public DateTime? Changed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? Birthday { get; set; }
        public string mail { get; set; }
        public int? telephoneNumber { get; set; }
        public int? userAccountControl { get; set; }
        public string objectGUID { get; set; }
        public string sAMAccountName { get; set; }
        public string Name { get; set; }
        public string OneSHash { get; set; }
        public string Hash { get; set; }

        /*
        public List<string> in_MainAssignment { get; set; }
        public List<string> out_MainAssignment { get; set; }
        */

        [JsonProperty("fieldTypes")]
        public string @fieldTypes { get; set; }


    }
    public class Unit
    {
        public long? Seed { get; set; }
        public DateTime? Created { get; set; }
        public string GUID { get; set; }
        public DateTime? Changed { get; set; }
        public string PGUID { get; set; }
        public string DepartmentColorRGB { get; set; }
        public string DepartmentColorClass { get; set; }
        public DateTime? Disabled { get; set; }
        public string Hash { get; set; }
        public string Name { get; set; }

    }
    public interface Edge
    {
        string Out { get; set; }
        string In { get; set; }
    }
    public class MainAssignment : Edge
    {
        public string Out { get; set; }
        public string In { get; set; }
    }
    public class OldMainAssignment : Edge
    {
        public string Out { get; set; }
        public string In { get; set; }
    }
    public class OutExtAssignment : Edge
    {
        public string Out { get; set; }
        public string In { get; set; }
    }
    public class SubUnit : Edge
    {
        public string Out { get; set; }
        public string In { get; set; }
    }
    public class MigrateCollection
    {
        public string @rid { get; set; }
        public string @class { get; set; }
        public string GUID { get; set; }
    }
    public class J_Address
    {
        [JsonProperty("GUID")]
        string GUID { get; set; }
        [JsonProperty("Адрес")]
        string Address { get; set; }
    }

    #endregion

    //DRIVER scope
    public static class OrientNumToCLRType
    {
        public static string ValuetoString(int value_)
        {
            string result = null;
            switch (value_)
            {

                case 3:
                    result = @"LONG";
                    break;
                case 6:
                    result = @"DATETIME";
                    break;
                case 7:
                    result = @"STRING";
                    break;

            }
            return result;
        }
    }

    /// <summary>
    /// Testing APIS
    /// Manages class for JSON API url,expected,ectual,ok values. Read/create + export JSON file with values.
    /// For every URL execute webrequest, reads string response, compares with Expected value, 
    /// changes statusess - OK(true/flase), Exception message - if needed. Exports result.
    /// </summary>
    public class APItester_sngltn
    {
        public string URI { get; set; }
        public string Expected { get; set; }
        public string Actual { get; set; }
        public bool OK { get; set; }
        public string ExceptionText { get; set; }
        public string Comment { get; set; }

        public static List<APItester_sngltn> TestCases = new List<APItester_sngltn>();

        public void OK_(string actual_)
        {
            this.Actual = actual_;
            this.ExceptionText = string.Empty;
            this.OK = true;
        }
        public void OK_(string actual_, string exception_)
        {
            this.Actual = actual_;
            this.ExceptionText = exception_;
            this.OK = true;
        }

        public void NotOK_(string actual_)
        {
            this.Actual = actual_;
            this.ExceptionText = string.Empty;
            this.OK = false;
        }
        public void NotOK_(string actual_, string exception_)
        {
            this.Actual = actual_;
            this.ExceptionText = exception_;
            this.OK = false;
        }

    }


    //<--Out - deprecated
    //WEB scope
    //divide to string method initiation (GET,POST -> execute), add parameters addition before call <- done
    //move web request object to separate class <- done
    //JSON reader to new class, return only web request, then decide how to handle it (JSON,XML,STRING,ACTION) <- done
    /*
    public class HTTPmanager
    {
        public static string OSESSIONID;

        public int HTTPauthProxy(string url, string login, string password)
        {
            WebRequest AuthRequest = WebRequest.Create(url);
            AuthRequest.Credentials = new NetworkCredential(login, password);
            AuthRequest.Method = "GET";
            AuthRequest.ContentType = "application/json; charset=utf-8";
            WebResponse response = AuthRequest.GetResponse();
            OSESSIONID = response.Headers.Get("Set-Cookie");
            int status = (int)((HttpWebResponse)response).StatusCode;
            response.Close();
            return status;
        }

        public string Get(string url_)
        {
            WebRequest wr = WebRequest.Create(url_);
            wr.Method = "GET";
            wr.ContentType = "application/json; charset=utf-8";

            using (HttpWebResponse response = (HttpWebResponse)wr.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var objText = reader.ReadToEnd();
                    var objClasses = JsonConvert.DeserializeObject(objText);
                    var jsonResult = objClasses.ToString();

                    return jsonResult;
                }
            }
        }

        public string GetAuthProx(string url_)
        {
            WebRequest wr = WebRequest.Create(url_);
            wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
            wr.Method = "GET";
            wr.ContentType = "application/json; charset=utf-8";

            using (var response = (HttpWebResponse)wr.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var objText = reader.ReadToEnd();
                    var objClasses = JsonConvert.DeserializeObject(objText);
                    var jsonResult = objClasses.ToString();

                    return jsonResult;
                }
            }
        }
        public string POSTAuthProx(string url_)
        {
            WebRequest wr = WebRequest.Create(url_);
            wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
            wr.Method = "POST";
            wr.ContentType = "application/json; charset=utf-8";

            using (var response = (HttpWebResponse)wr.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var objText = reader.ReadToEnd();
                    var objClasses = JsonConvert.DeserializeObject(objText);
                    var jsonResult = objClasses.ToString();

                    return jsonResult;
                }
            }
        }

    }
    */


    public interface IJsonManger
    {
        List<T> DesSample<T>(string input) where T : class;
        List<T> DesParent<T>(string input,string parent) where T : class;
        List<T> DesChildFromParent<T>(string input, string node, string child) where T : class;
    }
    /// <summary>
    /// Gets response from URL with method
    /// </summary>
    public interface IWebManager
    {
        WebResponse GetResponse(string url, string method);
    }
    /// <summary>
    /// Reads response and converts it to string
    /// </summary>
    public interface IWebResponseReader
    {
        string ReadResponse(HttpWebResponse response);
        string ReadResponse(WebResponse response);
        string ReadResponse(HttpResponseMessage response);
    }


    /// -->In new overwritten
    ///Base web manager for sending request with type method and reading response to URL
    ///WebRequest, Httpwebresponse
    public class WebManager : IWebManager
    {
        public WebRequest _request;
        internal string OSESSIONID;

        public WebManager()
        {
            _request = null;
            this.OSESSIONID = string.Empty;
        }
        public WebRequest addRequest(string url, string method)
        {
            try
            {
                _request = WebRequest.Create(url);
                _request.Method = method;
                _request.ContentLength = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
            return _request;
        }
        internal void addHeader(HttpRequestHeader header, string value)
        {
            _request.Headers.Add(header, value);
        }
        internal string getHeaderValue(string header)
        {
            string result = string.Empty;
            result = this._request.GetResponse().Headers.Get(header);
            return result;
        }
        internal void addCredentials(NetworkCredential credentials)
        {
            this._request.Credentials = credentials;
        }
        public virtual WebResponse GetResponse(string url, string method)
        {
            HttpWebResponse resp;
            addRequest(url, method);

            try
            {
                resp = (HttpWebResponse)this._request.GetResponse();
                
            }
            catch (Exception e)
            {
                throw e;
            }

            return resp;
        }
    }
    /// <summary>
    ///     Orient specific WebManager for authentication and authenticated resopnses sending to URL
    ///     with NetworkCredentials
    /// </summary>    
    public class OrientWebManager : WebManager
    {

        public new HttpWebResponse GetResponse(string url, string method)
        {
            HttpWebResponse resp=null;
            base.addRequest(url, method);
            addHeader(HttpRequestHeader.Cookie, this.OSESSIONID);
            try
            {
                resp = (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }

            return resp;
        }
        public WebResponse Authenticate(string url, NetworkCredential nc)
        {

            WebResponse resp;
            addRequest(url, "GET");
            addCredentials(nc);
            try
            {
                resp = this._request.GetResponse();
                OSESSIONID = getHeaderValue("Set-Cookie");
                return resp;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }

    /// <summary>
    /// WEB scope deprecation posible (Only if several credentials to different hosts needed, several DBs? )
    /// Contains Credentials for URI
    /// </summary>    
    public class CredentialPool
    {
        CredentialCache credentialsCache;

        public void Add(Uri uri, string type, string username, string password)
        {
            credentialsCache = new CredentialCache();
            credentialsCache.Add(uri, type, new NetworkCredential(username, password));
        }
        public NetworkCredential credentials(Uri uri_, string type_)
        {
            return credentialsCache.GetCredential(uri_, type_);
        }
    }

    /// <summary>
    /// DP Scope (data processing)    
    /// converts Responses to string
    /// </summary>
    public class WebResponseReader : IWebResponseReader
    {
        public string ReadResponse(HttpWebResponse response)
        {
            string result = string.Empty;
            Stream sm = response.GetResponseStream();
            StreamReader sr = new StreamReader(sm);
            result = sr.ReadToEnd();
            return result;
        }
        public string ReadResponse(WebResponse response)
        {
            string result = string.Empty;
            Stream sm = response.GetResponseStream();
            StreamReader sr = new StreamReader(sm);
            result = sr.ReadToEnd();
            return result;
        }
        public string ReadResponse(HttpResponseMessage response)
        {
            string result = string.Empty;
            System.Net.Http.HttpContent sm = response.Content;
            Task<Stream> sr = sm.ReadAsStreamAsync();
            Task<string> res = sm.ReadAsStringAsync();
            result = res.Result;
            return result;
        }
    }

    /// <summary>
    /// DP Scope (data processing)    
    /// works with JSON stings
    /// </summary>
    public class JSONmanager
    {

        //For sample JSON structure [{a:1,..,c:1},{a:10,..,c:10}]
        public IEnumerable<T> DeseializeSample<T>(string resp) where T : class
        {
            IEnumerable<T> res = JsonConvert.DeserializeObject<IEnumerable<T>>(resp);
            return res;
        }
        public IEnumerable<T> DeserializeNode<T>(string jInput, string Node) where T : class
        {
            IEnumerable<T> result = null;
            IEnumerable<JToken> res = JToken.Parse(jInput).Children()[Node].ToList();
            result = CollectionConvert<T>(res);
            return result;
        }
        //For JSON structure {NodeName:[{a:1,..,c:1},{a:10,..,c:10}]}
        public IEnumerable<T> DeserializeFromNode<T>(string jInput, string Node) where T : class
        {
            IEnumerable<T> result = null;
            IEnumerable<JToken> res = JToken.Parse(jInput)[Node].Children().ToList();
            result = CollectionConvert<T>(res);
            return result;
        }
        //For parsing not to model but to String for JSON structure {NodeName:[{a:1,..,c:1},{a:10,..,c:10}]}
        //where one item from collection can be parsed
        public IEnumerable<T> DeserializeFromNode<T>(string jInput, string Node, string field) where T : class
        {
            IEnumerable<T> result = null;
            IEnumerable<JToken> res = JObject.Parse(jInput)[Node].Children()[field].ToList();
            result = CollectionConvert<T>(res);
            return result;
        }
        public IEnumerable<T> CollectionConvert<T>(IEnumerable<JToken> input) where T: class
        {
            IEnumerable<T> result = null;
                result = (from s in input select s.ToObject<T>()).ToList();
            return result;
        }

        //Extracts collection of tokens [{"Tk1":"A"},..,{"Tk31":"Z"}] -> IJEnumerable<JTokens> ["A",..,"Z"]
        public IJEnumerable<JToken> ExtractTokens(string jInput, string field)
        {
            IJEnumerable<JToken> result = null;
            result = JToken.Parse(jInput).Children()[field];
            return result;
        }
        public IJEnumerable<JToken> ExtractTokenChildren(string jInput, string field)
        {
            IJEnumerable<JToken> result = null;
            result = JToken.Parse(jInput).Children()[field].Children();
            return result;
        }
        public IJEnumerable<JToken> ExtractTokens(string jInput, string Node, string field)
        {
            IJEnumerable<JToken> result = null;
            result = JObject.Parse(jInput)[Node].Children()[field];
            return result;
        }
    
        public string CollectionToStringFormat<T>(IEnumerable<T> list_, JsonSerializerSettings jss = null) where T : class
        {
            string result = null;
            result = JsonConvert.SerializeObject(list_, jss);
            return result;
        }

    }
    public class JSONorient : JSONmanager
    {
        public string FilterChildren(string input,string name)
        {            
            IEnumerable<JToken> x = this.ExtractTokenChildren(input, "Name");
            string x2 = this.CollectionToStringFormat<IEnumerable<JToken>>(x, new JsonSerializerSettings() { Formatting = Formatting.None });
            return x2;
        }
    }

    /// <summary>
    /// no any Buisiness logic
    /// divide to query class with inheritance|generic 
    /// to commandtpyes (command|query,batch,function)
    /// </summary>
    public class OrientApiUrlBuilder
    {
        string selectCommand = String.Empty;
        string functionCommand = String.Empty;

        public string SelectCommandSet(string className_, string filter_)
        {
            this.selectCommand = SelectClause(className_, WhereClause(filter_));
            return selectCommand;
        }
        public string SelectCommandGet()
        {
            if (this.selectCommand != String.Empty)
            {
                return this.selectCommand;
            }

            return null;
        }

        string SelectClause(string classname_, string whereClause = null)
        {
            return String.Format(@"select from {0} {1}", classname_, whereClause);
        }
        string WhereClause(string whereClause_)
        {
            return String.Format(@"where {0}", whereClause_);
        }
        public string FucntionSet(string Fucntion_, string params_)
        {
            this.functionCommand = String.Format(@"{0}/{1}", Fucntion_, params_);
            return functionCommand;
        }
        public string FunctionCommandGet()
        {
            return functionCommand;
        }

    }


    //For token items
    public interface ITypeToken
    {
        string Text { get; }
    }
    //For different API type URIs
    public interface IUrIBuilder
    {      
        string GetUrl();
    }
    
    /// <summary>
    /// Tokens for Orient API URIs 
    /// Different API types tend to different Http req strategies example: Fucntion/param or: Batch/ + JSON-body    
    /// </summary>
    public class Host : ITypeToken
    {
        public string Text { get { return "Batch"; } }
    }
    public class Db : ITypeToken
    {
        public string Text { get { return "Batch"; } }
    }
    public class Port : ITypeToken
    {
        public string Text { get { return "Batch"; } }
    }

    public class OrientFuncionToken : ITypeToken
    {
        public string Text { get { return "Function"; } }
    }
    public class OrientCommandToken : ITypeToken
    {
        public string Text { get { return "Command"; } }
    }
    public class OrientConnectToken : ITypeToken
    {
        public string Text { get { return "Connect"; } }
    }
    public class OrientBatchToken : ITypeToken
    {
        public string Text { get { return "Batch"; } }
    }


    public abstract class UriBuilder : IUrIBuilder
    {

        internal ITypeToken _Host;
        internal ITypeToken _Port;
        internal ITypeToken _DbName;
        internal ITypeToken _ApiType;

        internal string ApiUrl { get; set; }

        public UriBuilder(ITypeToken host_, ITypeToken DbName_, ITypeToken ApiType_, ITypeToken port_ = null)
        {
            this._Host = host_;
            this._Port = port_;
            this._DbName = DbName_;
            this._ApiType = ApiType_;
        }

        public virtual void SetUrl()
        {
            this.ApiUrl = string.Format("{0}/{1}/{2]", _Host, _Port, _DbName);
        }
        public virtual string GetUrl() {
            return this.ApiUrl;
        }

    }

    public class AuthUrIBuilder : UriBuilder
    {
        ITypeToken authToken;

        public AuthUrIBuilder(ITypeToken ApiType_, ITypeToken host_, ITypeToken DbName_) 
            : base (host_,DbName_,ApiType_)
        {

        }
        public override void SetUrl()
        {
            this.ApiUrl = string.Format("{0}/{1}/{2}",this._Host.Text, authToken.Text,this._DbName.Text);
        }
    }
    public class CommandUrIBuilder : UriBuilder
    {
        ITypeToken authToken;

        public CommandUrIBuilder(ITypeToken ApiType_,ITypeToken host_, ITypeToken DbName_, ITypeToken Port_)
            : base(ApiType_, host_, DbName_,  Port_)
        {

        }
        public override void SetUrl()
        {
            this.ApiUrl = string.Format("{0}/{1}/{2}/{3}", this._Host.Text, this._Port.Text, authToken.Text, this._DbName.Text);
        }
    }


}

