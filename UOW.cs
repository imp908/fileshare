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

    /// <summary>
    /// Unit of Work. Initializes Repositories with concrete entities
    /// Implements repositories and contains methods to getall, add entity (IEntity) to repo, save and dispose
    /// can be extended with generic get, add for TGR interfaces
    /// </summary>
    #region UOF
    public class UnitOfWorkConcrete
    {
        SQLDB_INIT ent;
        EditRepo<T_ACQ_M_SQL> ACQ_M;
        UserFilterRepo<REFMERCHANTS_SQL> REFMERCHANTS;
        SectorFilterRepo<KEY_CLIENTS_SQL> KK;

        public UnitOfWorkConcrete()
        {
            ent = new SQLDB_INIT();
            ACQ_M = new EditRepo<T_ACQ_M_SQL>(ent);
            REFMERCHANTS = new UserFilterRepo<REFMERCHANTS_SQL>(ent);
            KK = new SectorFilterRepo<KEY_CLIENTS_SQL>(ent);
        }

        ~UnitOfWorkConcrete()
        {
            this.Dispose();
        }

        //>>!!! to read UOF
        public IQueryable<T_ACQ_M_SQL> GetAllACQ_D()
        {
            IQueryable<T_ACQ_M_SQL> result_ = null;
            result_ = ACQ_M.GetAll<T_ACQ_M_SQL>();
            return result_;
        }
        public IQueryable<KEY_CLIENTS_SQL> GetAllKK()
        {
            IQueryable<KEY_CLIENTS_SQL> result_ = null;
            result_ = KK.GetAll<KEY_CLIENTS_SQL>();
            return result_;
        }

        public IQueryable<IMerchant> GetKeyClientsMerchants()
        {
            IQueryable<IMerchant> result_ = null;
            result_ = KK.GetAll<KEY_CLIENTS_SQL>();
            return result_;
        }
        public IQueryable<IMerchant> FilterByMerchant<T>() where T : class, IMerchant
        {
            IQueryable<IMerchant> result_ = null;
            MerchantFilterRepo<T, REFMERCHANTS_SQL> MerchantFilter = new MerchantFilterRepo<T, REFMERCHANTS_SQL>(ent);
            result_ = MerchantFilter.GetByMerchantFilter<T>();
            return result_;
        }
        public int GetMerchantFilterAmount()
        {
            int result_ = 0;
            MerchantFilterRepo<KEY_CLIENTS_SQL, REFMERCHANTS_SQL> MerchantFilter = new MerchantFilterRepo<KEY_CLIENTS_SQL, REFMERCHANTS_SQL>(ent);
            result_ = MerchantFilter.GetMerchantFilterAmount<KEY_CLIENTS_SQL>();
            return result_;
        }

        public void AddACQ_D(IEntityInt entity_)
        {
            ACQ_M.AddEntity((T_ACQ_M_SQL)entity_);
        }

        public void AddKK(IEntityInt entity_)
        {
            KK.AddEntity((KEY_CLIENTS_SQL)entity_);
        }
        public void DeleteBySectorID(int id_)
        {
            KK.DeleteBySector(id_);
        }

        public void AddREF(IEntityInt entity_)
        {
            REFMERCHANTS.AddEntity((REFMERCHANTS_SQL)entity_);
        }
        public void DeleteByUserID(int id_)
        {
            REFMERCHANTS.DeleteByUserID(id_);
        }

        private bool dispose = false;

        public void SaveAll()
        {
            try
            {
                ent.SaveChanges();
            }
            catch (Exception e)
            {
                foreach (var ev in e.Message)
                {

                }
            }
        }

        public void Dispose(bool disposing_)
        {
            if (!this.dispose)
            {
                if (disposing_)
                {
                    KK.Dispose();
                    ACQ_M.Dispose();
                }
            }
            this.dispose = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

    /// <summary>
    /// Generic Unit Of work. Tight coupled to Repositories for IEntity Int
    /// Receives DbContext uses one of Generic Repo for operations
    /// Contains Generic and Item specific methods
    /// </summary>
    public class UnitOfWorkGeneric
    {
        DbContext ent;

        public UnitOfWorkGeneric(DbContext input_)
        {
            ent = input_;
        }

        ~UnitOfWorkGeneric()
        {
            this.Dispose();
        }

        public IQueryable<T> GetAll<T>() where T : class, IEntityInt
        {
            IQueryable<T> result = null;
            ReadRepo<T> read = new ReadRepo<T>(ent);
            result = read.GetAll<T>();
            return result;
        }

        public void Add<T>(T item_) where T : class, IEntityInt
        {
            EditRepo<T> et = new EditRepo<T>(ent);
            et.AddEntity(item_);
            et.Save();
        }
        public IEnumerable<T> GetByDate<T>(DateTime dateStart, DateTime dateFinal) where T : class, IDate
        {
            IEnumerable<T> _result = null;
            DateFilterRepo<T> rr = new DateFilterRepo<T>(ent);
            _result = rr.GetByDate(dateStart, dateFinal).ToList();
            return _result;
        }
        //bind K of REFMERCHANTS with method
        public IQueryable<T> GetByMerchants<T>() where T : class, IMerchant
        {
            IQueryable<T> _result = null;
            MerchantFilterRepo<T, REFMERCHANTS_SQL> merchantRepo = new MerchantFilterRepo<T, REFMERCHANTS_SQL>(ent);
            _result = merchantRepo.GetByMerchantFilter<T>();
            return _result;
        }
        public int MerchantListCount()
        {
            int _result = 0;
            MerchantFilterRepo<REFMERCHANTS_SQL, REFMERCHANTS_SQL> merchantRepo = new MerchantFilterRepo<REFMERCHANTS_SQL, REFMERCHANTS_SQL>(ent);
            _result = merchantRepo.GetMerchantFilterAmount<REFMERCHANTS_SQL>();
            return _result;
        }
        public void DeleteBySector(int id_)
        {
            SectorFilterRepo<KEY_CLIENTS_SQL> sector = new SectorFilterRepo<KEY_CLIENTS_SQL>(ent);
            sector.DeleteBySector(id_);
        }
        public void DeleteByUserID(int id_)
        {
            UserFilterRepo<REFMERCHANTS_SQL> repo = new UserFilterRepo<REFMERCHANTS_SQL>(ent);
            repo.DeleteByUserID(id_);
        }

        public void RefreshValues()
        {
            //var a = ent.Database.Connection.CreateCommand();
            var c = ent.Database.SqlQuery<FakePOCO>(@"VALUES_INSERT").ToArray();
        }

        private bool dispose = false;

        public void SaveAll()
        {
            try
            {
                ent.SaveChanges();
            }
            catch (Exception e)
            {
                foreach (var ev in e.Message)
                {

                }
            }
        }
        public void Dispose(bool disposing_)
        {
            if (!this.dispose)
            {
                if (disposing_)
                {
                    ent.Dispose();
                }
            }
            this.dispose = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// Unit of work generic
    /// Decoupled from repositories with interfaces
    /// cant use every entity which implements Ientity
    /// only GetAll() and Add() methods implemented
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UnitOfWorkDecoupled<T> where T : class, IEntity
    {
        DbContext context;
        IReadRepo<T> readrepo;
        IEditRepo<T> editRepo;

        public UnitOfWorkDecoupled(DbContext context_)
        {
            this.context = context_;
        }
        public void RepoBind(IReadRepo<T> input_)
        {
            this.readrepo = input_;
        }
        public void RepoBind(IEditRepo<T> input_)
        {
            this.editRepo = input_;
        }
        public void ChangeContext(DbContext context_)
        {
            this.context = context_;
        }
        public IQueryable<T> GetAll()
        {
            IQueryable<T> result = null;
            result = this.readrepo.GetAll<T>();
            return result;
        }
        public void Add(T item_)
        {
            editRepo.AddEntity(item_);
        }

        public void Save()
        {
            this.context.SaveChanges();
        }
    }

    public class UOWDB<T> : UnitOfWorkDecoupled<T> where T : class, IEntity
    {
        public UOWDB(DbContext context_) : base(context_)
        {
                       
        }
        
    }

    #endregion  

}
