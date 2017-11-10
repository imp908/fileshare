using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using NewsAPI.Interfaces;
using NewsAPI.Helpers;
using NewsAPI.Implements;

namespace NewsAPI.Controllers
{

    public class PersonController : ApiController
    {
        private readonly IJsonValidator jsonValidator;
        private readonly IUserAuthenticator userAuthenticator;
        private readonly IPersonFunctions personFunctions;
        private readonly IWebManagers.IResponseReader webReader;      

        public PersonController(
            IJsonValidator jsonValidator,
            IUserAuthenticator userAuthenticator,
            IPersonFunctions personFunctions_)
        {
            this.jsonValidator = jsonValidator;
            this.userAuthenticator = userAuthenticator;
            this.personFunctions = personFunctions_;        
        }

        public PersonController()
        {
            JSONProxy jp = new JSONProxy();
            JsonManagers.JSONManager jm = new JsonManagers.JSONManager();
            //Old Json manager used in person fucntions response parsing
            FunctionsToString functions = new FunctionsToString(jp,jm);
            //binding of new JSON manager for parsing all Orient, 1C responses
            functions.BindJSONmanager(new JsonManagers.JSONManager());
            this.personFunctions = new OrientPersons(functions);
            this.webReader = new WebManagers.WebResponseReader();
            userAuthenticator = new UserAuthenticator();
        }

        [HttpGet]
        public IHttpActionResult GetUnit(string accountName)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем наименование подразделения по имени аккаунта
            // проксирование выполняется после вызова функции в PersonFunctions через JSONproxy
            string requestedEntity = personFunctions.GetUnitByAccount(accountName);

            // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
            OrientNewsHelper.ReturnEntities response = new OrientNewsHelper.ReturnEntities(requestedEntity, Request);

            return response;

        }

        [HttpGet]
        public IHttpActionResult GetDepartment(string accountName)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем менеджера работника по имени аккаунта
            // проксирование выполняется после вызова функции в PersonFunctions через JSONproxy
            string requestedEntity = personFunctions.GetDepartmentByAccount(accountName);

            // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
            OrientNewsHelper.ReturnEntities response = new OrientNewsHelper.ReturnEntities(requestedEntity, Request);

            return response;

        }

        [HttpGet]
        public IHttpActionResult GetManager(string accountName)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем менеджера работника по имени аккаунта
            // проксирование выполняется после вызова функции в PersonFunctions через JSONproxy
            string requestedEntity = personFunctions.GetManagerByAccount(accountName);

            // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
            OrientNewsHelper.ReturnEntities response = new OrientNewsHelper.ReturnEntities(requestedEntity, Request);

            return response;

        }

        [HttpGet]
        public IHttpActionResult GetColleges(string accountName)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем менеджера работника по имени аккаунта
            // проксирование выполняется после вызова функции в PersonFunctions через JSONproxy
            string requestedEntity = personFunctions.GetCollegesByAccount(accountName);

            // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
            OrientNewsHelper.ReturnEntities response = new OrientNewsHelper.ReturnEntities(requestedEntity, Request);

            return response;

        }

        [HttpGet]
        public IHttpActionResult GetManagers(string accountName)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем менеджера работника по имени аккаунта
            // проксирование выполняется после вызова функции в PersonFunctions через JSONproxy
            string requestedEntity = personFunctions.GetManagerHierarhyByAccount(accountName);

            // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
            OrientNewsHelper.ReturnEntities response = new OrientNewsHelper.ReturnEntities(requestedEntity, Request);

            return response;

        }

        [HttpGet]
        public IHttpActionResult GetCollegesLower(string accountName)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем менеджера работника по имени аккаунта
            // проксирование выполняется после вызова функции в PersonFunctions через JSONproxy
            string requestedEntity = personFunctions.GetCollegesLowerByAccount(accountName);

            // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
            OrientNewsHelper.ReturnEntities response = new OrientNewsHelper.ReturnEntities(requestedEntity, Request);

            return response;

        }

        [HttpGet]
        public IHttpActionResult GetGUID(string accountName)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            string res = personFunctions.GetGUID(accountName);
            WebManagers.ReturnEntities response = new WebManagers.ReturnEntities(res, Request);
            return response;
        }
       
        [HttpGet]
        public IHttpActionResult SerachPerson(string accountName)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем менеджера работника по имени аккаунта
            // проксирование выполняется после вызова функции в PersonFunctions через JSONproxy
            string requestedEntity = personFunctions.SearchPerson(accountName);

            // преобразуем строку в HttpResponseMessage с ReturnEntities с результатом в поле _value
            OrientNewsHelper.ReturnEntities response = new OrientNewsHelper.ReturnEntities(requestedEntity, Request);

            return response;

        }

        [HttpGet]
        public IHttpActionResult HoliVationAcc()
        {
            
            NSQLManager.AdinTceRepo repo = new NSQLManager.AdinTceRepo();
            WebManagers.WebResponseReader wr = new WebManagers.WebResponseReader();
            string name = userAuthenticator.AuthenticateUser(base.User);
            string res = wr.ReadResponse(GetGUID(name));         
            
            // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
            WebManagers.ReturnEntities response = new WebManagers.ReturnEntities(repo.HoliVation(res), Request);
            
            return response;

        }
        [HttpGet]
        public IHttpActionResult HoliVation(string accountName)
        {
            IHttpActionResult response = null;
            NSQLManager.AdinTceRepo repo = new NSQLManager.AdinTceRepo();
            WebManagers.WebResponseReader wr = new WebManagers.WebResponseReader();
            try
            {
                string res = wr.ReadResponse(GetGUID(accountName));
                // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
                response = new WebManagers.ReturnEntities(repo.HoliVation(res), Request);
            }
            catch (Exception e) { }
            return response;

        }

    }

}
