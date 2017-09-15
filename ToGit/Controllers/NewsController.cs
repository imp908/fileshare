using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NewsAPI.Interfaces;
using System.Threading;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NewsAPI.Implements;
using NewsAPI.Extensions;
using System.Configuration;
using System.Web;
using static NewsAPI.Helpers.OrientNewsHelper;
using System.Security.Principal;
using NewsAPI.Helpers;

namespace NewsAPI.Controllers
{
    public class NewsController : ApiController
    {
        private readonly INews news;
        private readonly IAddressBookProxy proxy;
        private readonly IJsonValidator jsonValidator;
        private readonly IUserAuthenticator userAuthenticator;

        public NewsController(INews news, IAddressBookProxy proxy, IJsonValidator jsonValidator, IUserAuthenticator userAuthenticator)
        {
            this.news = news;
            this.proxy = proxy;
            this.jsonValidator = jsonValidator;
            this.userAuthenticator = userAuthenticator;
        }

        [HttpPost]
        public IHttpActionResult POST([FromBody]string json)
        {
            if (jsonValidator.Validate(json))
            {
                // Создание объекта
                var response = news.CreateEntity(json);

                // Получение id созданного объекта
                var createdEntityId = proxy.ReturnAddedEntityId(response).ExtractEntityId(ConfigurationManager.AppSettings["orient_id_name"]);

                // Получение пользователя из реквеста
                var name = userAuthenticator.AuthenticateUser(base.User);

                // Создание edge авторства
                news.CreateAuthorEdge(name, createdEntityId).ExecuteAsync(new CancellationToken());

                // возврат id созданного объекта
                return new TextResult(createdEntityId, Request);
            }
            else { throw new Exception("invalid json string"); }
        }


        [HttpPost]
        public IHttpActionResult POST(int id, [FromBody]string json)
        {
            if (jsonValidator.Validate(json))
            {
                // Создание объекта
                var response = news.CreateEntity(json);

                // Получение id созданного объекта
                var createdEntityId = proxy.ReturnAddedEntityId(response).ExtractEntityId(ConfigurationManager.AppSettings["orient_id_name"]);

                // Получение пользователя из реквеста
                var name = userAuthenticator.AuthenticateUser(base.User);

                // Создание edge авторства
                news.CreateAuthorEdge(name, createdEntityId).ExecuteAsync(new CancellationToken());

                // Создание edge comment
                news.CreateCommentEdge(id.ToString(), createdEntityId).ExecuteAsync(new CancellationToken());

                // возврат id созданного объекта
                return new TextResult(createdEntityId, Request);
            }
            else { throw new Exception("invalid json string"); }
        }

        public IHttpActionResult PUT(int id, [FromBody]string json_changes)
        {
            if (jsonValidator.Validate(json_changes))
            {
                // Получение объекта по его Id
                var helper = new OrientNewsHelper();
                string target_entity = helper.GetEntity(id.ToString());

                // вычисление изменений
                var jsonToPut = helper.CreateChangesJson(target_entity, json_changes);

                // применение изменений
                var response = news.UpdateEntity(id.ToString(), jsonToPut);

                // возврат значения, который возвращает OrientDB
                return response;
            }
            throw new NotImplementedException();
        }

        public IHttpActionResult Delete()
        {
            throw new NotImplementedException();
        }


        //[HttpGet]
        public IHttpActionResult Get_PersonInfo()
        {
            //var userLogin = base.User.Identity.Name.Split('\\')[1];
            var name = userAuthenticator.AuthenticateUser(base.User);

            var response = news.GetPersonInfo(name);
            return proxy.ReturnPersonInfo(response);
        }

        public IHttpActionResult Get()
        {
            throw new NotImplementedException();
        }
        [Route("api/News/{type}")]
        public IHttpActionResult Get(string type)
        {           
            throw new NotImplementedException();
        }

        public IHttpActionResult Get(string type, int id)
        {
            throw new NotImplementedException();
        }

        public IHttpActionResult Get(string type, int id, int count)
        {
            throw new NotImplementedException();
        }

    }
}
