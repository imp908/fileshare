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
using System.Web;


namespace ConsoleApp1
{

    class OrientDriverConnnect
    {


        static void Main(string[] args)
        {

            HTTPcheck();

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

        public static void managersCheck()
        {

            string url = @"http://10.31.14.76/cleverence_ui/hs/IntraService/location/full";
            HTTPmanager hm = new HTTPmanager();
            JSONmanager jm = new JSONmanager();

            //sample JSON [{},{}] http get from url
            string jres = hm.Get(url);
            List<J_Address> addresses = jm.DeseializeSample<J_Address>(jres);

            //authenticate Orient
            hm.HTTPauthProxy(@"http://msk1-vm-ovisp02:2480/connect/news_test3", "root", "I9grekVmk5g");

            //authenticated htp response from command
            url = @"http://msk1-vm-ovisp02:2480/command/news_test3/sql/select from Person";
            jres = hm.GetAuthProx(url);
            //\{node:[{},{}]\}
            List<Person> persons = jm.DeserializeFromNode<Person>(jres, @"result");

            url = @"http://msk1-vm-ovisp02:2480/function/news_test3/SearchByLastNameTst/сав";
            jres = hm.GetAuthProx(url);
            //\{node:[{},{}]\}
            persons = jm.DeserializeFromNode<Person>(jres, @"result");

            //authenticated htp response from command
            url = @"http://msk1-vm-ovisp02:2480/command/news_test3/sql/select from Unit";
            jres = hm.GetAuthProx(url);
            //\{node:[{},{}]\}
            List<Unit> units = jm.DeserializeFromNode<Unit>(jres, @"result");

        }

        public static void BuilderCheck()
        {


            OrientApiUrlBuilder ob = new OrientApiUrlBuilder();
            HTTPmanager hm = new HTTPmanager();
            JSONmanager jm = new JSONmanager();

            //задаем параметры комманды, тело селекта и тело where (можно оставить пустым)
            ob.SelectCommandSet("Person", @"1=1");
            //авторизуемся
            hm.HTTPauthProxy(@"http://msk1-vm-ovisp02:2480/connect/news_test3", "root", "I9grekVmk5g");
            //получам ответ форматированный в JSON сторку
            string jsonStringResponse = hm.GetAuthProx(@"http://msk1-vm-ovisp02:2480/command/news_test3/sql/" + ob.SelectCommandGet());
            //десериализуем в модель, проксирование управляется полями POCO класса модели и атрибутами
            List<Person> persons = jm.DeserializeFromNode<Person>(jsonStringResponse, @"result");

            //Аналогично для Unit
            ob.SelectCommandSet("Unit", @"1=1");
            jsonStringResponse = hm.GetAuthProx(@"http://msk1-vm-ovisp02:2480/command/news_test3/sql/" + ob.SelectCommandGet());
            List<Unit> units = jm.DeserializeFromNode<Unit>(jsonStringResponse, @"result");

            //Аналогично для функции
            //Так как функция возвразает только одно поле, коллекция заполнится объектами только с 1 полем
            ob.FucntionSet("SearchByLastName", @"сав");
            jsonStringResponse = hm.GetAuthProx(@"http://msk1-vm-ovisp02:2480/function/news_test3/" + ob.FunctionCommandGet());
            List<Person> persons2 = jm.DeserializeFromNode<Person>(jsonStringResponse, @"result");

            //Кручу-верчу для получения "чистых" коллекицй 
            //(когда возращается только часть полей, null values игнорятся, без изменения модели)
            string jString = JsonConvert.DeserializeObject(
                JsonConvert.SerializeObject(persons2, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore })
                ).ToString();

        }

        public static void HTTPcheck()
        {
            ResponseReader reader = new ResponseReader();

            //read basic GET response
            string url = @"http://msk1-vm-ovisp01:8083/api/Person/GetCollegesLower/bs";
            WebManager hm = new WebManager();
            WebResponse response = hm.GetResponse(url, "GET");
            string sampleResult = reader.ReadResponse(response);


            //Authentication 
            string authUrl = @"http://msk1-vm-ovisp02:2480/connect/news_test3";
            CredentialPool cd = new CredentialPool();
            cd.Add(new Uri(authUrl), "Basic", "root", "I9grekVmk5g");
            NetworkCredential nc = cd.credentials(new Uri(authUrl), "Basic");

            //read Orient fucntion GET with authentication            
            url = @"http://msk1-vm-ovisp02:2480/function/news_test3/GetCollegesLowerByAccount/bs";
            OrientWebManager orm = new OrientWebManager();
            orm.Authenticate(authUrl, nc);
            response = orm.GetResponse(url, "GET");
            string resultOrient = reader.ReadResponse(response);


            //deserializing string request
            JSONmanager mng = new JSONmanager();
            List<string> a = mng.DeseializeSample<string>(sampleResult);
            List<string> b = mng.DeserializeFromNode(resultOrient, "result","Name");

            //change equality check
            //they are equal
            bool res = a.Equals(b);

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
            ConfigurationSettings.AppSettings["ParentHost"],
            "2480",
            ConfigurationSettings.AppSettings["ParentDBname"]
            );

            childBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();
            parentBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();

            childBasePool.HostName = ConfigurationSettings.AppSettings["ChildHost"];
            childBasePool.UserName = ConfigurationSettings.AppSettings["ChildLogin"];
            childBasePool.Password = ConfigurationSettings.AppSettings["ChildPassword"];
            childBasePool.Port = 2424;
            childBasePool.DatabaseName = ConfigurationSettings.AppSettings["ChildDBname"];
            childBasePool.DatabaseType = ODatabaseType.Graph;

            parentBasePool.HostName = ConfigurationSettings.AppSettings["ParentHost"];
            parentBasePool.UserName = ConfigurationSettings.AppSettings["ParentLogin"]; ;
            parentBasePool.Password = ConfigurationSettings.AppSettings["ParentPassword"];
            parentBasePool.Port = 2424;
            parentBasePool.DatabaseName = ConfigurationSettings.AppSettings["ParentDBname"];
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
            childClasses = GetDbClasses(childBase, ConfigurationSettings.AppSettings["GetAllClasses"]);
            parentClasses = GetDbClasses(parentBase, ConfigurationSettings.AppSettings["GetAllClasses"]);

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
            string SelectCommand = string.Format(ConfigurationSettings.AppSettings["SelectCommand"], "in,out", typeof(T).Name);
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
            ConfigurationSettings.AppSettings["ParentHost"],
            "2480",
            ConfigurationSettings.AppSettings["ParentDBname"]
            );

            //OrientDB_Net.binary.Innov8tive.API.ConnectionOptions

            OrientDB_Net.binary.Innov8tive.API.ConnectionOptions childBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();
            OrientDB_Net.binary.Innov8tive.API.ConnectionOptions parentBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();

            childBasePool.HostName = ConfigurationSettings.AppSettings["ChildHost"];
            childBasePool.UserName = ConfigurationSettings.AppSettings["ChildLogin"];
            childBasePool.Password = ConfigurationSettings.AppSettings["ChildPassword"];
            childBasePool.Port = 2424;
            childBasePool.DatabaseName = ConfigurationSettings.AppSettings["ChildDBname"];
            childBasePool.DatabaseType = ODatabaseType.Document;

            parentBasePool.HostName = ConfigurationSettings.AppSettings["ParentHost"];
            parentBasePool.UserName = ConfigurationSettings.AppSettings["ParentLogin"]; ;
            parentBasePool.Password = ConfigurationSettings.AppSettings["ParentPassword"];
            parentBasePool.Port = 2424;
            parentBasePool.DatabaseName = ConfigurationSettings.AppSettings["ParentDBname"];
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
                    string.Format(ConfigurationSettings.AppSettings["CreateClass"], name)
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
                    string.Format(ConfigurationSettings.AppSettings["CreateClassNested"], name, superclass)
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
            string comm = string.Format(ConfigurationSettings.AppSettings["CreateProperty"], className, name, typeStr);
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
            AuthRequest.Credentials = new NetworkCredential(ConfigurationSettings.AppSettings["ParentLogin"], ConfigurationSettings.AppSettings["ParentPassword"]);
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
            ConfigurationSettings.AppSettings["ParentHost"],
            "2480",
            ConfigurationSettings.AppSettings["ParentDBname"],
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
            string SelectCommand = string.Format(ConfigurationSettings.AppSettings["SelectCommand"], "*", typeof(T).Name);
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
                    ConfigurationSettings.AppSettings["InsertEntityCommand"],
                     item.GetType().Name
                    , command.Replace(@"""0001-01-01 00:00:00""", "null"));

                childBase_.Command(command);
            }

        }

        public string GetGUIDFromIDParentDB(string ID)
        {
            string result = null;
            string command_ = string.Format(ConfigurationSettings.AppSettings["SelectCommand"], "@rid,@class,GUID", ID);
            result = GetFromParentDB(SelectCommand(command_));
            return result;
        }
        public string GetChildId(string GUID, string class_)
        {
            string result = null;
            string command_ = string.Format(ConfigurationSettings.AppSettings["SelectWhereCommand"], "@rid", class_, @"GUID = '" + GUID + "'");
            OCommandResult Oresult = childBase.Command(command_);
            result = (from s in Oresult.ToList() select s).FirstOrDefault()["rid"].ToString();
            return result;
        }
        public void CreateEdge<T>(MigrateCollection From, MigrateCollection To) where T : class, Edge
        {
            string command_ = string.Format(ConfigurationSettings.AppSettings["CreateEdge"], typeof(T).Name, From.rid, To.rid);
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
  


    //WEB scope
    //divide to string method initiation (GET,POST -> execute), add parameters addition before call
    //move web request object to class global
    //JSON reader to new class, return only web request, then decide how to handle it (JSON,XML,STRING,ACTION)
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

    //Base web manager for sending request with type method and reading response to URL
    //WebRequest, Httpwebresponse
    public class WebManager
    {
        internal WebRequest _request;
        internal string OSESSIONID = String.Empty;

        internal WebRequest addRequest(string url, string method)
        {
            _request = WebRequest.Create(url);
            _request.Method = method;
            _request.ContentLength = 0;
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
                throw;
            }

            return resp;
        }


    }
    //Orient specific WebManager for authentication and authenticated resopnses sending to URL
    //with NetworkCredentials
    public class OrientWebManager : WebManager
    {

        public override WebResponse GetResponse(string url, string method)
        {
            HttpWebResponse resp;
            base.addRequest(url, method);
            addHeader(HttpRequestHeader.Cookie, this.OSESSIONID);
            try
            {
                resp = (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                throw;
            }

            return resp;
        }
        public void Authenticate(string url, NetworkCredential nc)
        {

            WebResponse resp;
            addRequest(url, "GET");
            addCredentials(nc);
            try
            {
                resp = this._request.GetResponse();
                OSESSIONID = getHeaderValue("Set-Cookie");
            }
            catch (Exception e)
            {
                throw;
            }

        }

    }
    
    //WEB scope
    //Contains Credentials for uri
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
    
    //DP Scope (data processing)
    //converts Responses to string
    public class ResponseReader
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
    }

    //DP Scope (data processing)
    public class JSONmanager
    {
        //For sample JSON structure [{a:1,..,c:1},{a:10,..,c:10}]
        public List<T> DeseializeSample<T>(string resp) where T : class
        {
            List<T> res = JsonConvert.DeserializeObject<List<T>>(resp);
            return res;
        }

        //For JSON structure {NodeName:[{a:1,..,c:1},{a:10,..,c:10}]}
        public List<T> DeserializeFromNode<T>(string jInput, string Node) where T : class
        {
            List<T> result = new List<T>();
            List<JToken> res = JObject.Parse(jInput)[Node].Children().ToList();
            result = (from s in res select s.ToObject<T>()).ToList();
            return result;
        }
        //For parsing not to model but to String for JSON structure {NodeName:[{a:1,..,c:1},{a:10,..,c:10}]}
        //where one item from collection can be parsed
        public List<string> DeserializeFromNode(string jInput, string Node,string field) 
        {
            List<string> result = new List<string>();
            List<JToken> res = JObject.Parse(jInput)[Node].Children()[field].ToList();
            result = (from s in res select s.ToString()).ToList<string>();
            return result;
        }

    }

    //DB scope 
    //no any Buisiness logic
    //divide to query class with inheritance to commandtpyes (command/query,batch,function)
    public class OrientApiUrlBuilder
    {
        string selectCommand = String.Empty;
        string functionCommand = String.Empty;

        public string SelectCommandSet(string className_,string filter_)
        {
            this.selectCommand = SelectClause(className_, WhereClause(filter_));
            return selectCommand;
        }
        public string SelectCommandGet()
        {
            if(this.selectCommand != String.Empty)
            {
                return this.selectCommand;
            }

            return null;
        }

        string SelectClause (string classname_,string whereClause = null)
        {
            return String.Format(@"select from {0} {1}", classname_, whereClause);
        }    
        string WhereClause (string whereClause_)
        {
            return String.Format(@"where {0}", whereClause_);
        }
        public string FucntionSet(string Fucntion_, string params_)
        {
            this.functionCommand= String.Format(@"{0}/{1}", Fucntion_, params_);
            return functionCommand;
        }
        public string FunctionCommandGet()
        {
            return functionCommand;
        }

    }

}
