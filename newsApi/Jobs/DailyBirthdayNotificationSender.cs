
using Quartz;
using NewsAPI.Helpers;
using NewsAPI.Interfaces;
using NewsAPI.Implements;

namespace NewsAPI.Jobs
{
    public class DailyBirthdayNotificationSender : IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            IPersonRelationNotifications personRelationNotifications = new IntranetPersonRelationNotification();
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получение всех существующих связей типа PersonRealation для указанного пользователя
            var response = personRelationNotifications.GetTodaysBirthdayRelations();

            // Проксируем результирующий набор данных перед последующей отправкой
            var messagesToSend = newsHelper.PrepareBirthdaysDataToSend(response);

            // Отправка писем получателям
             personRelationNotifications.SendNotificationsToRecipients(messagesToSend);

        }
    }
}