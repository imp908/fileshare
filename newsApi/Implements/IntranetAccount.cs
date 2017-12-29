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
    public class IntranetAccount : IAccount
    {
        public IntranetAccount()
        {
           OrientDB_HttpManager.AuthorizeOrientDB(
           ConfigurationManager.AppSettings["orient_auth_host"],
           ConfigurationManager.AppSettings["orient_login"],
           ConfigurationManager.AppSettings["orient_pswd"]
           );
        }

        public IHttpActionResult GetPersonInfo(string userLogin)
        {
            return new OrientDB_HttpManager.GetBatchResult(String.Format("{0}/{1}/{2}", ConfigurationManager.AppSettings["orient_func_host"], "SearchPersonExactly", userLogin));
        }
    }
}