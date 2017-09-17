using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Orient.Client;

using Newtonsoft.Json;

namespace OrientDrvr
{
    class Program
    {
        static void Main(string[] args)
        {
            OrientConnect oc = new OrientConnect();
            oc.CallDb();
        }

      
    }

    public  class OrientConnect
    {
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
        public void CallDb()
        {

            string SQLcommand = "select from V limit 10";
            
            //OrientDB_Net.binary.Innov8tive.API.ConnectionOptions

            OrientDB_Net.binary.Innov8tive.API.ConnectionOptions connectionOption = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();

            connectionOption.HostName = "localhost";
            connectionOption.UserName = "root";
            connectionOption.Password = "root";
            connectionOption.Port = 2424;
            connectionOption.DatabaseName = "test_db1";
            connectionOption.DatabaseType = ODatabaseType.Graph;

            string addVertex = string.Format(@"var d = orient.getGraph(); var vertex = db.addVertex(<record-id>)", connectionOption.DatabaseName, @"Person");
            string addClassV = string.Format(@"var gdb = orient.getGraph(); var newClass = db.createVertexType({0},{1})","Friend","V");



            http://orientdb.com/docs/last/NET-Server-CreateDatabase.html
            OServer os = new OServer(connectionOption.HostName, connectionOption.Port, connectionOption.UserName, connectionOption.Password);
            if (!os.DatabaseExist(connectionOption.DatabaseName, OStorageType.PLocal))
            {
                os.CreateDatabase(connectionOption.DatabaseName, connectionOption.DatabaseType, OStorageType.PLocal);
            }          

            var a = typeof(OServer);
            var b = os.GetType();

            Dictionary<string, string> serverConfig = os.ConfigList();
            var jsonConfig =JsonConvert.SerializeObject(serverConfig);
            
            ODatabase db = openDatabase(connectionOption);

            List<OCluster> clusterNamesBefore = db.GetClusters(false);

            // INITIALIZE CLUSTER ID's
            //out of range not work
            //short[] clusterIds = new short[] { 30,31,32 };
            short[] clusterIds = new short[] { 1,2,3 };

           

            //CREATE CLUSTERS
            //works only if clusters allready exists,
            //new clsuters, that out of DB range, throws 
            // Orient.Client.OException
            //clusters not found
            Orient.Client.API.Query.OClusterQuery query = db.Clusters(clusterIds);
            long count = query.Count();

            List<OCluster> clusterNamesAfter = db.GetClusters(false);

            short clusterId0 = db.GetClusterIdFor("V");
            short clusterId1 = db.GetClusterIdFor("E");

            //Driver Select command
            var res = db.Select().From("V").ToList( );
            //Driver update command
            /*
            OSqlUpdate ures = db.Update("V").Set("name", "name1");
            OSqlUpdate uAres = db.Update("V").Add("second_name", "name2");
            */

            //Error
            //db.Command(addClassV);

            //Error due to non indeponent
            //db.Query(createClass);

            //Add class
            //https://orientdb.com/docs/2.2/SQL-Create-Class.html
            db.Command(@"create class Person IF NOT EXISTS EXTENDS V");
            db.Command(@"create class Relation IF NOT EXISTS EXTENDS E");
            db.Command(@"CREATE PROPERTY Person.name IF NOT EXISTS STRING");
            db.Command(@"CREATE PROPERTY Relation.type IF NOT EXISTS STRING");
            db.Command(@"DROP CLASS Friend IF EXISTS");


            //Driver Command result
           Orient.Client.OCommandResult queryCommandResult = db.Command(SQLcommand);
           
            //SQL Queries to driver
            List<ODocument> queryResult = db.Query(SQLcommand);

            //SQL batch query result
            Orient.Client.API.Query.OCommandQuery batchResult=db.SqlBatch(SQLcommand);
            


            string gremlinCommand = string.Format(
            @"
            g = new OrientGraph('plocal:/data/{0}');
            vertices = g.{1};
            g.close();
            return vertices;",
                connectionOption.DatabaseName, "V");

            //error
            //OCommandResult result = db.Gremlin(gremlinCommand);

            //
            /*
            ODocument test = db.Insert()
             .Into("V")
             .Set("name", "name1") .Run();
            */


            db.Close();
            db.Dispose();

            if (os.DatabaseExist(connectionOption.DatabaseName, OStorageType.PLocal))
            {
                os.DropDatabase(connectionOption.DatabaseName, OStorageType.PLocal);
            }

            os.Close();
            os.Dispose();
        }
    }
}
