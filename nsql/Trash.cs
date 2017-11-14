using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


using JsonManagers;
using POCO;
using WebManagers;
using NSQLManager;
using IOrientObjects;
using IQueryManagers;
using QueryManagers;
using APItesting;

/// <summary>
/// Deprecated, unused and refactored code mooved here
/// can be deleted without any consequences
/// </summary>
namespace Trash
{

    public class Check
    {

        public static void JSONManagerIntegrationTests()
        {
            string str1 = "[\"v1\",\"v2\"]";
            string res1 = string.Concat(JsonConvert.DeserializeObject<IEnumerable<string>>(str1));

            string str2 = "{\"Name\":\"value1\"}";
            string res2 = string.Concat(JsonConvert.DeserializeObject(str2));
            string res2_2 = string.Concat(JToken.Parse(str2));
            Person res3 = JsonConvert.DeserializeObject<Person>(str2);

            string str4 = "[{\"Name\":\"value1\"},{\"Name\":\"value2\"}]";
            IEnumerable<Person> res4 = JsonConvert.DeserializeObject<IEnumerable<Person>>(str4);
            string str4_2 = string.Concat(JToken.Parse(str4).Children()["Name"]);

            string str5 = "{\"result\":[{\"Name\":\"value1\",\"sAMAccountName\":\"acc1\"},{\"Name\":\"value2\",\"sAMAccountName\":\"acc2\"}]}";
            string res5 = string.Concat(JToken.Parse(str5)["result"].Children()["Name"]);
            string str6 = "{\"news\":[{\"Title\":\"value1\",\"Article\":\"value3\"},{\"Title\":\"value2\",\"Article\":\"value4\"}]}";
            string res6 = string.Concat(JToken.Parse(str6)["news"].Children()["Title"]);
            string res7 = string.Concat(JToken.Parse(str6)["news"].Children()["Article"]);

            string str7 =
    "{\"news\":[{\"Title\":\"value1\",\"Article\":\"value3\"},{\"Title\":\"value2\",\"Article\":\"value4\",\"tags\":[\"value5\",\"value\"]}]}";
            string res8 = string.Concat(JToken.Parse(str7)["news"].Children()["tags"]);

            string str8 =
    "{\"news\":[{\"Title\":\"value1\",\"Article\":\"value3\"},{\"Title\":\"value2\",\"Article\":\"value4\",\"tags\":[{\"Name\":\"value7\"},{\"Name\":\"value8\"}]}]}";

            JSONManager jm2 = new JSONManager();

            //Read from 2lvl array to string
            IJEnumerable<JToken> personsJT = JToken.Parse(str8)["news"].Children()["tags"];
            List<string> col = jm2.JTokenToCollection(personsJT);
            string resp4 = jm2.CollectionToStringFormat<string>(col,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.None }
            );

            var a = JToken.Parse(str8).SelectToken("news", false).SelectToken("Title", false);

            /*{"parentNodeName":[{"Name":"value1","Name2":"Value1"},{"Name":"value2","Name2":"value2"}]} - > model {Name {get;set;}, Name2 {get;set;}} */
            //Extract tokens from JSON response parent Node, convert to collection of model objects
            IJEnumerable<JToken> jte = jm2.ExtractFromParentNode(str5, "result");
            //Extract + convert JSON to collection of model objects
            IEnumerable<Person> res9 = jm2.DeserializeFromParentNode<Person>(str5, "result");
            //to string  Selectable -> ignore nulls, no intending
            string resp0 = jm2.CollectionToStringFormat<Person>(res9,
                new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore, Formatting = Formatting.None });

            //extract from JSON parent node
            IJEnumerable<JToken> jte2 = jm2.ExtractFromParentChildNode(str8, "news", "Title");
            //convert to collection of strings
            IEnumerable<string> res10 = jm2.DeserializeFromParentChildNode<string>(str8, "news", "Title");
            //to string  Selectable -> ignore nulls, no intending
            string resp1 = jm2.CollectionToStringFormat<string>(res10
                , new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.None });

            //extract from child nodes
            IJEnumerable<JToken> jte3 = jm2.ExtractFromChildNode(str4, "Name");
            //to collection
            IEnumerable<string> res11 = jm2.DeserializeFromChildNode<string>(str4, "Name");
            //to string  Selectable -> ignore nulls, no intending
            string resp2 = jm2.CollectionToStringFormat<string>(res11
                , new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include, Formatting = Formatting.None });

            //Back to object 
            IEnumerable<Person> persons = JsonConvert.DeserializeObject<IEnumerable<Person>>(resp0);
        }
       
        public static void PersonApiCheck()
        {

            OrientWebManager wm = new OrientWebManager();

            APItester_sngltn ut = new APItester_sngltn();
            WebResponseReader wr = new WebResponseReader();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json"))
            {
                string res = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json");
                APItester_sngltn.TestCases = JsonConvert.DeserializeObject<List<APItester_sngltn>>(res);
            }
            else
            {
                APItester_sngltn.TestCases.Add(new APItester_sngltn()
                {
                    URI = "http://msk1-vm-ovisp01:8083/api/Person/GetManager/DegterevaSV"
                    ,
                    Expected = "[\"filimonovats\",\"kvv\"]"
                    ,
                    OK = false
                });
                APItester_sngltn.TestCases.Add(new APItester_sngltn()
                {
                    URI = "http://msk1-vm-ovisp01:8083/api/Person/GetColleges/lobanovamg"
                   ,
                    Expected = "[\"stalmakovsm\",\"Bagirovaev\",\"kotovaen\",\"iku\",\"SergiecIG\",\"a.vagin\",\"lobanovamg\",\"Tikhomirovaa\",\"popovalb\"]"
                   ,
                    OK = false
                });
            }


            foreach (APItester_sngltn tc in APItester_sngltn.TestCases)
            {
                try
                {
                    var resp = wm.GetResponse(tc.URI, "GET");
                    string rR = wr.ReadResponse(resp);

                    if (tc.Expected == rR)
                    {
                        tc.OK_(rR);
                    }
                    else
                    {
                        tc.NotOK_(rR);
                    }
                }
                catch (Exception e)
                {
                    //Handles error type names if they are expected
                    //logs exception message
                    if (tc.Expected == e.GetType().ToString())
                    {
                        tc.OK_(e.GetType().ToString(), e.Message);
                    }
                    else
                    {
                        tc.NotOK_(e.GetType().ToString(), e.Message);
                    }
                }
            }

            string jStr = JsonConvert.SerializeObject(APItester_sngltn.TestCases, Formatting.Indented);
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json", jStr);

        }
        //>>make trully async
        public static void PersonApiCheckAsync()
        {

            OrientWebManager wm = new OrientWebManager();

            APItester_sngltn ut = new APItester_sngltn();
            WebResponseReader wr = new WebResponseReader();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json"))
            {
                string res = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json");
                APItester_sngltn.TestCases = JsonConvert.DeserializeObject<List<APItester_sngltn>>(res);
            }
            else
            {
                APItester_sngltn.TestCases.Add(new APItester_sngltn()
                {
                    URI = "http://msk1-vm-ovisp01:8083/api/Person/GetManager/DegterevaSV"
                    ,
                    Expected = "[\"filimonovats\",\"kvv\"]"
                    ,
                    OK = false
                });
                APItester_sngltn.TestCases.Add(new APItester_sngltn()
                {
                    URI = "http://msk1-vm-ovisp01:8083/api/Person/GetColleges/lobanovamg"
                   ,
                    Expected = "[\"stalmakovsm\",\"Bagirovaev\",\"kotovaen\",\"iku\",\"SergiecIG\",\"a.vagin\",\"lobanovamg\",\"Tikhomirovaa\",\"popovalb\"]"
                   ,
                    OK = false
                });
            }


            foreach (APItester_sngltn tc in APItester_sngltn.TestCases)
            {
                try
                {
                    Task<HttpWebResponse> resp = wm.GetResponseAsync(tc.URI, "GET");

                    string rR = wr.ReadResponse(resp.Result);

                    if (tc.Expected == rR)
                    {
                        tc.OK_(rR);
                    }
                    else
                    {
                        tc.NotOK_(rR);
                    }
                }
                catch (Exception e)
                {
                    //Handles error type names if they are expected
                    //logs exception message
                    if (tc.Expected == e.GetType().ToString())
                    {
                        tc.OK_(e.GetType().ToString(), e.Message);
                    }
                    else
                    {
                        tc.NotOK_(e.GetType().ToString(), e.Message);
                    }
                }
            }

            string jStr = JsonConvert.SerializeObject(APItester_sngltn.TestCases, Formatting.Indented);
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json", jStr);

        }

        
        public static void JSONcheck()
        {
            JSONmanager jm = new JSONmanager();

            string input = "{\"result\":[{\"@type\":\"d\",\"@rid\":\"#-2:0\",\"@version\":0,\"Name\":\"kapkaev\"},{\"@type\":\"d\",\"@rid\":\"#-2:1\",\"@version\":0,\"Name\":\"kokuevol\"}]}";
            string input2 = "[{\"@type\":\"d\",\"@rid\":\"#-2:13\",\"@version\":0,\"Name\":[\"tishakovoi\"]}]";
            string input3 = "[{\"@type\":\"d\",\"@rid\":\"#-2:13\",\"@version\":0,\"Name\":[\"tishakovoi\"]},{\"@type\":\"d\",\"@rid\":\"#-2:13\",\"@version\":0,\"Name\":[\"kapkaev\"]}]";

            string rk = @"result";
            string nk = @"Name";

            var des1 = JsonConvert.DeserializeObject(input);
            var des2 = JsonConvert.DeserializeObject(input2);
            var des3 = JsonConvert.DeserializeObject(input3);

            JToken jt1 = null, jt2 = null, jt3 = null;
         
            JToken r1, r0 = null;
            IEnumerable<JToken> r2 = null, r3 = null;

            try
            {
                jt1 = JsonConvert.DeserializeObject<JToken>(input);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try
            {
                jt2 = JsonConvert.DeserializeObject<JToken>(input2);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try
            {
                jt3 = JsonConvert.DeserializeObject<JToken>(input3);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            //Json has parent node
            try { JToken e1 = jt1[rk]; } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try { IEnumerable<JToken> e1 = jt1[rk].Children(); } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try { IEnumerable<JToken> e1 = jt1[rk].Children()[nk]; } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            //Json has parent as collection

            //not OK
            try { JToken e1 = jt2[rk]; } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try { JToken e1 = jt2[nk]; } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            //OK
            try { IEnumerable<JToken> e2 = jt2.Children(); } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try { IEnumerable<JToken> e3 = jt2.Children()[nk]; } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            //try { IEnumerable<JToken> e4 = jt3.Children()[nk];}catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            try
            {
                r1 = JToken.Parse(input);
                r0 = JToken.Parse(input)[rk];
                r2 = JToken.Parse(input2).Children()[nk];
                r3 = JToken.Parse(input3).Children()[nk];
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

            try
            {
                IEnumerable<Person> z = jm.DeserializeFromNode<Person>(input, "result");
                IEnumerable<JToken> a = jm.ExtractTokens(input3, "Name");

                var r_0 = JsonConvert.SerializeObject(a);
                var r_1 = JsonConvert.SerializeObject(z);

                string c = jm.CollectionToStringFormat<JToken>(a, null);

                string r5 = jm.CollectionToStringFormat<Person>(z, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                string r6 = jm.CollectionToStringFormat<JToken>(a, null);

                var t2 = JsonConvert.DeserializeObject(c);
                var t3 = JsonConvert.DeserializeObject(r5);
                var t4 = JsonConvert.DeserializeObject(r6);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }


        }
        

        //<--Out - deprecated
        //WEB scope
        //divide to string method initiation (GET,POST -> execute), add parameters addition before call <- done
        //move web request object to separate class <- done
        //JSON reader to new class, return only web request, then decide how to handle it (JSON,XML,STRING,ACTION) <- done
        
        public class HTTPmanager
        {
            public static string OSESSIONID;

            public int HTTPauthProxy(string url, string login, string password)
            {
                WebRequest AuthRequest = WebRequest.Create(url);
                AuthRequest.Credentials = new NetworkCredential(login, password);
                AuthRequest.Method = "GET";
                AuthRequest.ContentType = "application/json; charset=utf-8";
                WebResponse response = AuthRequest.GetResponse();
                OSESSIONID = response.Headers.Get("Set-Cookie");
                int status = (int)((HttpWebResponse)response).StatusCode;
                response.Close();
                return status;
            }

            public string Get(string url_)
            {
                WebRequest wr = WebRequest.Create(url_);
                wr.Method = "GET";
                wr.ContentType = "application/json; charset=utf-8";

                using (HttpWebResponse response = (HttpWebResponse)wr.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var objText = reader.ReadToEnd();
                        var objClasses = JsonConvert.DeserializeObject(objText);
                        var jsonResult = objClasses.ToString();

                        return jsonResult;
                    }
                }
            }

            public string GetAuthProx(string url_)
            {
                WebRequest wr = WebRequest.Create(url_);
                wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
                wr.Method = "GET";
                wr.ContentType = "application/json; charset=utf-8";

                using (var response = (HttpWebResponse)wr.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var objText = reader.ReadToEnd();
                        var objClasses = JsonConvert.DeserializeObject(objText);
                        var jsonResult = objClasses.ToString();

                        return jsonResult;
                    }
                }
            }
            public string POSTAuthProx(string url_)
            {
                WebRequest wr = WebRequest.Create(url_);
                wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
                wr.Method = "POST";
                wr.ContentType = "application/json; charset=utf-8";

                using (var response = (HttpWebResponse)wr.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        var objText = reader.ReadToEnd();
                        var objClasses = JsonConvert.DeserializeObject(objText);
                        var jsonResult = objClasses.ToString();

                        return jsonResult;
                    }
                }
            }

        }
        


        //<<-- Out deprecated New Builder Above OrientTokens
        /// <summary>
        /// no any Buisiness logic
        /// divide to query class with inheritance|generic 
        /// to commandtpyes (command|query,batch,function)
        /// </summary>
        
        public class _OrientApiUrlBuilder
        {
            string selectCommand = String.Empty;
            string functionCommand = String.Empty;

            public string SelectCommandSet(string className_, string filter_)
            {
                this.selectCommand = SelectClause(className_, WhereClause(filter_));
                return selectCommand;
            }
            public string SelectCommandGet()
            {
                if (this.selectCommand != String.Empty)
                {
                    return this.selectCommand;
                }

                return null;
            }

            string SelectClause(string classname_, string whereClause = null)
            {
                return String.Format(@"select from {0} {1}", classname_, whereClause);
            }
            string WhereClause(string whereClause_)
            {
                return String.Format(@"where {0}", whereClause_);
            }
            public string FucntionSet(string Fucntion_, string params_)
            {
                this.functionCommand = String.Format(@"{0}/{1}", Fucntion_, params_);
                return functionCommand;
            }
            public string FunctionCommandGet()
            {
                return functionCommand;
            }

        }
        


        //Token managers check
        //check select cluase
        public static void SelectClusesAndURLBuildCheck()
        {

            //Get collection of tokens used for concatenating authentication URL part
            List<ITypeToken> urlTokens = new List<ITypeToken>()
            { new OrientHost(), new OrientPort(), new OrientAuthenticateToken(), new OrientDatabaseToken() };
            //Initialize Format for auth URL string concat 
            //- > {0}:{1}/{2}/{3}
            // <host>:<port>/connect/<dbname>
            OrientAuthenticationURLFormat ot = new OrientAuthenticationURLFormat();
            //Build auth url from url tokens with url concat rule
            OrientAuthenticationURIBuilder ur = new OrientAuthenticationURIBuilder(urlTokens, ot);
            //Authentication URL text
            string authUrl = ur.Text.Text;


            //Initialize Format for command URL string concat 
            //-> {0}:{1}/{2}/{3}
            // <host>:<port>/command/<dbname>/sql
            OrientCommandURLFormat cf = new OrientCommandURLFormat();
            //tokens for command url part
            List<ITypeToken> urlCommandTokens = new List<ITypeToken>()
            { new OrientHost(), new OrientPort(), new OrientCommandToken(), new OrientDatabaseToken(), new OrientCommandSQLTypeToken() };
            //Command URL text
            OrientCommandURIBuilder commandUrlPart = new OrientCommandURIBuilder(urlCommandTokens, cf);
            //command url
            string commandUrl = commandUrlPart.Text.Text;

            // tokens for query Select part
            List<ITypeToken> selectCommandTokens = new List<ITypeToken>()
            { new OrientSelectToken(), new OrientFromToken(), new OrientPersonToken()};
            //format for Select concat
            //{0}/{1} {2} {3}
            //<commandURL>/<select from classname>
            OrientSelectClauseFormat of = new OrientSelectClauseFormat();
            //build full command URL with URL and command Parts            
            OrientSelectClauseBuilder selectUrlPart =
                new OrientSelectClauseBuilder(selectCommandTokens);
            //select query URL text
            string selectQuery = selectUrlPart.Text.Text;

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


            //collection of FULL tokens 
            //@"{0}:{1}/{2}/{3}/{4}/{5} {6} {7} {8} {9}"
            List<ITextAggreagtor> CommandTokens = new List<ITextAggreagtor>(){
                commandUrlPart,selectUrlPart,whereUrlPart
            };
            //Aggregate all query TokenManagers to one Select URL command with where
            OrientCommandURIBuilder commandSample = new OrientCommandURIBuilder(
                CommandTokens, new TextToken() { Text = @"{0}/{1} {2}" }, TextBuilder.BuildTypeFormates.NESTED
                );
            //full select query command
            string selectcommandURL = commandSample.Text.Text;

        }
        //check insert clause
        public static void InsertClausesCheck()
        {
            Person per = new Person() { Name = "0", GUID = "0", Changed = DateTime.Now, Created = DateTime.Now };
            JSONManager jm = new JSONManager();
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
        }
        //check delete clause
        public static void DeleteClauseCheck()
        {

            List<ITypeToken> wT =
                new List<ITypeToken>() { new OrientWhereToken(), new TextToken() { Text = "\"Name\" = \"0\"" } };
            OrientWhereClauseFormat cf =
                new OrientWhereClauseFormat();
            OrientWhereClauseBuilder cb = new OrientWhereClauseBuilder(wT, cf);


            //<<ad where clause builder
            List<ITypeToken> dt =
                new List<ITypeToken>(){new OrientDeleteToken(), new OrientVertexToken(), new OrientPersonToken()
                , new TextToken(){Text = "where Name = 0" }};
            OrientDeleteVertexCluaseFormat df
                = new OrientDeleteVertexCluaseFormat();
            OrientDeleteClauseBuilder dlb = new OrientDeleteClauseBuilder(dt, df);

            string deleteClause = dlb.Text.Text;
        }
        //HTTP clause
        public static void HTTPclauseCheck()
        {

            //Initialize tokens for sql command clause
            List<ITypeToken> urlTokents =
    new List<ITypeToken>() { new OrientHost(), new OrientPort(), new OrientCommandToken(), new OrientDatabaseToken(), new OrientCommandSQLTypeToken() };

            //create model class for person
            //manual init for testng , furhter will be loaded from elsewhere
            Person per = new Person() { Name = "0", GUID = "0", Changed = new DateTime(2017, 01, 01, 00, 00, 00), Created = new DateTime(2017, 01, 01, 00, 00, 00) };
            JSONManager jm = new JSONManager();
            //parse model class to json, ignoring null and formattin
            string contentText = jm.SerializeObject(per,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None,
                    DateFormatString = @"yyyy-MM-dd HH:mm:ss"
                });
            //createing content token from string out of Person class
            TextToken personContent = new TextToken() { Text = contentText };

            //create unit 
            Unit unit = new Unit() { Name = "0", GUID = "0", Changed = new DateTime(2017, 01, 01, 00, 00, 00), Created = new DateTime(2017, 01, 01, 00, 00, 00) };
            //serialize unit
            string unitText = jm.SerializeObject(unit, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                DateFormatString = @"yyyy-MM-dd HH:mm:ss"
            });
            //create unit content token from string out of Unit class
            TextToken unitContent = new TextToken() { Text = unitText };

            //Tokens for command clause
            List<ITypeToken> createPersonTokens =
    new List<ITypeToken>() { new OrientCreateToken(), new OrientVertexToken(), new OrientPersonToken(), new OrientContentToken(), personContent };
            List<ITypeToken> selectPersonTokens =
    new List<ITypeToken>() { new OrientSelectToken(), new OrientFromToken(), new OrientPersonToken() };
            List<ITypeToken> deletePersonToken =
    new List<ITypeToken>() { new OrientDeleteToken(), new OrientVertexToken(), new OrientFromToken(), new OrientPersonToken() };

            List<ITypeToken> createUnitTokens =
    new List<ITypeToken>() { new OrientCreateToken(), new OrientVertexToken(), new OrientUnitToken(), new OrientContentToken(), unitContent };
            List<ITypeToken> selectUnitTokens =
    new List<ITypeToken>() { new OrientSelectToken(), new OrientFromToken(), new OrientUnitToken() };
            List<ITypeToken> deleteUnitToken =
    new List<ITypeToken>() { new OrientDeleteToken(), new OrientVertexToken(), new OrientFromToken(), new OrientUnitToken() };

            List<ITypeToken> whereTokens =
    new List<ITypeToken>() { new OrientWhereToken(), new TextToken() { Text = "Name = 0" } };

            //initializing string formats for every command
            OrientCommandURLFormat uf = new OrientCommandURLFormat();
            OrientCreateVertexCluaseFormat cf = new OrientCreateVertexCluaseFormat();
            OrientSelectClauseFormat sf = new OrientSelectClauseFormat();
            OrientDeleteVertexCluaseFormat df = new OrientDeleteVertexCluaseFormat();
            OrientWhereClauseFormat wf = new OrientWhereClauseFormat();

            //building command and url strings from tokens according to format
            OrientCommandURIBuilder ub = new OrientCommandURIBuilder(urlTokents, uf);

            OrientCreateClauseBuilder cpb = new OrientCreateClauseBuilder(createPersonTokens, cf);
            OrientSelectClauseBuilder spb = new OrientSelectClauseBuilder(selectPersonTokens, sf);
            OrientDeleteClauseBuilder dpb = new OrientDeleteClauseBuilder(deletePersonToken, df);

            OrientCreateClauseBuilder cub = new OrientCreateClauseBuilder(createUnitTokens, cf);
            OrientSelectClauseBuilder sub = new OrientSelectClauseBuilder(selectUnitTokens, sf);
            OrientDeleteClauseBuilder dub = new OrientDeleteClauseBuilder(deleteUnitToken, df);

            OrientWhereClauseBuilder wb = new OrientWhereClauseBuilder(whereTokens, wf);

            //commands check
            string url = ub.Text.Text;

            string createPerson = cpb.Text.Text;
            string select = spb.Text.Text;
            string delete = dpb.Text.Text;

            string createUnit = cub.Text.Text;
            string selectUnit = sub.Text.Text;
            string deleteUnit = dub.Text.Text;

            string where = wb.Text.Text;


            //build commands strings according to string format
            List<ITextAggreagtor> createPersonTk = new List<ITextAggreagtor>() { ub, cpb };
            List<ITextAggreagtor> selectTk = new List<ITextAggreagtor>() { ub, spb, wb };
            List<ITextAggreagtor> deleteTk = new List<ITextAggreagtor>() { ub, dpb, wb };

            List<ITextAggreagtor> createUnitTk = new List<ITextAggreagtor>() { ub, cub };
            List<ITextAggreagtor> selectUnitTk = new List<ITextAggreagtor>() { ub, sub, wb };
            List<ITextAggreagtor> deleteUnitTk = new List<ITextAggreagtor>() { ub, dub, wb };


            //building command urls according to string format
            //Nested type is for using new format to concatenate token formats
            //EXAMPLE for token formats
            //Token1 -> "{0}\{1}" ; Token2 -> "{0} {1} {2}"; UrlToken -> "{0}:{1}"; Result => UrlToken "{0}\{1}:{2} {3} {4}"
            OrientCommandURIBuilder cpU =
    new OrientCommandURIBuilder(createPersonTk, new TextToken() { Text = @"{0}/{1}" }, TextBuilder.BuildTypeFormates.NESTED);
            OrientCommandURIBuilder spU =
    new OrientCommandURIBuilder(selectTk, new TextToken() { Text = @"{0}/{1} {2}" }, TextBuilder.BuildTypeFormates.NESTED);
            OrientCommandURIBuilder dpU =
    new OrientCommandURIBuilder(deleteTk, new TextToken() { Text = @"{0}/{1} {2}" }, TextBuilder.BuildTypeFormates.NESTED);

            OrientCommandURIBuilder cuU =
    new OrientCommandURIBuilder(createUnitTk, new TextToken() { Text = @"{0}/{1}" }, TextBuilder.BuildTypeFormates.NESTED);
            OrientCommandURIBuilder suU =
    new OrientCommandURIBuilder(selectUnitTk, new TextToken() { Text = @"{0}/{1} {2}" }, TextBuilder.BuildTypeFormates.NESTED);
            OrientCommandURIBuilder duU =
    new OrientCommandURIBuilder(deleteUnitTk, new TextToken() { Text = @"{0}/{1} {2}" }, TextBuilder.BuildTypeFormates.NESTED);


            //check
            string cpUt = cpU.Text.Text;
            string spUt = spU.Text.Text;
            string dpUt = dpU.Text.Text;

            string cuUt = cuU.Text.Text;
            string suUt = suU.Text.Text;
            string duUt = duU.Text.Text;

            //create authentication tokens and build auth url
            List<ITypeToken> authtokens = new List<ITypeToken>()
            { new OrientHost(),new OrientPort(),new OrientAuthenticateToken(),new OrientDatabaseToken() };
            OrientAuthenticationURLFormat auf = new OrientAuthenticationURLFormat();
            OrientAuthenticationURIBuilder authBld = new OrientAuthenticationURIBuilder(authtokens, auf);

            //check auth url
            string authUrlText = authBld.Text.Text;

            //init 
            OrientWebManager owm = new OrientWebManager();
            //authenticate
            owm.Authenticate(authUrlText, new NetworkCredential(@"root", @"I9grekVmk5g"));
            //init
            WebResponseReader responseReader = new WebResponseReader();

            //create persons
            responseReader.ReadResponse(owm.GetResponse(cuUt, new POST().Text));
            responseReader.ReadResponse(owm.GetResponse(cuUt, new POST().Text));
            responseReader.ReadResponse(owm.GetResponse(cuUt, new POST().Text));

            //create units
            responseReader.ReadResponse(owm.GetResponse(cpUt, new POST().Text));
            responseReader.ReadResponse(owm.GetResponse(cpUt, new POST().Text));
            responseReader.ReadResponse(owm.GetResponse(cpUt, new POST().Text));

            //get all persons whith where cluase
            string unitsCreatedstr = responseReader.ReadResponse(owm.GetResponse(suUt, new POST().Text));
            //get all units whith where cluase
            string personsCreatedstr = responseReader.ReadResponse(owm.GetResponse(spUt, new POST().Text));
            //get list of created persons
            IEnumerable<Person> p1 = jm.DeserializeFromParentNode<Person>(personsCreatedstr, new RESULT().Text);



            //IEnumerable<Person> persons = null;
            List<string> personIDs = new List<string>();
            List<string> unitsIDs = new List<string>();
            List<string> SubUnitsIDs = new List<string>();
            List<string> MainAssignmentsIDs = new List<string>();

            for (int i = 0; i <= 10; i++)
            {
                personIDs.Add(
                    jm.DeserializeFromParentNode<Person>(responseReader.ReadResponse(owm.GetResponse(cpUt, new POST().Text)), new RESULT().Text)
                    .Select(s => s).FirstOrDefault().id.Replace(new OrientIdSharpToken().Text, string.Empty)
                );
            }
            for (int i = 0; i <= 10; i++)
            {
                unitsIDs.Add(
                    jm.DeserializeFromParentNode<Unit>(responseReader.ReadResponse(owm.GetResponse(cuUt, new POST().Text)), new RESULT().Text)
                    .Select(s => s).FirstOrDefault().id.Replace(new OrientIdSharpToken().Text, string.Empty)
                );
            }





            //MainAssignment content throught model create
            MainAssignment ma =
                new MainAssignment() { Name = "0", GUID = "0", Changed = new DateTime(2017, 01, 01, 00, 00, 00), Created = new DateTime(2017, 01, 01, 00, 00, 00) };

            string MainAssignmentContent = jm.SerializeObject(ma, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None,
                DateFormatString = @"yyyy-MM-dd HH:mm:ss"
            });


            //tokens for edge creation
            List<ITypeToken> SubUnitTk = new List<ITypeToken>() {
                new OrientCreateToken(), new OrientEdgeToken(), new OrientSubUnitToken(), new OrientFromToken(),
                new TextToken() { Text = unitsIDs[0] }, new OrientToToken(),new TextToken() { Text = unitsIDs[1] }
            };
            List<ITypeToken> MainAssignmentTk = new List<ITypeToken>() {
                new OrientCreateToken(), new OrientEdgeToken(), new OrientMainAssignmentToken(), new OrientFromToken(),
                new TextToken() { Text = unitsIDs[1] }, new OrientToToken(),new TextToken() { Text = personIDs[0] }
                , new OrientContentToken(), new TextToken(){ Text = MainAssignmentContent}
            };


            //generaates default format for builder from collection {0} .. {n}
            TextFormatGenerate SbUtf = new TextFormatGenerate(SubUnitTk);
            TextFormatGenerate MnAsf = new TextFormatGenerate(MainAssignmentTk);

            OrientCommandBuilder SuB = new OrientCommandBuilder(SubUnitTk, SbUtf);
            OrientCommandBuilder MaB = new OrientCommandBuilder(MainAssignmentTk, MnAsf);

            List<ITextAggreagtor> SuTb = new List<ITextAggreagtor>() { ub, SuB };
            List<ITextAggreagtor> MaTb = new List<ITextAggreagtor>() { ub, MaB };


            OrientCommandURIBuilder SuUB =
                new OrientCommandURIBuilder(SuTb, new TextToken() { Text = @"{0}/{1}" }, TextBuilder.BuildTypeFormates.NESTED);
            OrientCommandURIBuilder MaUB =
                new OrientCommandURIBuilder(MaTb, new TextToken() { Text = @"{0}/{1}" }, TextBuilder.BuildTypeFormates.NESTED);


            SubUnitsIDs.Add(
                  jm.DeserializeFromParentNode<SubUnit>(responseReader.ReadResponse(owm.GetResponse(SuUB.Text.Text, new POST().Text)), new RESULT().Text)
                  .Select(s => s).FirstOrDefault().id.Replace(new OrientIdSharpToken().Text, string.Empty)
              );

            MainAssignmentsIDs.Add(
                jm.DeserializeFromParentNode<MainAssignment>(responseReader.ReadResponse(owm.GetResponse(MaUB.Text.Text, new POST().Text)), new RESULT().Text)
                .Select(s => s).FirstOrDefault().id.Replace(new OrientIdSharpToken().Text, string.Empty)
            );


            //delete persons and units with where clause condition
            responseReader.ReadResponse(owm.GetResponse(dpUt, new POST().Text));
            responseReader.ReadResponse(owm.GetResponse(duUt, new POST().Text));

        }

        public static void ManagersCheck()
        {

            string url = @"http://10.31.14.76/cleverence_ui/hs/IntraService/location/full";

            OrientWebManager owm = new OrientWebManager();
            JSONmanager jm = new JSONmanager();
            WebResponseReader wr = new WebResponseReader();

            //sample JSON [{},{}] http get from url

            string jres = wr.ReadResponse(owm.GetResponse(url, "GET"));

            //authenticate Orient
            owm.Authenticate(@"http://msk1-vm-ovisp02:2480/connect/news_test3", new NetworkCredential("root", "I9grekVmk5g"));

            //authenticated htp response from command
            url = @"http://msk1-vm-ovisp02:2480/command/news_test3/sql/select from Person";
            jres = wr.ReadResponse(owm.GetResponse(url, "GET"));
            //в коллекцию объектов из модели {node:[{},{}]} -> List<model>({}) для работы в коде
            IEnumerable<Person> persons = jm.DeserializeFromNode<Person>(jres, @"result");
            //в JSON строку List<{}> -> [{},{}] для передачи в API
            string str = jm.CollectionToStringFormat<Person>(persons, null);

            //из функции
            url = @"http://msk1-vm-ovisp02:2480/function/news_test3/SearchByLastNameTst/сав";
            jres = wr.ReadResponse(owm.GetResponse(url, "GET"));
            //{node:[{},{}]} -> List<model>({}) 
            persons = jm.DeserializeFromNode<Person>(jres, @"result");
            str = jm.CollectionToStringFormat<Person>(persons, null);

            //из комманды другой класс
            //authenticated htp response from command
            url = @"http://msk1-vm-ovisp02:2480/command/news_test3/sql/select from Unit";
            jres = wr.ReadResponse(owm.GetResponse(url, "GET"));
            //{node:[{},{}]} -> List<model>({})
            IEnumerable<Unit> units = jm.DeserializeFromNode<Unit>(jres, @"result");
            //в строку [{"name":"a"},..,{"name":"b"}]
            str = jm.CollectionToStringFormat<Unit>(units, null);
            //в строку ["a",..,"b"]
            str = jm.CollectionToStringFormat<JToken>(jm.ExtractTokens(jres, "result", "Name"), null);

        }
        public static void BuilderCheck()
        {

            string login = "root";
            string password = "I9grekVmk5g";
            string authUrl = @"http://msk1-vm-ovisp02:2480/connect/news_test3";
            string commandUrl = @"http://msk1-vm-ovisp02:2480/command/news_test3/sql/";
            string functionUrl = @"http://msk1-vm-ovisp02:2480/function/news_test3/";

            _OrientApiUrlBuilder ob = new _OrientApiUrlBuilder();

            //<--Out
            //HTTPmanager hm = new HTTPmanager();

            //-->In
            OrientWebManager owm = new OrientWebManager();
            NetworkCredential nc = new NetworkCredential(login, password);
            WebResponseReader wrr = new WebResponseReader();

            JSONmanager jm = new JSONmanager();


            owm.Authenticate(authUrl, nc);

            //задаем параметры комманды, тело селекта и тело where. where (можно оставить пустым)
            ob.SelectCommandSet("Person", @"1=1");
            //авторизуемся
            //old
            //hm.HTTPauthProxy(@"http://msk1-vm-ovisp02:2480/connect/news_test3", login, password);
            //new
            owm.Authenticate(authUrl, nc);
            //получам ответ форматированный в JSON сторку
            //old
            //string jsonStringResponse = hm.GetAuthProx(@"http://msk1-vm-ovisp02:2480/command/news_test3/sql/" + ob.SelectCommandGet());
            //new
            WebResponse wr = owm.GetResponse(commandUrl + ob.SelectCommandGet(), "GET");
            string responseRaw = wrr.ReadResponse(wr);
            //десериализуем в модель, проксирование управляется полями POCO класса модели и атрибутами
            IEnumerable<Person> persons = jm.DeserializeFromNode<Person>(responseRaw, @"result");


            //Аналогично для Unit
            //задаем имя класса и фильтр where
            ob.SelectCommandSet("Unit", @"1=1");
            responseRaw = wrr.ReadResponse(owm.GetResponse(commandUrl + ob.SelectCommandGet(), "GET"));
            IEnumerable<Unit> units = jm.DeserializeFromNode<Unit>(responseRaw, @"result");


            //Аналогично для функции
            //имя функции и параметр
            //Так как функция возвразает только одно поле, коллекция заполнится объектами со значением только в 1 поле
            //остальные null
            ob.FucntionSet("SearchByLastNameTst", @"сав");
            responseRaw = wrr.ReadResponse(owm.GetResponse(functionUrl + ob.FunctionCommandGet(), "GET"));
            IEnumerable<Person> persons2 = jm.DeserializeFromNode<Person>(responseRaw, @"result");
            //Кручу-верчу для получения "чистых" коллекицй, без полей с null.
            //(когда возращается только часть полей, null values игнорятся, без изменения модели и залезания в строку ответа)
            string jSer = jm.CollectionToStringFormat<Person>(persons2, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
            //Промежуточная коллекция с экстракцией нужного токена, для понимания типа
            IJEnumerable<JToken> jt = jm.ExtractTokens(jSer, "LastName");
            //Формирование "чистой" строки из коллекции токенов
            string res0 = jm.CollectionToStringFormat<IJEnumerable<JToken>>(jt, null);


            //Аналогично, без иключения null, oneline          
            res0 = jm.CollectionToStringFormat<IJEnumerable<JToken>>(jm.ExtractTokens(
                jm.CollectionToStringFormat<Person>(
                    jm.DeserializeFromNode<Person>(responseRaw, @"result")
                , null)
                , "LastName"), null);

        }
        public static void HTTPcheck()
        {

            WebResponseReader reader = new WebResponseReader();

            //read basic GET response
            string url = @"http://msk1-vm-ovisp01:8083/api/Person/GetCollegesLower/bs";
            OrientWebManager hm = new OrientWebManager();
            WebResponse response = hm.GetResponse(url, "GET");
            string sampleResult = reader.ReadResponse(response);


            //Authentication
            string authUrl = @"http://msk1-vm-ovisp02:2480/connect/news_test3";
            NetworkCredential nc = new NetworkCredential("root", "I9grekVmk5g");

            //read Orient fucntion GET with authentication
            url = @"http://msk1-vm-ovisp02:2480/function/news_test3/GetCollegesLowerByAccount/bs";
            OrientWebManager orm = new OrientWebManager();
            orm.Authenticate(authUrl, nc);
            response = orm.GetResponse(url, "GET");
            string resultOrient = reader.ReadResponse(response);


            //deserializing string request
            JSONmanager mng = new JSONmanager();
            IEnumerable<string> a = mng.DeseializeSample<string>(sampleResult);
            IEnumerable<string> b = mng.DeserializeFromNode<string>(resultOrient, "result", "Name");


            //change equality check
            //they are equal
            bool res = a.Equals(b);

        }       


    }
  
    //DRIVER scope
    public static class OrientNumToCLRType
    {
        public static string ValuetoString(int value_)
        {
            string result = null;
            switch (value_)
            {

                case 3:
                    result = @"LONG";
                    break;
                case 6:
                    result = @"DATETIME";
                    break;
                case 7:
                    result = @"STRING";
                    break;

            }
            return result;
        }
    }

    /// <summary>
    /// out deprecated
    /// DP Scope (data processing)    
    /// works with JSON stings
    /// </summary>

    public class JSONmanager
    {

        //For sample JSON structure [{a:1,..,c:1},{a:10,..,c:10}] or collection of model classes IEnumerable<class>
        public IEnumerable<T> DeseializeSample<T>(string resp) where T : class
        {
            IEnumerable<T> res = null;
            try
            {
                res = JsonConvert.DeserializeObject<IEnumerable<T>>(resp);
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
            return res;
        }
        //For JSON structure {NodeName:[{a:1,..,c:1},{a:10,..,c:10}]}
        public IEnumerable<T> DeserializeFromNode<T>(string jInput, string Node) where T : class
        {
            IEnumerable<T> result = null;
            IEnumerable<JToken> res = JToken.Parse(jInput)[Node].Children();
            result = CollectionConvert<T>(res);
            return result;
        }
        //For parsing not to model but to String for JSON structure {NodeName:[{a:1,..,c:1},{a:10,..,c:10}]}
        //where one item from collection can be parsed
        public IEnumerable<T> DeserializeFromNode<T>(string jInput, string Node, string field) where T : class
        {
            IEnumerable<T> result = null;
            IEnumerable<JToken> res = JObject.Parse(jInput)[Node].Children()[field];
            result = CollectionConvert<T>(res);
            return result;
        }

        //Extracts collection of tokens [{"Tk1":"A"},..,{"Tk31":"Z"}] -> IJEnumerable<JTokens> ["A",..,"Z"]
        public IJEnumerable<JToken> ExtractTokens(string jInput, string field)
        {
            IJEnumerable<JToken> result = null;
            result = JToken.Parse(jInput).Children()[field];
            return result;
        }
        public IJEnumerable<JToken> ExtractTokenChildren(string jInput, string field)
        {
            IJEnumerable<JToken> result = null;
            result = JToken.Parse(jInput).Children()[field].Children();
            return result;
        }
        public IJEnumerable<JToken> ExtractTokens(string jInput, string Node, string field)
        {
            IJEnumerable<JToken> result = null;
            result = JObject.Parse(jInput)[Node].Children()[field];
            return result;
        }

        public IEnumerable<T> CollectionConvert<T>(IEnumerable<JToken> input) where T : class
        {
            IEnumerable<T> result = null;
            result = (from s in input select s.ToObject<T>()).ToList();
            return result;
        }
        public string CollectionToStringFormat<T>(IEnumerable<T> list_, JsonSerializerSettings jss = null) where T : class
        {
            string result = null;
            result = JsonConvert.SerializeObject(list_, jss);
            return result;
        }

    }

}