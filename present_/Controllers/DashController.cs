using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.IO;

using Presentation_.DAL;
using Presentation_.Models;

using Newtonsoft.Json;

using OfficeOpenXml;

namespace Presentation_.Controllers
{
    public class DashController : Controller
    {

        DWH_REPLICAEntities db = new DWH_REPLICAEntities();
        MainModel ml = new MainModel();
        Parameters pm = new Parameters();
        Execution ex = new Execution();
        upload ul = new upload();

        // GET: Dash
        public ActionResult Dash()
        {
            Test t = new Test();
            t.excelExportTest();
            return View(pm);
        }

        [HttpPost]
        public string GetJSONbyParams(DateTime dateFrom_, DateTime dateTo_,string dateType_,bool checkBox_, string selectedList_)
        {
            ml.GetJSONbyParams(db, dateFrom_, dateTo_,dateType_, checkBox_, selectedList_);

            string jsn = JsonConvert.SerializeObject(ml.FD_ACQ);

            return jsn;
        }

        [HttpGet]
        public FileResult ExportXLSX(Parameters pm) //(DateTime dateFrom_, DateTime dateTo_, string dateType_, string selectedList_)
        {
         
            string filename = Path.Combine(Server.MapPath("~/App_Data"), @"report.xlsx");

            FileInfo newFile = new FileInfo(filename);
            
            ml.GetJSONbyParams(db, pm.dateFrom, pm.dateTo, pm.dateFormatPublish.Select(s=>s).First().ToString(),pm.listInclude, pm.ParametersPublish, pm.entitySelected);
            System.IO.File.Delete(filename);
            ml.ExportToExcel(newFile);

            //return File(filename, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //return new FileStreamResult(new FileStream(filename, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            //return filename;

            return new FileStreamResult(new FileStream(filename, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            //working
            //return File(filename, "application/octet-stream");
        }

        [HttpPost]
        // GET: Upload
        public ActionResult UploadMulti(IEnumerable<HttpPostedFileBase> files_)
        {
            //ul.GetFiles(files_);
            ul.folder = HttpContext.Server.MapPath("~/App_Data/");
            ul.CurrentUser = "USER_1";
            ul.UploadFromFiles(files_);
            return View("Dash", pm);
        }


        //templated methods
        public string GetJSON()
        {
            ml.GetACQ(db);

            string jsn = JsonConvert.SerializeObject(ml.FD_ACQ);

            return jsn;
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            string filename = Path.GetFileName(file.FileName);
            string folder = Server.MapPath("~/App_Data/");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(Server.MapPath("~/App_Data/"), filename);

            file.SaveAs(path);

            return View("Dash", pm);
        }       

        [HttpGet]
        public FileResult Download()
        {
            string filename = Path.GetFileName(@"report.xlsx");
            string folder = Server.MapPath("~/App_Data/");
           
            string path = Path.Combine(Server.MapPath("~/App_Data/"), filename);

            //return File(path, System.Net.Mime.MediaTypeNames.Application.Octet);
            return new FileStreamResult(new FileStream(path, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        [HttpPost]
        public string GetJSONbyPOST()
        {
            ml.GetACQ(db);

            string jsn = JsonConvert.SerializeObject(ml.FD_ACQ);

            return jsn;
        }

        [HttpPost]
        public string GetJSONbyDate(DateTime dateFrom, DateTime dateTo)
        {
            ml.GetACQbyMonth(db, dateFrom, dateTo);

            string jsn = JsonConvert.SerializeObject(ml.FD_ACQ);

            return jsn;
        }

    }
}