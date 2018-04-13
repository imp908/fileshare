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
        OrientRealization.IOrientRepo repo;
        Quizes.QuizUOW qr;

        public class QuizParams{
          public int? monthDepth { get; set; }
        }

        public QuizController()
        {
            string host= string.Format("{0}:{1}"
            ,ConfigurationManager.AppSettings["OrientProdHost"],ConfigurationManager.AppSettings["OrientPort"]);

            repo= OrientRealization.RepoFactory.NewOrientRepo(ConfigurationManager.AppSettings["IntranetDB"]
            ,host
            ,ConfigurationManager.AppSettings["orient_dev_login"]
            ,ConfigurationManager.AppSettings["orient_dev_pswd"]);
            
            qr = new Quizes.QuizUOW(repo);
        }

        [HttpPost]
        [Route("api/Quiz")]
        public IHttpActionResult Post(QuizParams qp_)
        {           
            WebManagers.ReturnEntities response = null;
            string res = string.Empty;
            qr = new Quizes.QuizUOW(repo);
            QuizParams _qp = qp_!=null ? qp_ : new QuizParams();
            
            try
            {
                // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
                res=qr.GetQuizByMonthGap(_qp.monthDepth);
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
