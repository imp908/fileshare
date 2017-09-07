using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using System.Web.Mvc;

namespace WebApplication1.Controllers
{

    public class DefaultController : Controller
    {
        public ActionResult DefaultView()
        {
            WCF_UOW.WCFClient wcf = new WCF_UOW.WCFClient();
            wcf.Initialize(@"SQLDB_J");
            wcf.SetCurrentUser(0);
            try
            {
                wcf.Initialize(@"SQLDB_J");
                wcf.SetCurrentUser(0);
                var kk = wcf.GetKKByUserId();
            }
            catch(Exception e)
            {
                //throw new Exception(@"thrown",e);
            }

            return View();
        }

        public static int evaporator(double content,double evap_per_day, double threshold)
        {
            return -1;
        }        
       
    }
}