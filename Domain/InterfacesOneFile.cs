namespace mvccoresb.Domain.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    
    using mvccoresb.Domain.TestModels;
    
    public interface IEntityIntId
    {
        int Id{get;set;}
    }
    public interface IEntityGuidId
    {
        Guid Id { get; set; }
    }

    public interface IRepository
    {
        IQueryable<T> GeyAll<T>() where T : class;              

        void Add<T>(T item) where T : class;
        void AddRange<T>(IList<T> items) where T : class;
        void Delete<T>(T item) where T : class;
        void DeleteRange<T>(IList<T> items) where T : class;
        void Update<T>(T item) where T : class;
        void UpdateRange<T>(IList<T> items) where T : class;
        IQueryable<T> SkipTake<T>(int skip,int take)
            where T : class;
        IQueryable<T> QueryByFilter<T>(Expression<Func<T, bool>> expression)
            where T : class;
        void Save();

        void SaveIdentity(string command);
    }

    public interface ICQRScrud
    {
        T Add<T>(T item) where T : class;
        void AddRange<T>(IList<T> items) where T : class;
        void Delete<T>(T item) where T : class;
        void DeleteRange<T>(IList<T> items) where T : class;
        T Update<T>(T item) where T : class;
        void UpdateRange<T>(IList<T> items) where T : class;
        IQueryable<T> QueryByFilter<T>(Expression<Func<T, bool>> expression)
            where T : class;
    }

    public interface ICQRSEFBlogging
    {
        BlogEF AddBlog(BlogEF blog);
        BlogEF GetByIntId(int Id);

        PostAPI PersonAdsPostToBlog(PersonAdsPostCommand command);
    }
    
}