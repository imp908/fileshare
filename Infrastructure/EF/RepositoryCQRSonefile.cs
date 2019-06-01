
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
        
        /*Provides identity column manual insert while testing */
        public void SaveIdentity(string tableFullName)
        {
            string cmd = $"SET IDENTITY_INSERT {tableFullName} ON;";
            this._context.Database.OpenConnection();
            this._context.Database.ExecuteSqlCommand(cmd);
            this._context.SaveChanges();
            cmd = $"SET IDENTITY_INSERT {tableFullName} OFF;";
            this._context.Database.ExecuteSqlCommand(cmd);
            this._context.Database.CloseConnection();
        }
    }     

    public class CQRSEFBlogging : ICQRSEFBlogging
    {
        internal IRepository _repository;
        internal IMapper _mapper;

        public CQRSEFBlogging(IRepository repository)
        {
             this._repository = repository;
        }

        public CQRSEFBlogging(IRepository repository,IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }



        public BlogEF GetByIntId(int Id)
        {
            BlogEF blogEf = this._repository.QueryByFilter<BlogEF>( s => s.BlogId == Id)
                .Include(c => c.Posts).FirstOrDefault();

            try
            {                
                if(this._mapper!=null && blogEf!=null)
                {
                    var blogBll = this._mapper.Map(blogEf,blogEf.GetType(), typeof(BlogBLL));
                }
            }
            catch(Exception e)
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

        public PostAPI PersonAdsPostToBlog(PersonAdsPostCommand command)
        {
            PostAPI postReturn = new PostAPI();

            if (this._mapper == null || command == null)
            {
                return null;
            }           

            try
            {
                var postToAdd = this._mapper.Map(command, command.GetType(), typeof(PostEF)) as PostEF;
                this._repository.Add<PostEF>(postToAdd);
                this._repository.Save();

                var postAdded = this._repository.QueryByFilter<PostEF>(s => s.PostId == postToAdd.PostId)
                .Include(x => x.Blog).Include(x => x.Author)
                .FirstOrDefault();

                postReturn = this._mapper.Map(postAdded, postAdded.GetType(), typeof(PostAPI)) as PostAPI;
                return postReturn;
            }
            catch (Exception e)
            {

            }

            return postReturn;
        }

    
    }

}

