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

    //for entities with int ID
    public interface IEntityInt { int ID { get; set; } }

    public interface IRepository<T> where T : class, IEntityInt
    {
        void BindContext(DbContext context);
        DbContext GetContext();

        void Add(T item);
        IQueryable<T> GetALL();
        IQueryable<T> GetTOP10();
        T GetByID(int id_);
        void AddFromList(List<T> items);
        IQueryable<T> GetByList(List<T> items);
        void DeleteByID(int id_);
        void DeleteByList(List<T> items);
        IQueryable<T> GetByFilter(Expression<Func<T, bool>> expession);
        void Save();

        void Dispose();
    }

    public class Repository<T> : IRepository<T> where T : class, IEntityInt
    {
        bool disposed = false;

        public DbContext _context;

        public Repository ()
        {

        }
        public Repository(DbContext context_)
        {
            this._context = context_;
        }

        public DbContext GetContext()
        {
            DbContext result = null;
            if (this._context != null)
            {
                result = this._context;
            }
            return result;
        }

        public void BindContext(DbContext context)
        {
            this._context = context;
        }
        public void Add(T item)
        {
            this._context.Set<T>().Add(item);
        }
        public IQueryable<T> GetALL()
        {
            IQueryable<T> result = null;
                result = from s in this._context.Set<T>() select s;
            return result;
        }
        public IQueryable<T> GetTOP10()
        {
            IQueryable<T> result = null;
            result = ( from s in this._context.Set<T>() select  s).Take(10) ;
            return result;
        }
        public T GetByID(int id_)
        {
            T result = null;
            result = (from s in this._context.Set<T>() where s.ID == id_ select s).FirstOrDefault();
            return result;
        }
        public void AddFromList(List<T> items)
        {
            this._context.Set<T>().AddRange(items);
        }
        public IQueryable<T> GetByList(List<T> items)
        {
            IQueryable<T> result = null;            
            List<T> list = (from s in this._context.Set<T>() select s).ToList();
            result = (from s in list select s).Where(t => (from s2 in items select s2.ID).Contains(t.ID)).AsQueryable();
            return result;
        }
        public void DeleteByID(int id_)
        {
            this._context.Set<T>().Remove((from s in this._context.Set<T>() where s.ID == id_ select s).FirstOrDefault());
        }
        public void DeleteByList(List<T> items)
        {
            List<int> itemsL = (from s in items select s.ID).ToList();
            var toDelete = (from c in this._context.Set<T>().Where(c => itemsL.Contains(c.ID)) select c);

            try
            {
                if (toDelete.Count() > 0)
                {               
                    this._context.Set<T>().RemoveRange(toDelete);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            

        }
        public IQueryable<T> GetByFilter(Expression<Func<T, bool>> expession)
        {
            IQueryable<T> result = null;
            result = from s in this._context.Set<T>().Where(expession) select s;
            return result;
        }
        public void Save()
        {
            this._context.SaveChanges();
        }

        protected virtual void Dispose(bool value)
        {
            if (!this.disposed)
            {
                if (value)
                {
                    this._context.Dispose();
                  
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

    #region CustomExceptions
    public class EmptyRepository : System.Exception
    {
        public EmptyRepository() : base()
        {

        }
        public EmptyRepository(string input) : base(input)
        {

        }
    }
    public class EmptyContext : System.Exception
    {
        public EmptyContext() : base()
        {

        }
        public EmptyContext(string input) : base(input)
        {

        }
    }
    public class WrongRepositoryType : System.Exception
    {
        public WrongRepositoryType() : base()
        {

        }
        public WrongRepositoryType(string input) : base(input)
        {

        }
    }
    #endregion

}
