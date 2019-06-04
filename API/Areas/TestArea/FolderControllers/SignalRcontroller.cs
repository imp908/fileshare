using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using mvccoresb.Domain.Interfaces;

using mvccoresb.Domain.TestModels;

using Newtonsoft.Json;

namespace mvccoresb.TestArea.Controllers
{
    /** while mapping in startup.completionlist exists no custom attribute needed */
    //[Area("TestArea")]
    public class SignalRcontroller : Controller
    {
        public IActionResult Hub()
        {
            return View("../SignalR/hub");
        }

    }
}