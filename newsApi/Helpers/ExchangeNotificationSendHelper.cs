using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Exchange.WebServices.Autodiscover;
using System.Net;
using System.Text;
using System.IO;
using System.Reflection;

namespace NewsAPI.Helpers
{
    public class ExchangeNotificationSendHelper
    {
        public  void SendNotificationExchange(string subject, string mailTo, string body)
        {
            // считываем body письма из .html файла определенного содержания из папки "res" в самом проекте
           string test = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/res/rep.html"), Encoding.UTF8);

            /// непотребства ниже - это replace ключевых слов и соот-х значений для отправки 
            //test = test.Replace("@ReportTitle@", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
            ////test = test.Replace("@Tab1Content@", htmlTable(cs, daylyIncidents));
            //test = test.Replace("@Tab1Title@", "Dayly Inc");
            //test = test.Replace("@Tab1Name@", "Инциденты за смену");
            ////test = test.Replace("@Tab2Content@", htmlTable(cs, activeIncidents));
            //test = test.Replace("@Tab2Title@", "Active Inc");
            //test = test.Replace("@Tab2Name@", "Активные инциденты");
            ////test = test.Replace("@Tab3Content@", htmlTable(cs, archiveIncidents));
            //test = test.Replace("@Tab3Title@", "Archive Inc");
            //test = test.Replace("@Tab3Name@", "Архив инцидентов");
            //string fileName = @"result-" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm") + ".html";
            //File.WriteAllText(fileName, test, Encoding.UTF8);



            ExchangeService service = new ExchangeService();
            AutodiscoverService autd = new AutodiscoverService();
            service.Credentials = new NetworkCredential("notification@nspk.ru", "@3%Rh&8("); //Integration@nspk.ru/QwESdF123//  Учетная запись при помощи которой отсылаем.
                                                                                             //service.UseDefaultCredentials = true;

            EmailMessage email = new EmailMessage(service);
            StringBuilder sb = new StringBuilder();
                        
            email.ToRecipients.Add(mailTo);              
            email.Subject = subject;
            email.Body = new MessageBody(test);
          
            service.AutodiscoverUrl("chekmasovvj@nspk.ru", RedirectionUrlValidationCallback);
         
            // Отправка закомментирована от греха
           //   email.Send();
         
        }
   
    //Автонастройка для эксча в сети: 
    private static bool RedirectionUrlValidationCallback(string redirectionUrl)
    {
        // The default for the validation callback is to reject the URL.
        bool result = false;

        Uri redirectionUri = new Uri(redirectionUrl);

        // Validate the contents of the redirection URL. In this simple validation
        // callback, the redirection URL is considered valid if it is using HTTPS
        // to encrypt the authentication credentials. 
        if (redirectionUri.Scheme == "https")
        {
            result = true;
        }
        return result;
    }
 }
}