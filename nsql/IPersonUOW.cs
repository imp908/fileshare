using System.Collections.Generic;
using POCO;

using IOrientObjects;

namespace NSQLManager
{
    public interface IPersonUOW
    {
        IEnumerable<Person> GetAll();
        string GetByGUID(string GUID);
        IEnumerable<Person> GetObjByGUID(string GUID);
        string GetTrackedBirthday(string GUID);
        string AddTrackBirthday(IOrientEdge edge, string guidFrom, string guidTo);
    }
}