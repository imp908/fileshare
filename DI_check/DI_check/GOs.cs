using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using NLog;
using Newtonsoft.Json;

namespace DI_check
{
    public class GenModel
    {
        public static List<Models.EfModel.Blog> BlogsGen()
        {
            return new List<Models.EfModel.Blog>()
            {
                new Models.EfModel.Blog(){BlogId = "id1", Content ="ct1"},
                new Models.EfModel.Blog(){BlogId = "id2", Content ="ct2"}
            };
        }
    }
    public interface Igo
    {
        void GO();
    }

    public class BlogMap : Igo
    {
        IMapper mapper { get; set; }
        ILogger logger { get; set; }

        public BlogMap(IMapper mapper_, ILogger logger_)
        {
            this.mapper = mapper_;
            this.logger = logger_;
        }
        public BlogMap()
        {

        }
        public void GO()
        {
            List<Models.MongoModel.Blog> blogs = mapper.Map<List<Models.MongoModel.Blog>>(GenModel.BlogsGen());
            this.logger.Info(JsonConvert.SerializeObject(blogs));
            Console.WriteLine("Print GO");
        }
    }

    public class Text : Igo
    {
        public void GO()
        {
            Console.WriteLine("Text GO");
        }
    }

    public class RUN
    {
        Igo go;
        public RUN()
        {

        }
        public RUN(Igo go_)
        {
            this.go = go_;
        }
        public void GO()
        {
            this.go.GO();
        }
    }
}
