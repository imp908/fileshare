using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using Intranet.Models.DocsVision;
using Orient.Client;

namespace Intranet.Models
{
    public interface IDatabase
    {
        List<object> GetData(string userName);
        ODatabase GetDataBase<T>();
    }
    
}