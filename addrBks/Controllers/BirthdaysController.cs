using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NewsAPI.Controllers
{
    public class BirthdaysController : ApiController
    {

        [HttpPost]
        public IHttpActionResult TrackedBirthdays(string fromGUID, string toGUID)
        {
            WebManagers.ReturnEntities response = null;
            NSQLManager.IPersonUOW pUow = new NSQLManager.PersonUOW();
            string res = null;
            try
            {
                res = pUow.AddTrackBirthday(new POCO.TrackBirthdays(), fromGUID, toGUID);

                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return response;
        }
        [HttpGet]
        public IHttpActionResult TrackedBirthdays(string GUID)
        {
            WebManagers.ReturnEntities response = null;
            NSQLManager.IPersonUOW pUow = new NSQLManager.PersonUOW();
            string res = null;
            try
            {
                res = pUow.GetTrackedBirthday(GUID);

                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return response;
        }

    }
}
