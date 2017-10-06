using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using NewsAPI.Interfaces;
using NewsAPI.Helpers;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace NewsAPI.Implements
{

    public class OrientPersons : IPersonFunctions
    {
        IFunctionToString _functions;

        public OrientPersons(IFunctionToString functions_)
        {          
            _functions = functions_;
        }

        public string GetUnitByAccount(string AccountName)
        {
            string name = @"GetUnitByAccount";
            return _functions.CallFunctionItem(name, AccountName);
        }
        public string GetDepartmentByAccount(string AccountName)
        {
            string name = @"GetDepartmentByAccount";
            return _functions.CallFunctionItem(name, AccountName);
        }
        public string GetManagerByAccount(string AccountName)
        {
            string name = @"GetManagerByAccount";
            return _functions.CallFunctionItem(name, AccountName);
        }
        public string GetCollegesByAccount(string AccountName)
        {
            //string name = @"GetCollegesByAccount";
            string name = @"GetCollegesByAccount";
            return _functions.CallFunctionCollection(name, AccountName);
        }
        public string GetManagerHierarhyByAccount(string AccountName)
        {
            //string name = @"GetManagerHierarhyByAccount";
            string name = @"GetManagerHierarhyByAccount";
            return _functions.CallFunctionItems(name, AccountName);
        }
        public string GetCollegesLowerByAccount(string AccountName)
        {
            string name = @"GetCollegesLowerByAccount";
            return _functions.CallFunctionItems(name, AccountName);
        }

    }

    public class FunctionsToString : IFunctionToString
    {
        IJSONProxy _JSONProxy;

        public FunctionsToString(JSONProxy JSONProxy_)
        {
            _JSONProxy = JSONProxy_;
        }

        //для функций принимающих и возвращающих одно значение. функция должна возвращать имя нода "Name" как "select ... as Name".
        //можно добавить любое имя Node, а далее добавить поиск по Nod'ам
        public string CallFunctionItem(string functionName, string parameter)
        {
            string result = null;
            OrientNewsHelper newsHelper = new OrientNewsHelper();
            IHttpActionResult requestResult = newsHelper.ExecuteFunction(functionName, parameter);
            IHttpActionResult resultProxy =  new OrientNewsHelper.ReturnAddedEntityIdWithExecute(requestResult, "Name");
            result = resultProxy.ExecuteAsync(new CancellationToken()).Result.Content.ReadAsStringAsync().Result;
            return result;
        }
        //для функций принимающих одно значение, возвращающих коллекцию значений формата Name[a,b,c], не объектов.
        //функция в selct должна возвращать псевдоним нода "Name" через "select ... as Name ... from ".
        public string CallFunctionCollection(string name, string param)
        {
            string result = null;
            OrientNewsHelper newsHelper = new OrientNewsHelper();
            IHttpActionResult requestResult = newsHelper.ExecuteFunction(name, param);
            string cnt = requestResult.ExecuteAsync(new CancellationToken()).Result.Content.ReadAsStringAsync().Result;
            List<string> temColl = _JSONProxy.JSONfromCollectionNODE<string>(cnt);
            result = JsonConvert.SerializeObject(temColl);

            return result;
        }
        //для функций принимающих одно значение, возвращающих коллекцию значений формата result[{Name:a},{Name:b},{Name:a}]
        public string CallFunctionItems(string name, string param)
        {
            string result = null;
            OrientNewsHelper newsHelper = new OrientNewsHelper();
            IHttpActionResult requestResult = newsHelper.ExecuteFunction(name, param);
            string cnt = requestResult.ExecuteAsync(new CancellationToken()).Result.Content.ReadAsStringAsync().Result;
            List<string> temColl = _JSONProxy.JSONfromNODEcollection<string>(cnt);
            result = JsonConvert.SerializeObject(temColl);

            return result;
        }

    }

    public class JSONProxy : IJSONProxy
    {
        //возвращает внутреннюю коллекцию.
        //result[ ... Name[a,b,c] ] -> List
        //пока только T = string
        public List<T> JSONfromCollectionNODE<T>(string input_) where T : class
        {
            List<T> result = new List<T>();
            JObject jarr = JObject.Parse(input_);
            List<JToken> results = jarr["result"].Children()["Name"].Children().ToList();

            result = (from s in results select s.ToObject<T>()).ToList();

            return result;
        }
        //возвращает Nod'ы 1 lvl с именем Name в коллекцию 
        //result[{Name:a},{Name:b},{Name:c}] -> List
        public List<T> JSONfromNODEcollection<T>(string input_) where T : class
        {
            List<T> result = new List<T>();
            JObject jarr = JObject.Parse(input_);
            List<JToken> results = jarr["result"].Children()["Name"].ToList();

            result = (from s in results select s.ToObject<T>()).ToList();

            return result;
        }

        //Возвращает коллекции объектов из JSON для указанного типа модели POCO класса        
        //For sample JSON structure [{a:1,..,c:1},{a:10,..,c:10}]
        public List<T> DeserializeSample<T>(string resp) where T : class
        {
            List<T> res = JsonConvert.DeserializeObject<List<T>>(resp);
            return res;
        }
        //позволяет извлекать коллекцию из указанного Node
        //For JSON structure {NodeName:[{a:1,..,c:1},{a:10,..,c:10}]}
        public List<T> DeserializeFromNode<T>(string jInput, string Node) where T : class
        {
            List<T> result = new List<T>();
            List<JToken> res = JObject.Parse(jInput)[Node].Children().ToList();
            result = (from s in res select s.ToObject<T>()).ToList();
            return result;
        }

    }


    public class OrientModel
    {
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
    }
}