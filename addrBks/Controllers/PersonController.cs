using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web;

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
        private readonly NSQLManager.IPersonUOW pUOW;
        private readonly IWebManagers.IResponseReader responseReader;

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
           
            userAuthenticator = new UserAuthenticator();
            pUOW = new NSQLManager.PersonUOW();
            responseReader = new WebManagers.WebResponseReader();
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
        public IHttpActionResult SearchPerson(string accountName)
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

        //adintce APIs
        [HttpGet]
        public IHttpActionResult HoliVationAcc()
        {
            
            NSQLManager.AdinTceRepo repo = new NSQLManager.AdinTceRepo();
            WebManagers.WebResponseReader wr = new WebManagers.WebResponseReader();
            string name = string.Empty, GUID = string.Empty,res = string.Empty;
            WebManagers.ReturnEntities response=null;

            try
            {
                name = userAuthenticator.AuthenticateUser(base.User);
                if (name == string.Empty || name == null)
                {
                    res = @"Welcome Guest!";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                GUID = wr.ReadResponse(GetGUID(name));
                if(GUID == string.Empty || GUID == null)
                {
                    res = @"GUID searched";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                res = repo.HoliVation(GUID);
                if(res == null || res == string.Empty)
                {
                    res = @"Holidays searched";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            return response;

        }
        [HttpGet]
        public IHttpActionResult HoliVation(string accountName)
        {
            IHttpActionResult response = null;
            NSQLManager.AdinTceRepo repo = new NSQLManager.AdinTceRepo();
            WebManagers.WebResponseReader wr = new WebManagers.WebResponseReader();
            string res = string.Empty;
            try
            {
                 res = wr.ReadResponse(GetGUID(accountName)); 
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
            response = new WebManagers.ReturnEntities(repo.HoliVation(res), Request);
            
            return response;

        }
        [HttpGet]
        public IHttpActionResult Acc()
        {
         
            NSQLManager.AdinTceRepo repo = new NSQLManager.AdinTceRepo();
          
            string res ="Auth:{0};Envr:{1};WebCtx:{2};";
            WebManagers.ReturnEntities response = null;

            UserAuthenticator ua = new UserAuthenticator();
          
            try
            {
                res=string.Format(res,
                    ua.AuthenticateUser(base.User), Environment.UserName, HttpContext.Current.User.Identity.Name.ToString());                
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
           
            try
            {
                // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            return response;

        }


        [HttpPost]
        public IHttpActionResult TrackedBirthdays(string toGUID)
        {
            WebManagers.ReturnEntities response = null;
            NSQLManager.IPersonUOW pUow = new NSQLManager.PersonUOW();
            string name = userAuthenticator.AuthenticateUser(base.User);
            string fromGUID = responseReader.ReadResponse( GetGUID(name));

            string res = null;
            try
            {          
                res = pUow.AddTrackBirthday(new POCO.TrackBirthdays(), fromGUID, toGUID);

                response = new WebManagers.ReturnEntities(res, Request); 
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return response;
        }
        [HttpGet]
        public IHttpActionResult TrackedBirthdays()
        {
            WebManagers.ReturnEntities response = null;
            NSQLManager.IPersonUOW pUow = new NSQLManager.PersonUOW();
            string name = userAuthenticator.AuthenticateUser(base.User);
            string GUID=responseReader.ReadResponse(GetGUID(name));
            string res = null;
            try
            {
                res = pUow.GetByGUID(GUID);

                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return response;
        }

    }

}
