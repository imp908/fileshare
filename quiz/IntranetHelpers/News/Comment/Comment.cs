using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Models.NewsModel
{
    public class Comment : IComment
    {
        public Guid Id { get; set; }
        // public int Order { get; set; }
        public DateTime CommentDate { get; set; }
        public Guid ParentId { get; set; }
        public Guid NewsId { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public  int LikesCount { get; set; }


        public CommentsNews SelectCommentNews(string id)
        {
            throw new NotImplementedException();
        }

        public void InsertCommnets(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}