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
    public interface IUOW
    {
        void BindRepoUsers(IRepository<USERS_SQL> repository_);
        void BindRepoClients(IRepository<KEY_CLIENTS_SQL> repository_);
        void BindRepoMerchants(IRepository<KEY_CLIENTS_SQL> repository_);
        void BindRepoTACQM(IRepository<KEY_CLIENTS_SQL> repository_);

        void BindContext(DbContext context);
        int GetIDByCredentials(string name_, string sername_);
        void SetCurrentUser(int id_);
        string GetUserSernameByID();

        IQueryable<MERCHANT_LIST_SQL> GetMerchantListByUserId();
        void InsertMerchatList(List<MERCHANT_LIST_SQL> list);


    }

    public class UOW
    {

        IRepository<USERS_SQL> users_repository;
        IRepository<KEY_CLIENTS_SQL> clients_repository;
        IRepository<MERCHANT_LIST_SQL> merchantList_repository;
        IRepository<T_ACQ_M_SQL> acq_repository;

        int currentUserId;

        public void BindRepoUsers(IRepository<USERS_SQL> repository_)
        {
            this.users_repository = repository_;
        }
        public void BindRepoClients(IRepository<KEY_CLIENTS_SQL> repository_)
        {
            this.clients_repository = repository_;
        }
        public void BindRepoMerchants(IRepository<MERCHANT_LIST_SQL> repository_)
        {
            this.merchantList_repository = repository_;
        }
        public void BindRepoTACQM(IRepository<T_ACQ_M_SQL> repository_)
        {
            this.acq_repository = repository_;
        }

        public void BindContext(DbContext context)
        {
            this.users_repository.BindContext(context);
            this.clients_repository.BindContext(context);
            this.merchantList_repository.BindContext(context);
            this.acq_repository.BindContext(context);
        }
        public int GetIDByCredentials(string name_,string sername_)
        {
            int result = 0;
            var a = this.users_repository.GetContext();
            var b = a.Set<USERS_SQL>();
           
            try
            {
                result = (from s in this.users_repository.GetContext().Set<USERS_SQL>()
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
            result = from s in clients_repository.GetContext().Set<MERCHANT_LIST_SQL>() where s.USER_ID == this.currentUserId select s;
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
            var a = from s in acq_repository.GetContext().Set<T_ACQ_M_SQL>()
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
            result = from s in clients_repository.GetContext().Set<KEY_CLIENTS_SQL>() where s.RESPONSIBILITY_MANAGER == this.GetUserSernameByID() select s;
            return result;
        }
        //delete and insert merchants by userid
        public void InsertKKFromList(List<KEY_CLIENTS_SQL> items )
        {
            this.clients_repository.AddFromList(items);
        }

        public void Dispose()
        {
            this.acq_repository.Dispose();
            this.merchantList_repository.Dispose();
            this.clients_repository.Dispose();
            this.users_repository.Dispose();
            this.Dispose();
        }
        
    }
    
    public class GO
    {
        public void GO_()
        {
            DbContext context = new SQLDB_CHANGE(@"SQLDB_J");
            Repository<USERS_SQL> users = new Repository<USERS_SQL>(context);
            int cnt = users.GetALL().Count();
        }
    }
}