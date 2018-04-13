using NewsAPI.Helpers;
using NewsAPI.Interfaces;
using System.Web.Http;
using NewsAPI.Extensions;
using System;
using System.Configuration;
using System.Threading;
using static NewsAPI.Helpers.OrientNewsHelper;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;


namespace NewsAPI.Controllers
{
    public class PersonRelationController : ApiController
    {
        private readonly IAddressBookProxy proxy;
        private readonly IPersonRelation personRelation;
        private readonly IUserAuthenticator userAuthenticator;

        public PersonRelationController(IPersonRelation personRelation, IAddressBookProxy proxy, IUserAuthenticator userAuthenticator)
        {
            this.proxy = proxy;
            this.personRelation = personRelation;
            this.userAuthenticator = userAuthenticator;
        }
        
        [HttpGet]
        public IHttpActionResult Get()
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получение пользователя из реквеста
            var userLogin = userAuthenticator.AuthenticateUser(base.User);

            // Получение всех существующих связей типа PersonRealation для указанного пользователя
            var response = personRelation.GetPersonRelation(userLogin);

            // Проксируем результирующий набор данных перед выдачей
            var personRelations = new OrientNewsHelper.ReturnPersonRelations(response);

            return personRelations;
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody]string personGUID)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получение пользователя из реквеста
            var userLogin = userAuthenticator.AuthenticateUser(base.User);

            // Создание связи PersonRelation между объектами Person
            var response = personRelation.PostPersonRelation(userLogin, personGUID);

            // Проксируем результирующий набор данных перед выдачей
            var personRelationsCount = new OrientNewsHelper.ReturnPersonRelationCount(response);

            return personRelationsCount;
        }

        [HttpGet]
        [Route("api/PersonRelation/{personGUID}")]
        public IHttpActionResult Get(string personGUID)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получение пользователя из реквеста
            var userLogin = userAuthenticator.AuthenticateUser(base.User);

            // Удвление связи PersonRelation между объектами Person
            var response = personRelation.DeletePersonRelation(userLogin, personGUID);

            // Проксируем результирующий набор данных перед выдачей
            var personRelationsCount = new OrientNewsHelper.ReturnPersonRelationCount(response);

            return personRelationsCount;

        }


        [HttpOptions]
        public IHttpActionResult Options()
        {
            return Ok();
        }


    }
}