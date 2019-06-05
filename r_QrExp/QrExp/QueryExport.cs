using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;

using System.IO;

using Newtonsoft.Json;

namespace QrExp
{

    public static class PairObjectExport
    {

        public static void variablesLoad()
        {
           
            
          
            PairObjectInit.init();
        }

    }


    public static class PairObjectInit
    {

        public static void init()
        {

            string configpath = @"C:\Projects\QrExp\QrExp\test\rBuild";
            //Initialize input variables
            string reportPath = @"C:\Projects\QrExp\QrExp\test\rBuild";
            string reportName = @"TEST";

            ConfigManager parametermanger = new ConfigManager(reportName, reportPath);

            //initialize hardcode objects
            List <Pair> pr = new List<Pair>();
            pr.Add(new Pair { Name = "Param1", Value = "Val1" });
            pr.Add(new Pair { Name = "Param2", Value = "Val2" });
            Query qr = new Query("QueryName1", "QueryText1");
            qr.parametersAdd(pr);

            List<Pair> pr2 = new List<Pair>();
            pr2.Add(new Pair { Name = "Param1", Value = "Val1" });
            Query qr2 = new Query("QueryName2", "QueryText2");
            qr2.parametersAdd(pr2);

            List<Pair> pr3 = new List<Pair>();
            pr3.Add(new Pair { Name = "Param1", Value = "Val1" });
            Range rng1 = new Range("Range1", "rnName1");
            rng1.parametersAdd(pr3);

            List<Pair> pr4 = new List<Pair>();
            pr4.Add(new Pair { Name = "Param1", Value = "Val1" });
            Worksheet ws1 = new Worksheet("Ws1", "wsName1");
            ws1.parametersAdd(pr4);


            ObjectManager.objectAdd(qr);
            ObjectManager.objectAdd(qr2);
            ObjectManager.objectAdd(rng1);
            ObjectManager.objectAdd(ws1);


            List<PairObject> qrl = ObjectManager.objects;

            //print objects list
            string outpath = parametermanger.getParameterValue(reportName, ConfigParameterType.REF_JSON); 
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects, Formatting = Formatting.Indented };
            string js = JsonConvert.SerializeObject(ObjectManager.objects, settings);
            File.WriteAllText(outpath, js);

            js = File.ReadAllText(outpath);
            List<PairObject> qrl2 = JsonConvert.DeserializeObject<List<PairObject>>(js, settings);
            js = JsonConvert.SerializeObject(qrl2, Formatting.Indented, settings);
            File.WriteAllText(outpath, js);

            ObjectHierarhyManager.hierarhyAdd(0, 2, null);
            ObjectHierarhyManager.hierarhyAdd(1, 2, null);
            ObjectHierarhyManager.hierarhyAdd(2, 3, 0);
            ObjectHierarhyManager.hierarhyAdd(2, 3, 1);
            ObjectHierarhyManager.hierarhyAdd(3, null, 2);

            //print nodes list
            outpath = parametermanger.getParameterValue(reportName, ConfigParameterType.NODE_JSON);
            js = JsonConvert.SerializeObject(ObjectHierarhyManager.objectsHierarhy, Formatting.Indented);
            File.WriteAllText(outpath, js);

        }

    }

    public static class ObjectManager
    {
        public static List<PairObject> objects = new List<PairObject>();

        public static void objectAdd(PairObject object_)
        {
            objects.Add(object_);
        }

    }
    public static class ObjectHierarhyManager
    {
        public static List<ObjectsIDHierarhy> objectsHierarhy = new List<ObjectsIDHierarhy>();

        public static void hierarhyAdd(int? objectID_, int? parentID_, int? childID_)
        {
            nodeIDsAdd(objectID_, parentID_, childID_);
        }
        public static bool NodePresent(int? objectID_, int? parentID_, int? childID_)
        {
            bool result = false;

            foreach (ObjectsIDHierarhy obj_ in objectsHierarhy)
            {
                if (obj_.objectID == objectID_ && obj_.parentID == parentID_ && obj_.childID == childID_)
                {
                    result = true;
                }
            }

            return result;
        }
        public static void nodeIDsAdd(int? objectID_, int? parentID_, int? childID_)
        {
            if (!NodePresent(objectID_, parentID_, childID_))
            {
                objectsHierarhy.Add(new ObjectsIDHierarhy(objectID_, parentID_, childID_));
            }
        }

    }
    public class ObjectsIDHierarhy
    {
        public int? objectID;
        public int? parentID;
        public int? childID;

        public ObjectsIDHierarhy(int? objectID, int? parentID, int? childID)
        {
            this.objectID = objectID;
            this.parentID = parentID;
            this.childID = childID;
        }

    }

    public class Pair
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class PairObject : Pair
    {
        private static int id = -1;

        public int ID;
        public List<Pair> parameterList = new List<Pair>();
        public List<PairObject> childsList = new List<PairObject>();

        private Type tp;

        private void incrementID()
        {
            id = id    +1;
            ID = id;
        }

        public PairObject()
        {

        }
        public PairObject(string name_, string value_) : base()
        {
            incrementID();
            this.tp = this.GetType();
            this.Name = name_;
            this.Value = value_;
        }

        public PairObject(string name_, string value_, List<Pair> parameterList_) : base()
        {
            incrementID();
            this.tp = this.GetType();
            this.Name = name_;
            this.Value = value_;
            this.parameterList = parameterList_;
        }

        public void parameterAdd(Pair parameter_)
        {
            this.parameterList.Add(parameter_);
        }

        public void parametersAdd(List<Pair> parameterList_)
        {
            this.parameterList = parameterList_;
        }

        public virtual void Initialize()
        {

        }
    }
    public class Query : PairObject
    {
        public Query(string name_, string value_) : base(name_, value_)
        {

        }
        public override void Initialize()
        {
            base.Initialize();
        }

    }
    public class Range : PairObject
    {
        public Range(string name_, string value_) : base(name_, value_)
        {

        }
        public void publish()
        {

        }
    }
    public class Worksheet : PairObject
    {
        public Worksheet(string name_, string value_) : base(name_, value_)
        {

        }
        public void Add()
        {

        }
    }

    /// <summary>
    /// Creates dictionary of parameters for every report     
    /// </summary>
    public class ConfigManager
    {
        string ReportName;
        string ReportFolder;

        Dictionary<string, ConfigFiles> parameters = new Dictionary<string, ConfigFiles>();

        public ConfigManager(string ReportName_, string reportfolder_)
        {
            parameters.Add(ReportName_,new ConfigFiles(ReportFolder));
        }
        public string getParameterValue(string reportname_, ConfigParameterType pairType_)
        {
            string result = "";
            result = parameters.Where(s => s.Key == reportname_).FirstOrDefault().Value.getFolder(pairType_);
            return result;
        }
    }

    /// <summary>
    /// Paramter types for different input files with name path values
    /// Initialized with path from outside or assigns Assembly location folder
    /// </summary>
    public class ConfigFiles
    {
        public InputFiles inputType;
        //stores filepaths for parameter types
        public Dictionary<ConfigParameterType, string> parametersType = new Dictionary<ConfigParameterType, string>();
        //stores file states for parameter ypes
        public Dictionary<ConfigParameterType, bool> parametersCheck = new Dictionary<ConfigParameterType, bool>();

        public ConfigFiles() : this(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
        {
           
        }
        public ConfigFiles(string reportFolder)
        {           
            parametersType.Add(ConfigParameterType.REF_JSON, reportFolder + @"\queryJSON.txt" );            
            parametersType.Add(ConfigParameterType.QUERY_JSON,  reportFolder  + @"\refJSON.txt" );
            parametersType.Add(ConfigParameterType.MAIL_JSON, reportFolder + @"\mailJSON.txt");
            parametersType.Add(ConfigParameterType.NODE_JSON,  reportFolder  + @"\nodeJSON.txt" );

            parametersType.Add(ConfigParameterType.QUERY_TXT,  reportFolder +  @"\query.txt" );
            parametersType.Add(ConfigParameterType.REF_TXT,  reportFolder  + @"\ref.txt" );
            parametersType.Add(ConfigParameterType.MAIL_TXT,  reportFolder +  @"\mail.txt" );
            parametersType.Add(ConfigParameterType.NODE_TXT, reportFolder + @"\node.txt");
        }

        public string getFolder(ConfigParameterType pairType)
        {
            string pair = "";
            if (parametersType.TryGetValue(pairType, out pair))
            {
                return parametersType[pairType];
            }
            return null;
        }
        public void CheckFilePresence()
        {
            bool result = false;
            foreach (KeyValuePair<ConfigParameterType, string> pair in parametersType)
            {
                result = false;
                if (File.Exists(pair.Value))
                {
                    result = true;
                }
                parametersCheck.Add(pair.Key, result);
            }
           
        }
        
    }   
    
    public class Reportmanager
    {

        public Reportmanager(string configpath_)
        {

        }
    }

    

    public enum ConfigParameterType { REF_JSON, QUERY_JSON, MAIL_JSON,NODE_JSON, REF_TXT, QUERY_TXT, MAIL_TXT, NODE_TXT }
    public enum InputFiles { TXT,JSOM}
    public enum ObjectTypes {PATH,REPORTNAME, MAIL,REPORT,WB,WS,RANGE,QUERY}
}