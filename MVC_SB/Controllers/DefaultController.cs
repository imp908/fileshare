using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVC_SB.Models;

namespace MVC_SB.Controllers
{
   
    public class DefaultController : Controller
    {

        MultiFormModel multiPartFormModel = new MultiFormModel();

        // GET: Default
        public ActionResult MultiForm()
        {
            multiPartFormModel.InitializeTest();
            Session["multipart"] = multiPartFormModel.formsList;
            return View(multiPartFormModel);
        }

        [HttpPost]        
        public ActionResult MultiForm(int ID,DateTime? DateFrom, DateTime? DateTo,string command)
        {
            MultiFormModel multi;

            multiPartFormModel.formsList = (List<MultiFormModel>)Session["multipart"];
            if (command=="Start")
            {

                if (multiPartFormModel.formsList!=null)
                {
                    multi = multiPartFormModel.formsList.Where(s => s.ID == ID).FirstOrDefault();
                    multi.dateFrom = DateFrom;
                    multi.dateTo = DateTo;
                    multi.status = Statuses.STARTED;
                }
                //>>||| call wcf start cnnection
            }

            if (command == "Stop")
            {
                if (multiPartFormModel.formsList != null)
                {
                    multi = multiPartFormModel.formsList.Where(s => s.ID == ID).FirstOrDefault();
                    multi.dateFrom = DateFrom;
                    multi.dateTo = DateTo;
                    multi.status = Statuses.STOPED;
                }
                //>>|||
                //call wcf kill connection
            }

            return View(multiPartFormModel);
        }

    }
}