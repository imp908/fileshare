using System;
using System.Collections.Generic;
using System.Linq;

using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq.Expressions;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TGR;

namespace TGR
{
    //>>!!! every method can be used on separate entity, check DbSet initialize composition
    /// <summary>
    /// Truly generic repository
    /// </summary>
    #region TGR
    public interface IEntity { int ID { get; set; } }

    public interface IEntity<T> : IEntity { }
    //public abstract class Entity<T> : IEntity<T> where T : DbContext { public int ID { get; set; } }

    public interface IDate : IEntity { Nullable<System.DateTime> DATE { get; set; } }
    public interface IByDate { IQueryable<T> GetByDate<T>(DateTime st, DateTime fn) where T : class, IDate; }

    public interface IMerchant : IEntity { long? MERCHANT { get; set; } }
    public interface IByMerchant { IQueryable<T> GetByMerchantFilter<T, K>() where T : class, IMerchant where K : class, IMerchant; }

    public interface ISector : IMerchant { int? SECTOR_ID { get; set; } }
    public interface IBySector { void DeleteBySector(int id); }

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

    public interface IRead
    {
        IQueryable<T> GetByFilter<T>(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null) where T : class, IEntity;
    }
    public interface IGetFilter : IByDate, IByMerchant
    {
        IQueryable<T> GetByMerchantFilter<T>() where T : class, IMerchant;
        new IQueryable<T> GetByDate<T>(DateTime st, DateTime fn) where T : class, IDate;
    }

    /// <summary>
    /// Repository to readonly context, get all, get by specific Expression<Func<T, bool>> filter, save and dispose
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadRepo<T> : IRead where T : class, IEntity
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
        , Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null) where T : class, IEntity
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
    public class EditRepo<T> : ReadRepo<T> where T : class, IEntity
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
    public class MerchantFilterRepo<T> : EditRepo<T>, IByMerchant
        where T : class, IMerchant
    {
        public MerchantFilterRepo(DbContext context_) : base(context_)
        {

        }
        public IQueryable<T> GetByMerchantFilter<T, K>()
            where T : class, IMerchant
            where K : class, IMerchant
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

    /// <summary>
    /// Unit of Work implements repositories and contains methods to getall, add entity (IEntity) to repo, save and dispose
    /// can be extended with generic get, add for TGR interfaces
    /// </summary>
    #region UOF

    /*
    /// <summary>
    /// Generic Unit Of work. Receives DbContext uses one of Generic Repo for operations
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

        public void Add<T>(T item_) where T : class, IEntity
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
        public IQueryable<T> GetByMerchants<T,K>() 
            where T : class, IMerchant
            where K : class, IMerchant
        {
            IQueryable<T> _result = null;
            MerchantFilterRepo<T> merchantRepo = new MerchantFilterRepo<T>(ent);
            _result = merchantRepo.GetByMerchantFilter<T,K>();
            return _result;
        }
        public int MerchantListCount()
        {
            int _result = 0;
            MerchantFilterRepo<REFMERCHANTS, REFMERCHANTS> merchantRepo = new MerchantFilterRepo<REFMERCHANTS, REFMERCHANTS>(ent);
            _result = merchantRepo.GetMerchantFilterAmount<REFMERCHANTS>();
            return _result;
        }
        public void DeleteBySector<T>(int id_) where T : class, ISector
        {
            SectorFilterRepo<T> sector = new SectorFilterRepo<T>(ent);
            sector.DeleteBySector(id_);
        }
        public void DeleteByUserID<T>(int id_) where T : class, IUser
        {
            UserFilterRepo<T> repo = new UserFilterRepo<T>(ent);
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
    */

    #endregion

}

namespace tests
{

    
    public static class SQL_entity_test
    {

    }

}


namespace Repo.DAL
{
    public partial class T_ACQ_D : IChainable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
    public partial class T_CTL_D
    {
        [Key]
        public int ID { get; set; }
    }
    public partial class T_ECOMM_D : IEntity
    {
        [Key]
        public int ID { get; set; }
    }
    public partial class T_ECOMM_M
    {
        [Key]
        public int ID { get; set; }
    }

   
    public partial class REFMERCHANTS : IEntity, IUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string ITEM_ID { get; set; }
        public int USER_ID { get; set; }
    }

    public partial class KEY_CLIENTS : IEntity
    {
        //[Key, Column(Order = 1)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }
        public string INDUSTRY_FILE { get; set; }
        public string RESPONSIBILITY_GROUP { get; set; }
        public string RESPONSIBILITY_MANAGER { get; set; }
        public string GROUP_NAME { get; set; }
        public string INDUSTRY { get; set; }
        public string INDUSTRY_SECONDARY { get; set; }
        public string SE_NAME { get; set; }
        public string PHYSICAL_ADDRESS { get; set; }
        public string CITY { get; set; }
        [Required]
        public long? SE_NUMBER { get; set; }
        public string LEGAL_ENTITY { get; set; }
        public string PROVIDER_NAME { get; set; }
        public int? SECTOR_ID { get; set; }
        //[Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime DATE { get; set; }
        //[ForeignKey("SECTOR_ID")]
        //public SECTOR_NAMES SECTOR_NAMES { get; set; }
    }

    [Table("Sectors")]
    public partial class SECTOR
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string SECTOR_NAME { get; set; }

        public virtual ICollection<SECTOR_MASK> SECTOR_MASKS { get; set; }
    }

    [Table("SectorMasks")]
    public partial class SECTOR_MASK
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string MASK { get; set; }

        [ForeignKey("SECTOR")]
        public int SectorID { get; set; }

        //public int SECTOR_NAMEId { get; set; }
        public virtual SECTOR SECTOR { get; set; }
    }

    public partial class T_ACQ_D : IEntity, IChainable
    {
        public Nullable<System.DateTime> DATE { get; set; }
        [Required]
        public long? MERCHANT { get; set; }
    }
    public partial class T_ACQ_M : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
    public partial class KEY_CLIENTS : IEntity, IMerchant, ISector
    {
        [Required]
        public long? MERCHANT { get; set; }
    }
  
    public partial class REFMERCHANTS : IMerchant
    {
        [Required]
        public long? MERCHANT { get; set; }
    }
  
    // for running stored procedures disconnected from current POCO
    public class FakePOCO
    {

    }
}


namespace Repo.DAL.SQL_ent
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SQL_entity : DbContext
    {
        public SQL_entity()
            : base("SQL_entities")
        {
            Database.SetInitializer<SQL_entity>(new DropCreateDatabaseIfModelChanges<SQL_entity>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
        }

        public virtual DbSet<T_ACQ_D> T_ACQ_D { get; set; }
        public virtual DbSet<T_ACQ_M> T_ACQ_M { get; set; }
        public virtual DbSet<T_CTL_D> T_CTL_D { get; set; }
        public virtual DbSet<T_ECOMM_D> T_ECOMM_D { get; set; }
        public virtual DbSet<T_ECOMM_M> T_ECOMM_M { get; set; }

        public virtual DbSet<REFMERCHANTS> REFMERCHANTS { get; set; }
        public virtual DbSet<KEY_CLIENTS> KEY_CLIENTS { get; set; }

        public virtual DbSet<SECTOR> SECTOR_NAMES { get; set; }
        public virtual DbSet<SECTOR_MASK> SECTOR_MASKS { get; set; }

    }
}
