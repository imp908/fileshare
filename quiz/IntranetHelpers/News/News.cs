using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Models.NewsModel
{
    public class News
    {
        public Guid Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public State State { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        //public List<Comment> Comments { get; set; }
        //public List<TreeItem<Comment>> CommnetsTree { get; set; }
    }

    public enum State
    {
        Archived,
        Published,
        Drafted
    }

    public class CommentsNews
    {
        public News News { get; set; }
        public List<TreeItem<Comment>> CommnetsTree { get; set; }
    }
}