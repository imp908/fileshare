using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices;
using System.Timers;
using System.Text.RegularExpressions;
using System.Messaging;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using System.Web;
using Intranet.Models.Logger;
using System.Net.Http.Headers;
using System.Net.Http;
namespace Intranet.Models
{
    public class OutlookContext
    {
        private static ExchangeService service;
        private FileLogger logger = new FileLogger();
        private bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            bool result = false;
            Uri redirectionUri = new Uri(redirectionUrl);
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
     
        private void SetService()
        {
          //  ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Delegation;

           // using (((WindowsIdentity)HttpContext.Current.User.Identity).Impersonate())
            //    {
            //    var p = CredentialCache.DefaultNetworkCredentials;
            //   var winId =  (WindowsIdentity)HttpContext.Current.User.Identity;

            //    service = new ExchangeService(ExchangeVersion.Exchange2007_SP1)
            //    {
            //        ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, "sidelnikovsm@nspk.ru"),
            //    //    UseDefaultCredentials = true,
            //       Credentials = (NetworkCredential)CredentialCache.DefaultCredentials,
            //       // PreAuthenticate = true,
            //        //      HttpContext.Current.Usnew OAuthCredentials()
            //        //Credentials
            //        //UserAgent
            //        //  Credentials = CredentialCache.DefaultNetworkCredentials,
            //        Url = new Uri("https://webmail.nspk.ru/EWS/Exchange.asmx")
            //    };

         //   }

            //  var email = HttpContext.Current.User.Identity.Name.Split('\\')[1] + "@nspk.ru";
            //service.AutodiscoverUrl
            //    (
            //      email,
            //      //  "sidelnikovsm@nspk.ru", 
            //      // UserPrincipal.Current.EmailAddress,
            //        RedirectionUrlValidationCallback
            //    );
            //   service.Url = new Uri("https://webmail.nspk.ru/EWS/Exchange.asmx");

        }

        public string SetSubscription()
        {
            List<Message> MessageList = new List<Message>();

            if (service == null)
            {
                SetService();
            }
           

            var findResults = service.FindItems(
            WellKnownFolderName.Inbox,
            new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false),
            new ItemView(10));


            return findResults.Items.Count.ToString();

            //if (findResults.Items.Count != 0) 
            //{
            //    //foreach (var item in findResults.Items)
            //    //{
            //    //    var message = (EmailMessage)item;
            //    //    message.Load();

            //    //    message.IsRead = true;
            //    //    message.Update(ConflictResolutionMode.AlwaysOverwrite);
            //    //}
            //    return 
            //    MessageList.Clear();
            //}
        }
    }
}