using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Configuration;

namespace NewsAPI.Controllers
{
    public class QuizController : ApiController
    {
        Managers.Manager quizManager;

        public class QuizParams{
          public int? monthDepth { get; set; }
        }

        [HttpPost]
        [Route("api/Quiz")]
        public IHttpActionResult Post(QuizParams qp)
        {
            
          string host_Quiz= string.Format("{0}:{1}"
          ,ConfigurationManager.AppSettings["OrientProdHost"],ConfigurationManager.AppSettings["OrientPort"]);

          quizManager = new Managers.Manager(
          ConfigurationManager.AppSettings["IntranetDB"]
          ,host_Quiz
          ,ConfigurationManager.AppSettings["orient_dev_login"]
          ,ConfigurationManager.AppSettings["orient_dev_pswd"]
          );

          WebManagers.ReturnEntities response = null;
          string res = string.Empty;
          Quizes.QuizUOW qu = new Quizes.QuizUOW(quizManager.GetRepo());

            try
            {
              // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
              res=qu.GetQuizByMonthGap(qp.monthDepth);
              response=new WebManagers.ReturnEntities(res, Request);
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
