using NewsAPI.Helpers;
using NewsAPI.Interfaces;
using System.Web.Http;


namespace NewsAPI.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IAddressBookProxy proxy;
        private readonly IUserAuthenticator userAuthenticator;
        private readonly IAccount account;

        public AccountController(IAccount account,IUserAuthenticator userAuthenticator,IAddressBookProxy proxy)
        {
            this.proxy = proxy;
            this.userAuthenticator = userAuthenticator;
            this.account = account;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();
            var name = userAuthenticator.AuthenticateUser(base.User);

            var response = account.GetPersonInfo(name);
            return proxy.ReturnPersonInfo(response);
        }
    }
}