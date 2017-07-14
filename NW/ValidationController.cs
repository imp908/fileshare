using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Northwind.Models;

using Northwind.DAL;

namespace Northwind.Controllers
{


    public class ValidationController : Controller
    {
        ModelNorthwind db;

        LoginModel loginmodel_ = new LoginModel();

        // GET: Account
       
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel lm_)
        {
            ModelNorthwind db = new ModelNorthwind();
            Validations.ContextBind(db);
            Validations.LoginBind(lm_);
            if (Validations.AccountCheck() == null)
            {
                //redirect to register account
                return View("/Validation/Register");
            }
            return View("/Home/Index");
        }

        [HttpGet]
        public ActionResult Register()
        {          
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel rm_)
        {
            //add account
            ModelNorthwind db = new ModelNorthwind();
            Validations.ContextBind(db);
            Validations.RegisterBind(rm_);
            
            if (Validations.EmployeeCheck())
            {
                Validations.AccountAdd();
            }
            return View("Validation", "Login");
        }
    }
}