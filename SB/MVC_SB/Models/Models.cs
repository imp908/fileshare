using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.ComponentModel.DataAnnotations;

namespace MVC_SB.Models
{
    public class Models
    {
    }

    #region Multiform
    public class MultiFormModel
    {
        public IEnumerable<MultiFormModel> formsList;       
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? dateFrom { get; set; }
        public DateTime? dateTo { get; set; }
        public Statuses? status { get; set; }

        public void InitializeTest()
        {
            this.formsList =
               new List<MultiFormModel> {
                    new MultiFormModel { ID = 0, Name = "Name1", dateFrom = DateTime.Now, dateTo = DateTime.Now , status=null},
                    new MultiFormModel { ID = 1, Name = "Name2", dateFrom = DateTime.Now.AddDays(-3), dateTo = DateTime.Now.AddDays(-2) ,status=null},
                    new MultiFormModel { ID = 2, Name = "Name3", dateFrom = DateTime.Now.AddDays(-4), dateTo = DateTime.Now.AddDays(-5) ,status=null}
               };
        }
    }
    public enum Statuses { STARTED,STOPED};
   
    #endregion

}