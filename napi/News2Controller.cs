using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

namespace NewsAPI.Controllers
{
    public class News2Controller : ApiController
    {

      string acc;    
      Managers.Manager mng;

      public News2Controller()
      {
        string host_= string.Format("{0}:{1}"
        ,ConfigurationManager.AppSettings["OrientDevHost"],ConfigurationManager.AppSettings["OrientPort"]);

        mng = new Managers.Manager(
        ConfigurationManager.AppSettings["OrientUnitTestDB"]
        ,host_
        ,ConfigurationManager.AppSettings["orient_login"]
        ,ConfigurationManager.AppSettings["orient_pswd"]
        );

        Managers.Manager sourceManager = new Managers.Manager(
        ConfigurationManager.AppSettings["OrientSourceDB"]
        ,host_
        ,ConfigurationManager.AppSettings["orient_login"]
        ,ConfigurationManager.AppSettings["orient_pswd"]
        );

        acc=mng.UserAcc();

        if(acc==null)
        {
          throw new Exception("No account found");
        }

        if(mng.GetPersonUOW().GetPersonByAccount(acc)==null){
          POCO.Person personToAdd=sourceManager.GetPersonUOW().GetPersonByAccount(acc);
          if(personToAdd==null)
          {
            throw new Exception("No user object in source db exists");
          }
           POCO.Person personAdded=mng.GetPersonUOW().AddPerson(personToAdd);
        }
      }
        
      //GET
      [HttpGet]
      [Route("api/News2/{GUID_}/{offset}")]
      public IHttpActionResult GetNotes(string GUID_,int offset)
      {
        IHttpActionResult _response=null;
                 
        string res_ = mng.GetNotes(GUID_,offset);
            
        _response = new WebManagers.ReturnEntities(res_, Request);
        return _response;
      }
      //GET
      [HttpGet]
      [Route("api/News2/{offset}")]
      public IHttpActionResult GetNews(int offset)
      {
        IHttpActionResult _response=null;
                      
        string res_ = mng.GetNews(offset);
            
        _response = new WebManagers.ReturnEntities(res_, Request);
        return _response;
      }

      //POST news
      [HttpPost]
      [Route("api/News2")]
      public IHttpActionResult Post([FromBody] POCO.News note_)
      {
        IHttpActionResult _response=null;
        try{
          string res_=mng.PostNews(note_);
          _response = new WebManagers.ReturnEntities(res_, Request);
        } catch (Exception e){ _response = new WebManagers.ReturnEntities(e.Message, Request);}
        return _response;
      }     
        
      //POST comment
      [HttpPost]
      [Route("api/News2/{newsGUID_}")]
      public IHttpActionResult Post(string newsGUID_,[FromBody] POCO.Commentary comment_)
      {
        IHttpActionResult _response=null;
        try{            
          string res_ = mng.PostCommentary(newsGUID_,comment_);
          _response=new WebManagers.ReturnEntities(res_,Request);
        } catch (Exception e){ _response = new WebManagers.ReturnEntities(e.Message, Request);}
        return _response;
      }

       //PUT via PUT news
      [HttpPut]
      [Route("api/News2")]
      public IHttpActionResult Put([FromBody] POCO.Note note_)
      {
        IHttpActionResult _response=null;
        string res_ = mng.PutNote(note_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }

      //PUT via POST news
      [HttpPost]
      [Route("api/News2/update")]
      public IHttpActionResult Post([FromBody] POCO.Note note_)
      {
        IHttpActionResult _response=null;
        string res_ = mng.PutNote(note_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }
      
       //PUT via POST news
      [HttpPost]
      [Route("api/News2/GetParam")]
      public IHttpActionResult GetParam([FromBody] POCO.GETparameters params_)
      {
        IHttpActionResult _response=null;
        string res_ = mng.GetNewsHC(params_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }
    
    }
}
