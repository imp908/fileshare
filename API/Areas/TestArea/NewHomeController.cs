using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvccoresb.Domain.Models;

namespace mvccoresb.Areas.TestArea.Controllers
{
    //while named routing in startup.cs 
    //attribute can be omitted
    //[Area("TestArea")]
    public class NewHomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }

        //redirect to another view
        public IActionResult OldHomeIndex()
        {
            return View("../Home/Index");
        }
    }
}
