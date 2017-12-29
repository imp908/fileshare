using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace NewsAPI.Interfaces
{
    public interface IPersonRelation
    {
        IHttpActionResult PostPersonRelation(string userLogin, string personGuid);
        IHttpActionResult DeletePersonRelation(string userLogin, string personGuid);
        IHttpActionResult GetPersonRelation(string userLogin);
    }
}
