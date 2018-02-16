using NewsAPI.Helpers;
using NewsAPI.Interfaces;
using System.Web.Http;

namespace NewsAPI.Controllers
{
    public class PersonBirthdaysController : ApiController
    {
        private readonly IAddressBookProxy proxy;  
        private readonly IUserSettings userSettings;
        private readonly IPersonBirhtdays personBirthdays;

        public PersonBirthdaysController(IUserSettings userSettings,  IPersonBirhtdays personBirthdays, IAddressBookProxy proxy)
        {
            this.proxy = proxy;
            this.personBirthdays = personBirthdays;
            this.userSettings = userSettings;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            var birthdays = personBirthdays.GetActualPersonBirthdays();

            var bdays = new OrientNewsHelper.ReturnPersonsBirthdays(birthdays);

            return bdays;
                
        }
    }
}
