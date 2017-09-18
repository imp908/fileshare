using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Orient.Client;
using Newtonsoft.Json;

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
            OrientSpagettyCheck oc = new OrientSpagettyCheck();
            oc.SpagettyCommands();
        }

    }

    public class OrientSpagettyCheck
    {
        protected static string OSESSIONID;

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

        public ODatabase openDatabase(OrientDB_Net.binary.Innov8tive.API.ConnectionOptions connectionOption_)
        {
            ODatabase odb = new ODatabase(connectionOption_);
            return odb;
        }
        public void SpagettyCommands()
        {
                     
            string SQLcommand = "select from Person limit 10";
            
            //OrientDB_Net.binary.Innov8tive.API.ConnectionOptions

            OrientDB_Net.binary.Innov8tive.API.ConnectionOptions childBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();
            OrientDB_Net.binary.Innov8tive.API.ConnectionOptions parentBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();

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

            List<OCluster> clusterNamesBefore = childBase.GetClusters(false);
            

            // INITIALIZE CLUSTER ID's
            //out of range not work
            //short[] clusterIds = new short[] { 30,31,32 };
            short[] clusterIds = new short[] { 1, 2, 3 };


            //CREATE CLUSTERS
            //works only if clusters allready exists,
            //new clsuters, that out of DB range, throws
            // Orient.Client.OException
            //clusters not found
            Orient.Client.API.Query.OClusterQuery query = childBase.Clusters(clusterIds);
            long count = query.Count();

            List<OCluster> clusterNamesAfter = childBase.GetClusters(false);

            short clusterId0 = childBase.GetClusterIdFor("V");
            short clusterId1 = childBase.GetClusterIdFor("E");

            //Driver Select command
            var res = childBase.Select().From("V").ToList();

            //Driver update command
            /*
            OSqlUpdate ures = db.Update("V").Set("name", "name1");
            OSqlUpdate uAres = db.Update("V").Add("second_name", "name2");
            */

            //Error
            //db.Command(addClassV);

            //Error due to non indeponent
            //db.Query(createClass);

            //var batchRes = childBase.SqlBatch(addVertex);
            //batchResult.Run();

            //Driver Command result
            /*
            Orient.Client.OCommandResult queryCommandResult = childBase.Command(SQLcommand);
            Orient.Client.OCommandResult selectRes = parentBase.Command(@"select from Person limit 10");
            OSqlSelect comRes = parentBase.Select("Person");
            ODocument doc = selectRes.ToDocument();
            */

           
  
            //Get list of classes
            List<ODocument> childClasses = GetDbClasses(childBase,ConfigurationSettings.AppSettings["GetAllClasses"]);
            List<ODocument> parentClasses = GetDbClasses(parentBase,ConfigurationSettings.AppSettings["GetAllClasses"]);

            JsonSerialize(childClasses,@"child");
            JsonSerialize(parentClasses, @"parent");
            
            List<ODocument> uniqueParentKeys = FilterUniqueKeys(parentClasses, childClasses);

            JsonSerialize(uniqueParentKeys, @"unique");

            List<ODocument> ParentEdges = FilterClasses(uniqueParentKeys, "E");
            List<ODocument> NestedEdges = FilterClassesBySuperclass(uniqueParentKeys, ParentEdges);

            AddClassesExt(ParentEdges, childBase);
            AddProperties(ParentEdges, parentBase,childBase);
            AddClassesExt(NestedEdges, childBase);
            AddProperties(NestedEdges, parentBase, childBase);

            List<ODocument> ParentVertices = FilterClasses(uniqueParentKeys, "V");
            List<ODocument> NestedVertices = FilterClassesBySuperclass(uniqueParentKeys, ParentVertices);

            AddClassesExt(ParentVertices, childBase);
            AddProperties(ParentVertices, parentBase, childBase);
            AddClassesExt(NestedVertices, childBase);
            AddProperties(NestedVertices, parentBase, childBase);


            string commUrl = string.Format(@"{0}{1}:{2}/command/{3}/sql/{4}",
            @"http://",
            ConfigurationSettings.AppSettings["ParentHost"],
            "2480",
            ConfigurationSettings.AppSettings["ParentDBname"],
            @"select Seed,Created,GUID from unit");

            string Authurl = string.Format(@"{0}{1}:{2}/connect/{3}",
            @"http://",
            ConfigurationSettings.AppSettings["ParentHost"],
            "2480",
            ConfigurationSettings.AppSettings["ParentDBname"]
            );

            AuthorizeOrientDB(Authurl);
            JsonSerialize(GetObj(commUrl), "persSer");            
            string ApiGetRes = Get(commUrl);
            Person pers = JsonConvert.DeserializeObject<Person>(ApiGetRes);
            //GetObj(commUrl);
            //Get(commUrl);

            //valid insert command
            //childBase.Command(@"INSERT INTO Person CONTENT " + @"{""Seed"":""1"", ""Created"":""2015-06-01 00:00:00"",""GUID"":""7c70394f-1bfe-11e5-b2cc-f80f41d3dd35""}");

            //SQL Queries to driver
            //List<ODocument> queryResult = childBase.Query(SQLcommand);

            //SQL batch query result
            //Orient.Client.API.Query.OCommandQuery batchResult = childBase.SqlBatch(SQLcommand);


            //Add class
            //https://orientdb.com/docs/2.2/SQL-Create-Class.html
            /*
            childBase.Command(@"create class Person IF NOT EXISTS EXTENDS V");
            childBase.Command(@"create class Relation IF NOT EXISTS EXTENDS E");
            childBase.Command(@"CREATE PROPERTY Person.name IF NOT EXISTS STRING");
            childBase.Command(@"CREATE PROPERTY Relation.type IF NOT EXISTS STRING");
            childBase.Command(@"DROP CLASS Friend IF EXISTS");
           

            string gremlinCommand = string.Format(
            @"
            g = new OrientGraph('plocal:/data/{0}');
            vertices = g.{1};
            g.close();
            return vertices;",
            childBasePool.DatabaseName, "V");
             */

            //error
            //OCommandResult result = db.Gremlin(gremlinCommand);
            //
            /*
            ODocument test = db.Insert()
             .Into("V")
             .Set("name", "name1") .Run();
            */


            childBase.Close();
            childBase.Dispose();
            if (os.DatabaseExist(childBasePool.DatabaseName, OStorageType.PLocal))
            {
                os.DropDatabase(childBasePool.DatabaseName, OStorageType.PLocal);
            }
            os.Close();
            os.Dispose();
        }


        public List<ODocument> GetDbClasses(ODatabase db, string command)
        {
            return db.Query(command);
        }

        public List<ODocument> FilterUniqueKeys(List<ODocument>  parent_, List<ODocument>  child_)
        {
            List<ODocument> uniqueParentKeys =
                parent_.Except(
                from z in (from s in child_ where s.Keys.Contains("name") select s)
                join x in (from s in child_ where s.Keys.Contains("name") select s) on z["name"] equals x["name"]
                select z).ToList();
            return uniqueParentKeys;
        }
        
        public void JsonSerialize(object object_,string name_)
        {
            string object_str = JsonConvert.SerializeObject(object_, Formatting.Indented);
            File.WriteAllText(string.Format(@"C:\workflow\temp\serialized_{0}.txt",name_), object_str);
        }

        public void AddClasses(List<ODocument> collection,ODatabase database_)
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
        public void AddClassesExt(List<ODocument> collection, ODatabase database_)
        {
            foreach (ODocument class_ in collection)
            {
                var props = class_["properties"];
                string name = (string)class_["name"];
                string superclass = (string)class_["superClass"];
                database_.Command(
                    string.Format(ConfigurationSettings.AppSettings["CreateClassExt"], name, superclass)
                    );
            }
        }
        public void AddProperties(IEnumerable<ODocument> classes, ODatabase parentBase_, ODatabase childBase_)
        {
            foreach(ODocument class_ in classes)
            {
                IEnumerable<ODocument> properties = GetProperties(class_, parentBase_);
                if (properties != null)
                {
                    foreach (ODocument doc in properties)
                    {
                        AddProperty(doc, class_, childBase_);
                    }
                }
            
            }
        }
       

        public IEnumerable<ODocument> GetProperties(ODocument class_, ODatabase parentBase_)
        {
            IEnumerable<ODocument> result = null;
            Orient.Client.API.Query.OSqlSchema schema_ = parentBase_.Schema;
            string className = (string)class_["name"];
            if (className != "Object_SC" && (string)class_["superClass"] != "Object_SC" )
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
        public void AddProperty(ODocument property, ODocument class_, ODatabase childDatabase_)
        {
            string className = (string)class_["name"];
            string name = (string)property["name"];
            int type = (int)property["type"];
            string typeStr = OrientNumToType.ValuetoString(type);
            string comm = string.Format(ConfigurationSettings.AppSettings["CreateProperty"], className, name, typeStr);
            childDatabase_.Command(comm);
        }
        public List<ODocument> FilterClasses(List<ODocument> classes_,string type_)
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
        public string Get(string url)
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
                    var objText = reader.ReadToEnd();
                    var objClasses = JsonConvert.DeserializeObject(objText);
                    var jsonResult = objClasses.ToString();
                    
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

    }

    public static class OrientNumToType
    {
        public static string ValuetoString(int value_)
        {
            string result = null;
            switch(value_)
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


    public class Person
    {
        //@"{""Seed"":""1"", ""Created"":""2015-06-01 00:00:00"",""GUID"":""7c70394f-1bfe-11e5-b2cc-f80f41d3dd35""}"
        public long Seed { get; set; }
        public DateTime Created { get; set; }
        public string GUID { get; set; }
    }
}
