using System;
using System.Collections.Generic;

namespace EfModel{
   
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class User 
    {
        [Key]
        public string UserId {get; set;}
        public string Email {get; set;}
        public List<Blog> Blogs {get; set;}
    }

    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }

        [ForeignKey("User"),DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string userId {get;set;}
        public User User {get; set;}
    }

    public class Post
    {
        [Key]
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        [ForeignKey("Blog"),DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int blogId {get;set;}
        public Blog Blog { get; set; }
    }
}

namespace MongoModel{
    using MongoDB.Bson;
    using System.Dynamic;
    using MongoDB.Bson.Serialization.Attributes;
    using System.Security.Cryptography;

    public interface IEntity
    {
        string _id {get; set;}
    }

     public class UserMng : IEntity
     {
        [BsonId]
        public string _id {get; set;} = new Guid().ToString();
        public string UserId {get; set;}
        public string Email {get; set;}
        public List<BlogMng> Blogs {get; set;}
    }

    public class BlogMng : IEntity
    {
        [BsonId]
        public string _id {get; set;}
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<PostMng> Posts { get; set; }        
    }

    public class PostMng : IEntity
    {
        [BsonId]
        public string _id {get; set;} = new Guid().ToString();
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }        
        public ExpandoObject Params { get; set; } = new ExpandoObject(){};
    }
}