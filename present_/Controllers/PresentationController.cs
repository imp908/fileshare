using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Presentation_.DAL;
using Presentation_.Models;

using System.IO;

using System.Threading.Tasks;

namespace Presentation_.Controllers
{    
    public class PresentationController : Controller
    {
        Parameters param = new Parameters();
        MainModel mm = new MainModel();
        upload ul = new upload();
        Execution ex = new Execution();
        DWH_REPLICAEntities db = new DWH_REPLICAEntities();

        // GET: Pressentation
        public ActionResult PresentView()
        {
            
            Test ts = new Test();
            //ts.excelExportTest();        

            return View(param);
        }

        [HttpGet]
        public FileStreamResult ExcelExport(Parameters parameters_)
        {
            ex.mainmodel = mm;
            ex.parameters = parameters_;
            string filename = Path.Combine(Server.MapPath(@"~/App_Data"), @"report.xlsx");
        
            parameters_.formatSelcted = param.DateFormatReturn;
            parameters_.entitySelected = param.EntityReturn;

            ex.QueryFromParameters(db, parameters_);

            if(System.IO.File.Exists(filename))
            {
                System.IO.File.Delete(filename);
            }
            //System.IO.File.Create(filename);
            // pm.ExportToExcel(pm.FD_ACQ, filename);
            
            ex.ExcelExport(filename, mm);

            return new FileStreamResult(new FileStream(filename, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheet.sheet");
        }

        [HttpPost]
        public ActionResult ExcelImport(IEnumerable<HttpPostedFileBase> files)
        {
            ul.CurrentUser = "TEST_USER";
            ul.folder =  HttpContext.Server.MapPath("~/App_Data/");

            Task.Factory.StartNew(() => ul.UploadFromFiles(files));
            return View("PresentView", param);
        }

        //execute query
        public string GO(DateTime dateFrom_,DateTime dateTo_,string dateFormat_,string entitySelect_,bool listInclude_)
        {
            param.dateFrom = dateFrom_;
            param.dateTo = dateTo_;
            param.formatSelcted = param.DateFormatReturn;
            param.entitySelected = param.EntityReturn;
            param.listInclude = listInclude_;

            return ex.GO(param);
        }        
               
    }
}