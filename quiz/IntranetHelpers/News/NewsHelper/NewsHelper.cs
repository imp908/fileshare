using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Orient.Client;


namespace Intranet.Models.NewsModel
{
  /*  public class NewsHelper : IntranetHelper<News>, IComment
    {
        private readonly News News;

        public NewsHelper(IDatabase database) : base(database)
        {
        }

        public NewsHelper(News news,IDatabase database) 
            : base(news, database)
            {
                News = base.GeneralElement;
            }

        #region test
        static void Test(IEnumerable<TreeItem<Comment>> categories, int deep = 0)
        {
            foreach (var c in categories)
            {
                Console.WriteLine(new String('\t', deep) + c.Item.Author);
                Test(c.Children, deep + 1);
            }
        }
        #endregion
        private List<TreeItem<Comment>> GetComments(string id)
        {
            #region testComment
            var p = new List<Comment>()
            {
                new Comment
                {
                    Author = "sid",
                    Id = new Guid("6C1B33DE-7323-4C44-80CB-6F85B2496EAF"),
  
                    ParentId = Guid.Empty,
                    Text = "first comment",
                },
                new Comment
                {
                    Author = "sid",

                    Id = new Guid("83424944-D72E-403B-9226-AAD7F752A381"),
                    ParentId = Guid.Empty,
                    Text = "second comment",
                },
                new Comment
                {
                    Author = "sid",
      
                    Id = new Guid("3979E8C0-764D-434E-AE77-980B433FA143"),
                    ParentId = new Guid("83424944-D72E-403B-9226-AAD7F752A381"),
                    Text = "third comment",
                }
            };
            #endregion
            return ODatabase.Select().From("Comment").Where("NewsId").Equals(id).ToList<Comment>().OrderBy(x => x.CommentDate).GenerateTree
                (
                    c => c.Id,
                    c => c.ParentId)
                .ToList();

        //    return GenericHelpers.GenerateTree
        //        (ODatabase.Select().From("Comment").Where("NewsId").Equals(id).ToList<Comment>().OrderBy(x=>x.CommentDate), 
        ////        (p,
        //        c => c.Id, 
        //        c => c.ParentId)
        //        .ToList();
        }

        public void InsertCommnets(Comment comment)
        {
            ODatabase.Insert(comment).Into("Comment").Run();
        }

        public sealed override void Insert()
        {
            var containesInBase = Select(News.Id.ToString());
            if (containesInBase == null)
            {
                News.CreationDateTime = DateTime.Now;
                News.LastUpdateDateTime = DateTime.Now;

                var json = JsonConvert.SerializeObject(
                    News,
                    new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy HH:mm:ss" },
                    new StringEnumConverter()
                );
                ODatabase
                    .Insert()
                    .Into(News.GetType()
                              .Name + " Content " + json)
                    .Run();
            }
            else
            {
                News.CreationDateTime = containesInBase.CreationDateTime;
                News.LastUpdateDateTime = DateTime.Now;

                Update();
            }
        }

        public async Task InsertAsync()
        {
            var containesInBase = (await SelectAsync(News.Id.ToString())).ToList();

            await Task.Run(() =>
            {
                if (!containesInBase.Any())
                {
                    News.CreationDateTime = DateTime.Now;
                    News.LastUpdateDateTime = DateTime.Now;

                    var json = JsonConvert.SerializeObject(
                        News,
                        new IsoDateTimeConverter {DateTimeFormat = "dd/MM/yyyy HH:mm:ss"},
                        new StringEnumConverter()
                    );
                    ODatabase
                        .Insert()
                        .Into(News.GetType()
                                  .Name + " Content " + json)
                        .Run();
                }
                else
                {
                    News.CreationDateTime = containesInBase.FirstOrDefault().CreationDateTime;
                    News.LastUpdateDateTime = DateTime.Now;

                    Update();
                }
            });
        }

        public sealed override void Update()
        {
            ODatabase
                .Update(News)
                .Where("Id").Equals(News.Id)
                .Run();
        }

        public sealed override IEnumerable<News> Select()
        {
            return ODatabase.Select().From<News>().ToList<News>();// ODatabase.Select().From<News>().ToList<News>();
        }

        public sealed override async Task<IEnumerable<News>> SelectAsync()
        {
            return await Task.Run(() =>  ODatabase.Select().From<News>().ToList<News>());
        }

        public sealed override News Select(string id)
        {
            return ODatabase.Select().From<News>().ToList<News>().Where(x => x.Id == new Guid(id)).Where(x => x.Id == new Guid(id));
        }


        public async Task<IEnumerable<News>> SelectAsync(string id)
        {
            return await Task.Run(() =>
                ODatabase.Select()
                    .From<News>()
                    .ToList<News>()
                    .Where(x => x.Id == new Guid(id))
                    .Where(x => x.Id == new Guid(id)));
        }

        public CommentsNews SelectCommentNews(string id)
        {
            return
                new CommentsNews
                {
                    News = ODatabase.Select().From<News>().ToList<News>().FirstOrDefault(x => x.Id == new Guid(id)),
                    CommnetsTree = GetComments(id)
                };
        }

        public override void SendToArchive(string rid)
        {
            
        }
    }
    */
}
