using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using NSQLManager;

using System.Configuration;

namespace NewsAPI.Controllers
{


    public class Person2Controller : ApiController
    {
        public NewsUOWs.NewsRealUow _newsUOW;
        WebManagers.ReturnEntities _response;
         Managers.Manager mng;

        public Person2Controller()
        {
          string host_= string.Format("{0}:{1}"
        ,ConfigurationManager.AppSettings["OrientTargetHost"],ConfigurationManager.AppSettings["OrientPort"]);

        mng = new Managers.Manager(
        ConfigurationManager.AppSettings["OrientUnitTestDB"]
        ,host_
        ,ConfigurationManager.AppSettings["orient_login"]
        ,ConfigurationManager.AppSettings["orient_pswd"]
        );

          _newsUOW = mng.GetNewsUOW();
        }

        [HttpGet]
        public IHttpActionResult Acc()
        {
          string result_=mng.UserAcc();
          _response = new WebManagers.ReturnEntities(result_, Request);
          return _response;
        }

    }
}
