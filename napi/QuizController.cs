using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace NewsAPI.Controllers
{
    public class QuizController : ApiController
    {

        [HttpGet]
        [Route("api/Quiz")]
        public IHttpActionResult Get()
        {

            WebManagers.ReturnEntities response = null;
            string res = string.Empty;
            Quizes.QuizRepo qr = new Quizes.QuizRepo();

            try
            {
              // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
              res = qr.Quiz();
              response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e)
            {
              System.Diagnostics.Trace.WriteLine(e.Message);
              response = new WebManagers.ReturnEntities("Error: " + e.Message, Request);
            }

            return response;

        }

    }


  

}
