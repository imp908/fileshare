using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsAPI.Implements
{
    public interface IFunctionToString
    {      
        string CallFunctionItem(string name, string param);
        string CallFunctionCollection(string name, string param);
        string CallFunctionItems(string name, string param);

        string CallFunctionParentChildName(string name, string param);
        string CallFunctionParentChildId(string name, string param);
    }

    public interface IPersonFunctions
    {
        string GetUnitByAccount(string AccountName);
        string GetDepartmentByAccount(string AccountName);
        string GetManagerByAccount(string AccountName);
        string GetCollegesByAccount(string AccountName);       
        string GetManagerHierarhyByAccount(string AccountName);
        string GetCollegesLowerByAccount(string AccountName);
        string GetGUID(string AccountName);
        string SearchByLastName(string AccountName);
        string SearchPerson(string AccountName);

       

    }
}