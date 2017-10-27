//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;

using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Net.Http;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Moq;

namespace ConsoleApp1.Tests
{

    public class WebResponseUnitTest
    {
        Mock<IWebManager> webManager = new Mock<IWebManager>();

        public WebResponseUnitTest()
        {

            webManager.Setup(m => m.GetResponse(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void GetResponseReturnsNullTest()
        {
            webManager.Object.GetResponse(@"localhost", "GET");
            WebResponse res = webManager.Object.GetResponse(@"localhost", "GET");
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
            webResponseReader = new Mock<IResponseReader>();
            webResponseReader.Setup(s => s.ReadResponse(It.IsAny<WebResponse>())).Returns(@"OK");
            webResponseReader.Setup(s => s.ReadResponse(It.IsAny<HttpWebResponse>())).Returns(@"OK");
            webResponseReader.Setup(s => s.ReadResponse(It.IsAny<HttpResponseMessage>())).Returns(@"OK");

            webResp = new Mock<WebResponse>();
            httpWebResp = new Mock<HttpWebResponse>();
            httpRespMsg = new Mock<HttpResponseMessage>();
        }

        [Fact]
        public void GetWebResponseRturnsOKstring()
        {
            string res = webResponseReader.Object.ReadResponse(webResp.Object);
            Assert.Equal(@"OK", res);
        }
        [Fact]
        public void GetHttpWebResponseRturnsOKstring()
        {
            string res = webResponseReader.Object.ReadResponse(httpWebResp.Object);
            Assert.Equal(@"OK", res);
        }
        [Fact]
        public void GetHttpResponseMsgRturnsOKstring()
        {
            string res = webResponseReader.Object.ReadResponse(httpRespMsg.Object);
            Assert.Equal(@"OK", res);
        }

    }


    public class OrientWebManagerIntegrationTests
    {

        OrientWebManager orietWebManager;
        string authUrl, funcUrl, batchUrl, commandUrl, root, password;
        NetworkCredential nc;

        public OrientWebManagerIntegrationTests()
        {
            orietWebManager = new OrientWebManager();

            authUrl = ConfigurationManager.AppSettings["orient_auth_host"];
            funcUrl = ConfigurationManager.AppSettings["orient_func_host"];
            batchUrl = ConfigurationManager.AppSettings["orient_batch_host"];
            commandUrl = ConfigurationManager.AppSettings["orient_command_host"];

            root = ConfigurationManager.AppSettings["ChildLogin"];
            password = ConfigurationManager.AppSettings["ChildPassword"];

            nc = new NetworkCredential(root, password);

        }

        [Fact]
        public void GetResponseAnUthorizedReturnsNullIntegrationTest()
        {
            WebResponse response;
            response = orietWebManager.GetResponse(authUrl, "GET");
            Assert.Equal(response, null);
        }

        [Fact]
        public void AuthenticateIntegrationTest()
        {
            string result = orietWebManager.Authenticate(authUrl, nc).Headers.Get("Set-Cookie");
            bool check = result != string.Empty && result != null;
            Assert.True(check);
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
            webResponseReader = new WebResponseReader();
            webRequestsCheckURL = @"https://www.google.ru";

            webRequest = WebRequest.Create(webRequestsCheckURL);
            httpWebRequest = HttpWebRequest.CreateHttp(webRequestsCheckURL);

        }

        [Fact]
        public void GetWebResponseReturns()
        {
            string result = webResponseReader.ReadResponse(webRequest.GetResponse());

            Assert.NotNull(result);
            Assert.NotEqual(string.Empty, result);
        }

        [Fact]
        public void GetHttpWebResponse()
        {
            string result = webResponseReader.ReadResponse(httpWebRequest.GetResponse());

            Assert.NotNull(result);
            Assert.NotEqual(string.Empty, result);
        }

    }
    public class JSONmanagerIntegrationTests
    {
        JSONManager2 jm;
        string str0, act0, str1, act1, str4;

        //arrange
        public JSONmanagerIntegrationTests()
        {

            str0 = "{\"result\":[{\"Name\":\"value1\",\"sAMAccountName\":\"acc1\"},{\"Name\":\"value2\",\"sAMAccountName\":\"acc2\"}]}";
            act0 = "[{\"sAMAccountName\":\"acc1\",\"Name\":\"value1\"},{\"sAMAccountName\":\"acc2\",\"Name\":\"value2\"}]";

            str1 =
"{\"news\":[{\"Title\":\"value1\",\"Article\":\"value3\"},{\"Title\":\"value2\",\"Article\":\"value4\",\"tags\":[{\"Name\":\"value7\"},{\"Name\":\"value8\"}]}]}";
            act1 = "[\"value1\",\"value2\"]";

            str4 = "[{\"Name\":\"value1\"},{\"Name\":\"value2\"}]";
            jm = new JSONManager2();
        }

        [Fact]
        public void JSONmanagerParseJSONParentColectiontoClassReturnsString()
        {
            //Extract tokens from JSON response parent Node, convert to collection of model objects
            IJEnumerable<JToken> jte = jm.ExtractFromParentNode(str0, "result");
            //Extract + convert JSON to collection of model objects
            IEnumerable<Person> res = jm.DeserializeFromParentNode<Person>(str0, "result");
            //to string  Selectable -> ignore nulls, no intending
            string resp = jm.CollectionToStringFormat<Person>(res,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.None });

            Assert.Equal(resp, act0);
        }

        [Fact]
        public void JSONmanagerParseJSONParentChildColectiontoStringReturnsString()
        {
            //extract from JSON parent node
            IJEnumerable<JToken> jte = jm.ExtractFromParentChildNode(str1, "news", "Title");
            //convert to collection of strings
            IEnumerable<string> res = jm.DeserializeFromParentChildNode<string>(str1, "news", "Title");
            //to string  Selectable -> ignore nulls, no intending
            string resp = jm.CollectionToStringFormat<string>(res
                , new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.None });

            Assert.Equal(resp, act1);
        }

        [Fact]
        public void JSONmngParsefromChildCollectionreturnsString()
        {
            //extract from child nodes
            IJEnumerable<JToken> jte = jm.ExtractFromChildNode(str4, "Name");
            //to collection
            IEnumerable<string> res = jm.DeserializeFromChildNode<string>(str4, "Name");
            //to string  Selectable -> ignore nulls, no intending
            string resp = jm.CollectionToStringFormat<string>(res
                , new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.None });

            Assert.Equal(resp, act1);
        }

    }
    public class URIBuilderIntergrationTest
    {
        string 
            authUrlExpected, commandURLExpected,hostExpected,portExpected,dbExpected,commandExpected,
            patternAuthExpected, patternCommandExpected,commandTokenExpected
            , selectClauseExpected,whereClauseExpected,selectURLExpected
            ,createCommandExpected;

        ITypeToken host_,port_,db_,command_,authenticate_;

        List<ITypeToken> authURLTokens,commandURLTokens;

        OrientAuthenticationURLFormat authURLformat;
        OrientCommandURLFormat commURLformat;

        OrientAuthenticationURL AuthURL;
        OrientCommandURLBuilder CommURL;           
        
        //arrange
        public URIBuilderIntergrationTest()
        {

            authUrlExpected = @"http://msk1-vm-ovisp02:2480/connect/news_test3";
            commandURLExpected = @"http://msk1-vm-ovisp02:2480/command/news_test3/sql";
            selectClauseExpected = @"Select from Person";
            whereClauseExpected = @"where 1=1";
            selectURLExpected = @"http://msk1-vm-ovisp02:2480/command/news_test3/sql/Select from Person where 1=1";
            createCommandExpected =
            "Create Vertex Person content {\"Created\":\"2017-01-01 00:00:00\",\"GUID\":\"0\",\"Changed\":\"2017-01-01 00:00:00\",\"Name\":\"0\"}";

            host_ = new OrientHost();
            port_ = new OrientPort();
            db_ = new OrientDb();

            command_ = new OrientCommandToken();
            authenticate_ = new OrientAuthenticateToken();
                   
            authURLTokens = new List<ITypeToken>() { new OrientHost(), new OrientPort(), new OrientAuthenticateToken(), new OrientDb() };
            commandURLTokens = new List<ITypeToken>() { new OrientHost(), new OrientPort(), new OrientCommandToken(), new OrientDb(), new OrientCommandSQLTypeToken() };

            authURLformat = new OrientAuthenticationURLFormat();
            commURLformat = new OrientCommandURLFormat();

            AuthURL = new OrientAuthenticationURL(authURLTokens, authURLformat);
            CommURL = new OrientCommandURLBuilder(commandURLTokens, commURLformat);

            hostExpected = "http://msk1-vm-ovisp02";
            portExpected = "2480";
            dbExpected = "news_test3";
            commandExpected = "connect";
            commandTokenExpected = "command";
            patternAuthExpected = "{0}:{1}/{2}/{3}";
            patternCommandExpected = "{0}:{1}/{2}/{3}/{4}";

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
        public void OrientDbTest_ReturnsValidString()
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
            List<ITypeToken> selectCommandTokens = new List<ITypeToken>()
            { new OrientSelectToken(), new OrientFromToken(), new OrientPersonToken()};
            //format for Select concat
            //{0}/{1} {2} {3}
            //<commandURL>/<select from classname>
            OrientSelectClauseFormat of = new OrientSelectClauseFormat();
            //build full command URL with URL and command Parts            
            OrientSelectClauseBuilder selectUrlPart =
                new OrientSelectClauseBuilder(
                    selectCommandTokens
                    , of
                );
            //select query URL text
            string selectQuery = selectUrlPart.Text.Text;
            Assert.Equal(selectClauseExpected, selectQuery);
        }
        [Fact]
        public void WhereClauseBuild()
        {
            //Where command tokens with test hardcoded condition 
            //<<!!! condition to concatenation builder (infinite where)
            List<ITypeToken> whereCommandTokens = new List<ITypeToken>()
            { new OrientWhereToken(), new TextToken(){ Text=@"1=1"} };
            //format for where concat
            OrientWhereClauseFormat wf = new OrientWhereClauseFormat();
            //build where clause
            OrientWhereClauseBuilder whereUrlPart =
                new OrientWhereClauseBuilder(
                    whereCommandTokens, wf
                );
            //where query text
            string whereQuery = whereUrlPart.Text.Text;
            Assert.Equal(whereClauseExpected,whereQuery);
        }
        [Fact]
        public void SelectUrlBuild()
        {

            //Initialize Format for command URL string concat 
            //-> {0}:{1}/{2}/{3}
            // <host>:<port>/command/<dbname>/sql
            OrientCommandURLFormat cf = new OrientCommandURLFormat();
            //tokens for command url part
            List<ITypeToken> urlCommandTokens = new List<ITypeToken>()
            { new OrientHost(), new OrientPort(), new OrientCommandToken(), new OrientDb(), new OrientCommandSQLTypeToken() };
            //Command URL text
            OrientCommandURLBuilder commandUrlPart = new OrientCommandURLBuilder(urlCommandTokens, cf);
          

            // tokens for query Select part
            List<ITypeToken> selectCommandTokens = new List<ITypeToken>()
            { new OrientSelectToken(), new OrientFromToken(), new OrientPersonToken()};
            //format for Select concat
            //{0}/{1} {2} {3}
            //<commandURL>/<select from classname>
            OrientSelectClauseFormat of = new OrientSelectClauseFormat();
            //build full command URL with URL and command Parts            
            OrientSelectClauseBuilder selectUrlPart =
                new OrientSelectClauseBuilder(
                    selectCommandTokens
                    , of
                );
         
            //Where command tokens with test hardcoded condition 
            //<<!!! condition to concatenation builder (infinite where)
            List<ITypeToken> whereCommandTokens = new List<ITypeToken>()
            { new OrientWhereToken(), new TextToken(){ Text=@"1=1"} };
            //format for where concat
            OrientWhereClauseFormat wf = new OrientWhereClauseFormat();
            //build where clause
            OrientWhereClauseBuilder whereUrlPart =
                new OrientWhereClauseBuilder(
                    whereCommandTokens, wf
                );       

            //collection of FULL tokens 
            //@"{0}:{1}/{2}/{3}/{4}/{5} {6} {7} {8} {9}"
            List<ITextBuilder> CommandTokens = new List<ITextBuilder>(){
                commandUrlPart,selectUrlPart,whereUrlPart
            };
            //Aggregate all query TokenManagers to one Select URL command with where
            OrientCommandURLBuilder commandSample = new OrientCommandURLBuilder(
                CommandTokens, new TextToken() { Text = @"{0}/{1} {2}" }, URLbuilder.BuildTypeFormates.NESTED
                );
            //full select query command
            string selectcommandURL = commandSample.Text.Text;

            Assert.Equal(selectURLExpected, selectcommandURL);
        }
        [Fact]
        public void CreatePersonUrlBuild()
        {
            Person per = new Person()
            { Name = "0", GUID = "0", Changed = new DateTime(2017,01,01,00,00,00), Created = new DateTime(2017, 01, 01, 00, 00, 00) };
            JSONManager2 jm = new JSONManager2();
            string contentText = jm.SerializeObject(per,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None,
                    DateFormatString = @"yyyy-MM-dd HH:mm:ss"
                });
            TextToken content = new TextToken() { Text = contentText };

            List<ITypeToken> CreateTokens = new List<ITypeToken>() {
                new OrientCreateToken(),new OrientVertexToken(),new OrientPersonToken(), new OrientContentToken()
                , content};
            OrientCreateVertexCluaseFormat cf = new OrientCreateVertexCluaseFormat();
            OrientCreateClauseBuilder cb = new OrientCreateClauseBuilder(CreateTokens, cf);
            string CreateCommand = cb.Text.Text;

            Assert.Equal(createCommandExpected, CreateCommand);
        }

    }

    //collection of test cases with expected and result
    public class TestCase
    {
        public int CaseNumber { get; private set; }
        public string Input { get; set; }
        public string Expected { get; set; }
        public string Actual { get; private set; }
        public bool Equal { get; private set; }

        public TestCase()
        {
            this.CaseNumber += 1;
            this.Actual = string.Empty;
            this.Expected = string.Empty;
            this.Equal = false;
        }
        public void Check(string Actual_)
        {
            this.Actual = Actual_;
            if (this.Expected == this.Actual) { this.Equal = true; }
            else { this.Equal = false; }
        }
    }

}
