using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Presentation_.DAL;
using Presentation_.Models;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace Presentation_.Controllers
{
    public class HomeController : Controller
    {

        DWH_REPLICAEntities db = new DWH_REPLICAEntities();
        MainModel ml = new MainModel();

        // GET: Home
        public ActionResult Index()
        {
            ml.GetACQ(db);
            return View(); 
        }

        public ActionResult Presentations()
        {
            ml.GetACQ(db);
            return View(ml);
        }

        public string GetJSON()
        {
            ml.GetACQ(db);

            string jsn = JsonConvert.SerializeObject(ml.FD_ACQ);

            return jsn;
        }

    }
}