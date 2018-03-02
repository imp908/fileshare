using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using Intranet.Models; 
using Intranet.Models.NewsModel;

namespace Intranet.Controllers
{
  /*  public class NewsController : Controller
    {
        private readonly IntranetHelper<News> _newsHelper;

        public NewsController(IntranetHelper<News> helper)
        {
            _newsHelper = helper;
        }

        private static News News { get; set; }

        [HttpGet]
        public ActionResult Constructor(string id)
        {
            if (id == null) return View(new News());

            var news = _newsHelper.Select(id);
            ViewBag.Id = id;

            return View(news);
        }


        [HttpPost]
        public async Task<ActionResult> Constructor(string body, string subject, string id)
        {
            var ya = new StringWriter();
            HttpUtility.HtmlDecode(body, ya);

            News = new News
            {
                Author = _newsHelper.UserName,
                Id = !string.IsNullOrEmpty(id)? 
                        new Guid(id): 
                        Guid.NewGuid(),
                State = State.Drafted,
                Body = ya.GetStringBuilder().ToString(),
                Subject = subject
            };

            var helper = new NewsHelper(News, new OrientDbContext());

            await helper.InsertAsync();

            return View(new News());
        } 

        public async Task<ActionResult> List()
        {
            var news = await _newsHelper.SelectAsync();

            return View(news.ToList());
        }

        public ActionResult Viewer(string id)
        {
            var news = ((IComment)_newsHelper).SelectCommentNews(id);
            return View(news);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Viewer(string parentId,string commentText,string newsId)
        {
            if (!string.IsNullOrEmpty(commentText))
            {
                var comment = new Comment
                { 
                    NewsId = new Guid(newsId),
                    CommentDate = DateTime.Now,
                    Author = _newsHelper.UserName,
                    Text = commentText,
                    Id = Guid.NewGuid(),
                    ParentId = string.IsNullOrEmpty(parentId) ? Guid.Empty : new Guid(parentId)
                };

                ((IComment)_newsHelper).InsertCommnets(comment);
            }

            var news = ((IComment)_newsHelper).SelectCommentNews(newsId);
            return View(news);
        }
    }
    */
}