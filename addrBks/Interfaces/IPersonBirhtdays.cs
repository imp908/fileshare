
using System.Web.Http;

namespace NewsAPI.Interfaces
{
    public interface IPersonBirhtdays
    {
        IHttpActionResult GetActualPersonBirthdays();
    }
}