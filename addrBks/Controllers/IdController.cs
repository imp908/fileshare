using NewsAPI.Helpers;
using NewsAPI.Interfaces;
using System.Web.Http;
using static NewsAPI.Helpers.OrientNewsHelper;

namespace NewsAPI.Controllers
{
    public class IdController : ApiController
    {
        private readonly IAddressBookProxy addressBookProxy;
        public IdController(IAddressBookProxy addressBookProxy)
        {
            this.addressBookProxy = addressBookProxy;
        }

        public IHttpActionResult Get(int id)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем запрошенную сущность (новость, блог, коммент или проч.)
            string requestedEntity = newsHelper.GetEntity(id.ToString());

            // Готовим response к проксированию, т.к. требуется применить fetchPlan и извлечь нужный json с контентом запрошенной сущности
            var response = new TextResult(requestedEntity, Request);

            // возврат проскированного контента запрошенной сущности
            return addressBookProxy.ReturnRequestedEntityWithFetchPlan(response);
        }

        //[Route("api/Id/{id}/{count}")]
        public IHttpActionResult Get(int id, int count)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем out-траверс запрошенной сущности (новость, блог, коммент или проч.)
            string requestedEntity = newsHelper.GetEntityTraverse(id, count);

            // Готовим response к проксированию, т.к. требуется применить fetchPlan и извлечь из возвращенного набора нужные json с контентом запрошенной сущности и ее траверса
            var response = new TextResult(requestedEntity, Request);

            // возврат проксированного контента запрошенной сущности и ее траверса
            return addressBookProxy.ReturnRequestedEntityTraverseWithFetchPlan(response);
        }

        //[Route("api/Id/{id}/{start}/{count}")]
        public IHttpActionResult Get(int id, int start, int count)
        {
            // Получаем хелпер
            var newsHelper = new OrientNewsHelper();

            // Осуществляем авторизацию в OrientDb
            newsHelper.Authorize();

            // Получаем out-траверс запрошенной сущности (новость, блог, коммент или проч.)
            string requestedEntity = newsHelper.GetEntityTraverseInSpecificBounds(id, start, count);

            // Готовим response к проксированию, т.к. требуется применить fetchPlan и извлечь из возвращенного набора нужные json с контентом запрошенной сущности и ее траверса
            var response = new TextResult(requestedEntity, Request);

            // возврат проксированного контента запрошенной сущности и ее траверса
            return addressBookProxy.ReturnRequestedEntityTraverseWithFetchPlan(response);
        }
    }
}
