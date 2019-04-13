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
	  
      Managers.Manager targetManager;

      public News2Controller()
      {
        string host_Test= string.Format("{0}:{1}"
        ,ConfigurationManager.AppSettings["OrientTargetHost"],ConfigurationManager.AppSettings["OrientPort"]);

         string host_Source= string.Format("{0}:{1}"
        ,ConfigurationManager.AppSettings["OrientSourceHost"],ConfigurationManager.AppSettings["OrientPort"]);

        targetManager = new Managers.Manager(
        ConfigurationManager.AppSettings["OrientUnitTestDB"]
        ,host_Test
        ,ConfigurationManager.AppSettings["orient_dev_login"]
        ,ConfigurationManager.AppSettings["orient_dev_pswd"]
        );

        Managers.Manager sourceManager = new Managers.Manager(
        ConfigurationManager.AppSettings["OrientSourceDB"]
        ,host_Source
        ,ConfigurationManager.AppSettings["orient_login"]
        ,ConfigurationManager.AppSettings["orient_pswd"]
        );

        acc=targetManager.UserAcc();

        if(acc==null)
        {
          throw new Exception("No account found");
        }

        if(targetManager.GetPersonUOW().GetPersonByAccount(acc)==null){
          POCO.Person personToAdd=sourceManager.GetPersonUOW().GetPersonByAccount(acc);
          if(personToAdd==null)
          {
            throw new Exception("No user object in source db exists");
          }
           POCO.Person personAdded=targetManager.GetPersonUOW().CreatePerson(personToAdd);
        }
      }
        
      //GET
      [HttpGet]
      [Route("api/News2/{GUID_}/{offset}")]
      public IHttpActionResult GetNotes(string GUID_,int offset)
      {
        IHttpActionResult _response=null;
                 
        string res_ = targetManager.GetNotes(GUID_,offset);
            
        _response = new WebManagers.ReturnEntities(res_, Request);
        return _response;
      }
      //GET
      [HttpGet]
      [Route("api/News2/{offset}")]
      public IHttpActionResult GetNews(int offset)
      {
        IHttpActionResult _response=null;
                      
        string res_ = targetManager.GetNews(offset);
            
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
          string res_=targetManager.PostNews(note_);
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
          string res_ = targetManager.PostCommentary(newsGUID_,comment_);
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
        string res_ = targetManager.PutNote(note_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }

      //PUT via POST news
      [HttpPost]
      [Route("api/News2/update")]
      public IHttpActionResult Post([FromBody] POCO.Note note_)
      {
        IHttpActionResult _response=null;
        string res_ = targetManager.PutNote(note_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }

       //PUT via POST2 news
      [HttpPost]
      [Route("api/News2/GetParam")]
      public IHttpActionResult GetParam([FromBody] POCO.GETparameters params_)
      {
        IHttpActionResult _response=null;
        string res_ = targetManager.GetNewsHC(params_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }


      //Tag section
      //POST like
      [HttpPost]
      [Route("api/News2/Like")]
      public IHttpActionResult Like([FromBody] POCO.Note params_)
      {
        IHttpActionResult _response=null;
        string res_ = targetManager.Like(params_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }
      //POST Dislike
      [HttpPost]
      [Route("api/News2/Dislike")]
      public IHttpActionResult Dislike([FromBody] POCO.Note params_)
      {
        IHttpActionResult _response=null;
        string res_ = targetManager.Dislike(params_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }
      
      //POST AddTags
      [HttpPost]
      [Route("api/News2/AddTags")]
      public IHttpActionResult AddTags([FromBody] IEnumerable<POCO.Tag> params_)
      {
        IHttpActionResult _response=null;
        string res_ = targetManager.AddTagList(params_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }
      //POST DeleteTags
      [HttpPost]
      [Route("api/News2/DeleteTags")]
      public IHttpActionResult DeleteTags([FromBody] IEnumerable<POCO.Tag> params_)
      {
        IHttpActionResult _response=null;
        string res_ = targetManager.DeleteTagList(params_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }


      //POST Tag
      [HttpPost]
      [Route("api/News2/ToTag")]
      public IHttpActionResult ToTag([FromBody] POCO.PostTags params_)
      {
        IHttpActionResult _response=null;
        string res_ = targetManager.AddTag(params_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }
      //POST UnTag
      [HttpPost]
      [Route("api/News2/UnTag")]
      public IHttpActionResult UnTag([FromBody] POCO.PostTags params_)
      {
        IHttpActionResult _response=null;
        string res_ = targetManager.UnTag(params_);
        _response = new WebManagers.ReturnEntities(res_, Request);

        return _response;
      }     

    }
}
