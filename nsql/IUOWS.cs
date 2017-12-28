using System.Collections.Generic;
using POCO;
using IOrientObjects;


namespace IUOWs
{
   

    public interface IPersonUOW
    {
        IEnumerable<Person> GetAll();
        string GetByGUID(string GUID);
        IEnumerable<Person> GetObjByGUID(string GUID);
        string GetTrackedBirthday(string GUID);
        string AddTrackBirthday(E edge_, string guidFrom, string guidTo);
        string DeleteTrackedBirthday(E edge_, string guidFrom, string guidTo);
   }
}