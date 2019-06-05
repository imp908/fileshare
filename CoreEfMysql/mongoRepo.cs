using MongoDB.Driver;
using MongoModel;

namespace MongoRepo{
    

    public class MongoRepository<T>
        where T: class, IEntity
    {
        IMongoClient client;
        IMongoDatabase database;

        IMongoCollection<T> collection;

        public MongoRepository(string cst, string database, IMongoClient client)
        {
            this.client = client;
            this.database = this.client.GetDatabase(database);
            string tName=typeof(T).Name;
            this.collection=this.database.GetCollection<T>(tName);
        }

        public void DropCollection(string name)
        {
            this.database.DropCollection(name);
        }

        public void CreateCollection(string name)
        {
            this.database.CreateCollection(name);
        }

        public void DropDatabase(string name){
            this.client.DropDatabase(name);
        }

        public void CreateDatabase(string name){            
            this.database=this.client.GetDatabase(name);
        }

        public T AddEntity(T item)
        {
            this.collection.InsertOne(item);
            return item;
        }

        public T GetByID(string id){
            var filterLinqDef = Builders<T>.Filter.Eq(s => s._id, id);
            T ret;
            ret = this.collection.Find(filterLinqDef).ToList()[0];
            return ret;
        }
    }

    public class MongoRepoTest
    {
        public static void Test(){
            MongoRepository<PostMng> repo = new MongoRepository<PostMng>("mongodb://localhost:27017","UserMng", new MongoClient());
            PostMng post = new PostMng(){  Title="np", Content="nc"};
            PostMng postAdded=repo.AddEntity(post);

        }
    }

}