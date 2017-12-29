using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using NewsAPI.Interfaces;
using NewsAPI.Helpers;
using System.Configuration;

namespace NewsAPI.Implements
{
    public class IntranetUserSettings : IUserSettings
    {
        public IHttpActionResult GetUserSettings(string userLogin)
        {
            //   var query = String.Format("select @this.toJSON('fetchPlan:in_*:-2 out_*:-2') from (select out('PersonRelation').size() as personRelationCount from Person where sAMAccountName = '{0}')", userLogin);
            var query = String.Format("select @this.toJSON('fetchPlan:in_*:-2 out_*:-2')  from (select expand(out(\"CommonSettings\")) from Person where sAMAccountName = '{0}')", userLogin);
            var helper = new OrientNewsHelper();
            return helper.ExecuteCommand(query);
        }

        public IHttpActionResult PostUserSettings(string userLogin, string json)
        {
            string insert_query = String.Format(@"delete edge CommonSettings where in('CommonSettings').sAMAccountName[0] = '{1}';
            delete vertex UserSettings where in('CommonSettings').sAMAccountName[0] = '{1}';
            let $a = insert into UserSettings content {0};
            let $b = create edge CommonSettings from(select from Person where sAMAccountName = '{1}') to $a;
            let $c = select outV().GUID as GUID from $b
            select @this.toJSON('fetchPlan:in_*:-2 out_*:-2') from  $c",
                                                json, userLogin);

            string batch = OrientBatchBuilder.CreateBatch(insert_query);

            return new OrientDB_HttpManager.PostBatch(
                ConfigurationManager.AppSettings["orient_batch_host"],
                batch
                );
        }
    }
}