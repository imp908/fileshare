using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Threading.Tasks;

using Northwind.Models.Validation;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;


using System.ComponentModel;


namespace Northwind.Controllers
{
    [Authorize]
    public class EditController : Controller
    {

        private ApplicationUserManager_V UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager_V>();
            }
        }


        // GET: Employee     
        public async Task<ActionResult> Orders(string userID)
        {

            OrderView order = new OrderView();
            DbLevel db = new DbLevel();

            ApplicationUser AppUser = await UserManager.FindByIdAsync(userID);
            if (AppUser == null) { AppUser = UserManager.FindByName(User.Identity.GetUserId()); }
       
            if(AppUser != null)
            {
                order.orders = db.GetOrders(AppUser);
                order.employee = db.GetEmployee(AppUser);
                order.OrderDetails = db.GetOrderDetails(AppUser);
            }else
            {
                ModelState.AddModelError("", "Something gone wrong");
            }

            return View(order);
        }

        [HttpGet]
        public ActionResult Change(int? pid,int? oid)
        {
            ApplicationUser AppUser = UserManager.FindByName(User.Identity.GetUserId());
            DbLevel db = new DbLevel();
            OrderDetail od =
                db.GetOrderDetails(AppUser,pid,oid).FirstOrDefault();

            return View(od);
        }

        [HttpPost,ActionName("Change")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePost(
        [Bind(Include = "ContactName,ProductName,Quantity,UnitPrice,productID,orderID")] OrderDetail od_)
        {

            ApplicationUser AppUser = UserManager.FindByName(User.Identity.GetUserId());
            DbLevel db = new DbLevel();

            try
            {
                db.SetOrderDetail(od_);
            }
            catch(Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            OrderDetail od =
            db.GetOrderDetails(AppUser, od_.productID,od_.orderID).FirstOrDefault();

            return RedirectToAction("Orders","Edit");

        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Increase")]
        public ActionResult Increase(int ID, DateTime? DateFrom, DateTime? DateTo)
        {
            return View();
        }
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Decrease")]
        public ActionResult Decrease(int ID, DateTime? DateFrom, DateTime? DateTo)
        {
            return View();
        }

        public ActionResult GetUserName()
        {
            ApplicationUser AppUser =  UserManager.FindByName(User.Identity.GetUserId());
            string UserName = AppUser.FirstName + @" " + AppUser.LastName;
            return View(UserName);
        }



    }
}