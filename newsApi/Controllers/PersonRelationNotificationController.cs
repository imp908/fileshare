using NewsAPI.Helpers;
using NewsAPI.Interfaces;
using System.Web.Http;
using NewsAPI.Extensions;
using System;
using System.Configuration;
using System.Threading;
using static NewsAPI.Helpers.OrientNewsHelper;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;


namespace NewsAPI.Controllers
{
    public class PersonRelationNotificationController : ApiController
    {
        private readonly IAddressBookProxy proxy;
        private readonly IPersonRelationNotifications personRelationNotifications;
        private readonly IUserAuthenticator userAuthenticator;

        public PersonRelationNotificationController(IPersonRelationNotifications personRelationNotifications, IAddressBookProxy proxy, IUserAuthenticator userAuthenticator)
        {
            this.proxy = proxy;
            this.personRelationNotifications = personRelationNotifications;
            this.userAuthenticator = userAuthenticator;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            // Получаем хелперы
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получение пользователя из реквеста
            var userLogin = userAuthenticator.AuthenticateUser(base.User);

            // Получение всех существующих связей типа PersonRealation для указанного пользователя
            var resp = personRelationNotifications.GetTodaysBirthdayRelations();

            // Проксируем результирующий набор данных перед последующей отправкой
         //   var messagesToSend = newsHelper.PrepareBirthdaysDataToSend(response);

            // Отправка писем получателям
          //  var resp = personRelationNotifications.SendNotificationsToRecipients(messagesToSend);

             return resp;
          
        }


    }
}