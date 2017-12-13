//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Net;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Moq;
using WebManagers;
using IWebManagers;
using JsonManagers;

using IQueryManagers;
using QueryManagers;
using POCO;
using AdinTce;
using OrientRealization;
using System.Configuration;

namespace NSQLManagerTests.Tests
{

    public class WebResponseUnitTest
    {
        Mock<IWebManager> webManager=new Mock<IWebManager>();

        public WebResponseUnitTest()
        {

            webManager.Setup(m => m.GetResponse(It.IsAny<string>()));
       }

        [Fact]
        public void GetResponseReturnsNullTest()
        {         
            webManager.Object.GetResponse("GET");
            WebResponse res=webManager.Object.GetResponse("GET");
            Assert.Null(res);
       }

   }
    public class WebResponseReaderUnitTest
    {
        Mock<IResponseReader> webResponseReader;
        Mock<WebResponse> webResp;
        Mock<HttpWebResponse> httpWebResp;
        Mock<HttpResponseMessage> httpRespMsg;

        public WebResponseReaderUnitTest()
        {
            webResponseReader=new Mock<IResponseReader>();
            webResponseReader.Setup(s => s.ReadResponse(It.IsAny<WebResponse>())).Returns(@"OK");
            webResponseReader.Setup(s => s.ReadResponse(It.IsAny<HttpWebResponse>())).Returns(@"OK");
            webResponseReader.Setup(s => s.ReadResponse(It.IsAny<HttpResponseMessage>())).Returns(@"OK");

            webResp=new Mock<WebResponse>();
            httpWebResp=new Mock<HttpWebResponse>();
            httpRespMsg=new Mock<HttpResponseMessage>();
       }

        [Fact]
        public void GetWebResponseRturnsOKstring()
        {
            string res=webResponseReader.Object.ReadResponse(webResp.Object);
            Assert.Equal(@"OK", res);
       }
        [Fact]
        public void GetHttpWebResponseRturnsOKstring()
        {
            string res=webResponseReader.Object.ReadResponse(httpWebResp.Object);
            Assert.Equal(@"OK", res);
       }
        [Fact]
        public void GetHttpResponseMsgRturnsOKstring()
        {
            string res=webResponseReader.Object.ReadResponse(httpRespMsg.Object);
            Assert.Equal(@"OK", res);
       }

   }

    public class WebManager2IntegrationTests
    {

        WebRequestManager wm;
        string testUrl;
        public WebManager2IntegrationTests()
        {
            testUrl=string.Format("{0}:{1}"
                , ConfigurationManager.AppSettings["TestHost"]
                , ConfigurationManager.AppSettings["TestPort"]);
       }

        [Fact]
        public void WebManager2AddRequestGETCheck()
        {
            HttpStatusCode code=HttpStatusCode.NotImplemented;
            wm=new WebRequestManager();
            wm.AddRequest(testUrl);
            //wm.SetTimeout(5000);
            code=((HttpWebResponse)wm.GetHttpResponse("GET")).StatusCode;
            Assert.Equal(HttpStatusCode.OK, code);
       }
        [Fact]
        public void WebManager2AddRequestPOSTCheck()
        {
            HttpStatusCode code=HttpStatusCode.NotImplemented;
            wm=new WebRequestManager();
            wm.AddRequest(testUrl);
            code=((HttpWebResponse)wm.GetHttpResponse("POST")).StatusCode;
            Assert.Equal(HttpStatusCode.OK, code);
       }
        [Fact]
        public void WebManager2AddRequestSwapGetPostCheckSameMethodsAndURL()
        {
            HttpStatusCode codeBefore=HttpStatusCode.NotImplemented;
            HttpStatusCode codeAfter=HttpStatusCode.NotImplemented;
            HttpStatusCode codeDelete= HttpStatusCode.NotImplemented;
            HttpStatusCode codePut=HttpStatusCode.NotImplemented;
            string methodBefore="GET";
            string methodAfter="POST";
            string methodPut="POST";
            string methodDelete="DELETE";
            string aMb=null, aMa=null, aMd=null,aMp=null;
         
            wm=new WebRequestManager();
            wm.AddRequest(testUrl);

            using (HttpWebResponse wr=wm.GetHttpResponse(methodBefore))
            {
                codeBefore=wr.StatusCode;
                aMb=wr.Method;
           }
            using (HttpWebResponse wr2=wm.GetHttpResponse(methodAfter))
            {
                codeAfter=wr2.StatusCode;
                aMa=wr2.Method;
           }
            using (HttpWebResponse wr2=wm.GetHttpResponse(methodDelete))
            {
                codeDelete=wr2.StatusCode;
                aMd=wr2.Method;
           }
            using (HttpWebResponse wr2=wm.GetHttpResponse(methodPut))
            {
                codePut=wr2.StatusCode;
                aMp=wr2.Method;
           }
            Assert.Equal(HttpStatusCode.OK, codeBefore);
            Assert.Equal(HttpStatusCode.OK, codeAfter);
            Assert.Equal(HttpStatusCode.OK, codeDelete);
            Assert.Equal(HttpStatusCode.OK, codePut);
            Assert.Equal(methodBefore, aMb);
            Assert.Equal(methodAfter, aMa);
            Assert.Equal(methodDelete, aMd);
            Assert.Equal(methodPut, aMp);
       }
       
   }

    public class IntegrationMnagerFireTest
    {
        Manager mng;
        Person p;
        Unit u;
        MainAssignment mainAssignment;

        string login, password, dbHost, dbName;

        public IntegrationMnagerFireTest()
        {
            login = ConfigurationManager.AppSettings["orient_login"];
            password = ConfigurationManager.AppSettings["orient_pswd"];
            dbHost = string.Format("{0}:{1}"
                , ConfigurationManager.AppSettings["ParentHost"]
                , ConfigurationManager.AppSettings["ParentPort"]);
            dbName = ConfigurationManager.AppSettings["TestDBname"];

            p =
new Person() { Name = "0", GUID = "0", Changed = new DateTime(2017, 01, 01, 00, 00, 00), Created = new DateTime(2017, 01, 01, 00, 00, 00) };

            u =
new Unit() { Name = "0", GUID = "0", Changed = new DateTime(2017, 01, 01, 00, 00, 00), Created = new DateTime(2017, 01, 01, 00, 00, 00) };

            mainAssignment = new MainAssignment();

            TypeConverter tc = new TypeConverter();
            JsonManagers.JSONManager jm = new JSONManager();
            TokenMiniFactory tf = new TokenMiniFactory();
            UrlShemasExplicit us = new UrlShemasExplicit(
                new CommandBuilder(tf, new FormatFactory())
                , new FormatFromListGenerator(new TokenMiniFactory())
                , tf, new OrientBodyFactory());

            BodyShemas bs = new BodyShemas(new CommandFactory(), new FormatFactory(), new TokenMiniFactory(),
                new OrientBodyFactory());

            us.AddHost(dbHost);
            WebResponseReader wr = new WebResponseReader();
            WebRequestManager wm = new WebRequestManager();
            wm.SetCredentials(new NetworkCredential(login, password));
            CommandFactory cf = new CommandFactory();
            FormatFactory ff = new FormatFactory();
            OrientQueryFactory oqf = new OrientQueryFactory();
            OrientCLRconverter pc = new OrientCLRconverter();

            mng = new Manager(tc, jm, tf, us, bs, wm, wr, cf, ff, oqf, pc);

        }

        bool NotNullNotEmpty(string str_)
        {
            if (str_ == null) { return false; }
            if (str_ == string.Empty) { return false; }
            return true;
        }

        [Fact]
        public void DbBoilerplateCheck()
        {

            //db delete
            mng.DeleteDb(dbName, dbHost);

            //db crete
            string CreateResult = mng.CreateDb(dbName, dbHost).GetResult();

            //create class
            Type crClstr = mng.CreateClass<Unit,V>(dbName);
            Type maClstr = mng.CreateClass<MainAssignment, E>(dbName);

            string crClmt = mng.CreateClass("Person", "V", dbName).GetResult();

            //create property
            Person crPrstr = mng.CreateProperty<Person>(p, null);
            string crPrmt = mng.CreateProperty("Unit", "Name", typeof(string), false, false).GetResult();

            //add vertex
            Person crVrtstr = mng.CreateVertex<Person>(p, dbName);
            string crVrtmt = mng.CreateVertex("Unit", "{\"Name\":\"TestName\"}", null).GetResult();
            Unit u0 = mng.CreateVertex<Unit>(u, dbName);

            MainAssignment maA = mng.CreateEdge<MainAssignment>(mainAssignment, crVrtstr, u0, dbName);

            IEnumerable<Person> selectMt = mng.Select<Person>("1=1", dbName);

            IEnumerable<MainAssignment> a = mng.Select<MainAssignment>("1=1", dbName);

            //db delete
            string DeleteResult = mng.DeleteDb(dbName, dbHost).GetResult();



            Assert.Equal("Created", CreateResult);

            Assert.NotNull(crClstr);
            Assert.True(NotNullNotEmpty(crClmt));

            Assert.NotNull(crPrstr);
            Assert.True(NotNullNotEmpty(crPrmt));

            Assert.NotNull(crVrtstr);
            Assert.True(NotNullNotEmpty(crVrtmt));

            Assert.NotNull(u0);
            Assert.NotNull(maA);           

            Assert.True(selectMt.Count() > 0);
            Assert.True(a.Count() > 0);

            Assert.Equal("Deleted", DeleteResult);

        }

    }

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
                , ConfigurationManager.AppSettings["ParentHost"]
                , ConfigurationManager.AppSettings["ParentPort"])
            };

            databaseName=ConfigurationManager.AppSettings["TestDBname"];

            V=_miniFactory.NewToken("V");
        }

        public CommandsChain NewChain()
        {
            return  new CommandsChain(_miniFactory, _orientQueryFactory, _formatFactory, _commandFactory);
        }
      
        [Fact]
        public void IntegrationCombatDatabasePropertyCreateInsertDelete()
        {

            ITypeToken dbName = new TextToken() { Text = databaseName };
            //string personContent = "{\"Changed\": \"2017-10-19 18:00:09\", \"Created\": \"2015-02-02 12:43:56\", \"GUID\": \"2\", \"Name\": \"0\"}";

            Person p_ = new Person()
            {
                Changed = new DateTime(2017, 01, 01, 00, 00, 00)
                ,Created = new DateTime(2017, 01, 01, 00, 00, 00)
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
            }catch (Exception e) { }


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
            catch (Exception e) { }


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
            catch (Exception e) { }


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
            catch (Exception e) { }


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
            catch (Exception e) { }


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
            catch (Exception e) { }


            Assert.NotNull(createDbResult);
            Assert.NotNull(createClassResult);
            Assert.NotNull(propertyStrResult);
            Assert.NotNull(propertyBoolResult);
            Assert.NotNull(insertVertexResult);
            Assert.Equal(string.Empty, dropDbResult);
        }

        [Fact]
        public void IntegrationCombatDatabaseCreateDeleteCombat()
        {

            ITypeToken dbName=new TextToken() {Text=ConfigurationManager.AppSettings["TestDBname"]};

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
            catch (Exception e) {}
          
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
            catch (Exception e) {}

            Assert.NotNull(createDb);
            Assert.Equal(string.Empty, dropDb);
       }

    }

    public class OrientWebManagerIntegrationTests
    {

        OrientWebManager orietWebManager;
        string authUrl, funcUrl, batchUrl, commandUrl, root, password;
        NetworkCredential nc;

        public OrientWebManagerIntegrationTests()
        {
            orietWebManager=new OrientWebManager();

            authUrl=string.Format(@"{0}:{1}/{2}/{3}"
                , ConfigurationManager.AppSettings["ParentHost"]
                , ConfigurationManager.AppSettings["ParentPort"]
                , ConfigurationManager.AppSettings["AuthURL"]
                , ConfigurationManager.AppSettings["ParentDB"]);

            funcUrl=string.Format(@"{0}:{1}/{2}/{3}"
                , ConfigurationManager.AppSettings["ParentHost"]
                , ConfigurationManager.AppSettings["ParentPort"]
                , ConfigurationManager.AppSettings["FunctionURL"]
                , ConfigurationManager.AppSettings["ParentDB"]);

            batchUrl=string.Format(@"{0}:{1}/{2}/{3}"
                , ConfigurationManager.AppSettings["ParentHost"]
                , ConfigurationManager.AppSettings["ParentPort"]
                , ConfigurationManager.AppSettings["BatchURL"]
                , ConfigurationManager.AppSettings["ParentDB"]);

            commandUrl=string.Format(@"{0}:{1}/{2}/{3}"
                , ConfigurationManager.AppSettings["ParentHost"]
                , ConfigurationManager.AppSettings["ParentPort"]
                , ConfigurationManager.AppSettings["CommandURL"]
                , ConfigurationManager.AppSettings["ParentDB"]);

            root=ConfigurationManager.AppSettings["orient_login"];
            password=ConfigurationManager.AppSettings["orient_pswd"];

            nc=new NetworkCredential(root, password);

       }

        [Fact]
        public void GetResponseAnUthorizedReturnsNullIntegrationTest()
        {
            WebResponse response;
            response=orietWebManager.GetResponse(authUrl, "GET");
            Assert.Null(response);
       }

        [Fact]
        public void AuthenticateIntegrationTest()
        {
            string result=orietWebManager.Authenticate(authUrl, nc).Headers.Get("Set-Cookie");
            bool check=result != string.Empty && result != null;
            Assert.True(check);
       }

        [Fact]
        public void CommandExecuteCheck()
        {
            orietWebManager.GetResponse(authUrl, "GET");
            orietWebManager.Authenticate(authUrl, nc).Headers.Get("Set-Cookie");
            orietWebManager.GetResponseCred("GET");
       }

    }

    public class ChainigTest
    {

        CommandsChain commandOne;
        CommandBuilder commandBuilder, commandBuilder_SelectFrom;
        ITypeToken V, VSC, Person;
        List<ITypeToken> NGA;

        ICommandBuilder SelectNGAfromPerson;

        TokenMiniFactory miniFactory;
        FormatFactory formatFactory;
        CommandFactory commandFactory;

        public ChainigTest()
        {

            miniFactory=new TokenMiniFactory();
            formatFactory=new FormatFactory();
            commandFactory=new CommandFactory();

            commandBuilder=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            commandBuilder_SelectFrom=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            commandBuilder_SelectFrom.AddTokens(new List<ITypeToken> {new TextToken() {Text="Name,GUID"}});
            commandBuilder_SelectFrom.AddFormat(new TextToken() {Text="{0}"});


            V=miniFactory.NewToken("V");
            VSC=miniFactory.NewToken("VSC");
            Person=miniFactory.NewToken("Person");
            NGA=new List<ITypeToken>() {
                miniFactory.NewToken("Name"),miniFactory.NewToken("GUID"),miniFactory.NewToken("Acc")
           };
            SelectNGAfromPerson=commandFactory.CommandBuilder(miniFactory
                , formatFactory
                , NGA
                , formatFactory.FormatGenerator(miniFactory)
                    .FromatFromTokenArray(NGA, miniFactory.Coma())
            );

            this.commandOne=CommandInit();
       }

        public CommandsChain CommandInit()
        {
            TokenMiniFactory tokenMiniFactory=new TokenMiniFactory();
            OrientQueryFactory tokenQueryFactory=new OrientQueryFactory();
            FormatFactory formatFactoy=new FormatFactory();
            CommandFactory commandFactory=new CommandFactory();

            return new CommandsChain(tokenMiniFactory, tokenQueryFactory, formatFactoy, commandFactory);
       }

        //VERTEX
        [Fact]
        public void ChainingVertexCheck()
        {

            ITypeToken class_=miniFactory.NewToken("Person");
            ITypeToken content_=miniFactory.NewToken(
"{\"Changed\": \"2017-10-19 18:00:09\", \"Created\": \"2015-02-02 12:43:56\", \"GUID\": \"1\", \"Name\": \"0\"}"
                );
            ITypeToken format_=miniFactory.NewToken("{0}");
            List<ITypeToken> contentLt=new List<ITypeToken>() {content_};

            ICommandBuilder cb=commandFactory.CommandBuilder(miniFactory, formatFactory, contentLt, format_).Build();
            string res=commandOne
                .Create()
                .Class(class_)
                .Content(cb)
                .GetBuilder().Build().GetText();

            string exp =
 "Create Class Person content {\"Changed\": \"2017-10-19 18:00:09\", \"Created\": \"2015-02-02 12:43:56\", \"GUID\": \"1\", \"Name\": \"0\"} ";

            Assert.Equal(exp, res);
       }

        //PROPERTY
        [Fact]
        public void ChainingCreatePropertyBoolCheck()
        {

            ITypeToken class_=miniFactory.NewToken("UserSettings");
            ITypeToken prop_=miniFactory.NewToken("showBirthday");
            ITypeToken type_=miniFactory.NewToken("BOOLEAN");
            ITypeToken true_=null;

            string res=commandOne
                .Create()
                .Property(class_, prop_, type_, true_, true_).GetBuilder().Build().GetText();

            string exp =
"Create Property UserSettings.showBirthday BOOLEAN";

            Assert.Equal(exp, res);
       }
        [Fact]
        public void ChainingCreatePropertyStringCheck()
        {

            ITypeToken class_=miniFactory.NewToken("VSC");
            ITypeToken prop_=miniFactory.NewToken("Name");
            ITypeToken type_=miniFactory.NewToken("STRING");
            ITypeToken true_=miniFactory.NewToken("TRUE");

            string res=commandOne
                .Create()
                .Property(class_, prop_, type_, true_, true_).GetBuilder().Build().GetText();

            string exp =
"Create Property VSC.Name STRING (MANDATORY TRUE,NOTNULL TRUE)";

            Assert.Equal(exp, res);
       }

        //CLASS
        [Fact]
        public void ChainingCreateClassCheck()
        {

            ITypeToken VSC=miniFactory.NewToken("VSC");
            ITypeToken V=miniFactory.NewToken("V");

            ICommandBuilder cb=commandFactory.CommandBuilder(miniFactory, formatFactory);

            commandOne.Create().Class(VSC).Extends(V);

            string res=commandOne.GetBuilder().Build().GetText();
            string exp =
"Create Class VSC Extends V";

            Assert.Equal(exp, res);

       }


        //NEST
        [Fact]
        public void ChainingSquareCheck()
        {
            this.commandOne.NestSq();
            string result=this.commandOne.GetCommand();
            Assert.Equal("[]", result);
       }
        [Fact]
        public void ChainingNestSelectNestSquareCheck()
        {
            this.commandOne.From().Select(commandBuilder_SelectFrom)
                .NestSq().From().Select();
            string result=this.commandOne.GetCommand();
            Assert.Equal("Select  from[Select Name,GUID from]", result);
       }
        [Fact]
        public void ChainingNestSelectNestRoundCheck()
        {
            this.commandOne.From().Select(commandBuilder_SelectFrom)
                .NestRnd().From().Select();
            string result=this.commandOne.GetCommand();
            Assert.Equal("Select  from(Select Name,GUID from)", result);
       }
        [Fact]
        public void ChainingChainSelectFromParamCheck()
        {
            this.commandOne.From().Select(commandBuilder_SelectFrom)
                .Nest(new OrientRoundBraketLeftToken(), new OrientRoundBraketRightToken(),
                new TextToken() {Text=@" {0} {1} {2} "}).From().Select();

            string result=this.commandOne.GetCommand();
            Assert.Equal("Select  from(Select Name,GUID from)", result);
       }
        [Fact]
        public void ChainingChainSelectParameterCheck()
        {
            this.commandOne.Select(commandBuilder_SelectFrom).Select();
            string result=this.commandOne.GetCommand();
            Assert.Equal("Select Select Name,GUID", result);
       }

        [Fact]
        public void ChainingChainCreateEdgeFromTo()
        {
            ITypeToken edgeTk = miniFactory.NewToken("MainAssignment");
            ITypeToken fromId = miniFactory.NewToken("#15:0");
            ITypeToken toId = miniFactory.NewToken("#16:0");
            ITypeToken edgCnt = null;            

            this.commandOne.Create().Edge(edgeTk).FromV(fromId).ToV(toId).Content(edgCnt)
                   .GetBuilder().Build();

            string result = this.commandOne.GetCommand();
            Assert.Equal("Create Edge MainAssignment from #15:0 To #16:0", result);
        }
        

        [Fact]
        public void ChainingNestChainExtendedCheck()
        {
            this.commandOne.From()
                .Select(commandBuilder_SelectFrom).Create().Class(V).Extends(VSC)
                .Nest(new OrientRoundBraketLeftToken()
                , new OrientRoundBraketRightToken(), new TextToken() {Text=string.Empty});

            string result=this.commandOne.GetCommand();
            Assert.Equal("(Select Name,GUID fromCreate Class V Extends VSC)", result);
       }
        [Fact]
        public void ChainingNestChainCheck()
        {
            this.commandOne.From().Select()
.Nest(new OrientRoundBraketLeftToken(), new OrientRoundBraketRightToken(), new TextToken() {Text=string.Empty});

            string result=this.commandOne.GetCommand();
            Assert.Equal("(Select  from)", result);
       }

        [Fact]
        public void ChainingClassCheck()
        {
            this.commandOne.Class();

            string result=this.commandOne.GetCommand();
            Assert.Equal(" Class", result);
       }

        [Fact]
        public void ChainingCreateChainCheck()
        {

            this.commandOne.Create().Class(V).Extends(VSC);

            string result=this.commandOne.GetCommand();
            Assert.Equal("Create Class V Extends VSC", result);
       }

        [Fact]
        public void ChainingWhereParamCheck()
        {

            ICommandBuilder cb=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            cb.AddTokens(new List<ITypeToken>() {new TextToken() {Text="1=1"}});
            cb.AddFormat(new TextToken() {Text="{0}"});

            this.commandOne.Where(cb);
            string result=this.commandOne.GetCommand();
            Assert.Equal(" where 1=1", result);
       }
        [Fact]
        public void ChainingWhereCheck()
        {
            this.commandOne.Where();
            string result=this.commandOne.GetCommand();
            Assert.Equal(" where", result);
       }
        [Fact]
        public void ChainingNestCheck()
        {
            this.commandOne.NestRnd();
            string result=this.commandOne.GetCommand();
            Assert.Equal("()", result);
       }

    }

    public class CommandTest
    {

        CommandsChain commandOne;
        CommandBuilder commandBuilder, commandBuilder_SelectFrom;
        ITypeToken V,VSC,Person;
        List<ITypeToken> NGA;
        
        ICommandBuilder SelectNGAfromPerson;

        TokenMiniFactory miniFactory;        
        FormatFactory formatFactory;
        CommandFactory commandFactory;

        public CommandTest()
        {

            miniFactory=new TokenMiniFactory();
            formatFactory=new FormatFactory();
            commandFactory=new CommandFactory();           

            commandBuilder=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            commandBuilder_SelectFrom=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            commandBuilder_SelectFrom.AddTokens(new List<ITypeToken> {new TextToken() {Text="Name,GUID"}});
            commandBuilder_SelectFrom.AddFormat(new TextToken() {Text="{0}"});

            V=miniFactory.NewToken("V");
            VSC=miniFactory.NewToken("VSC");
            Person=miniFactory.NewToken("Person");
            NGA=new List<ITypeToken>() {
                miniFactory.NewToken("Name"),miniFactory.NewToken("GUID"),miniFactory.NewToken("Acc")
           };
            SelectNGAfromPerson=commandFactory.CommandBuilder(miniFactory
                , formatFactory
                , NGA
                , formatFactory.FormatGenerator(miniFactory)
                    .FromatFromTokenArray(NGA, miniFactory.Coma())
            );

            this.commandOne=CommandInit();
       }

        public CommandsChain CommandInit()
        {
            TokenMiniFactory tokenMiniFactory=new TokenMiniFactory();
            OrientQueryFactory tokenQueryFactory=new OrientQueryFactory();
            FormatFactory formatFactoy=new FormatFactory();
            CommandFactory commandFactory=new CommandFactory();

            return new CommandsChain(tokenMiniFactory, tokenQueryFactory, formatFactoy, commandFactory);
       }
     
        //WITH
        [Fact]
        public void CommandWithParametersCheck()
        {

            //select fields command builder with list of tokens from typetoken
            CommandBuilder select=new CommandBuilder(new TokenMiniFactory(), new FormatFactory()
                , new List<ITypeToken>() {new TextToken() {Text=@"GUID,Name,Unit"}}
                , new TextToken() {Text=@"{0}"}
                );

            //from type token
            TextToken person=new TextToken() {Text=@"Person"};

            //where token
            CommandBuilder where=new CommandBuilder(new TokenMiniFactory(), new FormatFactory()
            , new List<ITypeToken>() {new TextToken() {Text=@"1=1"}}
            , new TextToken() {Text=@"{0}"});

            List<ICommandBuilder> cb1=new List<ICommandBuilder>();

            cb1.Add(CommandInit().Select(select).GetBuilder());
            cb1.Add(CommandInit().From().GetBuilder());
            cb1.Add(CommandInit().Where(where).GetBuilder());
            cb1.Add(CommandInit().Create().GetBuilder());
            cb1.Add(CommandInit().Class(person).GetBuilder());
            cb1.Add(CommandInit().Extends(person).GetBuilder());
            cb1.Add(CommandInit().Vertex().GetBuilder());
            cb1.Add(CommandInit().Content(select).GetBuilder());

            commandBuilder.BindBuilders(cb1, CommandInit().GetGenerator()
                .FromatFromTokenArray(cb1,miniFactory.EmptyString()));
            commandBuilder.Build();

            string res=commandBuilder.GetText();
            string exp =
"Select GUID,Name,Unit from where 1=1Create Class Person Extends Person Vertexcontent GUID,Name,Unit ";

            Assert.Equal(exp, res);

       }

        [Fact]
        public void CommandFromParamCheck()
        {
            string result=CommandInit().From(new TextToken() {Text="Person"}).GetBuilder().GetText();
            Assert.Equal(" from Person", result);
       }
        [Fact]
        public void CommandFromCheck()
        {
            string result=CommandInit().From().GetBuilder().GetText();
            Assert.Equal(" from", result);
       }

        [Fact]
        public void CommandSelectFromCheck()
        {
            string result=CommandInit().From().Select().GetBuilder().GetText();
            Assert.Equal("Select  from", result);
       }
        [Fact]
        public void CommandSelectFromParamCheck()
        {
            string result=CommandInit().From(Person).Select(SelectNGAfromPerson).GetBuilder().GetText();
            Assert.Equal("Select Name,GUID,Acc from Person", result);
       }


        [Fact]
        public void FormatBuilderConcatenatesArraysOfBuildersWithFormatStoreCheck()
        {
            List<ICommandBuilder> bds=new List<ICommandBuilder>();

            ICommandBuilder cb1=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            cb1.AddTokens(new List<ITypeToken> {
                new TextToken() {Text="token 1"}
                , new TextToken() {Text="token 2"}
               });
            cb1.AddFormat(new TextToken() {Text="{0}.{1}"});

            ICommandBuilder cb2=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            cb2.AddTokens(new List<ITypeToken> {
                new TextToken() {Text="token 3"}
                , new TextToken() {Text="token 4"}
                , new TextToken() {Text="token 5"}
               });
            cb2.AddFormat(new TextToken() {Text="{0}:{1}/{3}"});

            bds.Add(cb1);
            bds.Add(cb2);

            CommandBuilder cb3=new CommandBuilder(miniFactory,formatFactory,bds, new TextToken() {Text="{0}_|_{1}"});
            string result=cb3.Build().GetText();
            Assert.Equal("token 1.token 2_|_token 3:token 4/token 5", result);
       }


        [Fact]
        public void StringRearrangeDoubledDigitOnePresenceCheck()
        {
            string str=@"{0}{1}{1}{0}{0}{1}{1}{0}{9}{10}";
            string resAct=commandBuilder.FormatStringReArrange(str);
            string resExp=@"{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}";
            Assert.Equal(resExp, resAct);
       }

        [Fact]
        public void StringRearrangeDoubledDigitTwoPresenceCheck()
        {
            string str=@"{0}{1}{1}{0}{0}{1}{1}{0}{9}{10}{12}";
            string resAct=commandBuilder.FormatStringReArrange(str);
            string resExp=@"{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}";
            Assert.Equal(resExp, resAct);
       }

        [Fact]
        public void StringRearrangeCorrectCheck()
        {
            string str=@"{0}:{1}/{2}/{3}/{4}/{5} {6} {7} {8} {9}";
            string resAct=commandBuilder.FormatStringReArrange(str);
            string resExp=@"{0}:{1}/{2}/{3}/{4}/{5} {6} {7} {8} {9}";
            Assert.Equal(resExp, resAct);
       }

    }

    public class ShemaBaseCheck
    {
        TokenMiniFactory tokenFactory;
        CommandFactory commandFactory;
        FormatFactory formatFactory;
        Shemas shemaUnderTest;

        List<ITypeToken> tokenSelectList
            ,where
            ,condition;

        public ShemaBaseCheck()
        {
            tokenFactory=new TokenMiniFactory();
            commandFactory=new CommandFactory();
            formatFactory=new FormatFactory();

            shemaUnderTest=new Shemas(commandFactory, formatFactory, tokenFactory);

            tokenSelectList=new List<ITypeToken>()
            {
                new TextToken(){Text=@"select"}
                ,new TextToken(){Text=@"from"}
                ,new TextToken(){Text=@"Person"}
           };

            where=new List<ITypeToken>()
            {
                  new TextToken(){Text=@"where"}
                ,new TextToken(){Text=@"1=1"}
           };

            condition=new List<ITypeToken>()
            {
                  new TextToken(){Text=@"in"}
                ,new TextToken(){Text=@"E"}
                ,new TextToken(){Text=@"["}
                ,new TextToken(){Text=@"'MainAssignment'"}
                ,new TextToken(){Text=@"]"}
           };
       }
        
        [Fact]
        public void ShemaBaseBuild()
        {
            shemaUnderTest.BuildNew(tokenSelectList, tokenFactory.Gap());
            string result=shemaUnderTest.GetBuilder().GetText();
            string expected="select from Person";
            Assert.Equal(expected, result);
       }
        [Fact]
        public void ShemaBaseBuildWithFormat()
        {
            shemaUnderTest.BuildFormatNew(where, new TextToken() {Text=@"{0} {1}"});
            string result=shemaUnderTest.GetBuilder().GetText();
            string expected="where 1=1";
            Assert.Equal(expected, result);
       }
        [Fact]
        public void ShemaBaseBuildCommandBuilders()
        {
            List<ICommandBuilder> commandBuilders=new List<ICommandBuilder>();
            shemaUnderTest.BuildNew(tokenSelectList, tokenFactory.Gap());
            commandBuilders.Add(shemaUnderTest.GetBuilder());
            shemaUnderTest.BuildFormatNew(where, new TextToken() {Text=@"{0} {1}"});
            commandBuilders.Add(shemaUnderTest.GetBuilder());

            shemaUnderTest.BuildNew(commandBuilders, new TextToken() {Text=@"|"});
            string result=shemaUnderTest.GetBuilder().GetText();
            string expected="select from Person|where 1=1";
            Assert.Equal(expected, result);
       }
        [Fact]
        public void ShemaBaseAddLeftDelimeter()
        {

            List<ICommandBuilder> commandBuilders=new List<ICommandBuilder>();
            shemaUnderTest.BuildNew(tokenSelectList, tokenFactory.Gap());
            commandBuilders.Add(shemaUnderTest.GetBuilder());
            shemaUnderTest.BuildFormatNew(where, new TextToken() {Text=@"{0} {1}"});
            commandBuilders.Add(shemaUnderTest.GetBuilder());

            shemaUnderTest.BuildNew(commandBuilders, new TextToken() {Text=@"|"});
            ICommandBuilder b0=shemaUnderTest.GetBuilder();

            shemaUnderTest.BuildFormatNew(condition, new TextToken() {Text=@"{0}{1}{2}{3}{4}"});
            ICommandBuilder b1=shemaUnderTest.GetBuilder();

            shemaUnderTest.AddLeft(b0, b1, new TextToken() {Text=@"<"});

            string result=shemaUnderTest.GetBuilder().GetText();
            string expected="select from Person|where 1=1<inE['MainAssignment']";
            Assert.Equal(expected, result);

       }
        [Fact]
        public void ShemaBaseAddRightDelimeter()
        {

            List<ICommandBuilder> commandBuilders=new List<ICommandBuilder>();
            shemaUnderTest.BuildNew(tokenSelectList, new TextToken() {Text=@" "});
            commandBuilders.Add(shemaUnderTest.GetBuilder());
            shemaUnderTest.BuildFormatNew(where, new TextToken() {Text=@"{0} {1}"});
            commandBuilders.Add(shemaUnderTest.GetBuilder());

            shemaUnderTest.BuildNew(commandBuilders, new TextToken() {Text=@"|"});
            ICommandBuilder b0=shemaUnderTest.GetBuilder();

            shemaUnderTest.BuildFormatNew(condition, new TextToken() {Text=@"{0}{1}{2}{3}{4}"});
            ICommandBuilder b1=shemaUnderTest.GetBuilder();

            shemaUnderTest.AddRight(b0, b1, new TextToken() {Text=@">"});

            string result=shemaUnderTest.GetBuilder().GetText();
            string expected="inE['MainAssignment']>select from Person|where 1=1";
            Assert.Equal(expected, result);

       }

    }
    public class UrlShemasExplicitTest
    {
        UrlShemasExplicit Urlshema;
        string testDBname;
        string parentHost;
        public UrlShemasExplicitTest()
        {
            ITokenMiniFactory tf=new TokenMiniFactory();
            IFormatFactory ff=new FormatFactory();

            CommandBuilder cb=new CommandBuilder(tf,ff);

            IFormatFromListGenerator fg=new FormatFromListGenerator(tf);

            IOrientBodyFactory qbf=new OrientBodyFactory();

            Urlshema=new UrlShemasExplicit(cb, fg, tf, qbf);
            Urlshema.AddHost(ConfigurationManager.AppSettings["ParentHost"]);
            testDBname=ConfigurationManager.AppSettings["ChildDBname"];
            parentHost=ConfigurationManager.AppSettings["ParentHost"];
       }

        [Fact]
        public void UrlShemasGetHostCheck()
        {
            Assert.Equal(ConfigurationManager.AppSettings["ParentHost"], Urlshema.GetHost().Text);           
       }
        [Fact]
        public void UrlShemasCreateCheck()
        {
            string result=string.Empty;
            string expected=parentHost + "/database/test_db/plocal";
            result=Urlshema.Database(new TextToken() {Text=testDBname}).GetText();
            Assert.Equal(expected,result);
       }
        [Fact]
        public void UrlShemasConnectCheck()
        {
            string result=string.Empty;
            string expected=parentHost + "/connect/test_db";
            result=Urlshema.Connect(new TextToken() {Text=testDBname}).GetText();
            Assert.Equal(expected, result);
       }
        [Fact]
        public void UrlShemasCommandCheck()
        {
            string result=string.Empty;
            string expected=parentHost + "/command/test_db/sql";
            result=Urlshema.Command(new TextToken() {Text=testDBname}).GetText();
            Assert.Equal(expected, result);
       }
        [Fact]
        public void UrlShemasBatchCheck()
        {
            string result=string.Empty;
            string expected=parentHost + "/batch/test_db/sql";
            result=Urlshema.Batch(new TextToken() {Text=testDBname}).GetText();
            Assert.Equal(expected, result);
       }
        

    }    
    public class BodyShemaTest
    {
        TokenMiniFactory tokenFactory;
        CommandFactory commandFactory;
        FormatFactory formatFactory;
        OrientBodyFactory queryFactory;

        BodyShemas Bodyshema;
        string testDBname;
        CommandBuilder cb;
        public BodyShemaTest()
        {
            tokenFactory=new TokenMiniFactory();
            commandFactory=new CommandFactory();
            formatFactory=new FormatFactory();
            queryFactory=new OrientBodyFactory();

            Bodyshema=new BodyShemas(commandFactory, formatFactory, tokenFactory, queryFactory);

            ITokenMiniFactory tf=new TokenMiniFactory();
            IFormatFactory ff=new FormatFactory();

            cb=new CommandBuilder(tf, ff);

            IFormatFromListGenerator fg=new FormatFromListGenerator(tf);

            IOrientBodyFactory qbf=new OrientBodyFactory();
      
            CommandBuilder cbd=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            cbd.AddTokens(new List<ITypeToken>() {
                new OrientCreateToken()
                , new OrientClassToken()
                , new OrientPropertyToken()
               });
            cbd.AddFormat(new TextToken() {Text="{0} {1} {2}"});

            cb.Build(new List<ICommandBuilder>() {
                cbd
           }, new TextToken() {Text="{0}"});

            Bodyshema=new BodyShemas(commandFactory, formatFactory,tokenFactory,queryFactory);
            Bodyshema.AddHost(ConfigurationManager.AppSettings["ParentHost"]);
            testDBname=ConfigurationManager.AppSettings["TestDbname"];
       }
        [Fact]
        public void BodyCommandShemaCheck()
        {
            string result=Bodyshema.Command(cb).GetText();
            string expected="{\"command\":\"Create Class Property\"}";

            Assert.Equal(expected, result);
       }
        [Fact]
        public void BodyBatchShemaCheck()
        {
            string result=Bodyshema.Batch(cb).GetText();
            string expected="{\"transaction\":TRUE,\"operations\":[{\"type\":\"script\",\"language\":\"sql\",\"script\":[\" Create Class Property \"]}]}";            
            Assert.Equal(expected, result);
       }
    }
    public class CommandShemaTest
    {
        CommandShemasExplicit commandShemas;
        CommandBuilder commandBuilder, commandBuilder_SelectFrom;
        TokenMiniFactory tokenfactory;

        List<ITypeToken>
            selectNameGuidAccTokenListForBuiler
            , selectNameGuidAccTokenList;

        public CommandShemaTest()
        {
            commandBuilder_SelectFrom =
    new CommandBuilder(new TokenMiniFactory(), new FormatFactory()) { Tokens = new List<ITypeToken>() { new TextToken() { Text = "Name,GUID" } } };
            commandBuilder = new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            tokenfactory = new TokenMiniFactory();

            //initialize command interfaces
            this.commandShemas = new CommandShemasExplicit(
                new CommandFactory()
                , new FormatFactory()
                , new TokenMiniFactory()
                , new OrientQueryFactory()
                );

            selectNameGuidAccTokenListForBuiler = new List<ITypeToken>() {
                new TextToken() {Text="Name"},new TextToken() {Text="GUID"},new TextToken() {Text="Acc"}};
            selectNameGuidAccTokenList = new List<ITypeToken>() {
                new TextToken() {Text="Name"},new TextToken() {Text=","}
                ,new TextToken() {Text="GUID"},new TextToken() {Text=","}
                ,new TextToken() {Text="Acc"}};
        }

        [Fact]
        public void ShemaUniversalCommandCheck()
        {
            List<ITypeToken> tokens = new List<ITypeToken> {
                new TextToken() {Text="Select"}
                ,new TextToken() {Text="Name"}
                ,new TextToken() {Text=","}
                ,new TextToken() {Text="GUID"}
                ,new TextToken() {Text="from"}
                ,new TextToken() {Text="where"}
           };

            CommandBuilder cb = new CommandBuilder(new TokenMiniFactory(), new FormatFactory()) { Tokens = tokens };
            List<ICommandBuilder> cbl = new List<ICommandBuilder>() { cb };
            commandShemas.BuildNew(cbl, new TokenMiniFactory().Gap());

            string select = cb.GetText();
            Assert.Equal("Select Name , GUID from where", select);
        }

        #region Property 
        [Fact]
        public void ShemaPropertyCheck()
        {

            ITypeToken class_ = new TextToken() { Text = "Person" };
            ITypeToken prop_ = new TextToken() { Text = "Name" };
            ITypeToken type_ = new TextToken() { Text = "STRING" };
            ITypeToken mandatory_ = new TextToken() { Text = "TRUE" };
            ITypeToken notnull_ = new TextToken() { Text = "TRUE" };

            string res = commandShemas.Property(class_, prop_, type_, mandatory_, notnull_).GetText();

            Assert.Equal(" Property Person.Name STRING (MANDATORY TRUE,NOTNULL TRUE)", res);
        }
        [Fact]
        public void ShemaPropertyItemCheck()
        {

            ITypeToken class_ = new TextToken() { Text = "Person" };
            ITypeToken prop_ = new TextToken() { Text = "Name" };

            string res = commandShemas.PropertyItem(class_, prop_).GetText();

            Assert.Equal(@" Property Person.Name", res);
        }
        [Fact]
        public void ShemaPropertyTypeCheck()
        {

            ITypeToken type_ = new TextToken() { Text = "STRING" };

            string res = commandShemas.PropertyType(type_).GetText();

            Assert.Equal(@" STRING", res);
        }
        [Fact]
        public void ShemaPropertyConditionCheck()
        {

            ITypeToken mandatory_ = new TextToken() { Text = "TRUE" };
            ITypeToken notnull_ = new TextToken() { Text = "TRUE" };

            string res = commandShemas.PropertyCondition(mandatory_, notnull_).GetText();

            Assert.Equal(@" (MANDATORY TRUE,NOTNULL TRUE)", res);
        }
        #endregion

        #region Nest

        [Fact]
        public void ShemaNestParamCheck()
        {
            CommandBuilder commandBuilder = new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            commandBuilder.AddTokens(new List<ITypeToken>() { new TextToken() { Text = "Select from" } });
            commandBuilder.AddFormat(new TextToken() { Text = "{0}" });

            string result =
            this.commandShemas.Nest(commandBuilder, new OrientRoundBraketLeftToken(), new OrientRoundBraketRightToken(), new TextToken() { Text = string.Empty }).Text.Text;

            Assert.Equal("(Select from)", result);
        }

        #endregion

        [Fact]
        public void ShemaExtendesCheck()
        {
            ITypeToken param = new TextToken() { Text = "Person" };
            string result = this.commandShemas.Extends(param).GetText();
            Assert.Equal(" Extends Person", result);
        }



        #region Vertex

        [Fact]
        public void ShemaVertexCheck()
        {
            string result = this.commandShemas.Vertex().GetText();
            Assert.Equal(" Vertex", result);
        }
        [Fact]
        public void ShemaVertexParamCheck()
        {
            string result = this.commandShemas.Vertex(new TextToken() { Text = "Person" }).GetText();
            Assert.Equal(" Vertex Person", result);
        }

        #endregion

        #region Create

        [Fact]
        public void ShemaCreateCheck()
        {
            string result = this.commandShemas.Create().GetText();
            Assert.Equal("Create", result);
        }
        [Fact]
        public void ShemaCreateParamCheck()
        {
            string result = this.commandShemas.Create(new TextToken() { Text = "class" }).GetText();
            Assert.Equal("Create class", result);
        }

      

        #endregion

        #region Where

        [Fact]
        public void ShemaWhereCheck()
        {
            string result = this.commandShemas.Where().GetText();
            Assert.Equal(" where", result);
        }
        [Fact]
        public void ShemaWhereWithParamsDoubleCheck()
        {
            List<ITypeToken> tokenList = new List<ITypeToken>() {
                new TextToken() {Text="1=1"},new TextToken() {Text="and"},new TextToken() {Text="2=2"}};
            this.commandBuilder.Build(tokenList, new FormatFromListGenerator(new TokenMiniFactory()).FromatFromTokenArray(tokenList, tokenfactory.Gap()));
            string result = this.commandShemas.Where(this.commandBuilder).GetText();
            Assert.Equal(" where 1=1 and 2=2", result);
        }

        #endregion

        #region From
        [Fact]
        public void ShemaFromCheck()
        {
            string result = this.commandShemas.From().GetText();
            Assert.Equal(" from", result);
        }
        [Fact]
        public void ShemaFromParamCheck()
        {
            string result = this.commandShemas.From(new TextToken() { Text = "Person" }).GetText();
            Assert.Equal(" from Person", result);
        }
        #endregion

        #region Select
        [Fact]
        public void ShemaSelectCheck()
        {
            string result = this.commandShemas.Select().GetText();
            Assert.Equal("Select ", result);
        }
        [Fact]
        public void ShemaSelectParametersCheck()
        {
            string result = this.commandShemas.Select(selectNameGuidAccTokenList).GetText();
            Assert.Equal("Select Name,GUID,Acc", result);
        }
        [Fact]
        public void ShemaSelectParametersTokenListCheck()
        {
            this.commandBuilder.Build(selectNameGuidAccTokenListForBuiler,
                new FormatFromListGenerator(new TokenMiniFactory()).FromatFromTokenArray(selectNameGuidAccTokenListForBuiler, tokenfactory.Coma()));
            string result = this.commandShemas.Select(this.commandBuilder).GetText();
            Assert.Equal("Select Name,GUID,Acc ", result);
        }
        #endregion

        #region Content
        [Fact]
        public void SchemaContentCheck()
        {
            string result = this.commandShemas.Content(new TextToken() { Text = "{\"a\":\"0\"}" }).Build().GetText();
            Assert.Equal(" content {\"a\":\"0\"}", result);
        }
        #endregion 
    }

    public class FormatGeneratorTest
    {
        IFormatFactory formatFactory_;
        ITokenMiniFactory miniFactory_;
        CommandFactory commandFactory_;

        IFormatFromListGenerator formatGenerator_;
     
        string ExpectedFromTokenListgenerator=@"{0} {1} {2}";
        string ExpectedFromTokenListWithDelimeter= @"{0}.{1}.{2}";
        string ExpectedFromCommandList=@"{0} {1} {2},{3} {4}";
        string ExpectedFromCommandListWithFormat=@"{0}:{1}.{2},{3}|{4}";

        public FormatGeneratorTest()
        {
            miniFactory_=new TokenMiniFactory();
            formatFactory_=new FormatFactory();
            commandFactory_=new CommandFactory();
            formatGenerator_=new FormatFromListGenerator(miniFactory_);
       }
        [Fact]
        public void FromTokenListgeneratorCheck()
        {
            List<ITypeToken> tokens=new List<ITypeToken>() {
                new TextToken() {Text="Item1"},new TextToken() {Text="Item2"},new TextToken() {Text="Item3"}
           };

            string result=formatGenerator_.FromatFromTokenArray(tokens,miniFactory_.Gap()).Text;

            Assert.Equal(ExpectedFromTokenListgenerator, result);
       }
        [Fact]
        public void FromTokenListWithDelimeterCheck()
        {
            List<ITypeToken> tokens=new List<ITypeToken>() {
                new TextToken() {Text="Item1"},new TextToken() {Text="Item2"},new TextToken() {Text="Item3"}
           };

            string result=formatGenerator_.FromatFromTokenArray(tokens, miniFactory_.Dot()).Text;

            Assert.Equal(ExpectedFromTokenListWithDelimeter, result);
       }
        [Fact]
        public void FromCommandListCheck()
        {
            List<ITypeToken> tokens1=new List<ITypeToken>() {
                new TextToken() {Text="Item1"},new TextToken() {Text="Item2"},new TextToken() {Text="Item3"}
           };

            List<ITypeToken> tokens2=new List<ITypeToken>() {
                new TextToken() {Text="Item4"},new TextToken() {Text="Item5"}
           };

            CommandBuilder cb1=new CommandBuilder(miniFactory_, formatFactory_, tokens1,
                formatFactory_.FormatGenerator(miniFactory_).FromatFromTokenArray(tokens1,miniFactory_.Gap()));
            CommandBuilder cb2=new CommandBuilder(miniFactory_, formatFactory_, tokens2,
                formatFactory_.FormatGenerator(miniFactory_).FromatFromTokenArray(tokens2, miniFactory_.Gap()));

            List<ICommandBuilder> builders=new List<ICommandBuilder>() {
                cb1,cb2
           };

            ICommandBuilder builder=commandFactory_.CommandBuilder(miniFactory_, formatFactory_);
            builder.BindBuilders(builders,
                formatFactory_.FormatGenerator(miniFactory_).FromatFromTokenArray(builders, miniFactory_.Coma()));
            string result=builder.Build().FormatPattern.Text;
           
            Assert.Equal(ExpectedFromCommandList, result);
       }
        [Fact]
        public void FromCommandListWithFormatCheck()
        {
            List<ITypeToken> tokens1=new List<ITypeToken>() {
                new TextToken() {Text="Item1"},new TextToken() {Text="Item2"},new TextToken() {Text="Item3"}
           };
            ITypeToken format1=miniFactory_.NewToken("{0}:{1}.{2}");

            List<ITypeToken> tokens2=new List<ITypeToken>() {
                new TextToken() {Text="Item4"},new TextToken() {Text="Item5"}
           };
            ITypeToken format2=miniFactory_.NewToken("{0}|{1}");

            CommandBuilder cb1=new CommandBuilder(miniFactory_, formatFactory_,tokens1,format1) {};
            CommandBuilder cb2=new CommandBuilder(miniFactory_, formatFactory_,tokens2,format2) {};            

            List<ICommandBuilder> builders=new List<ICommandBuilder>() {
                cb1,cb2
           };

            ICommandBuilder builder=commandFactory_.CommandBuilder(miniFactory_, formatFactory_);
            builder.BindBuilders(builders,
                formatFactory_.FormatGenerator(miniFactory_).FromatFromTokenArray(builders, miniFactory_.Coma()));
            string result=builder.Build().FormatPattern.Text;

            Assert.Equal(ExpectedFromCommandListWithFormat, result);
       }

    }

    public class WebResponseReaderIntegrationTest
    {
        WebResponseReader webResponseReader;

        WebRequest webRequest;
        HttpWebRequest httpWebRequest;

        string webRequestsCheckURL;

        //arrange
        public WebResponseReaderIntegrationTest()
        {
            webResponseReader=new WebResponseReader();
            webRequestsCheckURL=@"http://duckduckgo.com";

            webRequest=WebRequest.Create(webRequestsCheckURL);
            httpWebRequest=HttpWebRequest.CreateHttp(webRequestsCheckURL);

       }

        [Fact]
        public void GetWebResponseReturns()
        {
            string result=webResponseReader.ReadResponse(webRequest.GetResponse());

            Assert.NotNull(result);
            Assert.NotEqual(string.Empty, result);
       }

        [Fact]
        public void GetHttpWebResponse()
        {
            string result=webResponseReader.ReadResponse(httpWebRequest.GetResponse());

            Assert.NotNull(result);
            Assert.NotEqual(string.Empty, result);
       }
       
    }
    public class JSONmanagerIntegrationTests
    {
        JSONManager jm;
        string str0, act0, str1, act1, str4;

        //arrange
        public JSONmanagerIntegrationTests()
        {

            str0="{\"result\":[{\"Name\":\"value1\",\"sAMAccountName\":\"acc1\"},{\"Name\":\"value2\",\"sAMAccountName\":\"acc2\"}]}";
            act0="[{\"sAMAccountName\":\"acc1\",\"Name\":\"value1\"},{\"sAMAccountName\":\"acc2\",\"Name\":\"value2\"}]";

            str1 =
"{\"news\":[{\"Title\":\"value1\",\"Article\":\"value3\"},{\"Title\":\"value2\",\"Article\":\"value4\",\"tags\":[{\"Name\":\"value7\"},{\"Name\":\"value8\"}]}]}";
            act1="[\"value1\",\"value2\"]";

            str4="[{\"Name\":\"value1\"},{\"Name\":\"value2\"}]";
            jm=new JSONManager();
       }

        [Fact]
        public void JSONmanagerParseJSONParentColectiontoClassReturnsString()
        {
            //Extract tokens from JSON response parent Node, convert to collection of model objects
            IJEnumerable<JToken> jte=jm.ExtractFromParentNode(str0, "result");
            //Extract + convert JSON to collection of model objects
            IEnumerable<Person> res=jm.DeserializeFromParentNode<Person>(str0, "result");
            //to string  Selectable -> ignore nulls, no intending
            string resp=jm.CollectionToStringFormat<Person>(res,
                new JsonSerializerSettings() {NullValueHandling=NullValueHandling.Ignore, Formatting=Formatting.None});

            Assert.Equal(resp, act0);
       }

        [Fact]
        public void JSONmanagerParseJSONParentChildColectiontoStringReturnsString()
        {
            //extract from JSON parent node
            IJEnumerable<JToken> jte=jm.ExtractFromParentChildNode(str1, "news", "Title");
            //convert to collection of strings
            IEnumerable<string> res=jm.DeserializeFromParentChildNode<string>(str1, "news", "Title");
            //to string  Selectable -> ignore nulls, no intending
            string resp=jm.CollectionToStringFormat<string>(res
                , new JsonSerializerSettings() {NullValueHandling=NullValueHandling.Include, Formatting=Formatting.None});

            Assert.Equal(resp, act1);
       }

        [Fact]
        public void JSONmngParsefromChildCollectionreturnsString()
        {
            //extract from child nodes
            IJEnumerable<JToken> jte=jm.ExtractFromChildNode(str4, "Name");
            //to collection
            IEnumerable<string> res=jm.DeserializeFromChildNode<string>(str4, "Name");
            //to string  Selectable -> ignore nulls, no intending
            string resp=jm.CollectionToStringFormat<string>(res
                , new JsonSerializerSettings() {NullValueHandling=NullValueHandling.Include, Formatting=Formatting.None});

            Assert.Equal(resp, act1);
       }

    }
    public class URIBuilderIntergrationTest
    {
        string
        authUrlExpected, commandURLExpected, hostExpected, portExpected, dbExpected, commandExpected,
        patternAuthExpected, patternCommandExpected, commandTokenExpected
        , selectClauseExpected, whereClauseExpected, createCommandExpected
        , createPersonURLExpected, deletePersonURLExpected
        , formatGeneratedExpected
        ;

        ITypeToken host_, port_, db_, command_, authenticate_;

        List<ITypeToken> authURLTokens, commandURLTokens;

        OrientAuthenticationURLFormat authURLformat;
        OrientCommandURLFormat commURLformat;

        CommandBuilder AuthURL;
        CommandBuilder CommURL;
        private string selectPersonURLExpected;

        //arrange
        public URIBuilderIntergrationTest()
        {

            selectClauseExpected=@"Select from Person";
            whereClauseExpected=@"where 1=1";
            createCommandExpected =
            "Create Vertex Person content {\"Name\":\"0\",\"GUID\":\"0\",\"Created\":\"2017-01-01 00:00:00\",\"Changed\":\"2017-01-01 00:00:00\"}";

            host_=new OrientHost();
            port_=new OrientPort();
            db_=new OrientDatabaseNameToken();

            command_=new OrientCommandToken();
            authenticate_=new OrientAuthenticateToken();

            authURLTokens=new List<ITypeToken>() {new OrientHost(), new OrientPort(), new OrientAuthenticateToken(), new OrientDatabaseNameToken()};
            commandURLTokens=new List<ITypeToken>() {new OrientHost(), new OrientPort(), new OrientCommandToken(), new OrientDatabaseNameToken(), new OrientCommandSQLTypeToken()};

            authURLformat=new OrientAuthenticationURLFormat();
            commURLformat=new OrientCommandURLFormat();

            AuthURL=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            CommURL=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());

            AuthURL.AddTokens(authURLTokens);
            AuthURL.AddFormat(authURLformat);
            AuthURL.Build();

            CommURL.AddTokens(commandURLTokens);
            CommURL.AddFormat(commURLformat);
            CommURL.Build();

            hostExpected=ConfigurationManager.AppSettings["ParentHost"];
            portExpected="2480";
            dbExpected=ConfigurationManager.AppSettings["ParentDBname"];
            commandExpected="connect";
            commandTokenExpected="command";
            patternAuthExpected="{0}:{1}/{2}/{3}";
            patternCommandExpected="{0}:{1}/{2}/{3}/{4}";

            formatGeneratedExpected="{0} {1} {2} {3} {4} {5} {6}";

            authUrlExpected=string.Format(@"{0}:2480/connect/{1}", hostExpected, dbExpected);
            commandURLExpected=string.Format(@"{0}:2480/command/{1}/sql", hostExpected, dbExpected);

            createPersonURLExpected=string.Format(
                "{0}:2480/command/{1}/sql/Create Vertex Person content {2}"
               , ConfigurationManager.AppSettings["ParentHost"], dbExpected, "{\"Name\":\"0\",\"GUID\":\"0\",\"Created\":\"2017-01-01 00:00:00\",\"Changed\":\"2017-01-01 00:00:00\"}");
            selectPersonURLExpected=string.Format(@"{0}:2480/command/{1}/sql/{2}"
                 ,ConfigurationManager.AppSettings["ParentHost"], dbExpected, "Select from Person where Name=0");
            deletePersonURLExpected=string.Format(@"{0}:2480/command/{1}/sql/{2}"
                , ConfigurationManager.AppSettings["ParentHost"], dbExpected, "Delete Vertex from Person where Name=0");

       }

        //Token format and clause test
        [Fact]
        public void OrientAuthenticateTest_ReturnsValidString()
        {
            Assert.Equal(commandExpected, authenticate_.Text);
       }
        [Fact]
        public void OrientCommandTest_ReturnsValidString()
        {
            Assert.Equal(commandTokenExpected, command_.Text);
       }
        [Fact]
        public void OrientHostTest_ReturnsValidString()
        {
            Assert.Equal(hostExpected, host_.Text);
       }
        [Fact]
        public void OrientPortTest_ReturnsValidString()
        {
            Assert.Equal(portExpected, port_.Text);
       }
        [Fact]
        public void OrientDatabaseTokenTest_ReturnsValidString()
        {
            Assert.Equal(dbExpected, db_.Text);
       }
        [Fact]
        public void OrientAuthenticationFormatTest()
        {
            Assert.Equal(patternAuthExpected, authURLformat.Text);
       }
        [Fact]
        public void OrientAuthenticationUrlBuilderTestReturnsvalidConnectURL()
        {
            Assert.Equal(authUrlExpected, AuthURL.Text.Text);
       }
        [Fact]
        public void OrientCommandURLFormatTestReturnsValidCommandUrl()
        {
            Assert.Equal(patternCommandExpected, commURLformat.Text);
       }
        [Fact]
        public void OrientCommandUrlBuilderTestReturnsvalidConnectURL()
        {
            Assert.Equal(commandURLExpected, CommURL.Text.Text);
       }


        //test command,select,where clauses and slect full url test
        [Fact]
        public void SelectClauseBuild()
        {
            // tokens for query Select part
            List<ITypeToken> selectCommandTokens=new List<ITypeToken>()
            {new OrientSelectToken(), new OrientFromToken(), new OrientPersonToken()};
            //format for Select concat
            //{0}/{1} {2} {3}
            //<commandURL>/<select from classname>
            OrientSelectClauseFormat of=new OrientSelectClauseFormat();
            //build full command URL with URL and command Parts            
            CommandBuilder selectUrlPart=new CommandBuilder(
                new TokenMiniFactory(), new FormatFactory()
                );
            selectUrlPart.AddTokens(selectCommandTokens);
            selectUrlPart.AddFormat(of);
            selectUrlPart.Build();

            //select query URL text
            string selectQuery=selectUrlPart.Text.Text;
            Assert.Equal(selectClauseExpected, selectQuery);
       }
        [Fact]
        public void WhereClauseBuild()
        {
            //Where command tokens with test hardcoded condition 
            //<<!!! condition to concatenation builder (infinite where)
            List<ITypeToken> whereCommandTokens=new List<ITypeToken>()
            {new OrientWhereToken(), new TextToken(){Text=@"1=1"}};
            //format for where concat
            OrientWhereClauseFormat wf=new OrientWhereClauseFormat();
            //build where clause         
            CommandBuilder whereUrlPart=new CommandBuilder(
               new TokenMiniFactory(), new FormatFactory()
               );
            whereUrlPart.AddTokens(whereCommandTokens);
            whereUrlPart.AddFormat(wf);
            whereUrlPart.Build();

            //where query text
            string whereQuery=whereUrlPart.Text.Text;
            Assert.Equal(whereClauseExpected, whereQuery);
       }

        [Fact]
        public void SelectUrlBuild()
        {

            //Initialize Format for command URL string concat 
            //-> {0}:{1}/{2}/{3}
            // <host>:<port>/command/<dbname>/sql
            OrientCommandURLFormat cf=new OrientCommandURLFormat();
            //tokens for command url part
            List<ITypeToken> urlCommandTokens=new List<ITypeToken>()
            {new OrientHost(), new OrientPort(), new OrientCommandToken(), new OrientDatabaseNameToken(), new OrientCommandSQLTypeToken()};
            //Command URL text
            CommandBuilder commandUrlPart=new CommandBuilder(
            new TokenMiniFactory(), new FormatFactory()
            );
            commandUrlPart.AddTokens(urlCommandTokens);
            commandUrlPart.AddFormat(cf);   

            // tokens for query Select part
            List<ITypeToken> selectCommandTokens=new List<ITypeToken>()
            {new OrientSelectToken(), new OrientFromToken(), new OrientPersonToken()};
            //format for Select concat
            //{0}/{1} {2} {3}
            //<commandURL>/<select from classname>
            OrientSelectClauseFormat of=new OrientSelectClauseFormat();
            //build full command URL with URL and command Parts                       
            CommandBuilder selectUrlPart=new CommandBuilder(
               new TokenMiniFactory(), new FormatFactory()
               );
            selectUrlPart.AddTokens(selectCommandTokens);
            selectUrlPart.AddFormat(of);      

            //Where command tokens with test hardcoded condition 
            //<<!!! condition to concatenation builder (infinite where)
            List<ITypeToken> whereCommandTokens=new List<ITypeToken>()
            {new OrientWhereToken(), new TextToken(){Text=@"Name=0"}};
            //format for where concat
            OrientWhereClauseFormat wf=new OrientWhereClauseFormat();
            //build where clause          
            CommandBuilder whereUrlPart=new CommandBuilder(
            new TokenMiniFactory(), new FormatFactory()
            );
            whereUrlPart.AddTokens(whereCommandTokens);
            whereUrlPart.AddFormat(wf);

            //collection of FULL tokens 
            //@"{0}:{1}/{2}/{3}/{4}/{5} {6} {7} {8} {9}"
            List<ICommandBuilder> CommandTokens=new List<ICommandBuilder>(){
                commandUrlPart,selectUrlPart,whereUrlPart
           };
            //Aggregate all query TokenManagers to one Select URL command with where
            CommandBuilder commandSample=new CommandBuilder(new TokenMiniFactory(), new FormatFactory()
                , CommandTokens, new TextToken() {Text=@"{0}/{1} {2}"});          
            commandSample.Build();
            //full select query command
            string selectcommandURL=commandSample.Text.Text;

            Assert.Equal(selectPersonURLExpected, selectcommandURL);
       }
        [Fact]
        public void CreatePersonCommandTextBuild()
        {
            Person per=new Person()
            {Name="0", GUID="0", Changed=new DateTime(2017, 01, 01, 00, 00, 00), Created=new DateTime(2017, 01, 01, 00, 00, 00)};
            JSONManager jm=new JSONManager();
            string contentText=jm.SerializeObject(per,
                new JsonSerializerSettings()
                {
                    NullValueHandling=NullValueHandling.Ignore,
                    Formatting=Formatting.None,
                    DateFormatString=@"yyyy-MM-dd HH:mm:ss"
               });
            TextToken content=new TextToken() {Text=contentText};

            List<ITypeToken> CreateTokens=new List<ITypeToken>() {
                new OrientCreateToken(),new OrientVertexToken(),new OrientPersonToken(), new OrientContentToken()
                , content};
            OrientCreateVertexCluaseFormat cf=new OrientCreateVertexCluaseFormat();           
            CommandBuilder cb=new CommandBuilder(
          new TokenMiniFactory(), new FormatFactory()
          );
            cb.AddTokens(CreateTokens);
            cb.AddFormat(cf);
            cb.Build();
            string CreateCommand=cb.GetText();

            Assert.Equal(createCommandExpected, CreateCommand);
       }

        [Fact]
        public void CreateSelectDelete_URLS_build_check()
        {
            List<ITypeToken> commandTokents =
  new List<ITypeToken>() {new OrientHost(), new OrientPort(), new OrientCommandToken(), new OrientDatabaseNameToken(), new OrientCommandSQLTypeToken()};

            Person per=new Person()
            {Name="0", GUID="0", Changed=new DateTime(2017, 01, 01, 00, 00, 00), Created=new DateTime(2017, 01, 01, 00, 00, 00)};
            JSONManager jm=new JSONManager();
            string contentText=jm.SerializeObject(per,
                new JsonSerializerSettings()
                {
                    NullValueHandling=NullValueHandling.Ignore,
                    Formatting=Formatting.None,
                    DateFormatString=@"yyyy-MM-dd HH:mm:ss"
               });
            TextToken personContent=new TextToken() {Text=contentText};

            List<ITypeToken> CreateTokens =
    new List<ITypeToken>() {new OrientCreateToken(), new OrientVertexToken(), new OrientPersonToken(), new OrientContentToken(), personContent};
            List<ITypeToken> selectTokens =
    new List<ITypeToken>() {new OrientSelectToken(), new OrientFromToken(), new OrientPersonToken()};
            List<ITypeToken> DeleteToken =
    new List<ITypeToken>() {new OrientDeleteToken(), new OrientVertexToken(), new OrientFromToken(), new OrientPersonToken()};
            List<ITypeToken> whereTokens =
    new List<ITypeToken>() {new OrientWhereToken(), new TextToken() {Text="Name=0"}};

            OrientCommandURLFormat uf=new OrientCommandURLFormat();
            OrientCreateVertexCluaseFormat cf=new OrientCreateVertexCluaseFormat();
            OrientSelectClauseFormat sf=new OrientSelectClauseFormat();
            OrientDeleteVertexCluaseFormat df=new OrientDeleteVertexCluaseFormat();
            OrientWhereClauseFormat wf=new OrientWhereClauseFormat();
            
            CommandBuilder ub=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            ub.AddTokens(commandTokents);
            ub.AddFormat(uf);
            CommandBuilder cb=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            cb.AddTokens(CreateTokens);
            cb.AddFormat(cf);
            CommandBuilder sb=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            sb.AddTokens(selectTokens);
            sb.AddFormat(sf);
            CommandBuilder db=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            db.AddTokens(DeleteToken);
            db.AddFormat(df);
            CommandBuilder wb=new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            wb.AddTokens(whereTokens);
            wb.AddFormat(wf);


            string url=ub.GetText();
            string create=cb.GetText();
            string select=sb.GetText();
            string delete=db.GetText();
            string where=wb.GetText();

            List<ICommandBuilder> createTk=new List<ICommandBuilder>() {ub, cb};
            List<ICommandBuilder> selectTk=new List<ICommandBuilder>() {ub, sb, wb};
            List<ICommandBuilder> deleteTk=new List<ICommandBuilder>() {ub, db, wb};

            CommandBuilder cU =
    new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            CommandBuilder sU =
    new CommandBuilder(new TokenMiniFactory(), new FormatFactory());
            CommandBuilder dU =
    new CommandBuilder(new TokenMiniFactory(), new FormatFactory());

            //cU.AddFormat(new TextToken() {Text=@"{0}/{1}"});
            cU.BindBuilders(createTk, new TextToken() {Text=@"{0}/{1}"});
            cU.Build();

            sU.BindBuilders(selectTk, (new TextToken() {Text=@"{0}/{1} {2}"}));                       
            sU.Build();

            dU.BindBuilders(deleteTk, (new TextToken() {Text=@"{0}/{1} {2}"}));                   
            dU.Build();

            string cUt=cU.GetText();
            string sUt=sU.GetText();
            string dUt=dU.GetText();

            Assert.Equal(createPersonURLExpected, cUt);
            Assert.Equal(selectPersonURLExpected, sUt);
            Assert.Equal(deletePersonURLExpected, dUt);

       }

        [Fact]
        public void TokenFormatGeneratorCheck()
        {

            List<ITypeToken> tt=new List<ITypeToken>()
            {
                 new OrientCreateToken(), new OrientEdgeToken(), new OrientSubUnitToken(), new OrientFromToken(),
                new OrientUnitToken(), new OrientToToken(),new OrientUnitToken()
           };
            TextFormatGenerate og=new TextFormatGenerate(tt);

            string formatAct=og.Text;

            Assert.Equal(formatGeneratedExpected, formatAct);
       }

    }

    public class AdinTceTests
    {

        AdinTceCommandBuilder _CommandBuilder;
        AdinTceWebManager _webManager;
        AdinTceResponseReader _responseReader;
        AdinTceJsonManager _jsonManager;
        AdinTceRepo adinTceRepo;

        List<string> expectedUrls, actualUrls;

        string adinTceUrl=ConfigurationManager.AppSettings["AdinTceUrl"] + "/holiday/full";

        string testGUID= "542ceb48-8454-11e4-acb0-00c2c66d13b0";
        //"18222799-602e-11e4-ad69-00c2c66d13b0" //test_long
        //"18a14516-cbb4-11e4-b849-f80f41d3dd35" //test
        //"ed53c8ea-c179-11e4-8edf-f80f41d3dd35" //Fill
        //"c1a4c984-a00e-11e6-80db-005056813668" //Alex
        //"542ceb48-8454-11e4-acb0-00c2c66d13b0" //Rudenko
        //"ba124b8e-9857-11e7-8119-005056813668" //Myself
        //1ab7726d-0daf-11e5-980e-000c299fef81 //nesterovayv

        string expectedResult =
"{\"GUID\":\"18a14516-cbb4-11e4-b849-f80f41d3dd35\",\"Position\":\"Специалист 1 категории\",\"Holidays\":[{\"LeaveType\":\"Отпуск основной\",\"Days\":0.0},{\"LeaveType\":\"Отпуск основной\",\"Days\":0.0}],\"Vacations\":[{\"LeaveType\":\"Отпуск основной\",\"DateStart\":\"13.02.2017\",\"DateFinish\":\"19.02.2017\",\"DaysSpent\":7},{\"LeaveType\":\"Отпуск основной\",\"DateStart\":\"14.07.2017\",\"DateFinish\":\"27.07.2017\",\"DaysSpent\":14}]}";

        public AdinTceTests()
        {

            _CommandBuilder=new AdinTceCommandBuilder(new TokenMiniFactory(), new FormatFactory());
            _jsonManager=new AdinTceJsonManager();
            _responseReader=new AdinTceResponseReader();

            actualUrls=new List<string>();
            expectedUrls=new List<string>
            {
                string.Format("{0}/{1}"
                        , ConfigurationManager.AppSettings["AdinTceUrl"]
                        , "holiday/full"),
               string.Format("{0}/{1}"
                        , ConfigurationManager.AppSettings["AdinTceUrl"]
                        , "holiday/part"),
               string.Format("{0}/{1}"
                        , ConfigurationManager.AppSettings["AdinTceUrl"]
                        , "vacation/full"),
                string.Format("{0}/{1}"
                        , ConfigurationManager.AppSettings["AdinTceUrl"]
                        , "vacation/part")
           };

            AddActualUrls();

            _webManager=new AdinTceWebManager();            
            _webManager.SetCredentials(new System.Net.NetworkCredential(
              ConfigurationManager.AppSettings["AdinTceLogin"], ConfigurationManager.AppSettings["AdinTcePassword"]));
            _webManager.AddRequest(adinTceUrl);

            //_webManager.SetTimeout(11000);
            adinTceRepo=new AdinTceRepo(_CommandBuilder, _webManager, _responseReader, _jsonManager);

       }

        public void AddActualUrls()
        {

            _CommandBuilder.SetText(
            new List<IQueryManagers.ITypeToken>() {new AdinTceURLToken(), new AdinTceHolidatyToken(), new AdinTceFullToken()}
            , new AdinTceURLformat());
            actualUrls.Add(_CommandBuilder.GetText());

            _CommandBuilder.SetText(
            new List<IQueryManagers.ITypeToken>() {new AdinTceURLToken(), new AdinTceHolidatyToken(), new AdinTcePartToken()}
            , new AdinTceURLformat());
            actualUrls.Add(_CommandBuilder.GetText());

            _CommandBuilder.SetText(
            new List<IQueryManagers.ITypeToken>() {new AdinTceURLToken(), new AdinTceVacationToken(), new AdinTceFullToken()}
            , new AdinTceURLformat());
            actualUrls.Add(_CommandBuilder.GetText());

            _CommandBuilder.SetText(
            new List<IQueryManagers.ITypeToken>() {new AdinTceURLToken(), new AdinTceVacationToken(), new AdinTcePartToken()}
            , new AdinTceURLformat());
            actualUrls.Add(_CommandBuilder.GetText());

       }

        [Fact]
        public void AdinTceCommandBuilder()
        {
            bool result=true;
            for (int i=0; i < expectedUrls.Count; i++)
            {
                if (!actualUrls[i].Equals(expectedUrls[i])) {result=false;}
           }
            Assert.True(result);
       }

        [Fact]
        public void AdinTceWebManagerTest()
        {
            WebResponse wr=null;
          
            try
            {               
                wr=_webManager.GetResponse64("GET");
           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

            Assert.NotNull(wr);
       }

        [Fact]
        public void AdinTceWebResponseTest()
        {
            string result=null;
            try
            {
                _webManager.AddRequest(adinTceUrl);
                result=_responseReader.ReadResponse(_webManager.GetResponse64("GET"));
           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

            Assert.NotNull(result);
       }

        [Fact]
        public void AdinTce_jsonManagerTest()
        {
            List<Holiday> result=null;
            string temp=null;

            try
            {
                _webManager.SetCredentials(new NetworkCredential(
                    ConfigurationManager.AppSettings["AdinTceLogin"]
                    , ConfigurationManager.AppSettings["AdinTcePassword"]));
                _webManager.AddRequest(adinTceUrl);   
                
                temp=_responseReader.ReadResponse(_webManager.GetResponse64("GET"));
           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

            result=_jsonManager.DeserializeFromParentNode<Holiday>(temp).ToList();

            Assert.NotNull(result);

       }

        [Fact]
        public void AdinTce_resultTest()
        {

            string holStr="[ {\"GUID\": \"18a14516-cbb4-11e4-b849-f80f41d3dd35\", \"Position\": \"Специалист 1 категории Группы \\\"Тушино\\\"\", \"Holidays\": [ {\"LeaveType\": \"Основной\", \"Days\": 13} ]} ]";
            string vacStr= "[ { \"GUID\": \"18a14516-cbb4-11e4-b849-f80f41d3dd35\", \"Position\": \"Специалист 1 категории\", \"Holidays\": [ { \"LeaveType\": \"Отпуск основной\", \"DateStart\": \"13.02.2017\", \"DateFinish\": \"19.02.2017\", \"DaysSpent\": 7 }, { \"LeaveType\": \"Отпуск основной\", \"DateStart\": \"14.07.2017\", \"DateFinish\": \"27.07.2017\", \"DaysSpent\": 14 } ] } ]";
            string res=string.Empty;
            string temp=null, holidayUrl=null, vacationUrl=null, result=null;


            AdinTcePOCO adp=new AdinTcePOCO();

            IEnumerable<List< Holiday >> dhl=_jsonManager.DeserializeFromParentChildren<List<Holiday>>(vacStr, "Holidays");
            IEnumerable<List<Vacation>> vhl=_jsonManager.DeserializeFromParentChildren<List<Vacation>>(vacStr, "Holidays");
            IEnumerable<GUIDPOCO> gpl=_jsonManager.DeserializeFromParentNode<GUIDPOCO>(vacStr);

            adp.vacations = new List<Vacation>();
            foreach (List<Vacation> h1 in vhl)
            {
                adp.vacations.AddRange(h1);
            }          
            adp.holidays = new List<Holiday>();
            foreach (List< Holiday> h1 in dhl){                
                adp.holidays.AddRange(h1);                
            }

            adp.GUID_=gpl.Select(s => s).FirstOrDefault().GUID_;
            adp.Position=gpl.Select(s => s).FirstOrDefault().Position;

            res=JsonConvert.SerializeObject(adp);

            _CommandBuilder.SetText(
            new List<IQueryManagers.ITypeToken>() {new AdinTceURLToken(), new AdinTceHolidatyToken(), new AdinTcePartToken(),
            new QueryManagers.TextToken(){Text= testGUID}}
            , new AdinTcePartformat());
            holidayUrl=_CommandBuilder.GetText();

            _CommandBuilder.SetText(
            new List<IQueryManagers.ITypeToken>() {new AdinTceURLToken(), new AdinTceVacationToken(), new AdinTcePartToken(),
            new QueryManagers.TextToken(){Text= testGUID}}
            , new AdinTcePartformat());
            vacationUrl=_CommandBuilder.GetText();

            try
            {

                _webManager.AddRequest(holidayUrl);
                temp=_responseReader.ReadResponse(_webManager.GetResponse("GET"));

                gpl=_jsonManager.DeserializeFromParentNode<GUIDPOCO>(holStr);
                dhl=_jsonManager.DeserializeFromParentChildren<List<Holiday>>(holStr, "Holidays");

                _webManager.AddRequest(vacationUrl);
                temp=_responseReader.ReadResponse(_webManager.GetResponse("GET"));

                vhl=_jsonManager.DeserializeFromParentChildren<List<Vacation>>(vacStr, "Holidays");

                adp.holidays = new List<Holiday>();
                foreach (List<Holiday> h1 in dhl)
                {
                    adp.holidays.AddRange(h1);
                }

                adp.vacations= new List<Vacation>();
                foreach (List<Vacation> h1 in vhl)
                {
                    adp.vacations.AddRange(h1);
                }        
                adp.GUID_=gpl.Select(s => s).FirstOrDefault().GUID_;
                adp.Position=gpl.Select(s => s).FirstOrDefault().Position;

           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

            result=_jsonManager.SerializeObject(adp);

            Assert.Equal(expectedResult, result);

       }

        [Fact]
        public void AdinTceHoliVationTest()
        {
            string result=adinTceRepo.HoliVation(testGUID);
            Assert.NotNull( result);

       }


    }

    public class NewsUOWCHeck
    {

        NewsUOWs.NewsUow nu;
        public NewsUOWCHeck()
        {
            nu = new NewsUOWs.NewsUow();
        }

        [Fact]
        public void UOWAccCheck()
        {
            string result = nu.UserAcc();
            Assert.Equal("Neprintsevia", result);
        }
        [Fact]
        public void UOWByAccCheck()
        {
            string result = nu.GetByAccount("Neprintsevia").id;
            Assert.Equal("#22:174", result);
        }
        [Fact]
        public void UOWByNameCheck()
        {
            IEnumerable<Person> result = nu.SearchByName("олов");
            Assert.NotEmpty(result);
        }
        [Fact]
        public void UOWbyIdCheck()
        {
            string result = nu.GetOrientObjectById<Person>("22:174").id;
            Assert.Equal("#22:174",result);
        }
        [Fact]
        public void UOWCreateNewsCheck()
        {            
            JSONManager jm = new JSONManager();
            Note nt = new Note() { content = "Very interesting new", Name="News" };            
            string news = jm.SerializeObject(nt);
            Person p = nu.GetByAccount("Neprintsevia");
            string result = nu.CreateNews(p, nt).id;
            Assert.NotNull(result);
        }
        [Fact]
        public void UOWDeleteRelation()
        {
            Person p = nu.GetByAccount("Neprintsevia");
            
            string result = nu.DeleteNews(p, "");
            Assert.Equal("Deleted", result);
        }

    }

}
