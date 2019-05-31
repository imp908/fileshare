
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

    using AutoMapper;

    public class RepositoryEF : IRepository
    {
        DbContext _context;

        public RepositoryEF(DbContext context){
            _context=context;
        }
        
        public IQueryable<T> GeyAll<T>() 
            where T : class
        {
            return this._context.Set<T>();
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

        public IQueryable<T> SkipTake<T>(int skip=0,int take=10)
            where T : class
        {
            return this._context.Set<T>().Skip(skip).Take(take);
        }

        public void Save(){
            this._context.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return this._context.SaveChangesAsync();
        }
    }     

    public class UOWBlogging : IUOWBlogging
    {
        internal IRepository _repository;
        internal IMapper _mapper;

        public UOWBlogging(IRepository repository)
        {
             this._repository = repository;
        }

        public UOWBlogging(IRepository repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public BlogEF GetByIntId(int Id)
        {
            BlogEF blogEf = this._repository.QueryByFilter<BlogEF>( s => s.BlogId == Id).Include(c => c.Posts).FirstOrDefault();

            try{
                if(this._mapper!=null && blogEf!=null){
                    var blogBll = this._mapper.Map(blogEf,blogEf.GetType(), typeof(BlogBLL));
                }
            }catch(Exception e)
            {

            }

            return blogEf;
        }
        
        public BlogEF AddBlog(BlogEF blog)
        {
            this._repository.Add<BlogEF>(blog);
            this._repository.Save();
            return blog;
        }

        public List<BlogEF> GetBlogs(int skip=0,int take=10)
        {          
            return this._repository.SkipTake<BlogEF>(skip,take).ToList();;
        }
    }

}

