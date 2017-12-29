using NewsAPI.Helpers;
using NewsAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;


namespace NewsAPI.Implements
{
    public class IntranetPersonRelationNotification : IPersonRelationNotifications
    {
        public IHttpActionResult GetTodaysBirthdayRelations()
        {
            return new OrientDB_HttpManager.GetBatchResult(String.Format("{0}/{1}", ConfigurationManager.AppSettings["orient_func_host"], "GetTodaysBirthdayRelations"));
        }

        public void SendNotificationsToRecipients(Dictionary<string, string> messagesToSend)
        {
             new OrientNewsHelper.SendPersonRelationBirthdaysNotifications(messagesToSend).Send();
        }
    }
}