using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace NewsAPI.Interfaces
{
    public interface IPersonRelationNotifications
    {
        IHttpActionResult GetTodaysBirthdayRelations();
        void SendNotificationsToRecipients(Dictionary<string, string> messagesToSend);
    }
}