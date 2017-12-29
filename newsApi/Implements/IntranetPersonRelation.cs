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
    public class IntranetPersonRelation : IPersonRelation
    {
        public IHttpActionResult GetPersonRelation(string userLogin)
        {
            var query = String.Format("select SearchPersonExactly(sAMAccountName) as personRelations from (select expand(out('PersonRelation')) from Person where sAMAccountName = '{0}')", userLogin);
            var helper = new OrientNewsHelper();
            return helper.ExecuteCommand(query);
        }

        public IHttpActionResult PostPersonRelation(string userLogin, string personGuid)
        {
            string insert_query = String.Format("let $a = create edge PersonRelation from (select from Person where sAMAccountName = '{0}') to (select from Person where GUID = '{1}'); let $b = select out('PersonRelation').size() as personRelationCount from Person where sAMAccountName = '{0}'; select @this.toJSON('fetchPlan:in_*:-2 out_*:-2') from $b",
                                          userLogin, personGuid);          

            string batch = OrientBatchBuilder.CreateBatch(insert_query);

            return new OrientDB_HttpManager.PostBatch(
                ConfigurationManager.AppSettings["orient_batch_host"],
                batch
                );
        }

        public IHttpActionResult DeletePersonRelation(string userLogin, string personGuid)
        {

            string delete_query = String.Format("let $a = delete edge PersonRelation where @RID contains (select @RID from PersonRelation where out.sAMAccountName = '{0}' and in.GUID = '{1}'); let $b = select out('PersonRelation').size() as personRelationCount from Person where sAMAccountName = '{0}'; select @this.toJSON('fetchPlan:in_*:-2 out_*:-2') from $b",
                                        userLogin, personGuid);

            string batch = OrientBatchBuilder.CreateBatch(delete_query);

            return new OrientDB_HttpManager.PostBatch(
                ConfigurationManager.AppSettings["orient_batch_host"],
                batch
                );


        }

    }
}