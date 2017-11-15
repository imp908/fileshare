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
        string AddTrackBirthday(OrientEdge edge_, string guidFrom, string guidTo);
        string DeleteTrackedBirthday(OrientEdge edge_, string guidFrom, string guidTo);
    }
}