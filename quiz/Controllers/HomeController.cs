using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Intranet.Models;
//using Intranet.Models.DocsVision;
//using Intranet.Models.IntraService;
using Intranet.Models.QuizModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Script.Serialization;
using Orient.Client;

namespace Intranet.Controllers
{
    public class HomeController : Controller
    {
        private OrientDbContext _dataBaseContext;
      // private DocsvisionDbContext _docsvisionContext;
      //  private IntraServiceDbContext _intraServiceContext;
        private string _userName;

        public HomeController()
        {
         //   _dataBaseContext = new OrientDbContext();
          //  _userName = System.Web.HttpContext.Current.User.Identity.Name;
        }
        public ActionResult Index()
        {
          //  _docsvisionContext = new DocsvisionDbContext();
           // _intraServiceContext = new IntraServiceDbContext();

            List<Task<List<object>>> tasks = new List<Task<List<object>>>
            {
              //  Task.Factory.StartNew(() => _docsvisionContext.GetData(_userName)),
              //  Task.Factory.StartNew(() => _dataBaseContext.GetData(_userName)),
              //  Task.Factory.StartNew(() => _intraServiceContext.GetData(_userName))
            };
            Task.WaitAll(tasks.ToArray());



            var result = new MainPageModel()
            {
              //  Docsvision = tasks[0].Result.OfType<nspk_GetTaskForCurrentPerformerInfoForIntranet_Result>().ToList(), //docsvisionContext.GetInfo(),
              ///  Weather = tasks[1].Result.OfType<WeatherHelper.WeatherInformation>().ToList().FirstOrDefault(),         //dataBaseContext.GetInfo().FirstOrDefault(),
              //  IntraService = (GetTaskCountForIntranet_Result)tasks[2].Result.FirstOrDefault()                         //intraServiceContext.GetInfo().FirstOrDefault()
            };


          //  return Redirect("http://list.nspk.ru/may/");
           return View(result);
        }

        public ActionResult news()
        {
            return View();
        }
    }

}