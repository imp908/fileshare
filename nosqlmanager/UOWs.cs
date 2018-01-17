using System;
using System.Collections.Generic;
using System.Linq;

using IUOWs;
using IQueryManagers;
using IWebManagers;
using POCO;
using OrientRealization;
using QueryManagers;

using JsonManagers;
using IJsonManagers;
using WebManagers;

using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;

using System.Net;
using System.Text;

using System.Reflection;
using System.IO;

namespace IUOWs
{
  using IOrientObjects;

  public interface IPersonUOW
  {
      IEnumerable<Person> GetAll();
      string GetByGUID(string GUID);
      IEnumerable<Person> GetObjByGUID(string GUID);
      string GetTrackedBirthday(string GUID);
      string AddTrackBirthday(E edge_, string guidFrom, string guidTo);
      string DeleteTrackedBirthday(E edge_, string guidFrom, string guidTo);
  }

  public interface IUOW
  {
    void BindRepo(IOrientRepo repo_);
    T UOWdeserialize<T>(string item_) where T : class, IorientDefaultObject;
    string UOWserialize<T>(IEnumerable<T> item_) where T : class, IorientDefaultObject;
    string UOWserialize<T>(T item_) where T : class, IorientDefaultObject;
    string UserAcc();
  }

  public class UOW : IUOW
  {
    internal IOrientRepo _repo;
      
    public UOW(IOrientRepo repo_)
    {
      BindRepo(repo_);
    }
    public void BindRepo(IOrientRepo repo_)
    {
      this._repo=repo_;
    }

    public string UserAcc()
    {
      return WebManagers.UserAuthenticationMultiple.UserAcc();
    }

    public string UOWserialize<T>(T item_)
        where T:class,IOrientObjects.IorientDefaultObject
    {
      string result = null;
      result = _repo.ObjectToContentString<T>(item_);
      return result;
    }
      
    public string UOWserialize<T>(IEnumerable<T> item_)
        where T:class,IOrientObjects.IorientDefaultObject
    {
      string result = null;
      result = _repo.ObjectToContentString<T>(item_);
      return result;
    }
    public T UOWdeserialize<T>(string item_)
        where T : class, IOrientObjects.IorientDefaultObject
    {
      T result = null;
      result = _repo.ContentStringToObject<T>(item_);
      return result;
    }

    string ConvertToBase64(string input)
    {
      return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(input));
    }
    string ConvertFromBase64(string input)
    {
      return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
    }
    
  }

}

namespace PersonUOWs
{  

  public class PersonUOW : IUOWs.UOW
  {
      
    public PersonUOW(IOrientRepo repo_)
      : base(repo_)
    {
    }

    public Person GetPersonByAccount(string accountName_)
    {
      Person result=null;
      var a=from s in _repo.Props<Person>().ToList() where s.Name=="sAMAccountName" select s;
      result=_repo.SelectSingle<Person>("sAMAccountName='" + accountName_+"'", null);
      return result;
    }
    public Person GetPersonByGUID(string GUID_)
    {
      Person result = null;            
      result = _repo.SelectSingle<Person>("GUID='" + GUID_ + "'", null);
      return result;
    }
    public Person GetPersonByID(string ID_)
    {
      Person result = null;            
      result = _repo.SelectSingle<Person>("@rid='" + ID_ + "'", null);
      return result;
    }

    public Person AddPerson(Person p_)
    {
      Person result = null;
        result = _repo.CreateVertex<Person>(p_,null);
      return result;
    }

  }

}

namespace NewsUOWs
{

  public class NewsRealUow:IUOWs.UOW
  {
        
    public NewsRealUow(IOrientRepo repo_)
      :base(repo_)
    {
    }
     
    public Note GetNoteByGUID(string GUID_)
    {
      Note result = null;
      result = _repo.SelectSingle<Note>("GUID='" + GUID_ + "'", null);
      return result;
    }     

    public News GetNewsByGUID(string GUID_)
    {      
      News result = null;
      result = _repo.SelectSingle<News>("GUID='" + GUID_ + "'", null);
      return result;
    }     
    public News GetNewsById(string id_)
    {
      News result = null;
      result = _repo.SelectFromType<News>("@rid=" + id_ , null).FirstOrDefault();
      return result;
    }
      
    public IEnumerable<Note> GetByOffset(string guid_, int? offset_=3)
    {
      IEnumerable<Note> result_=null;
      Note nt=_repo.SelectSingle<Note>("GUID='"+guid_+"'",null);
      if(nt!=null) {
        int startDepth=nt.commentDepth==null?0:(int)nt.commentDepth;
        int endDepth=offset_==null?startDepth:startDepth+(int)offset_;
        IEnumerable<Note> temRes=_repo.TraverseInID<Note>(nt.id, new List<Type>(){typeof(Comment)},null,null);
        if(temRes!=null){
          result_ = temRes.Where(s => (s.class_ == "Commentary" || s.class_ == "News")&&(s.commentDepth>=startDepth&&s.commentDepth<=endDepth))
          .OrderByDescending(c=>c.created);
        }
      }
      return result_;
    }
    public IEnumerable<TestNews> GetByOffsetTest(string guid_, int? offset_=3)
    {
      IEnumerable<TestNews> result_=null;
      TestNews nt=_repo.SelectByCondFromType<TestNews>(typeof(News),"GUID='"+guid_+"'",null).FirstOrDefault();
      if(nt!=null) {
        int startDepth=nt.commentDepth == null ? 0 : (int)nt.commentDepth;
        int endDepth=offset_==null?startDepth:startDepth+(int)offset_;        
        IEnumerable<TestNews> temRes = _repo.TraverseFrom<TestNews, Comment, Commentary, Authorship, Comment>(nt.id, null);
        if(temRes!=null){
          result_ = temRes.Where(s => (s.class_ == "Commentary" || s.class_ == "News")&&(s.commentDepth>=startDepth&&s.commentDepth<=endDepth));
        }
      }
      return result_;
    }

    [Obsolete]
    public IEnumerable<News> GetNewsByOffset(int? offset_=20)
    {
      IEnumerable<News> result=null;
      int _offset=(int)offset_;
        result=_repo.SelectFromType<News>(null,null).OrderBy(s=>s.created).Take(_offset);
      return result;
    }
    [Obsolete]
    public IEnumerable<Note> GetCommentsWithCommentsByOffset(string guid_,int? offset_=3)
    {
      IEnumerable<Note> result=null;
      Note nt = _repo.SelectSingle<Note>("GUID='" + guid_ + "'", null);
      if(nt!=null){
        if(nt.class_=="Commentary"){
          int depthfrom=nt.commentDepth==null?0:(int)nt.commentDepth;
          int _offset=(int)offset_;
          result=_repo.SelectFromTraverseWithOffset<Note, Comment, Commentary, Authorship, Comment>(nt.id,"commentDepth",depthfrom,_offset, "test_db");
        }
      }
      return result;
    }
    [Obsolete]
    public IEnumerable<Note> GetNewsWithCommentsByOffset(string guid_,int? offset_=3)
    {
      IEnumerable<Note> result=null;
      Note nt=_repo.SelectSingle<Note>("GUID='" + guid_ + "'", null);
      if(nt!=null){          
        int depthfrom=nt.commentDepth==null?0:(int)nt.commentDepth;
        int _offset=(int)offset_;
        result=_repo.SelectTraverseWithOffset<Note,Comment,Commentary,Authorship,Comment>(nt.id,"commentDepth",depthfrom,_offset, "test_db");          
      }
      return result;
    }

    public IEnumerable<Person> SearchByName(string Name_)
    {
      IEnumerable<Person> result=null;
      Name_ = Name_.ToLower();
      result=_repo
          .SelectFromType<Person>("Name.toLowerCase() like '%"+Name_+"%' or sAMAccountName.toLowerCase() like '%"+Name_+"%'or mail.toLowerCase() like '%"+Name_+"%'"
          ,null);
      return result;
    }        

    public IEnumerable<T> GetOrientObjects<T>(string cond_=null)
      where T : class, IOrientObjects.IorientDefaultObject
    {
      IEnumerable<T> result = null;
      result = _repo.SelectFromType<T>(cond_, null);
      return result;
    }
      
    public Person CheckPerson (Person person_)
    {
      Person result = null;
        result=_repo.SelectSingle<Person>("GUID='"+person_.GUID+"'",null);
        if(result==null)
        {
          string personContent = _repo.ObjectToContentString<Person>(person_);
          result=_repo.CreateVertex<Person>(personContent, null);
        }
      return result;
    }
    public IEnumerable<Person> GetPersonsWithNews(Note news_= null)
    {
      IEnumerable<Person> result=null;
        result = _repo.SelectInEOutV<Note, Authorship, Person>(news_,null);       
      return result;
    }

    public Commentary CreateCommentary(Person from,string newsId_,string comment_)
    {
      Authorship auth=new Authorship(){};
      Comment commented=new Comment(){};
      Commentary commentaryTochange_=null;
      Commentary commentaryToAdd_=null;
      News newsToComment_=_repo.SelectSingle<News>("@rid="+newsId_,null);
      from=CheckPerson(from);

      commentaryTochange_=_repo.OrientStringToObject<Commentary>(comment_);

      int? depth=IsCommentToComment(newsId_);
      //is comment to comment
      if (depth==null)
      {            
        commentaryTochange_.commentDepth=1;               
      }
      else
      {
        commentaryTochange_.commentDepth=(depth+1);
      }
      //commentary Node created and relation from person created
      commentaryToAdd_=CreateCommentary(from,commentaryTochange_);

      if (commentaryToAdd_!=null)
      {               
          if (newsToComment_!=null)
          {
              newsToComment_.hasComments=true;
              //create relation from commment to news Nodes
              _repo.CreateEdge<Comment>(commented,newsToComment_, commentaryToAdd_);
          }
          else
          {
              //unsuccesfull news search
              //manager.Delete<Note>(commentary_);
              //check if has comments if no then hasComments=false;
          }
      }

      return commentaryToAdd_;
    }
    public Commentary CreateCommentary(Person from,Commentary comment_,Note newsId_)
    {
      Authorship auth = new Authorship(){};
      Comment commented = new Comment(){};
      Commentary commentary_ = null;

      from=CheckPerson(from);        

      int? depth = IsCommentToComment(newsId_.id);
      if(depth==null)
      {
        //comment to news
        comment_.commentDepth=1;          
      }
      else
      {
        //comment to comment
        comment_.commentDepth=(depth+1);
      }

      if(from!=null){

      Note newsToComment_=_repo.SelectByIDWithCondition<Note>(newsId_.id,null,null).FirstOrDefault();
          
      if(newsToComment_!=null){
          
        comment_.PGUID=newsToComment_.GUID;
        //commentary Node created and relation from person created
        commentary_=CreateCommentary(from, comment_);
        
        //UpdateNews(from,commentary_);
        if (commentary_!=null)
        {          
          if (newsToComment_!=null)
          {
            //create relation from commment to news Nodes
            Comment commentedCr=_repo.CreateEdge<Comment>(commented,newsToComment_,commentary_);
            newsToComment_.hasComments=true;

            UpdateNote(newsToComment_);
          }
          else
          {
            //unsuccesfull news search
            //manager.Delete<Note>(commentary_);
          }
          
        }}}

    return commentary_;
    }
    public Commentary CreateCommentary(Person from,Commentary note_)
    {
      Authorship auth=new Authorship();
      Commentary nt_ = null;
      Authorship newAuth = null;

      if(from!=null){
              
        note_.authAcc=from.sAMAccountName;
        note_.authGUID=from.GUID;
        note_.authName=from.Name;
        note_.author_=from;

        nt_=_repo.CreateVertex<Commentary>(note_,null);
        newAuth=_repo.CreateEdge<Authorship>(auth,from,nt_);
        from=CheckPerson(from);

        //if unsucceced clean created objects
        if(newAuth==null||nt_==null)
        {
          _repo.Delete<Commentary>(note_,null,null);
          _repo.Delete<Authorship>(auth,null,null);
        }
      }
      return nt_;
    }

    public News CreateNews(Person from,string news_)
    {
        from=CheckPerson(from);
        News note_=_repo.CreateVertex<News>(news_,null);
        News created=CreateNews(from, note_);
        return created;
    }
    public News CreateNews(Person from_,Note note_)
    {
      News nt_=null;
      if(from_!=null ) {
      Authorship auth=new Authorship();

      note_.PGUID=from_.GUID;
      note_.authAcc=from_.sAMAccountName;
      note_.authGUID=from_.GUID;
      note_.authName=from_.Name;

        Person personfrom_=CheckPerson(from_);
        if(personfrom_!=null){
          nt_=_repo.CreateVertex<News>(note_,null);
          Authorship newAuth=_repo.CreateEdge<Authorship>(auth,personfrom_,nt_);
          nt_.author_=personfrom_;
          nt_.commentDepth = 0;
          UpdateNote(nt_);
          //if unsucceced clean created objects
          if(auth==null||note_==null)
          {         
            _repo.Delete<Note>(note_,null,null);
            _repo.Delete<Authorship>(auth,null,null);
          }
        }
      }

      return nt_;
    }     

    public News UpdateNews(Person from_,News noteFrom)
    {
  noteFrom.changed=DateTime.Now;
  News noteTo = _repo.SelectSingle<News>("GUID='" + noteFrom.GUID + "'");
  News noteToAdd=_repo.UpdateProperties<News>(noteFrom, noteTo);
  _repo.UpdateEntity<Note>(noteToAdd,null);
  News nt=_repo.SelectSingle<News>("GUID='"+noteFrom.GUID+"'",null);
  return nt;
    }
    public Commentary UpdateCommentary(Person from_,Commentary noteFrom)
    {
  noteFrom.changed=DateTime.Now;
  Commentary noteTo = _repo.SelectSingle<Commentary>("GUID='" + noteFrom.GUID + "'");
  Commentary noteToAdd=_repo.UpdateProperties<Commentary>(noteFrom, noteTo);
  _repo.UpdateEntity<Note>(noteToAdd,null);
  Commentary nt=_repo.SelectSingle<Commentary>("GUID='"+noteFrom.GUID+"'",null);
  return nt;
    }     
      
    /// <summary>
    /// Checks type and account. If commentary validates user by sAMAccountName. 
    /// </summary>
    /// <param name="from_">Person validate</param>
    /// <param name="noteFrom"></param>
    /// <returns></returns>
    public Note UpdateNotePersonal(Person from_,Note noteFrom)
    {
      Note result = null;
      bool update = true;
      if(noteFrom.GetType()==typeof(Commentary)){
        if(from_.sAMAccountName!=noteFrom.author_.sAMAccountName){
          update = false;
        }}
      if(update){
        result=UpdateNote(from_, noteFrom);
      }
      return result;
    }

    /// <summary>
    /// Updates property by property object to object from. Custom Updatable system attribute true false checked.
    /// </summary>
    /// <param name="from_">Object from</param>
    /// <param name="noteFrom">Object to</param>
    /// <returns></returns>
    public Note UpdateNote(Person from_,Note noteFrom)
    {
Note nt = null;
noteFrom.changed=DateTime.Now;
Note noteTo = _repo.SelectSingle<Note>("GUID='"+noteFrom.GUID+"'");
if(noteTo!=null){
Person oldAuthor = GetPersonsWithNews(noteTo).FirstOrDefault();  

noteTo=_repo.UpdateProperties<Note>(noteFrom,noteTo);

//pinned status recheck for datechange
if(noteFrom.pinned!=null){
  noteTo.pinned.isTrue = noteFrom.pinned.isTrue;
  noteTo.pinned.dateChanged = noteFrom.changed;
}
//published status recheck for datechange
if(noteFrom.published!=null){
  noteTo.published.isTrue = noteFrom.published.isTrue;
  noteTo.published.dateChanged = noteFrom.changed;
}
_repo.DeleteEdge<Authorship, Person, Note>(oldAuthor,noteFrom,null,null);

noteTo.author_=from_;
  
Authorship authNew=_repo.CreateEdge<Authorship>(new Authorship(),from_,noteTo,null);

noteTo.authName = from_.Name;
noteTo.authAcc = from_.sAMAccountName;
noteTo.authGUID = from_.GUID;

Note updatedEntity=_repo.UpdateEntity<Note>(noteTo,null);
nt=_repo.SelectSingle<Note>("GUID='"+noteFrom.GUID+"'",null);
}
return nt;
    }
    public Note UpdateNote(Note noteFrom)
    {
Note updatedEntity=_repo.UpdateEntity<Note>(noteFrom,null);
Note nt=_repo.SelectSingle<Note>("GUID='"+noteFrom.GUID+"'",null);
return nt;
    }

    //Postponned due to PUT realization
    public News NewsTogglePublish(string newsGUID_,bool published_)
    {
        News nt = _repo.SelectSingle<News>("GUID='"+newsGUID_+"'");
        nt.published.isTrue = true;
        nt.published.dateChanged = DateTime.Now;
        nt.changed = nt.published.dateChanged;
        return nt;
    }
    public News NewsTogglePinn(string newsGUID_,bool pinned_)
    {
        News nt = _repo.SelectSingle<News>("GUID='"+newsGUID_+"'");
        nt.pinned.isTrue = true;
        nt.pinned.dateChanged = DateTime.Now;
        nt.changed = nt.published.dateChanged;
        return nt;
    } 

    //<<<rewrite to getnewsHC
    public IEnumerable<News> GetNews(int? offset,bool? published_,bool? pinned_)
    {
      IEnumerable<News> result = null;
      int endDepth=offset==null?20:(int)offset;
        result = _repo.SelectByCondFromType<News>(typeof(News),null,null);
        
        if(published_!=null)
        {
          result = from s in result where s.published.isTrue==published_ select s;
        }
        if(pinned_!=null)
        {
          result = from s in result where s.pinned.isTrue==pinned_ select s;
        }
        if(result!=null){
          DateTime? mindate = (from s in result select s.created).Min();
          DateTime? maxdate = (from s in result select s.created).Max();
          result = result.OrderByDescending(s=>s.created);
          result = result.Take(endDepth);
        }       

      return result;
    }   
    public IEnumerable<News> GetPersonNews(Person p_=null)
    {
        return _repo.Select<Person,Authorship, News>(p_);
    }
    public IEnumerable<News> GetPersonNewsHC(int? offset,bool? published_,bool? pinned_,bool? asc,Person p_=null)
    {
      string cond_=null;
      
      if(published_!=null){ cond_ += " and published.isTrue=" + published_; }
      if(pinned_!=null){ cond_ += " and pinned.isTrue=" + pinned_; }
      if(p_!=null){ cond_ += " and in('Authorship')[0].GUID='"+ p_.GUID +"'"; }
      if(asc==null || asc==true ){ cond_ += " order by Changed asc "; }else{cond_ += " order by Changed desc";}

      int val = 0;
      int.TryParse(offset.ToString(), out val);
      if(val>0){
        if(offset!=null){ cond_ += " limit " + val; }
      }

      return _repo.SelectByCondFromType<News>(typeof(News),cond_,null);

    }

    
    public Note GetNoteByID(string NewsId)
    {
        Note ret_=null;
          ret_=_repo.SelectByIDWithCondition<Note>(NewsId,null,null).FirstOrDefault();         
        return ret_;
    }

    public string DeleteNews(Person from, string id_)
    {
        string result = string.Empty;
        from=CheckPerson(from);
        News ntd = GetNewsById(id_);

        if (ntd != null) {
          string deleteN=_repo.DeleteEdge<Authorship,Person,News>(from,ntd,null,null).GetResult();
          string deleteR=_repo.Delete<News>(ntd,null,null).GetResult();
          if(deleteN=="Deleted"&&deleteR == "Deleted") { result = "Deleted"; }
        }
        return result;
    }
      
    /// <summary>
    /// Check inE types on Comment,Authorship. If has inE comment, then returns current Note.
    /// </summary>
    /// <param name="NewsId">Npte which type need to be checked</param>
    /// <returns></returns>
    public int? IsCommentToComment(string NewsId)
    {
      int? depth=null;
      Note nt=_repo.SelectByIDWithCondition<Note>(NewsId,null,null).FirstOrDefault();
      if (nt.class_=="Commentary")
      {
        depth=nt.commentDepth;
      }else {depth=null;}
      return depth;
    }   
   
  }

}

namespace Managers
{

  using NewsUOWs;
  using PersonUOWs;
  using OrientRealization;
  using IUOWs;

  public class Manager
  {
    //Base functionality
    IOrientRepo _repo;
    NewsRealUow _newsUOW;
    PersonUOW _personUOW;

    /// <summary>
    /// Dictionary for storing new UOWs
    /// </summary>
    Dictionary<string, IUOW> uows = new Dictionary<string, IUOW>();

    string _dbName;
    string _dbHost;
    
    public Manager(string dbName_=null,string url_=null,string login_=null,string password_=null)
    {   
    
      string login =string.IsNullOrEmpty(login_)?ConfigurationManager.AppSettings["orient_login"]:login_;
      string password=string.IsNullOrEmpty(password_)?ConfigurationManager.AppSettings["orient_pswd"]:password_;

      if(string.IsNullOrEmpty(login)||string.IsNullOrEmpty(password)){throw new Exception("Credentials not passed");}

      try{

        _dbName=string.IsNullOrEmpty(dbName_) 
        ?ConfigurationManager.AppSettings["OrientUnitTestDB"]
        :dbName_ ;
            
        _dbHost=string.IsNullOrEmpty(url_)
        ?string.Format("{0}:{1}",ConfigurationManager.AppSettings["OrientDevHost"],ConfigurationManager.AppSettings["OrientPort"])
        :url_;
      
        if(string.IsNullOrEmpty(_dbName)){throw new Exception("Database name is not defined");}
        if(string.IsNullOrEmpty(_dbHost)){throw new Exception("Host is not defined");}

        _repo=RepoFactory.NewOrientRepo(_dbName, _dbHost, login, password);
        initializeUOWs();      

      }catch(Exception e){
        System.Diagnostics.Trace.WriteLine(e.Message);
      }

    }

    public IOrientRepo GetRepo()
    {
      return this._repo;
    }
    public NewsRealUow GetNewsUOW()
    {
      return this._newsUOW;
    }
    public PersonUOW GetPersonUOW()
    {
      return this._personUOW;
    }

    void initializeRepo(string dbName_,string url_,string login,string password)
    {
      _repo=RepoFactory.NewOrientRepo(dbName_, url_, login, password);
      initializeUOWs();
    }
    void initializeUOWs()
    {
      _newsUOW=new NewsRealUow(this._repo);
      _personUOW=new PersonUOW(this._repo);
    }
    
    public void BindUOW(IUOW uow_,string UOWname)
    {
      IUOW res = null;
      if(!uows.ContainsKey(UOWname))
      {        
        res = uow_;     
        uows.Add(UOWname, res);        
      }
    }
    public IUOW GetUOW(string name_)
    {
      IUOW res = null;
      if(uows.ContainsKey(name_))
      {
        uows.TryGetValue(name_, out res);
      }
      return res;
    }    

    /// <summary>
    /// External repository binding
    /// </summary>
    /// <param name="repo_"></param>
    void bindRepo(IOrientRepo repo_)
    {
      this._repo=repo_;
      _newsUOW.BindRepo(_repo);
      _personUOW.BindRepo(_repo);  
    }
    
    public string UserAcc()
    {
      return WebManagers.UserAuthenticationMultiple.UserAcc();
    }
    
    public string GetNotes(string GUID_,int offset)
    {
      string res_ = null;
      IEnumerable<POCO.Note> pn=_newsUOW.GetByOffset(GUID_,offset);            
      res_ = _newsUOW.UOWserialize<POCO.Note>(pn);
      return res_;
    }

    public string GetNews(int offset)
    {
      string res_ = null;
            
      IEnumerable<POCO.News> pn=_newsUOW.GetNews(offset,null,null);            
      res_ = _newsUOW.UOWserialize<POCO.News>(pn);

      return res_;
    }
    public string GetNewsHC(GETparameters gp)
    {
      string res_ = null;
            
      IEnumerable<POCO.News> pn=_newsUOW.GetPersonNewsHC(gp.offest,gp.published,gp.pinned,gp.asc,gp.author);            
      res_ = _newsUOW.UOWserialize<POCO.News>(pn);

      return res_;
    }

    public string GetNews(int? offset,bool? published_,bool? pinned_)
    {
      string res_ = null;
            
      IEnumerable<POCO.News> pn=_newsUOW.GetNews(offset,published_,pinned_);            
      res_ = _newsUOW.UOWserialize<POCO.News>(pn);

      return res_;
    }

    public string PostNews(POCO.News note_,string acc_=null)
    {
      string res_ = null;
        
        string acc =acc_==null?UserAcc():acc_;
        
        Person person_=_personUOW.GetPersonByAccount(acc);
      
        if(person_==null){ throw new Exception("user not found for acc: " + acc); }

        News pn=_newsUOW.CreateNews(person_, note_);
        if(pn==null||pn.GUID==null){ throw new Exception("news not created for person " + person_.sAMAccountName); }

        res_=_newsUOW.UOWserialize(pn);

      return res_;
    }

    public string PostCommentary(string newsGUID_,POCO.Commentary commentary_)
    {
      string res_ = null;

        string acc=_newsUOW.UserAcc();
        Person person_=_personUOW.GetPersonByAccount(acc);
        if(person_==null){ throw new Exception("user not found for acc: " + acc); }

        POCO.Note news_=_newsUOW.GetNoteByGUID(newsGUID_);
        if(news_==null){ throw new Exception("note not found for GUID" + newsGUID_); }

        POCO.Commentary pn=_newsUOW.CreateCommentary(person_,commentary_,news_ );
        if(pn==null){ throw new Exception("commentary not created for person:"+pn); }

        res_=_newsUOW.UOWserialize<POCO.Commentary>(pn);

      return res_;
    }

    public string PutNote(POCO.Note note_)
    {
      string res_ = null;      
      string acc=_newsUOW.UserAcc();

      Person person_=_personUOW.GetPersonByAccount(acc);
      POCO.Note pn=_newsUOW.UpdateNotePersonal(person_,note_);
      res_=_newsUOW.UOWserialize<POCO.Note>(pn);

      return res_;
    }

    //DATABASE BOILERPLATE
    public void GenDB(bool cleanUpAter=false)
    {                      
        //ManagerCheck(manager);

        //node objects for insertion
        Person personOne =
new Person(){Seed=123,Name="0",GUID="000",changed=new DateTime(2017,01,01,00,00,00),created=new DateTime(2017,01,01,00,00,00)};
        Person personTwo=
new Person(){Seed=456,Name="0",GUID="001",changed=new DateTime(2017,01,01,00,00,00),created=new DateTime(2017,01,01,00,00,00)};
        MainAssignment mainAssignment=new MainAssignment();
        string pone=_repo.ObjectToContentString<Person>(personOne);
        List<Person> personsToAdd = new List<Person>() {
new Person(){
Seed =123,Name="Neprintsevia",sAMAccountName="Neprintsevia"
,changed=new DateTime(2017,01,01,00,00,00),created=new DateTime(2017,01,01,00,00,00)
}
//,new Person(){Seed =123,Name="YablokovAE",sAMAccountName="YablokovAE",changed=new DateTime(2017,01,01,00,00,00),created=new DateTime(2017,01,01,00,00,00)}      
};

        Unit u = new Unit() { Name = "Unit1" };

        for(int i=0;i<=10;i++)
        {
          personsToAdd.Add(
            new Person(){sAMAccountName="Person"+i,Name="Person"+i,GUID="GUID"+i}
            );
        }                 

        //db delete
        _repo.DeleteDb();

        //db crete
        _repo.CreateDb();

        _repo.DbPredefinedParameters();

        //create class
        Type oE=_repo.CreateClass<OrientEdge,E>();
        Type maCl=_repo.CreateClass<MainAssignment,E>();
        Type obc=_repo.CreateClass<Object_SC, V>();
        Type tp=_repo.CreateClass<Unit, V>();
        //Type tpp = manager.CreateClass<Note, V>("news_test5");
        Type nt=_repo.CreateClass<Note, V>();
            
        Type cmt=_repo.CreateClass<Commentary,Note>();
        Type nws=_repo.CreateClass<News,Note>();
        Type auCl=_repo.CreateClass<Authorship,E>();
        Type cmCl=_repo.CreateClass<Comment,E>();

        Note ntCl=new Note();
        Note ntCl0=new Note(){name="test name",content="test content"};
        Object_SC obs = new Object_SC() { GUID="1", changed=DateTime.Now, created=DateTime.Now, disabled=DateTime.Now };
        News ns = new News() {name="Real news"};
        Commentary cm = new Commentary() {name="Real comment"};         

        _repo.CreateClass("Person","V",null);
        MainAssignment ma = new MainAssignment() { };

        //create property
        //will not create properties - not initialized object all property types anonimous.
        _repo.CreateProperty<OrientEdge>(null, null);
        //create all properties even if all null.
        _repo.CreateProperty<MainAssignment>( new MainAssignment(), null);
        _repo.CreateProperty<Unit>( new Unit(), null);
        _repo.CreateProperty<Note>( new Note(), null);
        _repo.CreateProperty<Authorship>( new Authorship(), null);
        _repo.CreateProperty<Comment>( new Comment(), null);
        _repo.CreateProperty<Commentary>( new Commentary(), null);
        _repo.CreateProperty<News>( new News(), null);
        _repo.CreateProperty<Person>(personOne, null);
        //create single property from names
        //manager.CreateProperty("Unit", "Name", typeof(string), false, false);

        _repo.CreateVertex<Note>(ntCl,null );
        _repo.CreateVertex<Object_SC>(obs,null );

        _repo.CreateVertex<News>(ns,null);
        _repo.CreateVertex<Commentary>(cm,null);

        //add node
        Person p0 = _repo.CreateVertex<Person>(personTwo,null );        
        _repo.CreateVertex("Unit", "{\"Name\":\"TestName\"}",null);
        Unit u0 = _repo.CreateVertex<Unit>(u,null );

        //add test person
        foreach (Person prs in personsToAdd)
        {
          Person p = _repo.CreateVertex<Person>(prs, null);                
        }


        //add relation
        MainAssignment maA=_repo.CreateEdge<MainAssignment>(mainAssignment,p0, u0,null );
            
        //select from relation
        IEnumerable<MainAssignment> a = _repo.SelectFromType<MainAssignment>("1=1",null );

        Note ntCr=_repo.CreateVertex<Note>(ntCl0, null);
        Authorship aut=new Authorship();
        Authorship aCr=_repo.CreateEdge<Authorship>(aut,p0,ntCr,null);

        IEnumerable<Note> notes=
        _repo.SelectFromTraverseWithOffset<Note, Comment, Commentary, Authorship, Comment>
        (ntCr.id,"commentDepth",0,2);

        if (cleanUpAter)
        {
          //delete edge
          string res = _repo.DeleteEdge<Authorship, Person, Note>(p0, ntCr).GetResult();
          //Delete concrete node
          res = _repo.Delete<Unit>(u0).GetResult();
          //delete all nodes of type
          res = _repo.Delete<Person>().GetResult();

          //db delete
          _repo.DeleteDb();
        }
    }
    //GENERATE NEWS,COMMENTS
    public void GenNewsComments(bool newsGen=false)
    {

      List<Person> personsAdded = new List<Person>();
      List<News> newsAdded = new List<News>();
      List<Commentary> commentaryAdded = new List<Commentary>();

      List<V> nodes = new List<V>();

      string dtStr="{\"transaction\":true,\"operations\":[{\"type\":\"script\",\"language\":\"sql\",\"script\":[\" create Vertex Person content {\"Seed\":1005911,\"FirstName\":\"Илья\",\"LastName\":\"Непринцев\",\"MiddleName\":\"Александрович\",\"Birthday\":\"1987-03-09 00:00:00\",\"mail\":\"Neprintsevia@nspk.ru\",\"telephoneNumber\":1312,\"userAccountControl\":512,\"objectGUID\":\"7E3E2C99-E53B-4265-B959-95B26CA939C8\",\"sAMAccountName\":\"Neprintsevia\",\"OneSHash\":\"5de9dfe2c74b5eef8e13f4f43163f445\",\"Hash\":\"f202a15b6dac384aecbd2424dcca10a7\",\"@class\":\"Person\",\"Name\":\"Непринцев Илья Александрович\",\"GUID\":\"ba124b8e-9857-11e7-8119-005056813668\",\"Created\":\"2017-09-13 07:15:29\",\"Changed\":\"2017-12-05 10:07:19\"} \"]}]}";
      byte[] bt=Encoding.UTF8.GetBytes(dtStr);
      string dtStrRegen = Encoding.UTF8.GetString(bt, 0, bt.Count());
      bool res = dtStr.Equals(dtStrRegen);

      string sourceHost = string.Format("{0}:{1}"
      ,ConfigurationManager.AppSettings["OrientDevHost"],ConfigurationManager.AppSettings["OrientPort"]);

      //Add custom Unit of work for receiving news_test5 users from dev db.
      PersonUOWs.PersonUOW _sourceUOW=
      new PersonUOWs.PersonUOW(RepoFactory.NewOrientRepo(
      ConfigurationManager.AppSettings["OrientSourceDB"]
      ,sourceHost
      ,ConfigurationManager.AppSettings["orient_login"]
      ,ConfigurationManager.AppSettings["orient_pswd"]));    


      Person person_=_sourceUOW.GetPersonByAccount("Neprintsevia");
      string str=_newsUOW.UOWserialize<Person>(person_);

      Person pr=_repo.CreateVertex<Person>(str);

      personsAdded=_newsUOW.GetOrientObjects<Person>(null).ToList();         
       
      Random rnd=new Random();
      int persCnt=(int)rnd.Next(5,10);
      int newsCnt=(int)rnd.Next(5,10);
      int commentaryCnt=(int)rnd.Next(5,10);

      //get news with commentaries
      //news + comments
      //comment + coments
      //IEnumerable<Note> notes_=uow.GetByOffset("82f83601-d5cd-4108-b1b7-d27ac5a3933a",3);

      IEnumerable<News> news_=_newsUOW.GetNews(10,null,null);
          
      if(newsGen) {
        //news add
        for(int i=0;i<personsAdded.Count()-1;i++)
        {
              
          newsCnt=(int)rnd.Next(0,3);
          for(int i2=0;i2<newsCnt;i2++)
          {
            newsAdded.Add(
              _newsUOW.CreateNews(personsAdded[i],new News(){name="News"+i2,content="fucking interesting news",pic=string.Empty})
            );
          }
        }
      }          

      //commentaries gen
      nodes.AddRange(
        _newsUOW.GetOrientObjects<News>(null).ToList()
      );

      //rand, comments gen
      for(int i=0;i<personsAdded.Count()-1;i++)
      {
        commentaryCnt=(int)rnd.Next(8,15);
        for(int i2=0;i2<commentaryCnt;i2++){
          int nodeToCommentId=(int)rnd.Next(0,nodes.Count()-1);
          Note nodeToComment=_newsUOW.GetNoteByID(nodes[nodeToCommentId].id);
          nodes.Add(
            _newsUOW.CreateCommentary(personsAdded[i],new Commentary(){name="Commentary"+i2,content="fucking bullshit comentary"},nodeToComment)
          );
        }

      }


      //crazy gen
      nodes.Clear();

      string crazyComment=
      "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur pretium nibh dolor, ac ornare dui malesuada sed. Nam congue suscipit lectus in dapibus. Fusce pharetra urna a vehicula sollicitudin. Ut vel elit dolor. In hac habitasse platea dictumst. Proin tristique sem quis neque vehicula pellentesque. Ut magna tellus, condimentum ut sodales sit amet, efficitur eu magna.</p><figure class=\"imageimgNews\" style=\"float:left\"><img alt=\"\" height=\"123\" src=\"http://static.nspk.ru/files/42e65b812d1d4735c2d44528837c24b542408fb12eac9b10086d021a0f091f222e372f4ffe20e3c02703ce9a7698e02a55280a257b5876315cf5714204241302/1(1).jpg\" width=\"274\" /><figcaption>Название</figcaption></figure><p>&nbsp;</p><p>Donec lectus nibh, aliquam vitae semper vel, interdum et dolor. Donec vel metus vitae magna scelerisque varius sit amet sed elit. Vivamus lacinia accumsan lectus sit amet commodo. Sed porttitor ullamcorper fermentum. Donec ut facilisis purus. Nullam tempor, risus id maximus posuere, odio nibh suscipit ante, eget semper eros quam consectetur tellus. Nullam aliquam volutpat blandit. Nulla pharetra ultricies aliquam. Integer in tortor a quam imperdiet venenatis id eu sapien. Aenean nec eros arcu. Maecenas ac consequat justo. Proin quis aliquam justo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Quisque posuere libero et orci ornare vestibulum. Mauris ut varius odio. Nullam sed pulvinar felis.</p><p>Nulla facilisi. Etiam quis nisl libero. Integer eu porta mi. Sed a iaculis enim. Praesent pharetra odio ipsum, ac tempus ligula bibendum id. Nam aliquet odio ac erat mattis, quis commodo tellus posuere. Etiam congue ex at est dignissim, eget lacinia purus aliquet. Morbi in vehicula mauris. Sed eu vulputate mauris, varius porttitor purus. Duis placerat sed nisl a tempor. In congue lacus in orci convallis, vel aliquam arcu tincidunt. Nullam eleifend efficitur purus, id fermentum lectus semper at. Fusce dapibus arcu eget est dignissim porttitor.</p><p>Phasellus in mauris quis felis maximus euismod at vel urna. Vivamus sit amet malesuada ligula. Aliquam erat volutpat. Nunc suscipit bibendum interdum. Etiam cursus ligula eu dictum malesuada. Donec quis libero ac purus porttitor maximus in non orci. Vestibulum dictum eros dolor, et commodo libero tempor a. Integer viverra faucibus scelerisque. Etiam fermentum arcu non diam vulputate,</p><figure class=\"imageimgNews\" style=\"float:left\"><img alt=\"\" height=\"156\" src=\"http://static.nspk.ru/files/0c2af2d09824cdf5649115cfd33399d15d19ef4d974e1a182051ba5034e5e9594432d71fc5286ec70a3e4477d067df089fbeb5369cd11a35f090a1db64f55b34/3.jpg\" width=\"206\" /><figcaption>Название</figcaption></figure><p>&nbsp;</p><p>nec pellentesque dolor commodo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Quisque malesuada facilisis erat quis aliquet.</p><p>Nullam dapibus orci ac vehicula suscipit. Sed at lectus condimentum, scelerisque nulla eu, vehicula neque. Praesent nec massa sagittis massa blandit vehicula quis eget sem. Sed porta vitae purus nec facilisis. Praesent sed lectus id eros elementum placerat nec quis mauris. Quisque eget orci eget odio efficitur elementum eu non orci. Ut orci nunc, congue ut nibh non, molestie tincidunt eros. Quisque vel sagittis nisl. Aliquam volutpat efficitur enim, a congue lectus euismod vel. Maecenas porta orci vel arcu semper, sed ullamcorper arcu rutrum. Donec dignissim sem nec sagittis finibus. Nulla vitae mauris eu urna facilisis rutrum. Nunc elementum magna quis nisl aliquet commodo. Vivamus vel tempor lectus. Fusce ac odio commodo, rutrum lorem id, blandit lectus.</p><p>Sed mollis ex quam, interdum porta eros tincidunt non. Sed sagittis cursus ligula, eu ultrices quam eleifend eu. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam non commodo quam. Integer at lorem gravida, consectetur mauris sit amet, tincidunt massa. Pellentesque ac velit justo. Nulla facilisi. Maecenas eu nisi rutrum, convallis nisi sit amet, tincidunt dui. Suspendisse facilisis ornare venenatis. Nam porttitor tellus at mauris hendrerit gravida. Maecenas quis quam eget orci ullamcorper lacinia id id dui. Sed eget sagittis diam. Vestibulum pharetra est nec pretium euismod.</p><p>Ut ullamcorper, odio eu tempor lobortis, lectus nisi molestie leo, quis accumsan felis nisi sed ligula. Aliquam vitae quam a lorem accumsan tempus vel ac sem. Proin pulvinar ornare sagittis. Nulla vitae dictum purus. Vivamus rutrum auctor tincidunt. Curabitur nec eros leo. Morbi ullamcorper dictum elit, in sodales nisl lobortis vitae. Donec vitae odio dapibus, dignissim augue sed, accumsan nunc. Morbi at neque velit. Suspendisse potenti. Quisque consectetur, sem faucibus luctus scelerisque, justo ipsum venenatis tortor, sed ullamcorper risus ante non diam. Vivamus et tortor sed est imperdiet condimentum id eget sapien. Morbi in nulla vel mi dignissim vulputate. Vivamus erat lectus, laoreet non quam sed, aliquam tempus eros. Suspendisse in justo quis nisi dictum blandit. Vestibulum interdum, turpis non placerat ultricies, purus ipsum faucibus ipsum, id venenatis ex diam eget nunc.</p><p>Mauris diam lectus, bibendum in sapien nec, tempor malesuada odio. In rhoncus ornare purus, vitae facilisis orci iaculis in. Suspendisse potenti. Proin pharetra ut risus eget ullamcorper. Curabitur sit amet interdum magna. Sed fringilla lobortis ex, vel maximus quam semper eu. Nullam pretium sapien sit amet ex posuere luctus. Etiam condimentum tellus vel metus luctus dignissim. Donec et felis id leo convallis auctor vitae tincidunt nisl. Fusce dignissim varius orci vel hendrerit. Fusce gravida turpis odio, non sed.</p>";
      Person yab=_sourceUOW.GetPersonByAccount("YablokovAE");
            List<News> crazyNews = new List<News>();
      for(int i=0;i<30;i++){
        nodes.Add(
          _newsUOW.CreateNews(yab, new News() { name = "News" + i, content = crazyComment })
        );
 
      }

      //rand, comments gen
      for(int i2=0;i2<personsAdded.Count()-1;i2++)
      {
        commentaryCnt=(int)rnd.Next(8,15);
        for(int i3=0;i3<commentaryCnt;i3++){
          int nodeToCommentId=(int)rnd.Next(0,nodes.Count()-1);
          Note nodeToComment=_newsUOW.GetNoteByID(nodes[nodeToCommentId].id);
          nodes.Add(
            _newsUOW.CreateCommentary(personsAdded[i2],new Commentary(){name="Commentary"+i3,content="fucking bullshit comentary"},nodeToComment)
          );
        }

      }

      string location_ = Assembly.GetExecutingAssembly().Location;
      string path_ = Directory.GetParent(location_).ToString() + "\\nodes.json";
      File.WriteAllText(path_, JsonConvert.SerializeObject(nodes,Formatting.Indented));
        
    }

    public void DeleteDB()
    {
      _repo.DeleteDb(_dbName);
    }

  }

}



namespace AdinTce
{

    #region AdinTceRepo

    //adin tce repository
    public class AdinTceRepo
    {

        IQueryManagers.ICommandBuilder _CommandBuilder;
        IWebManagers.IWebRequestManager _webManager;
        IWebManagers.IResponseReader _responseReader;
        IJsonManagers.IJsonManger _jsonManager;

        IQueryManagers.ITypeToken GUIDtoken;

        AdinTceExplicitTokenBuilder tokenBuilder;

        AdinTcePOCO adp=new AdinTcePOCO();
        List<Holiday> holidays=new List<Holiday>();
        List<Vacation> vacations=new List<Vacation>();
        IEnumerable<GraphRead> graphs=new List<GraphRead>();
        IEnumerable<GUIDPOCO> guidpocos=new List<GUIDPOCO>();

        string holidayCommand, vacationCommand, graphCommand,
             holidaysResp=null, vacationsResp=null, graphResp=null;

        public AdinTceRepo(
            IQueryManagers.ICommandBuilder CommandBuilder_,
            IWebManagers.IWebRequestManager webManager_,
            IWebManagers.IResponseReader responseReader_,
            IJsonManagers.IJsonManger jsonManager_)
        {
          this._CommandBuilder=CommandBuilder_;
          this._webManager=webManager_;
          this._responseReader=responseReader_;
          this._jsonManager=jsonManager_;

          GUIDtoken=new AdinTceGUIDToken();
          tokenBuilder=new AdinTceExplicitTokenBuilder();
       }

        public AdinTceRepo()
        {

          this._CommandBuilder=new AdinTceCommandBuilder(new TokenMiniFactory(), new FormatFactory());
          this._webManager=new AdinTceWebManager();
          this._responseReader=new AdinTceResponseReader();
          this._jsonManager=new AdinTceJsonManager();

          _webManager.SetCredentials(new System.Net.NetworkCredential(
              ConfigurationManager.AppSettings["AdinTceLogin"], ConfigurationManager.AppSettings["AdinTcePassword"]));

          GUIDtoken=new AdinTceGUIDToken();
          tokenBuilder=new AdinTceExplicitTokenBuilder();
        }
        public string HoliVation(string GUID)
        {

            string result=string.Empty;

            GUIDtoken.Text=GUID;

            _CommandBuilder.SetText(tokenBuilder.HolidaysCommand(GUIDtoken), new AdinTcePartformat());
            holidayCommand=_CommandBuilder.GetText();
            _CommandBuilder.SetText(tokenBuilder.VacationsCommand(GUIDtoken), new AdinTcePartformat());
            vacationCommand=_CommandBuilder.GetText();
            _CommandBuilder.SetText(tokenBuilder.GraphCommand(GUIDtoken), new AdinTcePartformat());
            graphCommand=_CommandBuilder.GetText();

            GetResp();

            ParseResponseTry();

            result=_jsonManager.SerializeObject(adp);

            return result;
        }

        void AdpCheck()
        {
            if (adp==null)
            {
                adp=new AdinTcePOCO();
            }
        
            if (guidpocos.Count()==0&&(holidaysResp!=null||holidaysResp!=string.Empty))
            {
                guidpocos=_jsonManager.DeserializeFromParentNode<GUIDPOCO>(holidaysResp);
            }           
            if (holidays.Count()==0&&(holidaysResp!=null||holidaysResp!=string.Empty))
            {
                IEnumerable<List<AdinTce.Holiday>> hl = _jsonManager.DeserializeFromParentChildren<List<Holiday>>(holidaysResp, "Holidays");                
                foreach (List<Holiday> lt_ in hl)
                {                   
                    holidays.AddRange(lt_);                                      
                }                
            }
            if (vacations.Count()==0&&(vacationsResp!=null&&vacationsResp!=string.Empty))
            {

                IEnumerable<List<AdinTce.Vacation>> hl = _jsonManager.DeserializeFromParentChildren<List<Vacation>>(vacationsResp, "Holidays");
                vacations = new List<Vacation>();
                foreach (List<Vacation> lt_ in hl)
                {
                    vacations.AddRange(lt_);
                }

            }
            if (adp.GUID_==null)
            {
                adp.GUID_=guidpocos.Select(s => s).FirstOrDefault().GUID_;
            }
            if (adp.holidays==null)
            {
                adp.Position=guidpocos.Select(s => s).FirstOrDefault().Position;
            }

        }
        void GetResp()
        {
            Parallel.Invoke(new Action[] {HolidaysResp, VacationsResp, GraphResp});
        }
        private async Task<string> Request(string command_)
        {
            _webManager.AddRequest(command_);
            Task<string> task_=Task.Run(
                    () =>
                        _responseReader.ReadResponse(_webManager.GetResponse("GET"))
                );
            await task_;
            return task_.Result;
        }
        void ParseResponseTry()
        {

            if (holidaysResp != null && (holidaysResp != null ||holidaysResp!=string.Empty))
            {
                AdpCheck();
                try
                {
                    adp.holidays=holidays.ToList();
                }
                catch (Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}
            }

            if (vacationsResp != null && (vacationsResp!=null||vacationsResp != string.Empty))
            {
                AdpCheck();
                try
                {
                    adp.vacations=vacations.ToList();
                }
                catch (Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}
            }

            if (graphResp != null && (graphResp!=null|| graphResp != string.Empty))
            {

                AdpCheck();
                try
                {
                    graphs=_jsonManager.DeserializeFromParentChildren<GraphRead>(graphResp, "Holidays");
                    adp.Graphs=GrapthReadToWriteDateCheck(graphs.ToList());
                }
                catch (Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}
            }
        }
        public List<GraphWrite> GrapthReadToWriteDateCheck(List<GraphRead> ghl_)
        {
            List<GraphWrite> gw=new List<GraphWrite>();
            foreach (GraphRead gr in ghl_)
            {                
                GraphWrite gfw=new GraphWrite();
                if (gr.DateFinish==new DateTime()) {gfw.DateFinish=null;} else {gfw.DateFinish=gr.DateFinish;}
                if (gr.DateStart==new DateTime()) {gfw.DateStart=null;} else {gfw.DateStart=gr.DateStart;}
                gfw.LeaveType=gr.LeaveType;
                gfw.DaysSpent=gr.DaysSpent;

                gw.Add(gfw);

            }
            return gw;
        }

        void HolidaysResp()
        {
            AdinTceWebManager webManagerAc=new AdinTceWebManager();
            webManagerAc.SetCredentials(new System.Net.NetworkCredential(
             ConfigurationManager.AppSettings["AdinTceLogin"], ConfigurationManager.AppSettings["AdinTcePassword"]));
            webManagerAc.AddRequest(holidayCommand);
            holidaysResp=_responseReader.ReadResponse(webManagerAc.GetResponse64("GET"));

        }
        void VacationsResp()
        {
            AdinTceWebManager webManagerAc=new AdinTceWebManager();
            webManagerAc.SetCredentials(new System.Net.NetworkCredential(
             ConfigurationManager.AppSettings["AdinTceLogin"], ConfigurationManager.AppSettings["AdinTcePassword"]));
            webManagerAc.AddRequest(vacationCommand);
            vacationsResp=_responseReader.ReadResponse(webManagerAc.GetResponse64("GET"));

        }
        void GraphResp()
        {
            AdinTceWebManager webManagerAc=new AdinTceWebManager();
            webManagerAc.SetCredentials(new System.Net.NetworkCredential(
             ConfigurationManager.AppSettings["AdinTceLogin"], ConfigurationManager.AppSettings["AdinTcePassword"]));
            webManagerAc.AddRequest(graphCommand);
            graphResp=_responseReader.ReadResponse(webManagerAc.GetResponse64("GET"));
        }

    }

    #endregion

    //Command build from Tokens,with explicit sytax for repo call
    public class AdinTceExplicitTokenBuilder
    {

        public List<IQueryManagers.ITypeToken> HolidaysCommand(IQueryManagers.ITypeToken GUID)
        {
            List<IQueryManagers.ITypeToken> result=new List<IQueryManagers.ITypeToken>()
            {new AdinTceURLToken(), new AdinTceHolidatyToken(), new AdinTcePartToken(), GUID};
            return result;
        }
        public List<IQueryManagers.ITypeToken> VacationsCommand(IQueryManagers.ITypeToken GUID)
        {
            List<IQueryManagers.ITypeToken> result=new List<IQueryManagers.ITypeToken>()
            {new AdinTceURLToken(), new AdinTceVacationToken(), new AdinTcePartToken(), GUID};
            return result;
        }

        public List<IQueryManagers.ITypeToken> GraphCommand(IQueryManagers.ITypeToken GUID)
        {
            List<IQueryManagers.ITypeToken> result=new List<IQueryManagers.ITypeToken>()
            {new AdinTceURLToken(), new AdinTceGraphToken(), new AdinTcePartToken(), GUID};
            return result;
        }
    }

    #region AdinTceTokens

    //AdinTce Tokens
    public class AdinTceURLToken : IQueryManagers.ITypeToken
    {
        public string Text {get; set;}=ConfigurationManager.AppSettings["AdinTceUrl"];
   }
    public class AdinTceGraphToken : IQueryManagers.ITypeToken
    {
        public string Text {get; set;}=@"graph";
   }
    public class AdinTceHolidatyToken : IQueryManagers.ITypeToken
    {
        public string Text {get; set;}=@"holiday";
   }
    public class AdinTceVacationToken : IQueryManagers.ITypeToken
    {
        public string Text {get; set;}=@"vacation";
   }
    public class AdinTceFullToken : IQueryManagers.ITypeToken
    {
        public string Text {get; set;}=@"full";
   }
    public class AdinTcePartToken : IQueryManagers.ITypeToken
    {
        public string Text {get; set;}=@"part";
   }
    public class AdinTceGUIDToken : IQueryManagers.ITypeToken
    {
        public string Text {get; set;}
   }

    #endregion

    #region AdinTceFormats

    //AdinTce formats
    public class AdinTceURLformat : IQueryManagers.ITypeToken
    {
        public string Text {get; set;}=@"{0}/{1}/{2}";
   }
    public class AdinTcePartformat : IQueryManagers.ITypeToken
    {
        public string Text {get; set;}=@"{0}/{1}/{2}/{3}";
   }

    #endregion


    ///<summary>AdinTce realization of Base builder,web,reader,json
    ///</summary>
    public class AdinTceCommandBuilder : CommandBuilder
    {
        public AdinTceCommandBuilder(ITokenMiniFactory tokenFactory_, IFormatFactory formatFactory_)
            : base(tokenFactory_, formatFactory_)
        {

        }
    }

    public class AdinTceWebManager : WebRequestManager
    {

    }

    public class AdinTceResponseReader : WebResponseReader
    {

    }

    public class AdinTceJsonManager : JSONManager
    {

    }


    //AdinTce POCOs

    #region AdinTceFormats

    public class Holiday
    {
        public Holiday()
        {
            LeaveType=null;
            Days=0;
        }
        [JsonProperty("LeaveType")]
        public string LeaveType {get; set;}
        [JsonProperty("Days")]
        public double Days {get; set;}
    }

    public class Vacation
    {

        public Vacation()
        {
            DateStart=null;
            DateFinish=null;
            LeaveType=null;
            DaysSpent=0;
        }
        [JsonProperty("LeaveType")]
        public string LeaveType {get; set;}
        [JsonProperty("DateStart"), JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? DateStart {get; set;}
        [JsonProperty("DateFinish"), JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime? DateFinish {get; set;}
        [JsonProperty("DaysSpent")]
        public int DaysSpent {get; set;}
    }


    public class GraphRead
    {
        public GraphRead()
        {
            DateStart=null;
            DateFinish=null;
            LeaveType=null;
            DaysSpent=0;
        }
        [JsonProperty("LeaveType")]
        public string LeaveType {get; set;}
        [JsonProperty("DateStart"), JsonConverter(typeof(MonthDayYearDateNoDotsConverter))]
        public DateTime? DateStart {get; set;}
        [JsonProperty("DateFinish"), JsonConverter(typeof(MonthDayYearDateNoDotsConverter))]
        public DateTime? DateFinish {get; set;}
        [JsonProperty("Days")]
        public int DaysSpent {get; set;}
    }
    public class GraphWrite : GraphRead
    {
        public GraphWrite()
        {
            DateStart=null;
            DateFinish=null;
            LeaveType=null;
            DaysSpent=0;
        }
        [JsonProperty("DateStart"), JsonConverter(typeof(MonthDayYearDateConverter))]
        new public DateTime? DateStart {get; set;}
        [JsonProperty("DateFinish"), JsonConverter(typeof(MonthDayYearDateConverter))]
        new public DateTime? DateFinish {get; set;}

    }
    public class GUIDPOCO
    {
        [JsonProperty("GUID")]
        public string GUID_ {get; set;}
        [JsonProperty("Position")]
        public string Position {get; set;}
    }

    public class AdinTcePOCO
    {
        [JsonProperty("GUID")]
        public string GUID_ {get; set;}
        [JsonProperty("Position")]
        public string Position {get; set;}
        [JsonProperty("Holidays")]
        public List<Holiday> holidays {get; set;}
        [JsonProperty("Vacations")]
        public List<Vacation> vacations {get; set;}
        [JsonProperty("Graphs")]
        public List<GraphWrite> Graphs {get; set;}
    }

    #endregion

}

namespace Quizes
{

    public class QuizRepo
    {

        //IWebManager wm;
        IResponseReader wr;
        IJsonManger jm;
        string orientHost, orientDbName;

        public QuizRepo()
        {
            string nonConfigDb="Intranet";
            //wm=new WebManager();
            wr=new WebResponseReader();
            jm=new JSONManager();

            orientHost=string.Format("{0}:{1}/{2}"
            ,ConfigurationManager.AppSettings["OrientDevHost"]
            ,ConfigurationManager.AppSettings["OrientPort"]
            ,ConfigurationManager.AppSettings["CommandURL"]
            );

            orientDbName=ConfigurationManager.AppSettings["IntranetDB"];

            if (nonConfigDb==null)
            {
                if (orientDbName != null)
                {
                    orientHost=string.Format("{0}/{1}", orientHost, orientDbName);
               }
                else {orientHost=string.Format("{0}/{1}", orientHost, "test_db");}
           }
            else {orientHost=string.Format("{0}/{1}", orientHost, nonConfigDb);}

            orientHost=string.Format("{0}/{1}", orientHost,"sql");
        }

        public string Quiz(int? monthGap=null)
        {
            string result=string.Empty;
            result=GetQuiz(monthGap);
            return result;
        }

        public string GetQuiz(int? monthGap=null)
        {
            string quizStr=string.Empty;
            DateTime targetDate;
            
            if (monthGap != null){
                targetDate=DateTime.Now.AddMonths((int)monthGap);
           }else {targetDate=
                    //new DateTime(2017,07,11,0,0,0); 
                    DateTime.Now;
           }

            //DateTime formatDate=new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 0, 0, 0);           
            //string dateFromStr=formatDate.ToString("yyyy-MM-dd HH:mm:ss");

            //DateTime toDate=formatDate.AddDays(1).AddMilliseconds(-1);
            //string toDateStr=toDate.ToString("yyyy-MM-dd HH:mm:ss");

            WebRequest request=WebRequest.Create(orientHost);
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + System.Convert.ToBase64String(
            Encoding.ASCII.GetBytes("root:I9grekVmk5g")
            ));
            string stringData="{\"transaction\":true,\"operations\":[{\"type\":\"script\",\"language\":\"sql\",\"script\":[ \"select from Quiz\" ]}]}"; //place body here
            stringData="{\"command\":\"select from Quiz where State ='Published'\"}"; //place body here
            var data=Encoding.ASCII.GetBytes(stringData); // or UTF8            
            
            request.Method="POST";
            request.ContentType=""; //place MIME type here
            request.ContentLength=data.Length;

            var newStream=request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
          
            int dateFrom=targetDate.Month;

            try
            {

                string res=wr.ReadResponse((HttpWebResponse)request.GetResponse());
                IEnumerable<QuizGet> quizList=jm.DeserializeFromParentNode<QuizGet>(res, "result")
                    .OrderBy(s => s.EndDate);
                IEnumerable<QuizGet> qL=null;
                
                //Date filter
                try
                {

qL=from s in quizList
    where s.StartDate.Date <= targetDate.Date
    && s.EndDate.Date > targetDate.Date
   select s;

               }
                catch (Exception e) {quizStr=e.Message;}

                if (qL.Count() > 0)
                {

                    DateTime minD=qL.Min(s => s.StartDate);
                    DateTime maxD=qL.Max(s => s.StartDate);

                    List<QuizSend> quizSendL=new List<QuizSend>();

                    //QuizSend emptyQuiz=new QuizSend();

                    QuizSend defaultQuiz=new QuizSend()
                    {title="Опросы",href=new QuizHrefNode() {link="http://my.nspk.ru/Quiz/Execute/", target="_self"}, id=50, parentid=1};

                    //QuizSend checkQuiz=new QuizSend()
                    //{
                    //    title="TestQuiz",
                    //    ID=500,
                    //    href=new QuizHrefNode() {link="http://duckduckgo.com", target="_self"},
                    //    parentID=50
                    //};

                    quizSendL.Add(defaultQuiz);
                    //quizSendL.Add(checkQuiz);
                    //quizSendL.Add(emptyQuiz);

                    int id_=500;

                    foreach (QuizGet q in qL)
                    {
                        QuizSend qs=QuizConvert(q);
                        qs.id=id_;
                        qs.title = (q.Name == null || q.Name.Equals(string.Empty)) ? "Title_" + id_ : q.Name;
                        quizSendL.Add(qs);

                        id_ += 1;
                    }

                    quizStr=jm.SerializeObject(quizSendL, new JsonSerializerSettings() {NullValueHandling=NullValueHandling.Include});

                }
                else {quizStr="No values returned. Since month " + dateFrom;}
            }
            catch (Exception e) {quizStr=e.Message;}

            return quizStr;
        }

        internal string TestRepoGetQuizCheck(int monthGap)
        {
            string quizStr=string.Empty;

            WebRequest request=WebRequest.Create(orientHost);
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + System.Convert.ToBase64String(
            Encoding.ASCII.GetBytes("root:I9grekVmk5g")
            ));
            string stringData="{\"transaction\":true,\"operations\":[{\"type\":\"script\",\"language\":\"sql\",\"script\":[ \"select from Quiz\" ]}]}"; //place body here
            stringData="{\"command\":\"select from Quiz\"}"; //place body here
            var data=Encoding.ASCII.GetBytes(stringData); // or UTF8

            request.Method="POST";
            request.ContentType="";
            request.ContentLength=data.Length;

            var newStream=request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            int dateFrom=DateTime.Now.AddMonths(monthGap).Month;

            try
            {
                int monthFrom=DateTime.Now.AddMonths(monthGap).Month;
                string res=wr.ReadResponse((HttpWebResponse)request.GetResponse());
                IEnumerable<QuizGet> quizList=jm.DeserializeFromParentNode<QuizGet>(res, "result");
                IEnumerable<QuizGet> qL=null;
                try
                {
                    qL=from s in quizList
                             where
                s.StartDate.Year >= DateTime.Now.Year
                && s.StartDate.Month >= monthFrom
                             select s;
                }
                catch (Exception e) {quizStr=e.Message;}

                if (qL.Count() > 0)
                {

                    DateTime minD = qL.Min(s => s.StartDate);
                    DateTime maxD = qL.Max(s => s.StartDate);

                    List<QuizSend> quizSendL = new List<QuizSend>();

                    QuizSend emptyQuiz = new QuizSend();
                    QuizSend defaultQuiz = new QuizSend()
                    { title = "Опросы", href = new QuizHrefNode() { link = "", target = "_self" }, id = 50, parentid = 1 };

                    QuizSend checkQuiz = new QuizSend()
                    {
                        title = "TestQuiz",
                        id = 500,
                        href = new QuizHrefNode() { link = "http://duckduckgo.com", target = "_self" },
                        parentid = 50
                    };

                    quizSendL.Add(defaultQuiz);
                    quizSendL.Add(checkQuiz);
                    quizSendL.Add(emptyQuiz);

                    int id_ = 500;

                    foreach (QuizGet q in qL)
                    {
                        QuizSend qs = QuizConvert(q);
                        
                        qs.title=(q.Name == null || q.Name.Equals(string.Empty)) ? "Title_" + id_ : q.Name;
                      
                        qs.id=id_;
                        quizSendL.Add(qs);

                        id_ += 1;
                    }

                    quizStr=jm.SerializeObject(quizSendL);

                }
                else {quizStr="No values returned. Since " + dateFrom;}
            }
            catch (Exception e) {quizStr=e.Message;}

            return quizStr;
        }
        
        /// <summary>
        /// Converting of Quiz received object to Quiz to pass in JSON object
        /// </summary>
        /// <param name="qr"></param>
        /// <returns></returns>
        public QuizSend QuizConvert(QuizGet qr)
        {
            QuizSend qs=new QuizSend();
            qs.title=qr.Name;
            qs.href=new QuizHrefNode() {link="http://my.nspk.ru/Quiz/Execute/?" + qr.id , target="_self"};
            qs.parentid=50;
            
            return qs;
        }

    }

}
