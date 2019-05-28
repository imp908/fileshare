using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvccoresb.Domain.Models;

namespace mvccoresb.TestArea.Controllers
{
    /** while mapping in startup.completionlist exists no custom attribute needed */
    //[Area("TestArea")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewHomeIndex()
        {
            return View("../NewHome/Index");
        }
    }
}
