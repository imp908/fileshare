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


        public string GetEntity(string entityId)
        {
            // var selectQuery = String.Format("select from Object where {0} = '{1}'",
            var selectQuery = String.Format("select @this.toJSON('fetchPlan:in_*:-2 out_*:-2') from Object where {0} = {1}",
                // в OrientDB заложена возможность использования fetchPlan в строке  API запроса отдельно от самой команды SQL,
                // но данная функция рализована с ошибкой, исправить которую планируется в 3.0 верс.
                // из-за использования конструкции .toJSON() придется дважды парсить json ответа - 
                // первый раз чтобы добраться до this, второй - чтобы распарсить то, что мы изначально запрашивали командой
                ConfigurationManager.AppSettings["orient_id_name"],
                entityId
                );


            var helper = new OrientNewsHelper();
            var commandResult = helper.ExecuteCommand(selectQuery).ExecuteAsync(new CancellationToken()); //, "in_*:-2 out_*:-2"
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

        public IHttpActionResult ExecuteCommand(string query)
        {
            return new OrientDB_HttpManager.GetBatchResult(String.Format("{0}/{1}/{2}", ConfigurationManager.AppSettings["orient_command_host"], "sql", query));
        }

        public IHttpActionResult ExecuteCommand(string query, string fetchPlan)
        {
            return new OrientDB_HttpManager.GetBatchResult(String.Format("{0}/{1}/{2}/{3}/{4}", ConfigurationManager.AppSettings["orient_command_host"], "sql", query, "1000", fetchPlan));
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
                        var reponseJson = JObject.Parse(responseString);
                        person = reponseJson.SelectToken(String.Format("result[0]")).ToString();
                    }
                }

                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(person, Encoding.UTF8, "text/plain");

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
    }
}