using System;
using System.Web.Http;

using NewsAPI.Interfaces;
using NewsAPI.Implements;

namespace NewsAPI.Controllers
{
    public class BirthdaysController : ApiController
    {

        private readonly IUserAuthenticator userAuthenticator;
        private readonly NSQLManager.IPersonUOW pUOW;
        IHttpActionResult response;
        private readonly IWebManagers.IResponseReader responseReader;
        string name = string.Empty, GUID = string.Empty, res = string.Empty;
        PersonController pc;

        public BirthdaysController()
        {
            userAuthenticator = new UserAuthenticator();
            pUOW = new NSQLManager.PersonUOW();
            responseReader = new WebManagers.WebResponseReader();
            pc = new PersonController();
        }
        public BirthdaysController(NSQLManager.IPersonUOW pUOW_, IHttpActionResult response_)
        {
            this.pUOW = pUOW_;           
        }

        [HttpDelete]
        public IHttpActionResult DeleteBirthdays(string toGUID)
        {


            try
            {
                name = userAuthenticator.AuthenticateUser(base.User);
                if (name == string.Empty || name == null)
                {
                    res = @"Welcome Guest!";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                GUID = responseReader.ReadResponse(pc.GetGUID(name));
                if (GUID == string.Empty || GUID == null)
                {
                    res = @"GUID searched";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                res = pUOW.DeleteTrackedBirthday(new POCO.TrackBirthdays(), GUID, toGUID);
                if (res == null || res == string.Empty)
                {
                    res = @"Holidays searched";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            return response;



            //string res = null;
            //try
            //{
            //    res = pUOW.DeleteTrackedBirthday(new POCO.TrackBirthdays(), fromGUID, toGUID);
            //    response = new WebManagers.ReturnEntities(res, Request);
            //}
            //catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            //return response;
        }
        [HttpPost]
        public IHttpActionResult PostBirthdays(string toGUID)
        {

            try
            {
                name = userAuthenticator.AuthenticateUser(base.User);
                if (name == string.Empty || name == null)
                {
                    res = @"Welcome Guest!";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                GUID = responseReader.ReadResponse(pc.GetGUID(name));
                if (GUID == string.Empty || GUID == null)
                {
                    res = @"GUID searched";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                res = pUOW.AddTrackBirthday(new POCO.TrackBirthdays(), GUID, toGUID);
                if (res == null || res == string.Empty)
                {
                    res = @"Holidays searched";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            return response;
          
        }
        [HttpGet]
        public IHttpActionResult GetBirthdays()
        {

            try
            {
                name = userAuthenticator.AuthenticateUser(base.User);
                if (name == string.Empty || name == null)
                {
                    res = @"Welcome Guest!";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                GUID = responseReader.ReadResponse(pc.GetGUID(name));
                if (GUID == string.Empty || GUID == null)
                {
                    res = @"GUID searched";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                res = pUOW.GetByGUID(GUID);
                if (res == null || res == string.Empty)
                {
                    res = @"Holidays searched";
                    return new WebManagers.ReturnEntities(res, Request);
                }
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            try
            {
                // преобразуем строку в HttpResponseMessage со ReturnEntities с результатом в поле _value
                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

            return response;
         
        }

        [HttpDelete]
        public IHttpActionResult DeleteBirthdaysAcc(string fromGUID, string toGUID)
        {

            string res = null;
            try
            {
                res = pUOW.DeleteTrackedBirthday(new POCO.TrackBirthdays(), fromGUID, toGUID);
                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return response;
        }
        [HttpPost]
        public IHttpActionResult PostBirthdaysAcc(string fromGUID, string toGUID)
        {

            string res = null;
            try
            {
                res = pUOW.AddTrackBirthday(new POCO.TrackBirthdays(), fromGUID, toGUID);
                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return response;
        }
        [HttpGet]
        public IHttpActionResult GetBirthdaysAcc(string GUID)
        {

            string res = null;
            try
            {
                res = pUOW.GetByGUID(GUID);
                response = new WebManagers.ReturnEntities(res, Request);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return response;
        }

    }
}
