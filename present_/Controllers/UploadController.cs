using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Presentation_.Models;

namespace Presentation_.Controllers
{
    public class UploadController : Controller
    {
        upload ul = new upload();

        [HttpPost]
        // GET: Upload
        public ActionResult FilesPublish(IEnumerable<HttpPostedFileBase> files_)
        {
            Session["files"] = files_;
            ul.GetPostedFiles(files_);
            return View(ul.fileNames);
        }

        public ActionResult FilesLoad()
        {
            IEnumerable<HttpPostedFileBase> files_ = Session["files"] as IEnumerable<HttpPostedFileBase>;          
            ul.UploadFromFiles(files_);
            return RedirectToAction("Dash","Dash");
        }
    }
}