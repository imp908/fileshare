using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Northwind.DAL
{
    public class Accounts
    {
        [ForeignKey("Employee")]
        public int ID { get; set; }   
        public string Login{ get; set; }
        public string PasswordHash { get; set; }

        public virtual Employees Employee { get; set; }
    }
}