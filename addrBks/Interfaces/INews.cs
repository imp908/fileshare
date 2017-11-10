using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace NewsAPI.Interfaces
{
    public interface INews
    {
        IHttpActionResult CreateEntity(string content);
        IHttpActionResult UpdateEntity(string entityId, string content);
        IHttpActionResult CreateAuthorEdge(string author, string entity);
        IHttpActionResult CreateCommentEdge(string parent_id, string child_id);
    }
}
