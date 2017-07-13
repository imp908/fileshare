using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Linq.Expressions;

using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Repo_
{

    /// <summary>
    /// Trully generic repositories interfaces
    /// </summary>
    #region TGR

    //Interface for default SQL entities
    public interface IEntity { }
    //for entities with int ID
    public interface IEntityInt : IEntity { int ID { get; set; } }
    //for entities with decimal ID
    public interface IEntityDec : IEntity { decimal ID { get; set; } }

    public interface IEntity<T> : IEntityInt { }
    //public abstract class Entity<T> : IEntity<T> where T : DbContext { public int ID { get; set; } }

    public interface IDate : IEntityInt { Nullable<System.DateTime> DATE { get; set; } }
    public interface IByDate { IQueryable<T> GetByDate<T>(DateTime st, DateTime fn) where T : class, IDate; }

    public interface IMerchant : IEntityInt { long? MERCHANT { get; set; } }
    public interface IByMerchant { IQueryable<T> GetByMerchantFilter<T>() where T : class, IMerchant; }

    public interface IUser : IMerchant { int USER_ID { get; set; } }
    public interface IByUser { void DeleteByUserID(int id); }

    public interface IChainable
       : IDate, IMerchant
    {

    }
    public interface IChain
    {
        ChainingRepo<IChainable> FilterByDate(DateTime st, DateTime fn);
        ChainingRepo<IChainable> FilterByMerchants<K>() where K : class, IMerchant;
    }

    public interface IReadRepo<T> where T : class, IEntity
    {
       
        void Save();
        void Dispose(bool disposing_);
        void Dispose();
        IQueryable<T> GetAll();
        IQueryable<T> GetByFilter<T>(Expression<Func<T, bool>> filter = null
        , Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null) where T : class, IEntityInt;

    }
    public interface IEditRepo<T> where  T : class, IEntity
    {
        void AddEntity(T item_);
        T GetById(int ID_);
        void Edit(T item_);
    }

    //Interface for entities with Sector ID
    public interface ISector : IMerchant { int? SECTOR_ID { get; set; } }
    //Interface for sector repo
    public interface IBySector { void DeleteBySector(int id); }   


    /// <summary>
    /// Repository to readonly context, get all, get by specific Expression<Func<T, bool>> filter, save and dispose
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadRepo<T> : IReadRepo<T> where T : class, IEntity
    {

        internal DbContext _context { get; set; }
        internal DbSet<T> _dbSet { get; set; }
        public IQueryable<T> _result { get; set; }

        public ReadRepo(DbContext context_)
        {
            this._context = context_;
            this._dbSet = this._context.Set<T>();
        }

        private bool dispose = false;

        public void Save()
        {
            try
            {
                this._context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var ev in e.EntityValidationErrors)
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
                    this._context.Dispose();
                }
            }
            this.dispose = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IQueryable<T> GetAll() 
        {
            IQueryable<T> result = this._dbSet;
            return result;
        }
        public IQueryable<T> GetByFilter<T>(Expression<Func<T, bool>> filter = null
        , Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null) where T : class, IEntityInt
        {
            IQueryable<T> result = this._context.Set<T>();
            if (filter != null)
            {
                result = result.Where(filter);
            }
            return result;
        }

    }
    /// <summary>
    /// Repository inherited from ReadRepo to addEntity,GetByID and Edit (as atach) entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EditRepo<T> : ReadRepo<T> , IEditRepo<T> where T : class, IEntityInt
    {
        public EditRepo(DbContext context_) : base(context_)
        {

        }
        public void AddEntity(T item_)
        {
            this._dbSet.Add(item_);
        }
        public T GetById(int ID_)
        {
            T result = null;
            if ((from s in this._dbSet where s.ID == ID_ select s).Any())
            {
                result = (from s in this._dbSet where s.ID == ID_ select s).FirstOrDefault();
            }
            return result;
        }
        public void Edit(T item_)
        {
            this._dbSet.Attach(item_);
        }
    }
    /// <summary>
    /// Repository for filtering by merchantlist entity inherited from IMerchant interface, 
    /// with sector_id , which not needed to be chained (no data and merchant simultaneous filter)
    /// >>!!! used for entities with different properties
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SectorFilterRepo<T> : EditRepo<T>, IBySector
        where T : class, ISector
    {
        public SectorFilterRepo(DbContext context_) : base(context_)
        {

        }
        public void DeleteBySector(int id_)
        {
            //this._dbSet.RemoveRange(from s in this._dbSet where s.SECTOR_ID == id_ select s );
            foreach (var item_ in this._dbSet)
            {
                if (item_.SECTOR_ID != null)
                {
                    if (item_.SECTOR_ID.Value == id_)
                    {
                        this._dbSet.Remove(item_);
                    }
                }
            }
        }
    }
    public class UserFilterRepo<T> : EditRepo<T>, IByUser
        where T : class, IUser
    {
        public UserFilterRepo(DbContext context_) : base(context_)
        {

        }
        public void DeleteByUserID(int id_)
        {
            foreach (var item_ in this._dbSet.Where(s => s.USER_ID == id_))
            {
                this._dbSet.Remove(item_);
            }
        }
    }
    /// <summary>
    /// Repository for entity with merchant, filtering by refmerchants and merchant count
    /// compares K merchants in T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MerchantFilterRepo<T, K> : EditRepo<T>, IByMerchant
        where T : class, IMerchant
        where K : class, IMerchant
    {
        public MerchantFilterRepo(DbContext context_) : base(context_)
        {

        }
        public IQueryable<T> GetByMerchantFilter<T>() where T : class, IMerchant
        {
            IQueryable<T> result = null;
            IDbSet<K> merchant_entity = this._context.Set<K>();
            var merchs = from s in merchant_entity select s;
            DbSet<T> filtering_entity = this._context.Set<T>();
            result = from s in filtering_entity join k in merchs on s.MERCHANT equals k.MERCHANT select s;
            return result;
        }
        public int GetMerchantFilterAmount<T>() where T : class, IMerchant
        {
            int result_ = 0;
            IDbSet<T> merchant_entity = this._context.Set<T>();
            result_ = (from s in merchant_entity select s).Count();
            return result_;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DateFilterRepo<T> : EditRepo<T>
        where T : class, IDate
    {
        public DateFilterRepo(DbContext context_) : base(context_)
        {

        }
        public IQueryable<T> GetByDate(DateTime st, DateTime fn)
        {
            IQueryable<T> result = _context.Set<T>().Where(s => s.DATE >= st && s.DATE <= fn);
            return result;
        }
    }
    /// <summary>
    /// Repository for entities with chaining, entities with date and merchants fields
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChainingRepo<T> : EditRepo<T>, IChain where T : class, IChainable
    {
        public ChainingRepo(DbContext context_) : base(context_)
        {

        }

        public ChainingRepo<T> FilterByExpression(Expression<Func<T, bool>> filter_)
        {
            if (this._dbSet != null)
            {
                if (this._result == null)
                {
                    this._result = this._dbSet;
                }
                try
                {
                    this._result = this._result.Where(filter_);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                }
            }
            return this;
        }
        public new ChainingRepo<IChainable> GetAll()
        {
            ChainingRepo<IChainable> _result = new ChainingRepo<IChainable>(this._context);
            if (this._result == null)
            {
                this._result = from s in this._dbSet select s;
            }
            _result._result = from s in this._result select s;
            return _result;
        }
        public ChainingRepo<IChainable> FilterByDate(DateTime st, DateTime fn)
        {
            ChainingRepo<IChainable> _result = new ChainingRepo<IChainable>(this._context);
            if (this._result == null)
            {
                this._result = from s in this._dbSet where s.DATE >= st && s.DATE <= fn select s;
            }
            _result._result = from s in this._result where s.DATE >= st && s.DATE <= fn select s;
            return _result;
        }
        public ChainingRepo<IChainable> FilterByMerchants<K>() where K : class, IMerchant
        {
            ChainingRepo<IChainable> _result = new ChainingRepo<IChainable>(this._context);
            DbSet<K> filter_mercahnt = this._context.Set<K>();
            if (this._result == null)
            {
                this._result = from s in this._dbSet where s.MERCHANT == 929 select s;
            }
            _result._result = from s in this._result join t in filter_mercahnt on s.MERCHANT equals t.MERCHANT select s;
            return _result;
        }
    }
    #endregion

}
