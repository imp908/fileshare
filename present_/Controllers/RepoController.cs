using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Repo.DAL;
using Presentation_.Models;

using System.IO;

using Newtonsoft.Json;



namespace Presentation_.Controllers
{
    public class RepoController : Controller
    {

        rExcelExport model = new rExcelExport();
        Repo.DAL.SQL_ent.SQL_entity ent = new Repo.DAL.SQL_ent.SQL_entity();
        rUpload upload = new rUpload();

        // GET: Repo
        public ActionResult RepoView()
        {

            model.SetEntity(ent);
            model.GetEntityNames();

            //test.DBrecreate();
            //export import of filemasks
            //test.MaskIOCheck(HttpContext.Server.MapPath("~/App_Data/"));
            //test.MapperRegexpCheck(HttpContext.Server.MapPath("~/App_Data/"));
            //test.InsertCheckWrite(HttpContext.Server.MapPath("~/App_Data/"));
            //test.SQL_CLR_check(HttpContext.Server.MapPath("~/App_Data/"));

            upload.folder = HttpContext.Server.MapPath("~/App_Data/");
            upload.CurrentUser = "USER_1";
            //upload.UploadFromFiles(files_);

            return View("~/Views/Repo/RepoView.cshtml", model.values);
        }

        public FileStreamResult ExcelExport(string dateFrom_, string dateTo_,string tableName_,bool? listInclude_)
        {
            model.SetEntity(ent);
            model.GetEntityNames();

            rParameters params_ = new rParameters(dateFrom_, dateTo_, tableName_, listInclude_);
#if DEBUG
            model.SetFile(@"C:\FILES\SHARE\debug\report.xlsx");
#endif
#if (!DEBUG)
            model.SetFile( Path.Combine(Server.MapPath(@"~/App_Data"), @"report.xlsx"));
#endif
            model.SetParameters(params_);
            model.ExcelExport();
#if DEBUG
            return new FileStreamResult(new FileStream(model.FileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheet.sheet");
#endif
#if (!DEBUG)
            return new FileStreamResult(new FileStream(model.FileName, FileMode.Open), "application/vnd.openxmlformats-officedocument.spreadsheet.sheet");
#endif
        }
  
        public ActionResult ExcelImport(IEnumerable<HttpPostedFileBase> files_)
        {
            rUpload ul = new rUpload();
            Mapper.MasksInit(HttpContext.Server.MapPath("~/App_Data/"));

            model.SetEntity(ent);
            model.GetEntityNames();

            ul.folder = HttpContext.Server.MapPath("~/App_Data/");
            ul.CurrentUser = "USER_1";
            ul.UploadFromFiles(files_);

            return View("~/Views/Repo/RepoView.cshtml", model.values);
        }

        public string ListCheck()
        {
            string result = "";
            model.SetEntity(ent);
            result = JsonConvert.SerializeObject(model.ListCheck());
            return result;
        }
    }
}
