using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DI_check.Models.EfModel
{
   
    public class User
    {
        public string UserId { get; set; }
        public string Email{ get; set; }

        public List<Blog> blogs { get; set; }
    }

    public class Blog
    {
        public string BlogId { get; set; }
        public string Content { get; set; }
        public User user { get; set; }
    }


}

namespace DI_check.Models.MongoModel
{
    using MongoDB.Bson;
    public class User
    {
        public string UserId { get; set; }
        public string Email { get; set; }

        public List<Blog> blogs { get; set; }
    }

    public class Blog
    {
        public string BlogId { get; set; }
        public string Content { get; set; }
        public User user { get; set; }
    }


}
