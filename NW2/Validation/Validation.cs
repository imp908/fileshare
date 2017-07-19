using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

using Northwind.DAL;

namespace Northwind.Models.Validation
{

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ApplicationUser()
        {
        }
    }

 
    public class ApplicationUserManager_V : UserManager<ApplicationUser>
    {
        public ApplicationUserManager_V(IUserStore<ApplicationUser> store)
                : base(store)
        {
        }
        public static ApplicationUserManager_V Create(IdentityFactoryOptions<ApplicationUserManager_V> options,
                                                IOwinContext context)
        {
            NorthwindModel db = context.Get<NorthwindModel>();
            ApplicationUserManager_V manager = new ApplicationUserManager_V(new UserStore<ApplicationUser>(db));
            return manager;
        }

        
    }
        
    public class Register
    {
        public virtual Employees employee { get; set; }
        public int employeeID { get; set; }

        [Key]
        public int ID { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage =@"Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }      

        [Required]
        [Compare("Password", ErrorMessage = @"Passwords not match ")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required(ErrorMessage =@"Enter your First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = @"Enter your Last name")]
        public string LastName { get; set; }

        public string Message { get; set; }
      
    }

    public class Login
    {
      
        [Key]
        public int ID { get; set; }
        [EmailAddress]
        [Required(ErrorMessage =@"Enter your email")]
        public string Email { get; set; }
        [Required(ErrorMessage = @"Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
        [Required]
        [Compare("Password", ErrorMessage = @"Passwords not match ")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }        

        public string Message { get; set; }
    }

    public class DbLevel
    {
        DbContext db;

        public DbLevel()
        {
            db = new NorthwindModel();
        }

        public Employees GetEmployee(int EmployeID_)
        {
            if ((from s in db.Set<Employees>() where s.EmployeeID == EmployeID_ select s).Any())
            {
                return (from s in db.Set<Employees>() where s.EmployeeID == EmployeID_ select s).FirstOrDefault();
            }

            return null;
        }
        public Employees GetEmployee(Register register_)
        {
            if ((from s in db.Set<Employees>()
                 where s.FirstName == register_.FirstName && s.LastName == register_.LastName
                 select s).Any())
            {
                return (from s in db.Set<Employees>()
                        where s.FirstName == register_.FirstName && s.LastName == register_.LastName
                        select s).FirstOrDefault();
            }

            return null;
        }
        public Employees GetEmployee(ApplicationUser register_)
        {
            if ((from s in db.Set<Employees>()
                 where s.FirstName == register_.FirstName && s.LastName == register_.LastName
                 select s).Any())
            {
                return (from s in db.Set<Employees>()
                        where s.FirstName == register_.FirstName && s.LastName == register_.LastName
                        select s).FirstOrDefault();
            }

            return null;
        }

        public IQueryable<Order> GetOrders (ApplicationUser user)
        {
            IQueryable<Order> orders = null;
            int emplId = this.GetEmployee(user).EmployeeID;

            orders =
                (from s in db.Set<Order>().Include(i => i.Order_Details)                 
                select s).Where(t=>t.EmployeeID == emplId);            
            return orders;
        }
        public List<OrderDetail> GetOrderDetails(ApplicationUser user)
        {
            int emplId = this.GetEmployee(user).EmployeeID;

            var orderdetails =
                from s in db.Set<Product>()
                join t in db.Set<Order_Detail>() on s.ProductID equals t.ProductID
                join t2 in db.Set<Order>() on t.OrderID equals t2.OrderID
                join t3 in db.Set<Employees>() on t2.EmployeeID equals t3.EmployeeID
                where t3.EmployeeID == emplId
                select new
                {
                   s.ProductID,t2.OrderID, s.ProductName,t.Quantity,t.UnitPrice,t2.Customer.ContactName                    
                }
                ;

            List<OrderDetail> view = new List<OrderDetail>();
            foreach (var item in orderdetails)
            {
                view.Add(new OrderDetail
                {
                    productID = item.ProductID,
                    orderID = item.OrderID, 
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    ContactName = item.ContactName
                });
            }

            return view;
        }
        public List<OrderDetail> GetOrderDetails(ApplicationUser user,int? pid,int? oid)
        {
            int emplId = this.GetEmployee(user).EmployeeID;

            var orderdetails =
                from s in db.Set<Product>()
                join t in db.Set<Order_Detail>() on s.ProductID equals t.ProductID
                join t2 in db.Set<Order>() on t.OrderID equals t2.OrderID
                join t3 in db.Set<Employees>() on t2.EmployeeID equals t3.EmployeeID
                where t3.EmployeeID == emplId && t.ProductID == pid && t2.OrderID == oid
                select new
                {
                    s.ProductID,
                    t2.OrderID,
                    s.ProductName,
                    t.Quantity,
                    t.UnitPrice,
                    t2.Customer.ContactName
                }
                ;

            List<OrderDetail> view = new List<OrderDetail>();
            foreach (var item in orderdetails)
            {
                view.Add(new OrderDetail
                {
                    productID = item.ProductID,
                    orderID = item.OrderID,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    ContactName = item.ContactName
                });
            }

            return view;
        }
        public Order_Detail SetOrderDetail(OrderDetail od_)
        {
            Order_Detail result = null;

            result =
            (from s in this.db.Set<Order_Detail>()
             where s.ProductID == od_.productID && s.OrderID == od_.orderID select s).FirstOrDefault();
            result.Quantity = od_.Quantity;

            this.db.SaveChanges();

            return result;
        }

    }

    public class OrderDetail
    {
        public int productID { get; set; }
        public int orderID { get; set; }
        public string ProductName { get; set; }
        public short Quantity { get; set; }
        [Range(0,int.MaxValue,ErrorMessage =@"Please enter positive value")]
        public decimal UnitPrice { get; set; }
        public string ContactName { get; set; }
    }
    public class OrderView
    {
        public Order order { get; set; }
        public Employees employee { get; set; }
        public OrderDetail orderDetail { get; set; }

        public IQueryable<Order> orders { get; set; }
        public IQueryable<Product> products { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }

    //custom attribute for action to multiple submit buttons
    public class MultipleButtonAttribute : System.Web.Mvc.ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(System.Web.Mvc.ControllerContext controllerContext, string actionName, System.Reflection.MethodInfo methodInfo)
        {
            bool isValidName = false;
            string keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
            {
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }

}