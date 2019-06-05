using System;
using System.Collections.Generic;
using System.Linq;
using MySqlContext;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using EfModel;
using MongoModel;
using AutoMapper;

namespace Ii{
    public interface IGo
    {
        void GO();
    }

}
namespace cs_
{
    using Ii;

    public class ModelGen 
    {
    
    public User user;
    public  List<User> users;        
        List<Post> postsGen(){
            return new List<Post>(){ 
                new Post(){Title="Post1",Content="Conten1"},
                new Post(){Title="Post2",Content="Conten2"}
            };
        }
        List<Blog> blogsGen(){
            return new List<Blog>(){ 
                new Blog(){Url="url0",Posts=this.postsGen()},                
                new Blog(){Url="url2",Posts=this.postsGen()},
            };
        }
        public List<User> usersGen(){
            return new List<User>(){
                new User(){Email="mail0",Blogs=this.blogsGen()},
                new User(){Email="mail1",Blogs=this.blogsGen()}
            };
        }

        public static List<User> Users(){
            ModelGen mg = new ModelGen();
            return mg.usersGen();
        }
        
    }

    public class MysqlContextCheck : IGo
    {
        public List<User> users = null;

        public MysqlContextCheck(){

        }
        public void GO(){
            using (var db = new BloggingContext())
            {                
                db.Blogs.RemoveRange(db.Blogs);
                db.Users.RemoveRange(db.Users);
                db.Posts.RemoveRange(db.Posts);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records removed from database", count);

                //db.Users.Add(user);
                //db.Blogs.Add(blog);
                
                //db.Posts.Add(ModelGen.DummyPost());
                
                db.Users.AddRange(ModelGen.Users());

                count = db.SaveChanges();
                
                this.users = (from s in db.Users select s).ToList();

                Console.WriteLine("{0} records saved to database", count);
            }
        }
    }
    
    public class MongoCheck : IGo
    {
        IMapper _mapper;
        
        public List<UserMng> usersMng;

        public MongoCheck()
        {
            
        }
        public MongoCheck(IMapper mapper_)
        {
            this._mapper=mapper_;
        }
        public void GO()
        {
            var mongo = new MongoClient("mongodb://localhost:27017");
            
            mongo.DropDatabase("userMails");
            
            var db = mongo.GetDatabase("userMails");

            var collection = db.GetCollection<UserMng>(typeof(UserMng).Name);          

            collection.DeleteMany("{}");
            collection.InsertMany(usersMng);

            //http://mongodb.github.io/mongo-csharp-driver/2.7/reference/driver/definitions/
            //OK
            BsonDocument bsonFilter = new BsonDocument("Email","mail0");
            //OK
            var filterLinqDef = Builders<UserMng>.Filter.Eq(s => s.Email,"mail0");    
            
            collection = db.GetCollection<UserMng>(typeof(UserMng).Name);            
            
            long cnt = collection.Count(s=>s.UserId!=null);

            List<UserMng> resFiltered = collection.Find(filterLinqDef).ToList();

            BsonDocument bsonDoc = new BsonDocument{{"find","UserMng"}};
            //JsonCommand<BsonDocument> cmd = new JsonCommand<BsonDocument>("");

            var res =db.RunCommand<BsonDocument>(bsonDoc);
            var usersRes=res["result"].AsBsonArray.ToList();

            Console.WriteLine(string.Format("{0}:{1}","Mongo users:",collection));
        }
    }
}

namespace DbInstanceCheck
{   
    using Ii;
    public class DbCHeck{
        IGo _go;
        public DbCHeck(){

        }
        public DbCHeck(IGo dbCheck_){
            this._go=dbCheck_;
        }
        public void GO(){
            this._go.GO();
        }
    }    
}