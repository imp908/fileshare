using System;

using NUnit.Framework;

using System.Collections.Generic;
using System.Linq;

using System.Net;


using System.Configuration;


using WebManagers;

using JsonManagers;

using IQueryManagers;
using QueryManagers;
using POCO;

using OrientRealization;


namespace NSQLManagerIntegrationTests
{

  /// <summary>
  /// Testing database generation from web manager by command. Every test recreates its personal db instance test_db_0
  /// </summary>
  [TestFixture]
  public class IntegrationCombatTest
  {
    CommandsChain commandOne;

    JSONManager jsonManage;

    WebRequestManager webRequestManager;
    WebResponseReader webResponseReader;
    NetworkCredential nc=new NetworkCredential(ConfigurationManager.AppSettings["orient_login"]
            , ConfigurationManager.AppSettings["orient_pswd"]);

    //tokens instances used in query commands
    internal ITokenMiniFactory _miniFactory;
    //Format generator instance
    internal IFormatFactory _formatFactory;
    //command builder instance generation
    internal ICommandFactory _commandFactory;
    //command builder instance
    internal ICommandBuilder _commandBuilder;
    //format generator
    internal IFormatFromListGenerator _formatGenerator;

    IOrientBodyFactory _orientBodyFactory;
    IOrientQueryFactory _orientQueryFactory;

    UrlShemasExplicit _urlShemas;
    CommandShemasExplicit _commandSchema;
    BodyShemas _bodyShema;
    ITypeToken hostToken,V;

    
    string 
      //input vars
      orientAuth,databaseName
      //result conditions
      ,createDb,dropDb
      ,createClassResult,propertyStrResult, propertyBoolResult
      ,insertVertexResult, createDbResult, dropDbResult;

    public IntegrationCombatTest()
    {           

      jsonManage=new JSONManager();

      webRequestManager=new WebRequestManager();
      webResponseReader=new WebResponseReader();

      _commandFactory=new CommandFactory();
      _formatFactory=new FormatFactory();
      _miniFactory=new TokenMiniFactory();

      _orientBodyFactory=new OrientBodyFactory();
      _orientQueryFactory=new OrientQueryFactory();

      _commandBuilder=_commandFactory.CommandBuilder(_miniFactory, _formatFactory);
      _formatGenerator=_formatFactory.FormatGenerator(_miniFactory);

      _urlShemas=new UrlShemasExplicit(_commandFactory, _formatFactory, _miniFactory, _orientBodyFactory);
      _commandSchema=new CommandShemasExplicit(_commandFactory, _formatFactory, _miniFactory, _orientQueryFactory);
      _bodyShema=new BodyShemas(_commandFactory, _formatFactory, _miniFactory, _orientBodyFactory);

      commandOne=new CommandsChain(_miniFactory, _orientQueryFactory, _formatFactory, _commandFactory);

      orientAuth=string.Format("{0}{1}{2}",
            ConfigurationManager.AppSettings["orient_login"] 
            ,":"
            , ConfigurationManager.AppSettings["orient_pswd"]
            );

      hostToken=new TextToken()
      {
          Text =
          string.Format("{0}:{1}"
          , ConfigurationManager.AppSettings["OrientDevHost"]
          , ConfigurationManager.AppSettings["OrientPort"])
      };

      databaseName=ConfigurationManager.AppSettings["OrientUnitTestDB"]+"_0";

      V=_miniFactory.NewToken("V");
    }
 
    public CommandsChain NewChain()
    {
      return  new CommandsChain(_miniFactory, _orientQueryFactory, _formatFactory, _commandFactory);
    }
      
    [Test]
    public void IntegrationCombatDatabasePropertyCreateInsertDelete()
    {

      ITypeToken dbName=new TextToken(){Text=databaseName};
      //string personContent = "{\"Changed\": \"2017-10-19 18:00:09\", \"Created\": \"2015-02-02 12:43:56\", \"GUID\": \"2\", \"Name\": \"0\"}";

      Person p_ = new Person()
      {
        changed = new DateTime(2017, 01, 01, 00, 00, 00)
        ,created = new DateTime(2017, 01, 01, 00, 00, 00)
        ,GUID = "1"
        ,Name = "0"
      };

      //binding host from dbname to body shema builder
      //_bodyShema.AddHost(hostToken);
      //binding host from dbname to url shema builder
      _urlShemas.AddHost(hostToken.Text);

      string personObjectContent=jsonManage.SerializeObject(p_);
      List<ITypeToken> personObjectContentToken=new List<ITypeToken> { _miniFactory.NewToken(personObjectContent) };
      ITypeToken personToken=_miniFactory.NewToken("Person");
      ITypeToken vToken=_miniFactory.NewToken("V");
      ITypeToken propertyStrToken=_miniFactory.NewToken("TestPropStr");
      ITypeToken propertyBoolToken=_miniFactory.NewToken("TestPropBool");

      //Create DB
      //creating Orient REST API url for db creation from db name, using shemas
      string urlCommand = _urlShemas.Database(dbName).GetText();
      //bind request to Webmanager
      webRequestManager.AddRequest(urlCommand);
      //bind credentials to request
      webRequestManager.SetCredentials(nc);
      //build and bind authentication header from passed credentials
      //webRequestManager.SetBase64AuthHeader(orientAuth);
      //get response of POST method
      try
      {
          createDbResult = webResponseReader.ReadResponse(webRequestManager.GetResponse64("POST"));
      }catch (Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}


      //Create class
      ICommandBuilder commandBody = commandOne.Create().Class(personToken).Extends(vToken)
          .GetBuilder().Build();
      string requestBody = _bodyShema.Command(commandBody).Build().GetText();
      string urlStr = _urlShemas.Command(dbName).Build().GetText();
      urlCommand = _urlShemas.Command(dbName).GetText();
      webRequestManager.SwapRequestsURL(urlCommand);
      webRequestManager.SetContent(requestBody);
      try
      {
          createClassResult = webResponseReader.ReadResponse(webRequestManager.GetResponse64("POST"));
      }
      catch (Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}


      //Add property str
      ICommandBuilder addPropertyBody =
      NewChain().Create().Property(personToken, propertyStrToken, 
      _orientBodyFactory.StringToken(), 
      _orientBodyFactory.False(), 
      _orientBodyFactory.False())
          .GetBuilder().Build();

      requestBody = _bodyShema.Command(addPropertyBody).Build().GetText();
      urlStr = _urlShemas.Command(dbName).Build().GetText();
      urlCommand = _urlShemas.Command(dbName).GetText();
      webRequestManager.SwapRequestsURL(urlCommand);
      webRequestManager.SetContent(requestBody);
      try
      {
        propertyStrResult = webResponseReader.ReadResponse(webRequestManager.GetResponse64("POST"));
      }
      catch (Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}


      //Add property bool
      ICommandBuilder addPropertyStrBody =
      NewChain().Create().Property(personToken
      , propertyBoolToken
      , _orientBodyFactory.BooleanToken()
      , _orientBodyFactory.False()
      , _orientBodyFactory.False())
          .GetBuilder().Build();

      requestBody = _bodyShema.Command(addPropertyStrBody).Build().GetText();
      urlStr = _urlShemas.Command(dbName).Build().GetText();
      urlCommand = _urlShemas.Command(dbName).GetText();
      webRequestManager.SwapRequestsURL(urlCommand);
      webRequestManager.SetContent(requestBody);
      try
      {
          propertyBoolResult = webResponseReader.ReadResponse(webRequestManager.GetResponse64("POST"));
      }
      catch (Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}


      //Insert vertex
      ICommandBuilder contentBody = _commandFactory
          .CommandBuilder(_miniFactory, _formatFactory, personObjectContentToken, _miniFactory.NewToken("{0}")).Build();
      ICommandBuilder insertVertexBody = NewChain().Create().Vertex(personToken).Content(contentBody)
          .GetBuilder().Build();

      requestBody = _bodyShema.Batch(insertVertexBody).Build().GetText();
      urlStr = _urlShemas.Command(dbName).Build().GetText();
      urlCommand = _urlShemas.Batch(dbName).GetText();
      webRequestManager.SwapRequestsURL(urlCommand);
      webRequestManager.SetContent(requestBody);
      try
      {
          insertVertexResult = webResponseReader.ReadResponse(webRequestManager.GetResponse64("POST"));
      }
      catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }


      //delete DB 
      urlCommand = _urlShemas.Database(dbName).GetText();
      //bind request to Webmanager
      webRequestManager.AddRequest(urlCommand);
      //bind credentials to request
      webRequestManager.SetCredentials(nc);
      //build and bind authentication header from passed credentials
      webRequestManager.SetBase64AuthHeader(orientAuth);
      //get response of POST method 
      try
      {
        dropDbResult = webResponseReader.ReadResponse(webRequestManager.GetResponse64("DELETE"));
      }
      catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
   
      Assert.NotNull(createDbResult);
      Assert.NotNull(createClassResult);
      Assert.NotNull(propertyStrResult);
      Assert.NotNull(propertyBoolResult);
      Assert.NotNull(insertVertexResult);
      Assert.AreEqual(string.Empty, dropDbResult);
    }

    [Test]
    public void IntegrationCombatDatabaseCreateDeleteCombat()
    {

      ITypeToken dbName=new TextToken() {Text=databaseName};

      //binding host from dbname to shema builder
      _urlShemas.AddHost(hostToken.Text);
      //creating Orient REST API url for db creation from db name, using shemas
      string urlCommand=_urlShemas.Database(dbName).GetText();
      //bind request to Webmanager
      webRequestManager.AddRequest(urlCommand);
      //bind credentials to request
      webRequestManager.SetCredentials(nc);
      //build and bind authentication header from passed credentials
      webRequestManager.SetBase64AuthHeader(orientAuth);

      //Create DB
      //get response of POST method 
      try
      {
        createDb=webResponseReader.ReadResponse(webRequestManager.GetResponse64("POST"));
      }
      catch (Exception e){System.Diagnostics.Trace.WriteLine(e.Message);}
          
      //
      List<ITypeToken> tokens_=new List<ITypeToken>() {
        _miniFactory.EmptyString()
      };

      //Generate command url
      _commandFactory.CommandBuilder(_miniFactory, _formatFactory, tokens_, _miniFactory.EmptyString());

      //delete DB
      //get response of DELETE method 
      //get response of DELETE method 
      try
      {
        dropDb=webResponseReader.ReadResponse(webRequestManager.GetResponse64("DELETE"));
      }
      catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

      webResponseReader.ReadResponse(webRequestManager.GetResponse64("POST"));

      Assert.NotNull(createDb);
      Assert.AreEqual(string.Empty, dropDb);
    }

  }

  /// <summary>
  /// Testing db generation with manager and crud operations with UOWS. Db generated once test_db_1 and deleted on tear down.
  /// </summary>
  [TestFixture]
  public class IntegrationNewsUOWCHeck
  {        
        
    Func<string, string> stringEmpty = delegate (string s) { if(s==string.Empty){ return null;}else{ return s; } };
    Func<int, string> intEmpty = delegate (int s) { if(s==0){ return null;}else{ return s.ToString(); } };        

    NewsUOWs.NewsRealUow nu;
    PersonUOWs.PersonUOW pu;
    Managers.Manager mng;

    public IntegrationNewsUOWCHeck()
    {
      string dbName = ConfigurationManager.AppSettings["OrientUnitTestDB"]+"_1";

      string orientHost = string.Format(
      "{0}:{1}",ConfigurationManager.AppSettings["OrientDevHost"],ConfigurationManager.AppSettings["OrientPort"]
      );

      nu=new NewsUOWs.NewsRealUow(dbName,orientHost);
      pu=new PersonUOWs.PersonUOW(dbName,orientHost);
      mng=new Managers.Manager(dbName,orientHost);

      System.Diagnostics.Trace.WriteLine("Db create start");
      mng.GenDB();
      System.Diagnostics.Trace.WriteLine("Db create finished");
      System.Diagnostics.Trace.WriteLine("notes create start");
      mng.GenNewsComments(true);
      System.Diagnostics.Trace.WriteLine("notes create finished");
    }
  
    [Test]
    public void UOWAccCheck()
    {
      System.Diagnostics.Trace.WriteLine("test started");
      List<string> results = new List<string>();
      List<bool> resultList = new List<bool>();                              

      results.Add(intEmpty(pu.UserAcc().Count()));

      List<Person> pers = nu.GetOrientObjects<Person>(null).ToList();
      results.Add(intEmpty(pers.Count()));
      results.Add(pu.GetPersonByID(pers[0].id).id);
                     
      JSONManager jm = new JSONManager();
      Note nt = new Note() { content="Very interesting new",name="News" };            
      string news = jm.SerializeObject(nt);
      Person p = pu.GetPersonByAccount("Neprintsevia");
      string result = nu.CreateNews(p, nt).id;

      results.Add(stringEmpty(result));
          
      Person p2 = pu.GetPersonByAccount("Neprintsevia");
       
      string res = nu.DeleteNews(p2, "");
      if(res==null||res==string.Empty)
      {
        results.Add("Delete person succes");
      }

      Assert.AreEqual(5,results.Count());
    }

  }

}
