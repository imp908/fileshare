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
        void BindRepoMerchants(IRepository<MERCHANT_LIST_SQL> repository_);
        void BindRepoTACQM(IRepository<T_ACQ_M_SQL> repository_);

        void BindContext(DbContext context);

        int GetIDByCredentials(string name_, string sername_);
        void SetCurrentUser(int id_);
        string GetCurrentUserSername();

        IQueryable<MERCHANT_LIST_SQL> GetMerchantListByUserId();
        void InsertMerchatList(List<MERCHANT_LIST_SQL> list);

        IQueryable<T_ACQ_M_SQL> GetAcqByDate(DateTime st, DateTime fn);
        IQueryable<T_ACQ_M_SQL> GetAcqByMerchants(List<MERCHANT_LIST_SQL> list);

        IQueryable<KEY_CLIENTS_SQL> GetKKByUserId();
        void InsertKKFromList(List<KEY_CLIENTS_SQL> items);
    }

    public class UOW : IUOW
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
        public string GetCurrentUserSername()
        {
            string result = string.Empty;
            result = this.users_repository.GetByID(this.currentUserId).Sername;
            return result;
        }

        public IQueryable<MERCHANT_LIST_SQL> GetMerchantListByUserId() 
        {
            IQueryable<MERCHANT_LIST_SQL> result = null;
            result = from s in clients_repository.GetContext().Set<MERCHANT_LIST_SQL>()
                     where s.USER_ID == this.currentUserId select s;
            return result;
        }
        public void InsertMerchatList(List<MERCHANT_LIST_SQL> list)
        {            
            merchantList_repository.DeleteByList(list);
            merchantList_repository.AddFromList(list);
            merchantList_repository.Save();
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
            string sername = this.GetCurrentUserSername();
            if (sername != string.Empty)
            {
                result = from s in clients_repository.GetContext().Set<KEY_CLIENTS_SQL>() where s.RESPONSIBILITY_MANAGER == sername select s;
            }
            return result;
        }

        //delete and insert merchants by userid
        public void InsertKKFromList(List<KEY_CLIENTS_SQL> items )
        {
            this.clients_repository.AddFromList(items);
        }

        public void Dispose()
        {
            if (this.acq_repository != null)
            {
                this.acq_repository.Dispose();
            }
            if (this.merchantList_repository != null)
            {
                this.merchantList_repository.Dispose();
            }
            if (this.clients_repository != null)
            {
                this.clients_repository.Dispose();
            }
            if (this.users_repository != null)
            {
                this.users_repository.Dispose();
            }                        
        }
        
    }
    
    public class GO
    {
        public void GO_()
        {
            Random rnd = new Random();

            List<KEY_CLIENTS_SQL> clients = new List<KEY_CLIENTS_SQL>();
            IList<REFMERCHANTS_SQL> rm = new List<REFMERCHANTS_SQL>();

            List<MERCHANT_LIST_SQL> merchantsToInsert = new List<MERCHANT_LIST_SQL>() {
new MERCHANT_LIST_SQL() { MERCHANT = 9290000090, USER_ID = 3, UPDATE_DATE = new DateTime(2017, 08, 05, 00, 00, 08) }
,new MERCHANT_LIST_SQL() { MERCHANT = 9290000091, USER_ID = 3, UPDATE_DATE = new DateTime(2017, 08, 05, 00, 00, 08) }
,new MERCHANT_LIST_SQL() { MERCHANT = 9290000081, USER_ID = 4, UPDATE_DATE = new DateTime(2017, 08, 05, 00, 00, 08) }
,new MERCHANT_LIST_SQL() { MERCHANT = 9290000082, USER_ID = 4, UPDATE_DATE = new DateTime(2017, 08, 05, 00, 00, 08) }
,new MERCHANT_LIST_SQL() { MERCHANT = 9290000083, USER_ID = 4, UPDATE_DATE = new DateTime(2017, 08, 05, 00, 00, 08) }
            };

            List<MERCHANT_LIST_SQL> merchantsGen = new List<MERCHANT_LIST_SQL>();

            for (long i = 9290000100; i< (9290000100+1000); i++)
            {              
                merchantsGen.Add(new MERCHANT_LIST_SQL { MERCHANT=i, USER_ID=rnd.Next(4, 7) , UPDATE_DATE= DateTime.Now});
            }

            DbContext context = new SQLDB_CHANGE(@"SQLDB_J");
            Repository<MERCHANT_LIST_SQL> repo = new Repository<MERCHANT_LIST_SQL>(context);

            int cnt = repo.GetALL().Count();

            repo.DeleteByList((from s in repo.GetALL()
                               where s.UPDATE_DATE > new DateTime(2017, 08, 05, 00, 00, 08) select s).ToList());
            repo.Save();
            cnt = repo.GetALL().Count();
         
            repo.AddFromList(merchantsGen);
            repo.Save();
            cnt = repo.GetALL().Count();

            repo.DeleteByList(merchantsGen);
            repo.Save();
            cnt = repo.GetALL().Count();

            repo.AddFromList(merchantsToInsert);
            repo.Save();
            cnt = repo.GetALL().Count();

            repo.DeleteByList(merchantsToInsert);
            repo.Save();
            cnt = repo.GetALL().Count();

        }
    }
}