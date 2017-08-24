using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IWCF2
    {
        static UOW.IUOW iuow;
        static Service1()
        {
            try
            {
                iuow = new UOW.UOW();
                iuow.DefaultInitialize(@"SQLDB_J");
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
        }

        public void SetCurrentUser(int id)
        {
            iuow.SetCurrentUser(id);
        }
        public List<Model.SQLmodel.KEY_CLIENTS_SQL> GetKKByUserId()
        {
            IQueryable<Model.SQLmodel.KEY_CLIENTS_SQL> result = null;
            result = (from s in iuow.GetKKByUserId() select s);
            return result.ToList();
        }
        public void InsertKKFromList(List<Model.SQLmodel.KEY_CLIENTS_SQL> items)
        {
            iuow.InsertKKFromList(items);
        }
        public IQueryable<Model.SQLmodel.T_ACQ_M_SQL> GetAcqByDate(DateTime st, DateTime fn)
        {
            IQueryable<Model.SQLmodel.T_ACQ_M_SQL> result = null;
                result = iuow.GetAcqByDate(st, fn);
            return result;
        }
    }

}
