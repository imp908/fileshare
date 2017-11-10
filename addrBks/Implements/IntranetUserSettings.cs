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
            throw new Exception();
        }

        public IHttpActionResult PostUserSettings(string userLogin, string json)
        {
            string insert_query = String.Format(@"let $a = insert into UserSettings content {0};
            let $b = create edge E from(select from Person where sAMAccountName = '{1}') to $a;
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