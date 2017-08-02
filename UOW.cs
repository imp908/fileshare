using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Data.Entity;

using Repo_;
using Model.SQLmodel;
using DAL.DAL;


namespace UOW
{
    
    public class UOW
    {

        Repository<USERS_SQL> users_repository = new Repository<USERS_SQL>();
        Repository<KEY_CLIENTS_SQL> clients_repository = new Repository<KEY_CLIENTS_SQL>();
        Repository<MERCHANT_LIST_SQL> merchantList_repository = new Repository<MERCHANT_LIST_SQL>();
        Repository<T_ACQ_M_SQL> acq_repository = new Repository<T_ACQ_M_SQL>();

        int currentUserId;

        public void BindContext(DbContext context)
        {
            this.users_repository.BindContext(context);
        }
        public int GetIDByCredentials(string name_,string sername_)
        {
            int result = 0;
            try
            {
                result = (from s in this.users_repository._context.Set<USERS_SQL>()
                          where s.Name == name_ && s.Sername == sername_
                          select s.ID).FirstOrDefault();              
            }catch (Exception e )
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            return result;
        }
        public void SetCurrentUser(int id_)
        {
            this.currentUserId = id_;
        }        
        public string GetUserSernameByID()
        {
            string result = string.Empty;
            var a = (from s in this.users_repository.GetByID(this.currentUserId).Sername select s).FirstOrDefault() ;
            return result;
        }

        public IQueryable<MERCHANT_LIST_SQL> GetMerchantListByUserId() 
        {
            IQueryable<MERCHANT_LIST_SQL> result = null;
            result = from s in clients_repository._context.Set<MERCHANT_LIST_SQL>() where s.USER_ID == this.currentUserId select s;
            return result;
        }
        public void InsertMerchatList(List<MERCHANT_LIST_SQL> list)
        {
            merchantList_repository.DeleteByList(list.AsQueryable());
            merchantList_repository.AddFromList(list);
        }

        public IQueryable<T_ACQ_M_SQL> GetAcqByDate(DateTime st,DateTime fn)
        {
            IQueryable<T_ACQ_M_SQL> result = null;
            var a = from s in acq_repository._context.Set<T_ACQ_M_SQL>()
                    where s.DATE >= st && s.DATE <= fn
                    select s;
            return result;
        }
        public IQueryable<T_ACQ_M_SQL> GetAcqByMerchants(List<MERCHANT_LIST_SQL> list)
        {
            IQueryable<T_ACQ_M_SQL> result = null;
            result =
                from s in acq_repository.GetALL()
                join s0 in list on s.MERCHANT equals s0.MERCHANT
                select s;
            return result;
        }

        public IQueryable<KEY_CLIENTS_SQL> GetKKByUserId()
        {
            IQueryable<KEY_CLIENTS_SQL> result = null;
            result = from s in clients_repository._context.Set<KEY_CLIENTS_SQL>() where s.RESPONSIBILITY_MANAGER == this.GetUserSernameByID() select s;
            return result;
        }
        // delete and insert merchants by userid
        public void InsertKKFromList(List<KEY_CLIENTS_SQL> items )
        {
            this.clients_repository.AddFromList(items);
        }

       
    }
  
}