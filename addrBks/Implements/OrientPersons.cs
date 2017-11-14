using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Http;
using NewsAPI.Interfaces;
using NewsAPI.Helpers;
using System.Configuration;
using System.Threading;

using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


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
            return _functions.CallFunctionItem(name, AccountName.ToLower());
        }
        public string GetDepartmentByAccount(string AccountName)
        {
            string name = @"GetDepartmentByAccount";
            return _functions.CallFunctionItem(name, AccountName.ToLower());
        }
        public string GetManagerByAccount(string AccountName)
        {
            string name = @"GetManagerByAccount";
            return _functions.CallFunctionItem(name, AccountName.ToLower());
        }
        public string GetCollegesByAccount(string AccountName)
        {
            //string name = @"GetCollegesByAccount";
            string name = @"GetCollegesByAccount";
            return _functions.CallFunctionItems(name, AccountName.ToLower());
        }
        public string GetManagerHierarhyByAccount(string AccountName)
        {
            //string name = @"GetManagerHierarhyByAccount";
            string name = @"GetManagerHierarhyByAccount";
            return _functions.CallFunctionItems(name, AccountName.ToLower());
        }
        public string GetCollegesLowerByAccount(string AccountName)
        {
            string name = @"GetCollegesLowerByAccount";
            return _functions.CallFunctionItems(name, AccountName.ToLower());
        }
        public string SearchByLastName(string AccountName)
        {
            string name = @"SearchByLastName";
            return _functions.CallFunctionItems(name, AccountName.ToLower());
        }
        //change JSON parsing
        public string SearchPerson(string AccountName)
        {
            string name = @"SearchByFNameLName";
            return _functions.CallFunctionParentChildId(name, AccountName.ToLower());
            
        }       
        public string GetGUID(string AccountName)
        {
            string name = @"GetGUID";
            return _functions.CallFunctionParentChildName(name, AccountName.ToLower());
        }

    }

    public class FunctionsToString : IFunctionToString
    {
        IJSONProxy _JSONProxy;
        IJsonManagers.IJsonManger _jsonManager;

        public FunctionsToString(JSONProxy JSONProxy_, IJsonManagers.IJsonManger jsonManager_)
        {
            _JSONProxy = JSONProxy_;
            _jsonManager = jsonManager_;

        }
        public void BindJSONmanager(IJsonManagers.IJsonManger jsonManager_)
        {
            _jsonManager = jsonManager_;
        }
        //для функций принимающих и возвращающих одно значение. функция должна возвращать имя нода "Name" как "select ... as Name".
        //можно добавить любое имя Node, а далее добавить поиск по Nod'ам
        public string CallFunctionItem(string functionName, string parameter)
        {
            string result = null;
            OrientNewsHelper newsHelper = new OrientNewsHelper();
            IHttpActionResult requestResult = newsHelper.ExecuteFunction(functionName, parameter);
            IHttpActionResult resultProxy = new OrientNewsHelper.ReturnAddedEntityIdWithExecute(requestResult, "Name");
            try {
                result = resultProxy.ExecuteAsync(new CancellationToken()).Result.Content.ReadAsStringAsync().Result;
            } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
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
            IEnumerable<string> temColl = _JSONProxy.JSONfromCollectionNODE<string>(cnt);
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
            IEnumerable<string> temColl = _JSONProxy.JSONfromNODEcollection<string>(cnt);
            result = JsonConvert.SerializeObject(temColl);

            return result;
        }

        public string CallFunctionParentChildName(string name, string param)
        {
            string result = null;
            OrientNewsHelper newsHelper = new OrientNewsHelper();
            IHttpActionResult requestResult = newsHelper.ExecuteFunction(name, param);
            string cnt = requestResult.ExecuteAsync(new CancellationToken()).Result.Content.ReadAsStringAsync().Result;

            try
            {
                result =
                    _jsonManager.DeserializeFromParentNode<POCO.Person>(cnt, "result").FirstOrDefault().Name;
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return result;
        }
        public string CallFunctionParentChildId(string name, string param) 
        {
            string result = null;
            OrientNewsHelper newsHelper = new OrientNewsHelper();
            IHttpActionResult requestResult = newsHelper.ExecuteFunction(name, param);
            string cnt = requestResult.ExecuteAsync(new CancellationToken()).Result.Content.ReadAsStringAsync().Result;

            try
            {
                result =                   
                    _jsonManager.DeserializeFromParentChildNode<string>(cnt, "result", "suggestion").FirstOrDefault()
                    ;
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return result;
        }
        public IJEnumerable<JToken> ExtractTokens(string jInput, string field)
        {
            IJEnumerable<JToken> result = null;
            result = JToken.Parse(jInput).Children()[field];
            return result;
        }

    }

    public class JSONProxy : IJSONProxy
    {
        //возвращает внутреннюю коллекцию.
        //result[ ... Name[a,b,c] ] -> List
        //пока только T = string
        public IEnumerable<T> JSONfromCollectionNODE<T>(string input_) where T : class
        {
            IEnumerable<T> result = new List<T>();
            JObject jarr = JObject.Parse(input_);
            List<JToken> results = jarr["result"].Children()["Name"].Children().ToList();

            result = (from s in results select s.ToObject<T>()).ToList();

            return result;
        }
        //возвращает внутреннюю коллекцию.
        //result[ ... Name[a,b,c] ] -> List
        //пока только T = string
        public IEnumerable<T> JSONfromCollectionNODE<T>(string input_, string name_) where T : class
        {
            IEnumerable<T> result = new List<T>();
            JObject jarr = JObject.Parse(input_);
            List<JToken> results = jarr["result"].Children()[name_].ToList();

            result = (from s in results select s.ToObject<T>()).ToList();

            return result;
        }
        //возвращает Nod'ы 1 lvl с именем Name в коллекцию 
        //result[{Name:a},{Name:b},{Name:c}] -> List
        public IEnumerable<T> JSONfromNODEcollection<T>(string input_) where T : class
        {
            IEnumerable<T> result = new List<T>();
            JObject jarr = JObject.Parse(input_);
            List<JToken> results = jarr["result"].Children()["Name"].ToList();

            result = (from s in results select s.ToObject<T>()).ToList();

            return result;
        }

        //Возвращает коллекции объектов из JSON для указанного типа модели POCO класса        
        //For sample JSON structure [{a:1,..,c:1},{a:10,..,c:10}]
        public IEnumerable<T> DeserializeSample<T>(string resp) where T : class
        {
            List<T> res = JsonConvert.DeserializeObject<List<T>>(resp);
            return res;
        }
        //позволяет извлекать коллекцию из указанного Node
        //For JSON structure {NodeName:[{a:1,..,c:1},{a:10,..,c:10}]}
        public IEnumerable<T> DeserializeFromNode<T>(string jInput, string Node) where T : class
        {
            IEnumerable<T> result = new List<T>();
            IEnumerable<JToken> res = JObject.Parse(jInput)[Node].Children().ToList();
            result = CollectionConvert<T>(res);
            return result;
        }

        public IEnumerable<T> CollectionConvert<T>(IEnumerable<JToken> input) where T : class
        {
            IEnumerable<T> result = null;
            result = (from s in input select s.ToObject<T>()).ToList();
            return result;
        }

    }   

}