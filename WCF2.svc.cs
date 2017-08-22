using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WCF2" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WCF2.svc or WCF2.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class WCF2 : IWCF2
    {
        static UOW.IUOW iuow;
        static WCF2()
        {
            DAL.DAL.SQLDB_CHANGE context = new DAL.DAL.SQLDB_CHANGE(@"SQLDB_J");
            Repo_.Repository<Model.SQLmodel.KEY_CLIENTS_SQL> kkRepo = 
                new Repo_.Repository<Model.SQLmodel.KEY_CLIENTS_SQL>(context);
            Repo_.Repository<Model.SQLmodel.USERS_SQL> userRepo =
                new Repo_.Repository<Model.SQLmodel.USERS_SQL>(context);
            iuow = new UOW.UOW();
            iuow.BindRepoClients(kkRepo);
            iuow.BindRepoUsers(userRepo);
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

    }

}
