using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Intranet.Models.NewsModel
{
    public interface IComment
    {
        CommentsNews SelectCommentNews(string id);
        void InsertCommnets(Comment comment);
    }

}