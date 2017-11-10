using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using NewsAPI.Interfaces;
using NewsAPI.Helpers;
using System.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;
using NewsAPI.Extensions;
using static NewsAPI.Helpers.OrientNewsHelper;

namespace NewsAPI.Implements
{
    public class OrientNews : INews
    {
        public OrientNews()
        {
           OrientDB_HttpManager.AuthorizeOrientDB(
           ConfigurationManager.AppSettings["orient_auth_host"],
           ConfigurationManager.AppSettings["orient_login"],
           ConfigurationManager.AppSettings["orient_pswd"]
           );
        }

        public IHttpActionResult UpdateEntity(string entityId, string content)
        {
            string merge_query = String.Format("update Object merge {0} where {1} = {2}",
                content,
                ConfigurationManager.AppSettings["orient_id_name"],
                entityId
                );

            string batch = OrientBatchBuilder.CreateBatch(merge_query);

            return new OrientDB_HttpManager.PostBatch(
                ConfigurationManager.AppSettings["orient_batch_host"],
                batch
                );
        }

        public IHttpActionResult CreateEntity(string content)
        {
            string insert_query = String.Format("insert into Object content {0}", content);

            string batch = OrientBatchBuilder.CreateBatch(insert_query);

            return new OrientDB_HttpManager.PostBatch(
                ConfigurationManager.AppSettings["orient_batch_host"],
                batch
                );
        }



        public IHttpActionResult CreateAuthorEdge(string author, string entityid)
        {
            string createAuthorshipEdge_query = String.Format("create edge Authorship from (select from Person where sAMAccountName = '{0}') to (select from Object where Id = {1})", author, entityid);

            string batch = OrientBatchBuilder.CreateBatch(createAuthorshipEdge_query);

            return new OrientDB_HttpManager.PostBatch(
                ConfigurationManager.AppSettings["orient_batch_host"],
                batch
                );
        }

        public IHttpActionResult CreateCommentEdge(string parent_id, string child_id)
        {
            string createCommentEdge_query = String.Format("create edge Comment from (select from Object where {0} = {1}) to (select from Object where {0} = {2})",
                ConfigurationManager.AppSettings["orient_id_name"],
                parent_id,
                child_id);

            String batch = OrientBatchBuilder.CreateBatch(createCommentEdge_query);

            return new OrientDB_HttpManager.PostBatch(
                ConfigurationManager.AppSettings["orient_batch_host"],
                batch
                );
        }

        //public IHttpActionResult GetPersonInfo(string userLogin)
        //{
        //    return new OrientDB_HttpManager.GetBatchResult(String.Format("{0}/{1}/{2}", ConfigurationManager.AppSettings["orient_func_host"], "SearchPerson", userLogin));
        //}

        
    }
}