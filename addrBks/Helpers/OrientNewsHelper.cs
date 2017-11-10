using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Configuration;
using Newtonsoft.Json;
using NewsAPI.Extensions;
using System.Text.RegularExpressions;

namespace NewsAPI.Helpers
{
    public class OrientNewsHelper
    {

        public string CreateChangesJson(string target_json, string changes_json)
        {
            // из-за использования конструкции .toJSON() придется дважды парсить json ответа - 
            // первый раз чтобы добраться до this, второй - чтобы распарсить то, что мы изначально запрашивали командой
            var target = JObject.Parse(target_json);
            var targetTokenThis = target.ToObject<Dictionary<string, string>>();
            var targetTokens = JObject.Parse(targetTokenThis["this"]).ToObject<Dictionary<string, string>>();

            var changes = JObject.Parse(changes_json);
            var changesTokens = changes.ToObject<Dictionary<string, string>>();

            // Выберем те токены, значение которых изменилось
            var tokensKeysToPut = new List<string>();
            var existTokens = targetTokens.Where(w => w.Key.ContainsAny(changesTokens.Select(s => s.Key).ToArray()));
            foreach (var existToken in existTokens)
            {
                var changesToken = changesTokens.Where(w => w.Key == existToken.Key);
                if (changesToken.Select(s => s.Value).FirstOrDefault() != existToken.Value)
                {
                    tokensKeysToPut.Add(changesToken.Select(s => s.Key).FirstOrDefault());
                }
            }

            // Выберем токены, которых не было в контенте изменяемого объекта
            var tokensNew = changesTokens.Where(w => !w.Key.ContainsAny(existTokens.Select(s => s.Key).ToArray()));
            tokensKeysToPut.AddRange(tokensNew.Select(s => s.Key));

            // creating json


            var propertiesToPut = from d1 in changesTokens
                                  from d2 in tokensKeysToPut
                                  where d1.Key == d2
                                  select new JProperty(d1.Key, d1.Value);

            JObject joToPut = new JObject(propertiesToPut);


            var jsonToPut = string.Join("", Regex.Split(joToPut.ToString(), @"(?:\r\n|\n|\r)"));

            return jsonToPut;

        }

        public void Authorize()
        {
            OrientDB_HttpManager.AuthorizeOrientDB(
              ConfigurationManager.AppSettings["orient_auth_host"],
              ConfigurationManager.AppSettings["orient_login"],
              ConfigurationManager.AppSettings["orient_pswd"]
              );
        }

        public string GetEntityTraverse(int entityId, int count)
        {
            var selectQuery = String.Format(
                // Здесь мы выбираем саму запись по ее Id (root уровень), все дочерние записи 1-го уровня по указанному лимиту (переменная count),
                // траверс от записи 1-го уровня с глубиной = 1 к записям 2-го уровня. На них уже указанный лимит не распространяется
                @"select @this.toJson('fetchPlan:in_*:-2 out_*:-2') from
                (select *,ifnull(in('Comment').Id, null)[0] as PId  from
                (select expand($c) 
                let $a = (traverse out() from(select expand(out()) from Object where {0} = {1} order by Created asc limit {2}) while $depth <= 1), 
                $b = (select from Object where {0} = {1}), 
                $c = unionall($a,$b) ))",
                 ConfigurationManager.AppSettings["orient_id_name"],
                 entityId,
                 count
                 );

            var helper = new OrientNewsHelper();
            var commandResult = helper.ExecuteCommand(selectQuery).ExecuteAsync(new CancellationToken());
            string returnedSet;


            using (var contentStream = commandResult.Result.Content.ReadAsStreamAsync().Result)
            {
                using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                {
                    string response_string = reader.ReadToEnd();
                    var reponse_json = JObject.Parse(response_string);

                    returnedSet = reponse_json.SelectToken(String.Format("result")).ToString();
                }
            }

            return returnedSet;
        }

        public string GetEntityTraverseInSpecificBounds(int entityId, int start, int count)
        {
            var selectQuery = String.Format(
                // Здесь мы выбираем саму запись по ее Id (root уровень), все дочерние записи 1-го уровня по указанному лимиту (переменная count), 
                // в заданных пределах (от start, count записей)
                // траверс от записи 1-го уровня с глубиной = 1 к записям 2-го уровня. На них уже указанный лимит не распространяется
                @"select @this.toJson('fetchPlan:in_*:-2 out_*:-2') from 
                (select *,ifnull(in('Comment').Id, null)[0] as PId  from
                (select expand($c) 
                let $a = (traverse out() from (select * from (select expand(out ()) from Object where {0} = {1} order by Created asc limit {3}) 
                where @rid not in ( select expand(out ()) from Object where {0} = {1} order by Created asc limit {2})) while $depth <= 1), 
                $b = (select from Object where {0} = {1}), 
                $c = unionall($a,$b) ))",
                 ConfigurationManager.AppSettings["orient_id_name"],
                 entityId,
                 start,
                 start + count
                 );

            var helper = new OrientNewsHelper();
            var commandResult = helper.ExecuteCommand(selectQuery).ExecuteAsync(new CancellationToken());
            string returnedSet;


            using (var contentStream = commandResult.Result.Content.ReadAsStreamAsync().Result)
            {
                using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                {
                    string response_string = reader.ReadToEnd();
                    var reponse_json = JObject.Parse(response_string);

                    returnedSet = reponse_json.SelectToken(String.Format("result")).ToString();
                }
            }

            return returnedSet;

        }



        public string GetEntity(string entityId)
        {
            var selectQuery = String.Format("select @this.toJSON('fetchPlan:in_*:-2 out_*:-2') from (select *, ifnull(in(\"Comment\").Id, null)[0] as PId from Object where {0} = {1})",
                   // в OrientDB заложена возможность использования fetchPlan в строке  API запроса отдельно от самой команды SQL,
                   // но данная функция рализована с ошибкой, исправить которую планируется в 3.0 верс.
                   // из-за использования конструкции .toJSON() придется дважды парсить json ответа - 
                   // первый раз чтобы добраться до this, второй - чтобы распарсить то, что мы изначально запрашивали командой
                   ConfigurationManager.AppSettings["orient_id_name"],
                   entityId
                   );


            var helper = new OrientNewsHelper();
            var commandResult = helper.ExecuteCommand(selectQuery).ExecuteAsync(new CancellationToken());
            string changedObject;

            using (var contentStream = commandResult.Result.Content.ReadAsStreamAsync().Result)
            {
                using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                {
                    string response_string = reader.ReadToEnd();
                    var reponse_json = JObject.Parse(response_string);

                    changedObject = reponse_json.SelectToken(String.Format("result[0]")).ToString();
                }
            }

            return changedObject;
        }


        public string GetEntitiesOfType(string typeName, int count = 10)
        {
            var selectQuery = String.Format("select @this.toJSON('fetchPlan:in_*:-2 out_*:-2') from (select *, ifnull(in(\"Comment\").Id, null)[0] as PId from {0} order by Created asc limit {1})",
              // в OrientDB заложена возможность использования fetchPlan в строке  API запроса отдельно от самой команды SQL,
              // но данная функция рализована с ошибкой, исправить которую планируется в 3.0 верс.
              // из-за использования конструкции .toJSON() придется дважды парсить json ответа - 
              // первый раз чтобы добраться до this, второй - чтобы распарсить то, что мы изначально запрашивали командой
              //
              // здесь выбираются первые 10 записей указанного типа
              typeName,
              count // default value 10
              );

            var helper = new OrientNewsHelper();
            var commandResult = helper.ExecuteCommand(selectQuery).ExecuteAsync(new CancellationToken());
            string returnedSet;

            using (var contentStream = commandResult.Result.Content.ReadAsStreamAsync().Result)
            {
                using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                {
                    string response_string = reader.ReadToEnd();
                    var reponse_json = JObject.Parse(response_string);

                    returnedSet = reponse_json.SelectToken(String.Format("result")).ToString();
                }
            }

            return returnedSet;
        }

        public string GetEntitiesOfTypeInSpecificBounds(string typeName, int start, int count)
        {
            var selectQuery = String.Format(
                @"select @this.toJSON('fetchPlan:in_*:-2 out_*:-2') from (select *, ifnull(in('Comment').Id, null)[0] as PId from {0}
                where @rid not in (select  from {0}
                order by Created asc limit {1})  
                order by Created asc limit {2})",
            // в OrientDB заложена возможность использования fetchPlan в строке  API запроса отдельно от самой команды SQL,
            // но данная функция рализована с ошибкой, исправить которую планируется в 3.0 верс.
            // из-за использования конструкции .toJSON() придется дважды парсить json ответа - 
            // первый раз чтобы добраться до this, второй - чтобы распарсить то, что мы изначально запрашивали командой
            //
            // здесь выбирается count записей указанного типа с позиции start 
            typeName,
            start,
            count
              );

            var helper = new OrientNewsHelper();
            var commandResult = helper.ExecuteCommand(selectQuery).ExecuteAsync(new CancellationToken());
            string returnedSet;

            using (var contentStream = commandResult.Result.Content.ReadAsStreamAsync().Result)
            {
                using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                {
                    string response_string = reader.ReadToEnd();
                    var reponse_json = JObject.Parse(response_string);

                    returnedSet = reponse_json.SelectToken(String.Format("result")).ToString();
                }
            }

            return returnedSet;
        }

        public IHttpActionResult ExecuteCommand(string query)
        {
            return new OrientDB_HttpManager.GetBatchResult(String.Format("{0}/{1}/{2}", ConfigurationManager.AppSettings["orient_command_host"], "sql", query));
        }

        public IHttpActionResult ExecuteCommand(string query, string fetchPlan)
        {
            return new OrientDB_HttpManager.GetBatchResult(String.Format("{0}/{1}/{2}/{3}/{4}", ConfigurationManager.AppSettings["orient_command_host"], "sql", query, "1000", fetchPlan));
        }

        public class ReturnRequestedEntitiesRecordSetWithFetchPlanWithExecute : IHttpActionResult
        {
            Task<HttpResponseMessage> returnedTask;

            public ReturnRequestedEntitiesRecordSetWithFetchPlanWithExecute(IHttpActionResult ar)
            {
                this.returnedTask = ar.ExecuteAsync(new CancellationToken());
            }

            async Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
            {
                string json_all = "[";
                using (var contentStream = returnedTask.Result.Content.ReadAsStreamAsync().Result)
                {
                    using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                    {
                        string requestedEntity_json = reader.ReadToEnd();

                        var recordSet = (IEnumerable<object>)JsonConvert.DeserializeObject(requestedEntity_json);

                        foreach (var record in recordSet)
                        {
                            var target = JObject.Parse(record.ToString());
                            var targetTokenThis = target.ToObject<Dictionary<string, string>>();
                            var targetTokens = JObject.Parse(targetTokenThis["this"]);
                            string json = string.Join("", Regex.Split(targetTokens.ToString(), @"(?:\r\n|\n|\r)"));

                            json_all += json + ", ";
                        }
                        json_all = json_all.Remove(json_all.Length - 2, 2);
                        json_all += "]";

                    }
                }

                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(json_all, Encoding.UTF8, "application/json");

                return await Task.FromResult(resp);
            }
        }

        public class ReturnRequestedEntityWithFetchPlanWithExecute : IHttpActionResult
        {
            Task<HttpResponseMessage> returnedTask;

            public ReturnRequestedEntityWithFetchPlanWithExecute(IHttpActionResult ar)
            {
                this.returnedTask = ar.ExecuteAsync(new CancellationToken());
            }

            async Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
            {
                string json;
                using (var contentStream = returnedTask.Result.Content.ReadAsStreamAsync().Result)
                {
                    using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                    {
                        string requestedEntity_json = reader.ReadToEnd();
                        var target = JObject.Parse(requestedEntity_json);
                        var targetTokenThis = target.ToObject<Dictionary<string, string>>();

                        var targetTokens = JObject.Parse(targetTokenThis["this"]);
                        json = string.Join("", Regex.Split(targetTokens.ToString(), @"(?:\r\n|\n|\r)"));
                    }
                }

                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(json, Encoding.UTF8, "application/json");

                return await Task.FromResult(resp);
            }
        }

        public class ReturnAddedEntityIdWithExecute : IHttpActionResult
        {
            Task<HttpResponseMessage> returnedTask;
            string idName;

            public ReturnAddedEntityIdWithExecute(IHttpActionResult ar, string idName)
            {
                this.returnedTask = ar.ExecuteAsync(new CancellationToken());
                this.idName = idName;
            }

            async Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
            {
                string addedEntityId;
                using (var contentStream = returnedTask.Result.Content.ReadAsStreamAsync().Result)
                {
                    using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                    {
                        string responseString = reader.ReadToEnd();
                        var reponseJson = JObject.Parse(responseString);
                        addedEntityId = reponseJson.SelectToken(String.Format("result[0].{0}", idName)).ToString();
                    }
                }

                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(addedEntityId, Encoding.UTF8, "text/plain");

                return await Task.FromResult(resp);
            }
        }


        public class ReturnPersonInfo : IHttpActionResult
        {
            Task<HttpResponseMessage> returnedTask;

            public ReturnPersonInfo(IHttpActionResult ar)
            {
                this.returnedTask = ar.ExecuteAsync(new CancellationToken());
            }

            async Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
            {
                string person;
                using (var contentStream = returnedTask.Result.Content.ReadAsStreamAsync().Result)
                {
                    using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                    {
                        string responseString = reader.ReadToEnd();
                        var responseJson = JObject.Parse(responseString);

                        if (responseJson.First.First.HasValues) // not equal ""
                        {
                            person = responseJson.SelectToken(String.Format("result[0]")).ToString();
                        }
                        else
                        {
                            person = "{ \"shortFName\": \"Гость\"}";
                        }
                    }
                }

                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(person, Encoding.UTF8, "text/plain");

                return await Task.FromResult(resp);
            }
        }

        public class ReturnPersonGuid : IHttpActionResult
        {
            Task<HttpResponseMessage> returnedTask;

            public ReturnPersonGuid(IHttpActionResult ar)
            {
                this.returnedTask = ar.ExecuteAsync(new CancellationToken());
            }

            async public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                string json;
                using (var contentStream = returnedTask.Result.Content.ReadAsStreamAsync().Result)
                {
                    using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                    {
                        string requestedEntity_json = reader.ReadToEnd();
                        var targetwithResultPart = JObject.Parse(requestedEntity_json);
                        var targetNoResultPart = targetwithResultPart.SelectToken(String.Format("result[0]"));
                        var targetTokenThis = targetNoResultPart.ToObject<Dictionary<string, string>>();

                        var targetTokens = JObject.Parse(targetTokenThis["this"]).SelectToken("GUID");
                        json = string.Join("", Regex.Split(targetTokens.ToString(), @"(?:\r\n|\n|\r)"));
                    }
                }


                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(json, Encoding.UTF8, "text/plain");

                return await Task.FromResult(resp);
            }
        }

        public class ReturnPersonsBirthdays : IHttpActionResult
        {


            Task<HttpResponseMessage> returnedTask;

            public ReturnPersonsBirthdays(IHttpActionResult ar)
            {
                this.returnedTask = ar.ExecuteAsync(new CancellationToken());
            }

            async public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                List<JToken> actualBDays = null;

                List<JObject> jsonToResponse = new List<JObject>();


                using (var contentStream = returnedTask.Result.Content.ReadAsStreamAsync().Result)
                {
                    using (var reader = new StreamReader(contentStream, Encoding.UTF8))
                    {
                        string requestedEntity_json = reader.ReadToEnd();
                        var recordSet = (IEnumerable<object>)JsonConvert.DeserializeObject(requestedEntity_json);

                        var targetwithResultPart = JObject.Parse(requestedEntity_json);
                        var targetNoResultPart = targetwithResultPart.SelectToken(String.Format("result"));
                        var targetExactlyFull = targetNoResultPart.Select(s => s["this"]);

                        actualBDays = targetExactlyFull.Where(w =>
                        {
                            JToken bdayToken;
                            bool valueExist = JObject.Parse(w.Value<string>()).TryGetValue("Birthday", out bdayToken);
                            if (valueExist)
                            {
                                DateTime bday = DateTime.Parse(bdayToken.Value<string>());
                                DateTime? bdayThisYear = new DateTime(2017, bday.Month, bday.Day);
                                if (
                                   bdayThisYear >= DateTime.Now
                                      &&
                                   bdayThisYear <= DateTime.Now.AddDays(30)
                                   )
                                {
                                    return true;
                                }
                                else { return false; }
                            }
                            else { return false; }
                        })
                          .ToList();

                        foreach (var item in actualBDays)
                        {
                            JObject innerItem = new JObject();

                            JToken name;
                            bool nameExist = JObject.Parse(item.Value<string>()).TryGetValue("Name", out name);
                            if (nameExist)
                            {
                                innerItem.Add("name", name);
                            }

                            JToken bday;
                            bool bdayExist = JObject.Parse(item.Value<string>()).TryGetValue("Birthday", out bday);
                            if (bdayExist)
                            {
                                DateTime actualBday = DateTime.Parse(bday.ToString());
                                string returnedBday = string.Format("{0}.{1}", actualBday.Day.ToString(), actualBday.Month.ToString());
                                innerItem.Add("birthday", returnedBday);
                            }

                            jsonToResponse.Add(innerItem);
                        }
                    }
                }

                var json = JsonConvert.SerializeObject(jsonToResponse);

                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(json, Encoding.UTF8, "text/plain");

                return await Task.FromResult(resp);
            }
        }


        public class TextResult : IHttpActionResult
        {
            string _value;
            HttpRequestMessage _request;

            public TextResult(string value, HttpRequestMessage request)
            {
                _value = value;
                _request = request;
            }
            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage()
                {
                    Content = new StringContent(_value),
                    RequestMessage = _request
                };
                return Task.FromResult(response);
            }
        }


        // обертка возвращающая IHttpActionResult с результатом в content
        // все переформатирование JSON выведено в JSONProxy.JSONstringToCollectionFromName
        public class ReturnEntities : IHttpActionResult
        {
            HttpRequestMessage _returnedTask;
            public string _result;

            public ReturnEntities(string result_, HttpRequestMessage ar_)
            {
                this._returnedTask = ar_;
                this._result = result_;
            }

            async Task<HttpResponseMessage> IHttpActionResult.ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(_result, Encoding.UTF8, "text/plain");

                return await Task.FromResult(response);
            }
        }

        //oneparameter function method (претендент на отдельный хелпер : функции, комманды)
        public IHttpActionResult ExecuteFunction(string function, string param)
        {
            return new OrientDB_HttpManager.PostBatchResult(String.Format("{0}/{1}/{2}", ConfigurationManager.AppSettings["orient_func_host"], function, param));
        }

    }
}