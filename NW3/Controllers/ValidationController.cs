using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Threading.Tasks;

using Northwind.Models.Validation;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using Northwind.DAL;

using Microsoft.Owin.Security;
using System.Security.Claims;

using System.ComponentModel.DataAnnotations;

namespace Northwind.Controllers
{
    public class ValidationController : Controller
    {        

        private ApplicationUserManager_V UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager_V>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        // GET: Validation
        [AllowAnonymous]   
        public ActionResult Login()
        {
          
            Login lg = new Models.Validation.Login();
            if (TempData["Message"] == null)
            {
                lg.Message = @"Login please";
            }
            else {
                lg.Message = TempData["Message"] as string;
                TempData["Message"] = null;
            }
            return View(lg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Login login_)
        {

            if(ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(login_.Email, login_.Password );

                if (user != null)
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, login_.Email)); //, user.FirstName,user.LastName));
                    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(identity);                  
                
                    if (AuthenticationManager.AuthenticationResponseGrant.Identity.IsAuthenticated)
                    {

                        DbLevel NW = new DbLevel();
                        Employees empl = NW.GetEmployee(user);

                        if (empl != null)
                        {
                            TempData["Message"] = @"Welcome" + empl.FirstName + "!";
                            return RedirectToAction("Orders","Edit", new { userID = user.Id } );
                        }
                        else
                        {
                            TempData["Message"] = @"Something werid happened. No employee.";
                            return View("Register", "Validation");
                        }                           
                        
                    }
                    else
                    {
                        TempData["Message"] = @"User not signed in";
                    }
                }
                else
                {
                    TempData["Message"] = @"Wrong password or username";                   
                }                             
            }
         
            login_.Message = TempData["Message"] as string;
            return View(login_);
        }

      
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login","Validation");
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            Register rg = new Models.Validation.Register();

            if (TempData["Message"] == null )
            {
                rg.Message = @"Register using your email and First and Last name";
            }
            else {
                rg.Message = TempData["Message"] as string; 
                TempData["Message"] = null;
            }

            return View(rg);
        }            

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Register login_)
        {
            if (ModelState.IsValid) 
            {
                DbLevel NW = new DbLevel();
                Employees empl = NW.GetEmployee(login_);
                ApplicationUser user = new ApplicationUser() {
                    UserName = login_.Email , FirstName = login_.FirstName , LastName = login_.LastName };
                               

                NorthwindModel db = new NorthwindModel();

                if (empl != null) 
                {
                    IdentityResult result = await UserManager.CreateAsync(user, login_.Password);

                    if (result.Succeeded)
                    {

                        login_.Message = empl.FirstName + @" your account created. Login with your credentials";
                        TempData["Message"] = login_.Message;
                        TempData["EmployeeID"] = empl.EmployeeID;
                        return RedirectToAction("Login", "Validation");
                    }          
                                                                    
                    if (result.Errors != null && result.Errors.Count() != 0)
                    {                   
                        //String buillder ommitted for simplicity.
                        foreach (string error_ in  (from s in result.Errors select s))
                        {
                            login_.Message += " " + error_;
                        }

                        TempData["Message"] = login_.Message;
                        return RedirectToAction("Register", "Validation");
                    }
                }
                else
                {
                    TempData["Message"] = @"Employee not found. Are you working here?";
                    login_.Message = TempData["Message"] as string;
                    
                }               
            }
            
            return View(login_);
        }

        public ActionResult UserNamePrint()
        {
            string result = "";
            string userId = User.Identity.GetUserId();
            ApplicationUser user = UserManager.FindById(userId);

            if (user != null)
            {
                result = user.FirstName + " " + user.LastName;
            }
            return View("_Layout", result);
        }

    }
}