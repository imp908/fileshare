
namespace mvccoresb.Infrastructure.EF
{

    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    using mvccoresb.Domain.TestModels;
    using mvccoresb.Domain.GeoModel;

    using System;
    
    using System.Threading.Tasks;

    using System.Linq;

    using System.Linq.Expressions;
    
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    using mvccoresb.Domain.Interfaces;    
    using mvccoresb.Domain.TestModels;

    using Newtonsoft;

    public class RepositoryEF : IRepository
    {
        DbContext _context;

        public RepositoryEF(DbContext context){
            _context=context;
        }
        
        public void Add<T> (T item)
            where T : class
        {
            this._context.Set<T>().Add(item);            
        }

        public Task<EntityEntry<T>> AddAsync<T>(T item)
           where T : class
        {
            return this._context.Set<T>().AddAsync(item);
        }

        public void AddRange<T>(IList<T> items)
            where T : class
        {
            this._context.Set<T>().AddRange(items);
        }

        public Task AddRangeAsync<T>(IList<T> items)
                where T : class
        {
            return this._context.Set<T>().AddRangeAsync(items);
        }

        public void Delete<T>(T item)
           where T : class
        {
            this._context.Set<T>().Remove(item);
        }

        public void DeleteRange<T>(IList<T> items)
            where T : class
        {
            this._context.Set<T>().RemoveRange(items);
        }

        public void Update<T>(T item)
            where T : class
        {
            this._context.Set<T>().Update(item);
        }

        public void UpdateRange<T>(IList<T> items)
            where T : class
        {
            this._context.Set<T>().UpdateRange(items);
        }
    
        public IQueryable<T> QueryByFilter<T>(Expression<Func<T,bool>> expression) 
            where T : class
        {            
            return this._context.Set<T>().Where(expression);
        }

        public void Save(){
            this._context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return this._context.SaveChangesAsync();
        }
    }
    
    public class UOW : IUOW
    {
        internal IRepository _repository;

        public UOW(IRepository repository){
            this._repository = repository;
        }

        public T Add<T>(T item)
            where T: class
        {
            this._repository.Add(item);
            this._repository.Save();
            return item;
        }
        public void AddRange<T>(IList<T> items)
            where T : class
        {
            this._repository.Add(items);
            this._repository.Save();
        }

        public void Delete<T>(T item)
            where T : class
        {
            this._repository.Delete(item);
            this._repository.Save();
        }
        public void DeleteRange<T>(IList<T> items)
            where T : class
        {
            this._repository.DeleteRange(items);
            this._repository.Save();
        }

        public T Update<T>(T item)
            where T : class
        {
            this._repository.Update(item);
            this._repository.Save();
            return item;
        }
        public void UpdateRange<T>(IList<T> items)
            where T : class
        {
            this._repository.UpdateRange(items);
            this._repository.Save();
        }

        public IQueryable<T> QueryByFilter<T>(Expression<Func<T,bool>> expression)
            where T : class
        {
            return this._repository.QueryByFilter(expression);
        }
    }

    public class UOWblogs : UOW, IUOWBlogging
    {
        public UOWblogs(IRepository repo) : base(repo){}

        public IBlog GetByIntId(int Id)
        {
            return base._repository.QueryByFilter<BlogEF>( s => s.BlogId == Id).FirstOrDefault();
        }
        
    }

}

