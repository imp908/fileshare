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
            ,ConfigurationManager.AppSettings["OrientProdHost"],ConfigurationManager.AppSettings["OrientPort"]);

            mng = new Managers.Manager(
            ConfigurationManager.AppSettings["OrientSourceDB"]
            ,host_
            ,ConfigurationManager.AppSettings["orient_dev_login"]
            ,ConfigurationManager.AppSettings["orient_prod_pswd"]
            );

            _newsUOW = mng.GetNewsUOW();
        }

        [HttpGet]
        [Route("api/Person2/SearchPerson/{name_}")]
        public IHttpActionResult SearchPerson(string name_)
        {
            string result_=mng.SearchByName(name_);
            _response = new WebManagers.ReturnEntities(result_, Request);
            return _response;
        }

    }
}
