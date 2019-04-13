using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransactionSite_.DAL;

using TransactionSite_.Models;

using System.Threading.Tasks;

namespace TransactionSite_.Controllers
{
    public class HomeController : AsyncController
    {
        ServiceLayer sl = new ServiceLayer();    
        
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // GET: Home
        public ActionResult Index(string str)
        {            
            DBgen();
            return View(str);
        }

        [HttpGet]
        public async Task<ActionResult> Migrate()
        {         
            return View(sl);
        }

        /*
        [HttpPost]
        public async Task<ActionResult> Migrate( int ID,DateTime? DateFrom, DateTime? DateTo)
        {
            sl.UpdateModel(ID, DateFrom, DateTo);
            return View(sl);
        }
        */

        [HttpPost]
        [MultipleButton(Name ="action", Argument ="Update")]
        public ActionResult Update(int ID, DateTime? DateFrom, DateTime? DateTo)
        {
            ServiceLayer sl_ = new ServiceLayer();
            ServiceReference1.Service1Client migrateService = new ServiceReference1.Service1Client();
            sl.result = "(Status:Updating)";

            //sl_.init();
            sl_.DateFrom = (DateTime)DateFrom;
            sl_.DateTo = (DateTime)DateTo;
            sl_.tableList.Where(s => s.ID == ID).First().DateFrom = (DateTime)DateFrom;
            sl_.tableList.Where(s => s.ID == ID).First().DateTo = (DateTime)DateTo;


            Task.Run(() => migrateService.migrate_((DateTime)DateFrom, (DateTime)DateTo));

            return View("Insert",sl_.tableList);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Stop")]
        public ActionResult Stop(int ID, DateTime? DateFrom, DateTime? DateTo)
        {
            ServiceLayer sl_ = new ServiceLayer();

            if (ModelState.IsValid) { ModelState.Clear(); }

            if (sl.client != null)
            {
                sl.client.Thread_cancel();
            }           
            sl.result = "(Status:Stoped)";

            return View("Insert", sl_.tableList);
        }
        
        [HttpGet]
        public ActionResult Insert(ServiceLayer sl)
        {            
            if(!sl.initialized)
            {
                sl.init();
                Session["sl"] = sl;
            }
            IEnumerable<QueryState> ts = sl.tableEnum;
            return View(ts);
        }

        [HttpPost]
        public ActionResult Insert_(int ID, DateTime? DateFrom, DateTime? DateTo)
        {
            ServiceLayer sl_ = new ServiceLayer();
            ServiceReference1.Service1Client migrateService = new ServiceReference1.Service1Client();

            //sl_.init();
            sl_.DateFrom = (DateTime)DateFrom;
            sl_.DateTo= (DateTime)DateTo;
            sl_.tableList.Where(s => s.ID == ID).First().DateFrom= (DateTime)DateFrom;
            sl_.tableList.Where(s => s.ID == ID).First().DateTo = (DateTime)DateTo;
           

            Task.Run(() => migrateService.migrate_((DateTime)DateFrom, (DateTime)DateTo));
            
            return View(sl_.tableEnum);
        }

        //[HttpPost]
        public ActionResult Insert__(int ID, DateTime? DateFrom, DateTime? DateTo)
        //public ActionResult Insert_(TableState ts_)
        {
            QueryState ts = sl.tableList.Where(s => s.ID == ID).FirstOrDefault();         
            return RedirectToAction("Insert", sl);
           // return View("Insert", sl.tableEnum);
        }

        [HttpPost]
        public ActionResult Insert_Submit(QueryState ts)
        {
            return View("Insert");
        }

        public void DBgen()
        {
            SQL_CONTEXT db = new SQL_CONTEXT();
           
            if (!db.Database.Exists())
            {
                db.Database.Delete();
                db.SaveChanges();
                db.Database.CreateIfNotExists();
                db.SaveChanges();
            }
        }

        public void migration()
        {
            sl.migrate("FD_ACQ_D", new DateTime(2016, 01, 01), new DateTime(2016, 08, 28));
        }
     
    }
}