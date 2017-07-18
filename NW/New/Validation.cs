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

    public class Register : Login
    {
        public virtual Employees employee { get; set; }
        public int employeeID { get; set; }       

        [Required(ErrorMessage = @"Enter your First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = @"Enter your Last name")]
        public string LastName { get; set; }
             
    }

    public class Login 
    {
      
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = @"Enter your email")]
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
            if((from s in db.Set<Employees>() where s.EmployeeID == EmployeID_ select s).Any())
            {
                return (from s in db.Set<Employees>() where s.EmployeeID == EmployeID_ select s).FirstOrDefault();
            }

            return null;
        }
        public Employees GetEmployee(Register register_)
        {
            if ((from s in db.Set<Employees>()
            where s.FirstName == register_.FirstName && s.LastName == register_.LastName select s).Any())
            {
                return (from s in db.Set<Employees>()
                    where s.FirstName == register_.FirstName && s.LastName == register_.LastName select s).FirstOrDefault();
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

    }
}