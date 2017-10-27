using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using Orient.Client;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

using System.IO;

using System.Net;
using System.Net.Http;

using System.Web.Http;

namespace ConsoleApp1
{
    public class Record
    {
        List<string> Name { get; set; }
    }
    class OrientDriverConnnect
    {

        static void Main(string[] args)
        {

            InsertClausesCheck();
            DeleteClauseCheck();
            SelectClusesAndURLBuildCheck();
            HTTPclauseCheck();

            JSONManagerIntegrationTests();

            PersonApiCheckAsync();
            PersonApiCheck();

            JSONcheck();

            BuilderCheck();
            ManagersCheck();

            HTTPcheck();

        }

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

            JSONManager2 jm2 = new JSONManager2();

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
        public static void OrientSpagettyCheck()
        {

            OrientSpagettyCheck oc = new OrientSpagettyCheck();
            oc.Initialize();
            oc.DBmigrate();
            oc.MigrateEdgeData<MainAssignment>();
            oc.MigrateEdgeData<OldMainAssignment>();
            oc.MigrateEdgeData<OutExtAssignment>();
            oc.MigrateEdgeData<SubUnit>();

        }

        public static void PersonApiCheck()
        {

            WebManager wm = new WebManager();

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

            WebManager wm = new WebManager();

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

        //<-- deprecated
        //mooved to tests
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

            JToken jt1 = null, jt2 = null;
            Record jt3 = new Record();
            JToken r1, r0 = null;
            IEnumerable<JToken> r2 = null, r3 = null;

            try {
                jt1 = JsonConvert.DeserializeObject<JToken>(input);
            } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try {
                jt2 = JsonConvert.DeserializeObject<JToken>(input2);
            } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }
            try {
                jt3 = JsonConvert.DeserializeObject<Record>(input3);
            } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

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

            try {
                r1 = JToken.Parse(input);
                r0 = JToken.Parse(input)[rk];
                r2 = JToken.Parse(input2).Children()[nk];
                r3 = JToken.Parse(input3).Children()[nk];
            } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e); }

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

        //Token managers check
        //check select cluase
        public static void SelectClusesAndURLBuildCheck()
        {

            //Get collection of tokens used for concatenating authentication URL part
            List<ITypeToken> urlTokens = new List<ITypeToken>()
            { new OrientHost(), new OrientPort(), new OrientAuthenticateToken(), new OrientDb() };
            //Initialize Format for auth URL string concat 
            //- > {0}:{1}/{2}/{3}
            // <host>:<port>/connect/<dbname>
            OrientAuthenticationURLFormat ot = new OrientAuthenticationURLFormat();
            //Build auth url from url tokens with url concat rule
            OrientAuthenticationURL ur = new OrientAuthenticationURL(urlTokens, ot);
            //Authentication URL text
            string authUrl = ur.Text.Text;


            //Initialize Format for command URL string concat 
            //-> {0}:{1}/{2}/{3}
            // <host>:<port>/command/<dbname>/sql
            OrientCommandURLFormat cf = new OrientCommandURLFormat();
            //tokens for command url part
            List<ITypeToken> urlCommandTokens = new List<ITypeToken>()
            { new OrientHost(), new OrientPort(), new OrientCommandToken(), new OrientDb(), new OrientCommandSQLTypeToken() };
            //Command URL text
            OrientCommandURLBuilder commandUrlPart = new OrientCommandURLBuilder(urlCommandTokens, cf);
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
                new OrientSelectClauseBuilder(
                    selectCommandTokens
                    , of
                );
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
            List<ITextBuilder> CommandTokens = new List<ITextBuilder>(){
                commandUrlPart,selectUrlPart,whereUrlPart
            };
            //Aggregate all query TokenManagers to one Select URL command with where
            OrientCommandURLBuilder commandSample = new OrientCommandURLBuilder(
                CommandTokens, new TextToken() { Text = @"{0}/{1} {2}" }, URLbuilder.BuildTypeFormates.NESTED
                );
            //full select query command
            string selectcommandURL = commandSample.Text.Text;

        }
        //check insert clause
        public static void InsertClausesCheck()
        {
            Person per = new Person() { Name = "0", GUID = "0", Changed = DateTime.Now, Created = DateTime.Now };
            JSONManager2 jm = new JSONManager2();
            string contentText = jm.SerializeObject(per, 
                new JsonSerializerSettings() {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None,
                    DateFormatString = @"yyyy-MM-dd HH:mm:ss" });
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
                new List<ITypeToken>() {new OrientWhereToken(), new TextToken(){Text="\"Name\" = \"0\"" }};
            OrientWhereClauseFormat cf = 
                new OrientWhereClauseFormat();
            OrientWhereClauseBuilder cb = new OrientWhereClauseBuilder(wT, cf);


            //<<ad where clause builder
            List<ITypeToken> dt = 
                new List<ITypeToken>(){new OrientDeleteToken(), new OrientVertexToken(), new OrientPersonToken()
                , new TextToken(){Text = "where Name = 0" }};
            OrientDeleteVertexCluaseFormat df 
                = new OrientDeleteVertexCluaseFormat();
            OrientDeleteClauseBuilder dlb = new OrientDeleteClauseBuilder(dt,df);

            string deleteClause = dlb.Text.Text;
        }
        //HTTP clause
        public static void HTTPclauseCheck()
        {


            List<ITypeToken> commandTokents = 
    new List<ITypeToken>(){ new OrientHost(),new OrientPort(), new OrientCommandToken(), new OrientDb(),new OrientCommandSQLTypeToken()};

            Person per = new Person() { Name = "0", GUID = "0", Changed = DateTime.Now, Created = DateTime.Now };
            JSONManager2 jm = new JSONManager2();
            string contentText = jm.SerializeObject(per,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Formatting = Formatting.None,
                    DateFormatString = @"yyyy-MM-dd HH:mm:ss"
                });
            TextToken personContent = new TextToken() { Text = contentText };

            List<ITypeToken> CreateTokens =
    new List<ITypeToken>() { new OrientCreateToken(), new OrientVertexToken(), new OrientPersonToken(), new OrientContentToken() , personContent };
            List<ITypeToken> selectTokens =
    new List<ITypeToken>() { new OrientSelectToken(), new OrientFromToken(), new OrientPersonToken() };
            List<ITypeToken> DeleteToken =
    new List<ITypeToken>() { new OrientDeleteToken(), new OrientVertexToken(), new OrientPersonToken(), null,null };
            List<ITypeToken> whereTokens =
    new List<ITypeToken>() { new OrientWhereToken(), new TextToken() { Text = "Name = 0" } };

            OrientCommandURLFormat uf = new OrientCommandURLFormat();
            OrientCreateVertexCluaseFormat cf = new OrientCreateVertexCluaseFormat();
            OrientSelectClauseFormat sf = new OrientSelectClauseFormat();
            OrientDeleteVertexCluaseFormat df = new OrientDeleteVertexCluaseFormat();
            OrientWhereClauseFormat wf = new OrientWhereClauseFormat();

            OrientCommandURLBuilder ub = new OrientCommandURLBuilder(commandTokents, uf);
            OrientCreateClauseBuilder cb = new OrientCreateClauseBuilder(CreateTokens, cf);
            OrientSelectClauseBuilder sb = new OrientSelectClauseBuilder(selectTokens, sf);
            OrientDeleteClauseBuilder db = new OrientDeleteClauseBuilder(DeleteToken, df);
            OrientWhereClauseBuilder wb = new OrientWhereClauseBuilder(whereTokens, wf);

            string url = ub.Text.Text;
            string create = cb.Text.Text;
            string select = sb.Text.Text;
            string delete = db.Text.Text;
            string where = wb.Text.Text;

            List<ITextBuilder> createTk = new List<ITextBuilder>() {ub,cb};
            List<ITextBuilder> selectTk = new List<ITextBuilder>() { ub, sb, wb };
            List<ITextBuilder> deleteTk = new List<ITextBuilder>() { ub, db, wb };

            OrientCommandURLBuilder cU =
    new OrientCommandURLBuilder(createTk, new TextToken() { Text = @"{0}/{1}" },URLbuilder.BuildTypeFormates.NESTED);
            OrientCommandURLBuilder sU =
    new OrientCommandURLBuilder(selectTk, new TextToken() { Text = @"{0}/{1} {2}" }, URLbuilder.BuildTypeFormates.NESTED);
            OrientCommandURLBuilder dU =
    new OrientCommandURLBuilder(deleteTk, new TextToken() { Text = @"{0}/{1} {2}" }, URLbuilder.BuildTypeFormates.NESTED);

            string cUt = cU.Text.Text;
            string sUt = sU.Text.Text;
            string dUt = dU.Text.Text;

        }

        public static void ManagersCheck()
        {

            string url = @"http://10.31.14.76/cleverence_ui/hs/IntraService/location/full";

            OrientWebManager owm = new OrientWebManager();
            JSONmanager jm = new JSONmanager();
            WebResponseReader wr = new WebResponseReader();

            //sample JSON [{},{}] http get from url

            string jres = wr.ReadResponse(owm.GetResponse(url, "GET"));
            IEnumerable<J_Address> addresses = jm.DeseializeSample<J_Address>(jres);

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

            OrientApiUrlBuilder ob = new OrientApiUrlBuilder();

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
            WebManager hm = new WebManager();
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



    //DB CREATE, Objects from existing DB move, AND DROP. no class segregation
    public class OrientSpagettyCheck
    {

        protected static string OSESSIONID;

        string Authurl;

        OrientDB_Net.binary.Innov8tive.API.ConnectionOptions childBasePool;
        OrientDB_Net.binary.Innov8tive.API.ConnectionOptions parentBasePool;

        OServer os;

        ODatabase childBase;
        ODatabase parentBase;

        List<ODocument> childClasses;
        List<ODocument> parentClasses;

        public ODatabase openDatabase(string _host, int _port,
        string _dbName, string _user, string _passwd)
        {

            // CONSOLE LOG
            Console.WriteLine("Opening Database: {0}", _dbName);

            // OPEN DATABASE
            ODatabase database = new ODatabase(_host, _port, _dbName,
                ODatabaseType.Graph, _user, _passwd);

            // RETURN ODATABASE INSTANCE
            return database;
        }

        public void Initialize()
        {
            Authurl = string.Format(@"{0}{1}:{2}/connect/{3}",
            @"http://",
            ConfigurationManager.AppSettings["ParentHost"],
            "2480",
            ConfigurationManager.AppSettings["ParentDBname"]
            );

            childBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();
            parentBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();

            childBasePool.HostName = ConfigurationManager.AppSettings["ChildHost"];
            childBasePool.UserName = ConfigurationManager.AppSettings["ChildLogin"];
            childBasePool.Password = ConfigurationManager.AppSettings["ChildPassword"];
            childBasePool.Port = 2424;
            childBasePool.DatabaseName = ConfigurationManager.AppSettings["ChildDBname"];
            childBasePool.DatabaseType = ODatabaseType.Graph;

            parentBasePool.HostName = ConfigurationManager.AppSettings["ParentHost"];
            parentBasePool.UserName = ConfigurationManager.AppSettings["ParentLogin"]; ;
            parentBasePool.Password = ConfigurationManager.AppSettings["ParentPassword"];
            parentBasePool.Port = 2424;
            parentBasePool.DatabaseName = ConfigurationManager.AppSettings["ParentDBname"];
            parentBasePool.DatabaseType = ODatabaseType.Graph;

            //Create DB
            //http://orientdb.com/docs/last/NET-Server-CreateDatabase.html
            os = new OServer(childBasePool.HostName, childBasePool.Port, childBasePool.UserName, childBasePool.Password);

            childBase = openDatabase(childBasePool);
            parentBase = openDatabase(parentBasePool);
        }
        public void DBmigrate()
        {

            //OServer os = new OServer("msk1-vm-ovisp02",2424, "root", "I9grekVmk5g");
            if (os.DatabaseExist("news_test3", OStorageType.PLocal))
            {
                os.DropDatabase("news_test3", OStorageType.PLocal);
            }

            os.CreateDatabase("news_test3", childBasePool.DatabaseType, OStorageType.PLocal);

            //Get list of classes
            childClasses = GetDbClasses(childBase, ConfigurationManager.AppSettings["GetAllClasses"]);
            parentClasses = GetDbClasses(parentBase, ConfigurationManager.AppSettings["GetAllClasses"]);

            //Exort to JSON txt
            JsonSerialize(childClasses, @"child");
            JsonSerialize(parentClasses, @"parent");

            List<ODocument> uniqueParentKeys = FilterUniqueKeys(parentClasses, childClasses);

            JsonSerialize(uniqueParentKeys, @"unique");

            //Add properties to Vertix and Edge classes
            List<ODocument> ParentEdges = FilterClasses(uniqueParentKeys, "E");
            List<ODocument> NestedEdges = FilterClassesBySuperclass(uniqueParentKeys, ParentEdges);

            AddClassesNested(ParentEdges, childBase);
            AddProperties(ParentEdges, parentBase, childBase);
            AddClassesNested(NestedEdges, childBase);
            AddProperties(NestedEdges, parentBase, childBase);

            List<ODocument> ParentVertices = FilterClasses(uniqueParentKeys, "V");
            List<ODocument> NestedVertices = FilterClassesBySuperclass(uniqueParentKeys, ParentVertices);

            AddClassesNested(ParentVertices, childBase);
            AddProperties(ParentVertices, parentBase, childBase);
            AddClassesNested(NestedVertices, childBase);
            AddProperties(NestedVertices, parentBase, childBase);

            //Migrate Data from parent db in config with select from class to API
            AuthorizeOrientDB(Authurl);
            MigrateEntity<Person>(childBase);
            MigrateEntity<Unit>(childBase);

            //Drop Db
            childBase.Close();
            childBase.Dispose();
            os.Close();
            os.Dispose();
        }
        public void MigrateEdgeData<T>() where T : class, Edge
        {
            AuthorizeOrientDB(Authurl);
            string SelectCommand = string.Format(ConfigurationManager.AppSettings["SelectCommand"], "in,out", typeof(T).Name);
            List<T> MainAssignmentCollection = GetEntityCollectionFromParentBase<T>(SelectCommand);

            List<MigrateCollection> parentInGUIDs, parentOutGUIDs;
            List<MigrateCollection> childInIDs = new List<MigrateCollection>();
            List<MigrateCollection> childOutIDs = new List<MigrateCollection>();

            foreach (T mAssign in MainAssignmentCollection)
            {
                parentInGUIDs = JSONstringToCollection<MigrateCollection>(GetGUIDFromIDParentDB(mAssign.In.Replace(@"#", "")));
                parentOutGUIDs = JSONstringToCollection<MigrateCollection>(GetGUIDFromIDParentDB(mAssign.Out.Replace(@"#", "")));
                childInIDs = new List<MigrateCollection>();
                childOutIDs = new List<MigrateCollection>();

                foreach (MigrateCollection mA in parentInGUIDs)
                {
                    childInIDs.Add(new MigrateCollection() { rid = GetChildId(mA.GUID, mA.@class) });
                }
                foreach (MigrateCollection mA in parentOutGUIDs)
                {
                    childOutIDs.Add(new MigrateCollection() { rid = GetChildId(mA.GUID, mA.@class) });
                }

                foreach (MigrateCollection in_ in childInIDs)
                {
                    foreach (MigrateCollection out_ in childOutIDs)
                    {
                        CreateEdge<T>(out_, in_);
                    }
                }
            }
        }

        public ODatabase openDatabase(OrientDB_Net.binary.Innov8tive.API.ConnectionOptions connectionOption_)
        {
            ODatabase odb = new ODatabase(connectionOption_);
            return odb;
        }
        public void SpagettyCommands()
        {

            string Authurl = string.Format(@"{0}{1}:{2}/connect/{3}",
            @"http://",
            ConfigurationManager.AppSettings["ParentHost"],
            "2480",
            ConfigurationManager.AppSettings["ParentDBname"]
            );

            //OrientDB_Net.binary.Innov8tive.API.ConnectionOptions

            OrientDB_Net.binary.Innov8tive.API.ConnectionOptions childBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();
            OrientDB_Net.binary.Innov8tive.API.ConnectionOptions parentBasePool = new OrientDB_Net.binary.Innov8tive.API.ConnectionOptions();

            childBasePool.HostName = ConfigurationManager.AppSettings["ChildHost"];
            childBasePool.UserName = ConfigurationManager.AppSettings["ChildLogin"];
            childBasePool.Password = ConfigurationManager.AppSettings["ChildPassword"];
            childBasePool.Port = 2424;
            childBasePool.DatabaseName = ConfigurationManager.AppSettings["ChildDBname"];
            childBasePool.DatabaseType = ODatabaseType.Document;

            parentBasePool.HostName = ConfigurationManager.AppSettings["ParentHost"];
            parentBasePool.UserName = ConfigurationManager.AppSettings["ParentLogin"]; ;
            parentBasePool.Password = ConfigurationManager.AppSettings["ParentPassword"];
            parentBasePool.Port = 2424;
            parentBasePool.DatabaseName = ConfigurationManager.AppSettings["ParentDBname"];
            parentBasePool.DatabaseType = ODatabaseType.Graph;


            string addVertex = string.Format(@"var d = orient.getGraph(); var vertex = db.addVertex({0} EXTENDS V)", @"Test_vertex");
            string addClassV = string.Format(@"var gdb = orient.getGraph(); var newClass = db.createVertexType({0},{1})", "Friend", "V");

            //http://orientdb.com/docs/last/NET-Server-CreateDatabase.html
            OServer os = new OServer(childBasePool.HostName, childBasePool.Port, childBasePool.UserName, childBasePool.Password);
            //OServer os = new OServer("msk1-vm-ovisp02",2424, "root", "I9grekVmk5g");
            if (!os.DatabaseExist(childBasePool.DatabaseName, OStorageType.PLocal))
            {
                os.CreateDatabase(childBasePool.DatabaseName, childBasePool.DatabaseType, OStorageType.PLocal);
            }

            var a = typeof(OServer);
            var b = os.GetType();

            Dictionary<string, string> serverConfig = os.ConfigList();
            var jsonConfig = JsonConvert.SerializeObject(serverConfig);

            ODatabase childBase = openDatabase(childBasePool);
            ODatabase parentBase = openDatabase(parentBasePool);



        }

        public void ChilldBaseDrop()
        {
            if (os.DatabaseExist(childBasePool.DatabaseName, OStorageType.PLocal))
            {
                os.DropDatabase(childBasePool.DatabaseName, OStorageType.PLocal);
            }
        }
        public List<ODocument> GetDbClasses(ODatabase db, string command)
        {
            return db.Query(command);
        }

        public List<ODocument> FilterUniqueKeys(List<ODocument> parent_, List<ODocument> child_)
        {
            List<ODocument> uniqueParentKeys =
                parent_.Except(
                from z in (from s in child_ where s.Keys.Contains("name") select s)
                join x in (from s in child_ where s.Keys.Contains("name") select s) on z["name"] equals x["name"]
                select z).ToList();
            return uniqueParentKeys;
        }

        public void JsonSerialize(object object_, string name_)
        {
            string object_str = JsonConvert.SerializeObject(object_, Formatting.Indented);
            File.WriteAllText(string.Format(@"C:\workflow\temp\serialized_{0}.txt", name_), object_str);
        }

        public void AddClasses(List<ODocument> collection, ODatabase database_)
        {
            foreach (ODocument class_ in collection)
            {
                var props = class_["properties"];
                string name = (string)class_["name"];
                string superclass = (string)class_["superClass"];
                database_.Command(
                    string.Format(ConfigurationManager.AppSettings["CreateClass"], name)
                    );
            }
        }
        public void AddClassesNested(List<ODocument> collection, ODatabase database_)
        {
            foreach (ODocument class_ in collection)
            {
                var props = class_["properties"];
                string name = (string)class_["name"];
                string superclass = (string)class_["superClass"];
                database_.Command(
                    string.Format(ConfigurationManager.AppSettings["CreateClassNested"], name, superclass)
                    );
            }
        }
        public void AddProperties(IEnumerable<ODocument> classes, ODatabase parentBase_, ODatabase childBase_)
        {
            foreach (ODocument class_ in classes)
            {
                IEnumerable<ODocument> properties = GetProperties(class_, parentBase_);
                if (properties != null)
                {
                    foreach (ODocument property in properties)
                    {
                        AddPropertyToClass(property, class_, childBase_);
                    }
                }


            }
        }

        public IEnumerable<ODocument> GetProperties(ODocument class_, ODatabase parentBase_)
        {
            IEnumerable<ODocument> result = null;
            Orient.Client.API.Query.OSqlSchema schema_ = parentBase_.Schema;
            string className = (string)class_["name"];
            if (className != "Object_SC" && (string)class_["superClass"] != "Object_SC")
            {
                if ((string)class_["superClass"] != "V" || (string)class_["superClass"] != "E")
                {
                    result = schema_.Properties((string)class_["superClass"]);
                    IEnumerable<ODocument> result2 = schema_.Properties((string)class_["name"]);
                    result = result.Concat(result2);
                }
                else
                {
                    result = schema_.Properties((string)class_["name"]);
                }

            }
            return result;
        }
        public void AddPropertyToClass(ODocument property_, ODocument class_, ODatabase childDatabase_)
        {
            string className = (string)class_["name"];
            string name = (string)property_["name"];
            int type = (int)property_["type"];
            string typeStr = OrientNumToCLRType.ValuetoString(type);
            string comm = string.Format(ConfigurationManager.AppSettings["CreateProperty"], className, name, typeStr);
            childDatabase_.Command(comm);
        }
        public List<ODocument> FilterClasses(List<ODocument> classes_, string type_)
        {
            return (from s in classes_ where (string)s["superClass"] == type_ select s).ToList();
        }
        public List<ODocument> FilterClassesBySuperclass(List<ODocument> classes_, List<ODocument> filter_)
        {
            return (from s in classes_
                    join c in filter_ on s["superClass"] equals c["name"]
                    select s).ToList();
        }

        public void AuthorizeOrientDB(string url)
        {
            WebRequest AuthRequest = WebRequest.Create(url);
            AuthRequest.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ParentLogin"], ConfigurationManager.AppSettings["ParentPassword"]);
            AuthRequest.Method = "GET";
            AuthRequest.ContentType = "application/json; charset=utf-8";
            WebResponse response = AuthRequest.GetResponse();
            OSESSIONID = response.Headers.Get("Set-Cookie");
            int status = (int)((HttpWebResponse)response).StatusCode;
            response.Close();
        }
        public string GetFromParentDB(string url)
        {
            WebRequest wr = WebRequest.Create(url);
            wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
            wr.Method = "GET";
            wr.ContentType = "application/json; charset=utf-8";
            var resp = new HttpResponseMessage(HttpStatusCode.OK);

            string res = WebRequestToString(wr);
            return res;
        }
        public object GetObj(string url)
        {
            WebRequest wr = WebRequest.Create(url);
            wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
            wr.Method = "GET";
            wr.ContentType = "application/json; charset=utf-8";
            var resp = new HttpResponseMessage(HttpStatusCode.OK);

            object res = WebRequestToObj(wr);
            return res;
        }
        public string WebRequestToString(WebRequest wr)
        {
            using (var responseApi = (HttpWebResponse)wr.GetResponse())
            {
                using (var reader = new StreamReader(responseApi.GetResponseStream()))
                {
                    string objText = reader.ReadToEnd();
                    var objClasses = JsonConvert.DeserializeObject(objText);
                    string jsonResult = objClasses.ToString();
                    return jsonResult;
                }
            }
        }
        public object WebRequestToObj(WebRequest wr)
        {
            using (var responseApi = (HttpWebResponse)wr.GetResponse())
            {
                using (var reader = new StreamReader(responseApi.GetResponseStream()))
                {
                    var objText = reader.ReadToEnd();
                    var objClasses = JsonConvert.DeserializeObject(objText);


                    return objClasses;

                }
            }
        }

        public List<T> JSONstringToCollection<T>(string input_) where T : class
        {
            JObject jarr = JObject.Parse(input_);
            List<JToken> results = jarr["result"].Children().ToList();
            List<T> persList = new List<T>();

            foreach (JToken jt in results)
            {
                persList.Add(jt.ToObject<T>());
            }
            return persList;
        }
        public string ObjectToJSONString<T>(T Object) where T : class
        {
            string res = JsonConvert.SerializeObject(Object, new IsoDateTimeConverter() { DateTimeFormat = @"yyyy-MM-dd HH:mm:ss" }).ToString();
            return res;
        }

        public string JsonToCommand(string ApiGetRes)
        {
            ApiGetRes = ApiGetRes.Replace("[", "").Replace("]", "");
            return ApiGetRes;
        }
        public string SelectCommand(string command_)
        {
            return string.Format(@"{0}{1}:{2}/command/{3}/sql/{4}",
            @"http://",
            ConfigurationManager.AppSettings["ParentHost"],
            "2480",
            ConfigurationManager.AppSettings["ParentDBname"],
            //@"select Seed,Created,GUID,Changed,FirstName,LastName,MiddleName,Birthday,mail,telephoneNumber,userAccountControl,objectGUID,sAMAccountName,Name,Hash,OneSHash from person LIMIT 5");         
            command_);
        }

        public List<T> GetEntityCollectionFromParentBase<T>(string command_) where T : class
        {
            List<T> list = JSONstringToCollection<T>(GetFromParentDB(SelectCommand(command_)));
            return list;
        }
        public void MigrateEntity<T>(ODatabase childBase_) where T : class
        {
            string SelectCommand = string.Format(ConfigurationManager.AppSettings["SelectCommand"], "*", typeof(T).Name);
            List<T> entities = GetEntityCollectionFromParentBase<T>(SelectCommand);
            //Person pers = JsonConvert.DeserializeObject<Person>(ApiGetRes);
            //GetObj(commUrl);
            //Get(commUrl);
            foreach (T item in entities)
            {
                string command = JsonToCommand(ObjectToJSONString<T>(item));
                //valid insert command
                //command = @"{""Seed"":""1005821"",""Created"":""2014-12-15 12:21:24"",""GUID"":""4e28f31a-89b8-11e4-bab2-00c2c66d13b0"",""Changed"":""2017-08-23 10:06:45"",""DeparmentColorRGB"":null,""DeparmentColorClass"":null,""Disabled"":null}";
                command = string.Format(
                    ConfigurationManager.AppSettings["InsertEntityCommand"],
                     item.GetType().Name
                    , command.Replace(@"""0001-01-01 00:00:00""", "null"));

                childBase_.Command(command);
            }

        }

        public string GetGUIDFromIDParentDB(string ID)
        {
            string result = null;
            string command_ = string.Format(ConfigurationManager.AppSettings["SelectCommand"], "@rid,@class,GUID", ID);
            result = GetFromParentDB(SelectCommand(command_));
            return result;
        }
        public string GetChildId(string GUID, string class_)
        {
            string result = null;
            string command_ = string.Format(ConfigurationManager.AppSettings["SelectWhereCommand"], "@rid", class_, @"GUID = '" + GUID + "'");
            OCommandResult Oresult = childBase.Command(command_);
            result = (from s in Oresult.ToList() select s).FirstOrDefault()["rid"].ToString();
            return result;
        }
        public void CreateEdge<T>(MigrateCollection From, MigrateCollection To) where T : class, Edge
        {
            string command_ = string.Format(ConfigurationManager.AppSettings["CreateEdge"], typeof(T).Name, From.rid, To.rid);
            childBase.Command(command_);
        }

    }

    //MODEL scope
    #region POCOs
    //@JsonInclude(Include.NON_NULL)

    public class Person
    {
        [JsonProperty("type")]
        public string @type { get; set; }
        public string @rid { get; set; }
        public string @version { get; set; }
        [JsonProperty("class")]
        public string @class { get; set; }

        public long? Seed { get; set; }
        public DateTime? Created { get; set; }
        public string GUID { get; set; }
        public DateTime? Changed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? Birthday { get; set; }
        public string mail { get; set; }
        public int? telephoneNumber { get; set; }
        public int? userAccountControl { get; set; }
        public string objectGUID { get; set; }
        public string sAMAccountName { get; set; }
        public string Name { get; set; }
        public string OneSHash { get; set; }
        public string Hash { get; set; }

        /*
        public List<string> in_MainAssignment { get; set; }
        public List<string> out_MainAssignment { get; set; }
        */

        [JsonProperty("fieldTypes")]
        public string @fieldTypes { get; set; }


    }
    public class Unit
    {
        public long? Seed { get; set; }
        public DateTime? Created { get; set; }
        public string GUID { get; set; }
        public DateTime? Changed { get; set; }
        public string PGUID { get; set; }
        public string DepartmentColorRGB { get; set; }
        public string DepartmentColorClass { get; set; }
        public DateTime? Disabled { get; set; }
        public string Hash { get; set; }
        public string Name { get; set; }

    }
    public interface Edge
    {
        string Out { get; set; }
        string In { get; set; }
    }
    public class MainAssignment : Edge
    {
        public string Out { get; set; }
        public string In { get; set; }
    }
    public class OldMainAssignment : Edge
    {
        public string Out { get; set; }
        public string In { get; set; }
    }
    public class OutExtAssignment : Edge
    {
        public string Out { get; set; }
        public string In { get; set; }
    }
    public class SubUnit : Edge
    {
        public string Out { get; set; }
        public string In { get; set; }
    }
    public class MigrateCollection
    {
        public string @rid { get; set; }
        public string @class { get; set; }
        public string GUID { get; set; }
    }
    public class J_Address
    {
        [JsonProperty("GUID")]
        string GUID { get; set; }
        [JsonProperty("Адрес")]
        string Address { get; set; }
    }

    #endregion

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
    /// Testing APIS
    /// Manages class for JSON API url,expected,ectual,ok values. Read/create + export JSON file with values.
    /// For every URL execute webrequest, reads string response, compares with Expected value, 
    /// changes statusess - OK(true/flase), Exception message - if needed. Exports result.
    /// </summary>
    public class APItester_sngltn
    {
        public string URI { get; set; }
        public string Expected { get; set; }
        public string Actual { get; set; }
        public bool OK { get; set; }
        public string ExceptionText { get; set; }
        public string Comment { get; set; }

        public static List<APItester_sngltn> TestCases = new List<APItester_sngltn>();

        public void OK_(string actual_)
        {
            this.Actual = actual_;
            this.ExceptionText = string.Empty;
            this.OK = true;
        }
        public void OK_(string actual_, string exception_)
        {
            this.Actual = actual_;
            this.ExceptionText = exception_;
            this.OK = true;
        }

        public void NotOK_(string actual_)
        {
            this.Actual = actual_;
            this.ExceptionText = string.Empty;
            this.OK = false;
        }
        public void NotOK_(string actual_, string exception_)
        {
            this.Actual = actual_;
            this.ExceptionText = exception_;
            this.OK = false;
        }

    }


    //<--Out - deprecated
    //WEB scope
    //divide to string method initiation (GET,POST -> execute), add parameters addition before call <- done
    //move web request object to separate class <- done
    //JSON reader to new class, return only web request, then decide how to handle it (JSON,XML,STRING,ACTION) <- done
    /*
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
    */


    public interface IJsonManger
    {

        IJEnumerable<JToken> ExtractFromParentNode(string input, string parentNodeName);
        IJEnumerable<JToken> ExtractFromParentChildNode(string input, string parentNodeName, string childNodeName);
        IJEnumerable<JToken> ExtractFromChildNode(string input, string childNodeName);

        string SerializeObject(object input_, JsonSerializerSettings settings_);

        IEnumerable<T> DeserializeFromParentNode<T>(string input, string parentNodeName) where T : class;
        IEnumerable<T> DeserializeFromParentChildNode<T>(string input, string parentNodeName, string childNodeName) where T : class;
        IEnumerable<T> DeserializeFromChildNode<T>(string input, string childNodeName) where T : class;

        IEnumerable<T> JTokensToCollection<T>(IEnumerable<JToken> input) where T : class;
        string CollectionToStringFormat<T>(IEnumerable<T> list_, JsonSerializerSettings jss = null) where T : class;

    }
    /// <summary>
    /// Gets response from URL with method
    /// </summary>
    public interface IWebManager
    {
        WebResponse GetResponse(string url, string method);
    }
    /// <summary>
    /// Reads response and converts it to string
    /// </summary>
    public interface IResponseReader
    {
        string ReadResponse(HttpWebResponse response);
        string ReadResponse(WebResponse response);
        string ReadResponse(HttpResponseMessage response);
    }


    /// -->In new overwritten
    ///Base web manager for sending request with type method and reading response to URL
    ///WebRequest, Httpwebresponse
    public class WebManager : IWebManager
    {

        public WebRequest _request;
        internal string OSESSIONID;

        public WebManager()
        {
            _request = null;
            this.OSESSIONID = string.Empty;
        }
        public WebRequest addRequest(string url, string method)
        {
            try
            {
                _request = WebRequest.Create(url);
                _request.Method = method;
                _request.ContentLength = 0;
            }
            catch (Exception e)
            {
                throw e;
            }
            return _request;
        }
        internal void addHeader(HttpRequestHeader header, string value)
        {
            _request.Headers.Add(header, value);
        }
        internal string getHeaderValue(string header)
        {
            string result = string.Empty;
            result = this._request.GetResponse().Headers.Get(header);
            return result;
        }
        internal void addCredentials(NetworkCredential credentials)
        {
            this._request.Credentials = credentials;
        }
        public virtual WebResponse GetResponse(string url, string method)
        {
            HttpWebResponse resp;
            addRequest(url, method);

            try
            {
                resp = (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                throw e;
            }

            return resp;
        }
        public virtual async Task<HttpWebResponse> GetResponseAsync(string url, string method)
        {
            HttpWebResponse resp;
            addRequest(url, method);
            try
            {
                resp = (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                throw e;
            }

            return await Task.FromResult(resp);
        }

    }
    /// <summary>
    ///     Orient specific WebManager for authentication and authenticated resopnses sending to URL
    ///     with NetworkCredentials
    /// </summary>    
    public class OrientWebManager : WebManager
    {
        //>> add async
        public new HttpWebResponse GetResponse(string url, string method)
        {
            HttpWebResponse resp = null;
            base.addRequest(url, method);
            addHeader(HttpRequestHeader.Cookie, this.OSESSIONID);
            try
            {
                resp = (HttpWebResponse)this._request.GetResponse();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
            }

            return resp;
        }
        public WebResponse Authenticate(string url, NetworkCredential nc)
        {

            WebResponse resp;
            addRequest(url, "GET");
            addCredentials(nc);
            try
            {
                resp = this._request.GetResponse();
                OSESSIONID = getHeaderValue("Set-Cookie");
                return resp;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }

    /// <summary>
    /// WEB scope deprecation posible (Only if several credentials to different hosts needed, several DBs? )
    /// Contains Credentials for URI
    /// </summary>    
    public class CredentialPool
    {
        CredentialCache credentialsCache;

        public void Add(Uri uri, string type, string username, string password)
        {
            credentialsCache = new CredentialCache();
            credentialsCache.Add(uri, type, new NetworkCredential(username, password));
        }
        public NetworkCredential credentials(Uri uri_, string type_)
        {
            return credentialsCache.GetCredential(uri_, type_);
        }
    }

    /// <summary>
    /// DP Scope (data processing)    
    /// converts Responses to string
    /// </summary>
    public class WebResponseReader : IResponseReader
    {
        public string ReadResponse(WebResponse response)
        {
            string result = string.Empty;
            Stream sm = response.GetResponseStream();
            StreamReader sr = new StreamReader(sm);
            result = sr.ReadToEnd();
            return result;
        }
        public string ReadResponse(HttpWebResponse response)
        {
            string result = string.Empty;
            Stream sm = response.GetResponseStream();
            StreamReader sr = new StreamReader(sm);
            result = sr.ReadToEnd();
            return result;
        }
        public string ReadResponse(HttpResponseMessage response)
        {
            string result = string.Empty;
            System.Net.Http.HttpContent sm = response.Content;
            Task<Stream> sr = sm.ReadAsStreamAsync();
            Task<string> res = sm.ReadAsStringAsync();
            result = res.Result;
            return result;
        }
        public string ReadResponse(Task<HttpResponseMessage> response)
        {
            string result = string.Empty;
            Task<string> st = null;
            try {
                st = response.Result.Content.ReadAsStringAsync();
                result = st.Result;
            }
            catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }

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
            try {
                res = JsonConvert.DeserializeObject<IEnumerable<T>>(resp);
            } catch (Exception e) { System.Diagnostics.Trace.WriteLine(e.Message); }
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
    /// <summary>
    /// JSON manager revised 
    /// Newtonsoft JSON wrapper
    /// extracts IJEnumerable<Jtoken> from string
    /// deserializess to collection of objects or strings (if convertable)
    /// converts to string results with options
    /// </summary>
    public class JSONManager2 : IJsonManger
    {        

        public IJEnumerable<JToken> ExtractFromParentNode(string input, string parentNodeName)
        {
            IJEnumerable<JToken> result = null;
            result = JToken.Parse(input)[parentNodeName];
            return result;
        }
        public IJEnumerable<JToken> ExtractFromParentChildNode(string input, string parentNodeName, string childNodeName)
        {
            IJEnumerable<JToken> result = null;
            result = JToken.Parse(input)[parentNodeName].Children()[childNodeName];
            return result;
        }
        public IJEnumerable<JToken> ExtractFromChildNode(string input, string childNodeName)
        {
            IJEnumerable<JToken> result = null;
            result = JToken.Parse(input).Children()[childNodeName];
            return result;
        }

        public IEnumerable<T> DeserializeFromParentNode<T>(string input, string parentNodeName) where T : class
        {
            IEnumerable<T> result = null;
            result = JTokensToCollection<T>(ExtractFromParentNode(input, parentNodeName));
            return result;
        }
        public IEnumerable<T> DeserializeFromParentChildNode<T>(string input, string parentNodeName, string childNodeName) where T : class
        {
            IEnumerable<T> result = null;
            result = JTokensToCollection<T>(ExtractFromParentChildNode(input, parentNodeName, childNodeName));
            return result;
        }
        public IEnumerable<T> DeserializeFromChildNode<T>(string input, string childNodeName) where T : class
        {
            IEnumerable<T> result = null;
            result = JTokensToCollection<T>(ExtractFromChildNode(input, childNodeName));
            return result;
        }

        public string SerializeObject(object input_, JsonSerializerSettings settings_)
        {
            string result = string.Empty;
            result = JsonConvert.SerializeObject(input_, settings_);
            return result;
        }

        public IEnumerable<T> JTokensToCollection<T>(IEnumerable<JToken> input) where T : class
        {
            IEnumerable<T> result = null;
            result = (from s in input select s.ToObject<T>()).ToList();
            return result;
        }
        public List<string> JTokenToCollection(IEnumerable<JToken> input)
        {
            List<string> result = new List<string>();
            foreach (JToken jt in input.Children())
            {
                result.Add(JsonConvert.SerializeObject(jt));
            }
            return result;
        }
        public string CollectionToStringFormat<T>(IEnumerable<T> list_, JsonSerializerSettings jss = null) where T : class
        {
            string result = null;
            result = JsonConvert.SerializeObject(list_, jss);
            return result;
        }
    }


    //<<-- Out deprecated New Builder Above OrientTokens
    /// <summary>
    /// no any Buisiness logic
    /// divide to query class with inheritance|generic 
    /// to commandtpyes (command|query,batch,function)
    /// </summary>
    public class OrientApiUrlBuilder
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



    /// <summary>
    /// URL scope
    /// </summary>

    /// <summary>
    /// Tokens for Orient API URIs 
    /// Different API types tend to different Http req strategies example: Fucntion/param or: Batch/ + JSON-body
    /// (add types to ItypeToken for plugging-in)
    /// </summary>
    //For token items
    public interface ITypeToken
    {
        string Text { get; set; }
    }
    //Building Item from Token types
    public interface ITextBuilder
    {
        ITypeToken Text { get; }
        ITypeToken FormatPattern { get; }
        List<ITypeToken> Tokens { get; }

        string GetText();
        void SetText(List<ITypeToken> tokens_, ITypeToken FormatPattern_);
    }



    /// <summary>
    ///  Tokens realization for different string concatenations
    /// </summary>
    //Tokens for Orient Comamnd and Authenticate URLs
    public class OrientHost : ITypeToken
    {
        public string Text { get { return "http://msk1-vm-ovisp02"; } set { Text = value; } }
    }
    public class OrientDb : ITypeToken
    {
        public string Text { get { return "news_test3"; } set { Text = value; } }
    }
    public class OrientPort : ITypeToken
    {
        public string Text { get { return "2480"; } set { Text = value; } }
    }
    public class OrientAuthenticateToken : ITypeToken
    {
        public string Text { get { return "connect"; } set { Text = value; } }
    }
    public class OrientCommandToken : ITypeToken
    {
        public string Text { get { return "command"; } set { Text = value; } }
    }
    public class OrientCommandSQLTypeToken : ITypeToken
    {
        public string Text { get { return "sql"; } set { Text = value; } }
    }
    public class OrientFuncionToken : ITypeToken
    {
        public string Text { get { return "Function"; } set { Text = value; } }
    }
    public class OrientBatchToken : ITypeToken
    {
        public string Text { get { return "Batch"; } set { Text = value; } }
    }
    //Tokens for storing Resulted build strings (URLs, commands e.t.c)
    public class TextToken : ITypeToken
    {
        public string Text { get; set; }
    }
    //Auth Orient URL
    public class OrientAuthenticationURLFormat : ITypeToken
    {
        public string Text { get { return @"{0}:{1}/{2}/{3}"; } set { Text = value; } }
    }
    //Command URL part format
    public class OrientCommandURLFormat : ITypeToken
    {
        public string Text { get { return @"{0}:{1}/{2}/{3}/{4}"; } set { Text = value; } }
    }



    /// <summary>
    /// Base class for url tokens concatenation
    /// </summary>
    //TextBuilder realization for Format placeholders for URL concatenation
    public abstract class URLbuilder : ITextBuilder
    {
        public ITypeToken Text { get; private set; }
        public ITypeToken FormatPattern { get; private set; }
        public List<ITypeToken> Tokens { get; private set; }

        //concatenates Tokens from colection with format pattern
        public URLbuilder(List<ITypeToken> tokens_, ITypeToken FormatPattern_)
        {
            this.Text = new TextToken();
            this.FormatPattern = FormatPattern_;
            this.Tokens = tokens_;
            SetText(this.Tokens, this.FormatPattern);
        }
        //cocatenates URLbuilders Token collections from URLbuilders with format pattern
        public URLbuilder(List<ITextBuilder> texts_, ITypeToken FormatPattern_, BuildTypeFormates type_)
        {
            this.Text = new TextToken();
            this.FormatPattern = FormatPattern_;
            this.Tokens = new List<ITypeToken>();

            if (type_ == BuildTypeFormates.FULL)
            {
                this.FormatPattern = FormatPattern_;
                this.Tokens = texts_.SelectMany(s => s.Tokens).ToList();
                SetText(this.Tokens, this.FormatPattern);
            }
            if (type_ == BuildTypeFormates.NESTED)
            {
                List<ITypeToken> str = new List<ITypeToken>();
                foreach (ITextBuilder tb in texts_)
                {
                    //build string
                    tb.SetText(tb.Tokens, tb.FormatPattern);
                    //add tokens to list
                    this.Tokens.AddRange(tb.Tokens);
                    //concatenate formats according to new, nested format
                    str.Add(tb.FormatPattern);
                }

                //add format concatenation 
                //concatenate collection of formats according to format
                this.FormatPattern.Text = string.Format(this.FormatPattern.Text, (from s in str select s.Text).ToArray())
                //recount foramt variables from 0 to max
                this.FormatPattern.Text = FormatStringReArrange(this.FormatPattern.Text);
                //build new string
                SetText(this.Tokens, this.FormatPattern);
            }
        }
        public void SetText(List<ITypeToken> tokens_, ITypeToken FormatPattern_)
        {
            List<string> str = new List<string>();
            foreach (ITypeToken tt in this.Tokens)
            {               
                if (tt != null){str.Add(tt.Text);}
                //else { str.Add(null); }
            }
            this.Text.Text = string.Format(this.FormatPattern.Text,str.ToArray());
        }
        public string GetText()
        {
            return this.Text.Text;
        }

        public enum BuildTypeFormates { FULL, NESTED }

        //recounts format string parameters from 0 for concatenated from several format strings
        public string FormatStringReArrange(string input_)
        {
            string result = string.Empty;
            List<char> input_chars = input_.ToCharArray().ToList();
            int i = 0, i2 = 0, ctr = 0;
            for (i = 0; i < input_chars.Count; i++) {
                i2 = i;
                if (char.IsDigit(input_chars[i])) {
                    while (char.IsDigit(input_chars[i2 + 1])) {
                        i2 += 1;
                    }
                    for (int i3 = i; i3 <= i2; i3++) {
                        input_chars.RemoveAt(i3);
                    }

                    char[] chToInsert = ctr.ToString().ToCharArray();

                    if (chToInsert.Count() > 1){
                  
                        for (int i4 = 0; i4 < chToInsert.Count(); i4++){
                            input_chars.Insert(i, chToInsert[i4]);
                            i += 1;
                        }
                        i -= 1;
                    }
                    else{
                        input_chars.Insert(i, chToInsert[0]);
                    }
                    ctr += 1;
                }
            }

            return result = string.Concat(input_chars);
        }
    }
    //Authentication URL build
    public class OrientAuthenticationURL : URLbuilder
    {
        public OrientAuthenticationURL(List<ITypeToken> tokens_, OrientAuthenticationURLFormat FormatPattern_)
             : base(tokens_, FormatPattern_)
        {

        }
    }
    //Command URL build
    public class OrientCommandURLBuilder : URLbuilder
    {
        public OrientCommandURLBuilder(List<ITypeToken> tokens_, OrientCommandURLFormat FormatPattern_)
            : base(tokens_, FormatPattern_)
        {

        }
        public OrientCommandURLBuilder(List<ITextBuilder> texts_, ITypeToken FormatPattern_, URLbuilder.BuildTypeFormates type_)
          : base(texts_, FormatPattern_, type_)
        {

        }
    }



    /// <summary>
    /// query command scope
    /// </summary>

    /// </summary>
    /// command queries contains prevoius command as first parameter, 
    /// cause WHERE not intended to be used without select
    /// </summary>
    //command query part format
    public class OrientSelectClauseFormat : ITypeToken
    {
        public string Text { get { return @"{0} {1} {2}"; } set { Text = value; } }
    }
    //Command for concatenating select command and where clause
    public class OrientWhereClauseFormat : ITypeToken
    {
        public string Text { get { return @"{0} {1}"; } set { Text = value; } }
    }
    //create vertex command Format
    public class OrientCreateVertexCluaseFormat : ITypeToken
    {
        public string Text { get { return @"{0} {1} {2} {3} {4}"; } set { Text = value; } }
    }
    //delete vertex command Format
    public class OrientDeleteVertexCluaseFormat : ITypeToken
    {
        public string Text { get { return @"{0} {1} {2} {3}"; } set { Text = value; } }
    }

    public class OrientSelectToken : ITypeToken
    {
        public string Text { get { return "Select"; } set { Text = value; } }
    }
    public class OrientFromToken : ITypeToken
    {
        public string Text { get { return "from"; } set { Text = value; } }
    }
    public class OrientWhereToken : ITypeToken
    {
        public string Text { get { return "where"; } set { Text = value; } }
    }

    public class OrientCreateToken : ITypeToken
    {
        public string Text { get { return @"Create"; } set { Text = value; } }
    }
    public class OrientContentToken : ITypeToken
    {
        public string Text{get{ return @"content"; } set{ Text = value; } }
    }
    public class OrientDeleteToken : ITypeToken
    {
        public string Text { get { return @"Delete"; } set { Text = value; } }
    }

    public class OrientVertexToken : ITypeToken
    {
        public string Text { get { return @"Vertex"; } set { Text = value; } }
    }

    public class OrientPersonToken : ITypeToken
    {
        public string Text { get { return "Person"; } set { Text = value; } }
    }
    public class OrientUnitToken : ITypeToken
    {
        public string Text { get { return "Unit"; } set { Text = value; } }
    }


    public class OrientSelectClauseBuilder : URLbuilder
    {
        public OrientSelectClauseBuilder(List<ITypeToken> tokens_, OrientSelectClauseFormat FormatPattern_)
            : base(tokens_, FormatPattern_)
        {

        }
    }
    public class OrientWhereClauseBuilder : URLbuilder
    {
        public OrientWhereClauseBuilder(List<ITypeToken> tokens_, OrientWhereClauseFormat FormatPattern_)
            : base(tokens_, FormatPattern_)
        {

        }
    }
   
    public class OrientCreateClauseBuilder : URLbuilder
    {
        public OrientCreateClauseBuilder(List<ITypeToken> tokens_,ITypeToken format_) 
            : base(tokens_, format_)
        {

        }
    }
    public class OrientDeleteClauseBuilder : URLbuilder
    {
        public OrientDeleteClauseBuilder(List<ITypeToken> tokens_,ITypeToken format_)
            : base(tokens_,format_)
        {

        }
    }



}

