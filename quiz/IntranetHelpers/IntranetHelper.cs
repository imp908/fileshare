using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intranet.Models.NewsModel;
using Orient.Client;

namespace Intranet.Models
{
    public abstract class IntranetHelper<T> : IHelper<T>
    {
        protected ODatabase ODatabase;
        public readonly string UserName;
        protected readonly T GeneralElement;
        private IDatabase DataBaseContext;


        protected IntranetHelper(IDatabase database)
        {
            UserName = HttpContext.Current.User.Identity.Name;
            DataBaseContext = database;
            ODatabase = DataBaseContext.GetDataBase<ODatabase>();
        }

        protected IntranetHelper(T param, IDatabase database) : this(database)
        {
            GeneralElement = param;
        }

        public abstract IEnumerable<T> Select();
        public abstract T Select(string id);
        public abstract Task<IEnumerable<T>> SelectAsync();

        public abstract void Insert();

        public abstract void SendToArchive(string rid);
        public abstract void Update();

        protected static IEnumerable<string> AccessGroup => new List<string>
        {
            "NSPK\\YablokovAE",
            "NSPK\\ignatenkofi",
            "NSPK\\salyukovev",
            "NSPK\\a.chilinyak",
            "NSPK\\sidelnikovsm",
            "NSPK\\saa",
            "NSPK\\oss",
            "NSPK\\eliseevavv",
            "NSPK\\maslennikovaov",
            "NSPK\\starkovio",
            "NSPK\\chekmasovvj",
            "NSPK\\nagaytsevsn",
            "NSPK\\pletmintsevamv",
            "NSPK\\karelinaik",
            "NSPK\\Neprintsevia"
        };

        public bool IsEmployeInGroup()
        {
            return AccessGroup.Contains(UserName);
        }

    }

}
