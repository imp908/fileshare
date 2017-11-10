
using System.Web.Http;
using NewsAPI.Interfaces;
using NewsAPI.Helpers;

namespace NewsAPI.Implements
{
    public class IntranetPersonBirthdays : IPersonBirhtdays
    {
        public IHttpActionResult GetActualPersonBirthdays()
        {
            var query = "select  @this.toJSON('fetchPlan:in_*:-2 out_*:-2') from (select  Name, Birthday from Person where (inE(\"MainAssignment\")[0].Disabled is null or inE(\"MainAssignment\")[0].Disabled >= sysdate() ) and(Disabled is null) and(inE().State != 'Отпуск по уходу за ребенком' and inE().State != 'Отпуск по беременности и родам' ))";
            var helper = new OrientNewsHelper();
            var personBdays_resp = helper.ExecuteCommand(query);
            return new OrientNewsHelper.ReturnPersonsBirthdays(personBdays_resp);
        }
    }
}