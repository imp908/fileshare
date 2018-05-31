using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web;

using NewsAPI.Interfaces;
using NewsAPI.Helpers;
using NewsAPI.Implements;

using System.Configuration;

namespace NewsAPI.Controllers
{

  public class PersonController : ApiController
  {
    private readonly IJsonValidator jsonValidator;
    private readonly IUserAuthenticator userAuthenticator;
    private readonly IPersonFunctions personFunctions;
    private readonly IWebManagers.IResponseReader webReader;
        
    private readonly IWebManagers.IResponseReader responseReader;
    private readonly AdinTce.AdinTceRepo repo;
    public NewsUOWs.NewsRealUow _newsUOW;

    Managers.Manager mng;
    Managers.Manager sourceManager;

    public PersonController(
    IJsonValidator jsonValidator,
    IUserAuthenticator userAuthenticator,
    IPersonFunctions personFunctions_)
    {
        this.jsonValidator = jsonValidator;
        this.userAuthenticator = userAuthenticator;
        this.personFunctions = personFunctions_;

        string host_= string.Format("{0}:{1}"
        ,ConfigurationManager.AppSettings["OrientProdHost"],ConfigurationManager.AppSettings["OrientPort"]);

        mng = new Managers.Manager(
        ConfigurationManager.AppSettings["OrientProdDB"]
        ,host_
        ,ConfigurationManager.AppSettings["orient_login"]
        ,ConfigurationManager.AppSettings["orient_dev_pswd"]
        );

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
        repo = new AdinTce.AdinTceRepo();
        responseReader = new WebManagers.WebResponseReader();

        string host_= string.Format("{0}:{1}"
        ,ConfigurationManager.AppSettings["OrientProdHost"],ConfigurationManager.AppSettings["OrientPort"]);

        mng = new Managers.Manager(
        ConfigurationManager.AppSettings["OrientProdDB"]
        ,host_
        ,ConfigurationManager.AppSettings["orient_login"]
        ,ConfigurationManager.AppSettings["orient_dev_pswd"]
        );

        string host_Source= string.Format("{0}:{1}"
        ,ConfigurationManager.AppSettings["OrientProdHost"],ConfigurationManager.AppSettings["OrientPort"]);

        sourceManager = new Managers.Manager(
        ConfigurationManager.AppSettings["OrientSourceDB"]
        ,host_Source
        ,ConfigurationManager.AppSettings["orient_login"]
        ,ConfigurationManager.AppSettings["orient_prod_pswd"]
        );

        this._newsUOW = mng.GetNewsUOW();
    }

    [HttpGet]
    //[Route("api/Person/GetUnit/{accountName}")]
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
    //[Route("api/Person/GetDepartment/{accountName}")]
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
    //[Route("api/Person/GetManager/{accountName}")]
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
    //[Route("api/Person/GetColleges/{accountName}")]
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
    //[Route("api/Person/GetManagers/{accountName}")]
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
    //[Route("api/Person/GetCollegesLower/{accountName}")]
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
    //[Route("api/Person/GetGUID/{accountName}")]
    public IHttpActionResult GetGUID(string accountName)
    {
        // Получаем хелпер
        var newsHelper = new OrientNewsHelper();

        // Осуществляем авторизацию в OrientDb
        //newsHelper.Authorize();

        string res = sourceManager.GetPersonUOW().GetPersonByAccount(accountName).GUID;
        //string res = personFunctions.GetGUID(accountName);
        WebManagers.ReturnEntities response = new WebManagers.ReturnEntities(res, Request);
        return response;
    }
       
    [HttpGet]
    //[Route("api/Person/SerachPerson/{accountName}")]
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
    [Route("api/Person/HoliVationAcc")]
    public IHttpActionResult HoliVationAcc()
    {
        WebManagers.ReturnEntities response=null;                   
        WebManagers.WebResponseReader wr = new WebManagers.WebResponseReader();
        string name = string.Empty,res = string.Empty;
            
        name=this.mng.UserAcc();

        if(name==null||name==string.Empty){

            try
            {
                name=userAuthenticator.AuthenticateUser(base.User);               
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
        }

        if(name != null && name != string.Empty)
        {
            try
            {
                res = wr.ReadResponse(GetGUID(name));
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
        }           

        if(res!=null && res!=string.Empty)
        {
            
            try {                
                // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
                response = new WebManagers.ReturnEntities(repo.HoliVation(res), Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

        }
        else
        {
            response = new WebManagers.ReturnEntities(string.Empty, Request);
        }

        if(response== null || response._result==null)
        {
            response = new WebManagers.ReturnEntities(string.Empty, Request);
        }

        return response;

    }

    [HttpGet]
    [Route("api/Person/HoliVation/{accountName}")]
    public IHttpActionResult HoliVation(string accountName)
    {
        IHttpActionResult response = null;         
        WebManagers.WebResponseReader wr = new WebManagers.WebResponseReader();
        try
        {
            string res = wr.ReadResponse(GetGUID(accountName));
            // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
            response = new WebManagers.ReturnEntities(repo.HoliVation(res), Request);
        }
        catch (Exception e) { response = new WebManagers.ReturnEntities("Welcome guest!", Request); System.Diagnostics.Trace.WriteLine(e.Message); }
        return response;

    }

    [HttpGet]
    [Route("api/Person/Acc")]
    public IHttpActionResult Acc()
    {

        string userAcc=mng.UserAcc();
            
        string patern = "BaseUser:{0};Environment:{1};HttpContext:{2}; AuthType:{3}; IsAuth:{4}; Name:{5}";
        WebManagers.ReturnEntities response = null;
        string res = string.Empty;
        UserAuthenticator ua = new UserAuthenticator();
        string auth = string.Empty;
            
        if (userAcc == null||userAcc==string.Empty){

          try
          {
              ua.AuthenticateUser(base.User);
          }
          catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

          try
          {
              res = string.Format(patern,
                  auth, 
                  Environment.UserName, 
                  HttpContext.Current.User.Identity.Name.ToString()
                  , User.Identity.AuthenticationType
                  , User.Identity.IsAuthenticated
                  , User.Identity.Name
                  );
          }
          catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

        }
        else{ res = userAcc; }

        try
        {
            // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
            response = new WebManagers.ReturnEntities(res, Request);
        }
        catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message);
            response = new WebManagers.ReturnEntities("Welcome guest!", Request);
        }

        return response;

    }

  }

}
