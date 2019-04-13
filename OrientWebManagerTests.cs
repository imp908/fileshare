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
            webResponseReader.Setup(s=> s.ReadResponse(It.IsAny<WebResponse>())).Returns( @"OK" ) ;
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
        string str0 ,act0,str1,act1,str4;

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


    //collection of test cases with expected and result
    public class TestCase
    {
        public int CaseNumber { get ; private set; }
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
