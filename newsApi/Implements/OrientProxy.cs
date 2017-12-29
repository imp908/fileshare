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
    public class OrientProxy : IAddressBookProxy
    {
        public IHttpActionResult ReturnAddedEntityId(IHttpActionResult ar)
        {
            return new OrientNewsHelper.ReturnAddedEntityIdWithExecute(ar,
                ConfigurationManager.AppSettings["orient_id_name"]
                );
        }

        public IHttpActionResult ReturnRequestedEntityWithFetchPlan(IHttpActionResult ar)
        {
            return new OrientNewsHelper.ReturnRequestedEntityWithFetchPlanWithExecute(ar);
        }

        public IHttpActionResult ReturnRequestedEntityTraverseWithFetchPlan(IHttpActionResult ar)
        {
            return new OrientNewsHelper.ReturnRequestedEntitiesRecordSetWithFetchPlanWithExecute(ar);
        }

        public IHttpActionResult ReturnPersonInfo(IHttpActionResult ar)
        {
            return new OrientNewsHelper.ReturnPersonInfo(ar);                
        }

        public IHttpActionResult ReturnPersonGuid(IHttpActionResult ar)
        {
            return new OrientNewsHelper.ReturnPersonGuid(ar);
        }
    }
}