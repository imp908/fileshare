using System;
using System.Collections.Generic;

namespace Model{
    public class User{
        public string UserId {get; set;}
        public string Email {get; set;}
        public List<Blog> Blogs {get; set;}
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }

        public int UserId {get; set;}
        public User User {get; set;}
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}