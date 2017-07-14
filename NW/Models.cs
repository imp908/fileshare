using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using rn = System.Security.Cryptography;

using Northwind.DAL;
namespace Northwind.Models
{
    public class Models
    {
    }

    public class LoginModel
    {
        [Key]
        [Column(@"EmployeeID")]
        int ID { get; set; }
   
        public string Password { get; set; }

        public string Login { get; set; }
    }
    public class RegisterModel : LoginModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public static class Validations
    {
        static ModelNorthwind db;
        static RegisterModel rm;
        static LoginModel lm;

        public static void ContextBind(ModelNorthwind db_)
        {
            db = db_;
        }
        public static void LoginBind(LoginModel lm_)
        {
            lm = lm_;
        }
        public static void RegisterBind(RegisterModel rm_)
        {
            rm = rm_;
        }

        public static int? AccountCheck()
        {

            int? result = null;

            if ((from s in db.Employees
                 where (s.FirstName ==
lm.Login && s.LastName == lm.Password)
                 select s).Any())
            {
                result = (from s in db.Accounts
                          where (s.Login ==
                            lm.Login && s.PasswordHash == lm.Password)
                          select s).FirstOrDefault().Employee.EmployeeID;
            }

            return result;
        }
        public static bool EmployeeCheck()
        {
            bool result = false;
            if ((from s in db.Employees
                 where s.FirstName == rm.FirstName
                 && s.LastName == rm.LastName
                 select s).Any())
            {
                result = true;
            }

            return result;
        }
        public static void AccountAdd()
        {

            Employees employee = (from s in db.Employees
                             where s.FirstName == rm.FirstName &&
                             s.LastName == rm.LastName
                             select s).FirstOrDefault();

            Accounts account = new Accounts() { Login = rm.Login, PasswordHash = rm.Password, Employee = employee };
            try
            {
                db.Accounts.Add(account);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
        }

    }

}