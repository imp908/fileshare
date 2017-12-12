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

using IQueryManagers;
using QueryManagers;
using APItesting;
using OrientRealization;
using PersonUOWs;
using Repos;
using IUOWs;

/// <summary>
/// Deprecated, unused and refactored code mooved here
/// can be deleted without any consequences.
/// </summary>
namespace Trash
{

    public class Check
    {

        public static void JSONManagerIntegrationTests()
        {
            string str1="[\"v1\",\"v2\"]";
            string res1=string.Concat(JsonConvert.DeserializeObject<IEnumerable<string>>(str1));

            string str2="{\"Name\":\"value1\"}";
            string res2=string.Concat(JsonConvert.DeserializeObject(str2));
            string res2_2=string.Concat(JToken.Parse(str2));
            Person res3=JsonConvert.DeserializeObject<Person>(str2);

            string str4="[{\"Name\":\"value1\"},{\"Name\":\"value2\"}]";
            IEnumerable<Person> res4=JsonConvert.DeserializeObject<IEnumerable<Person>>(str4);
            string str4_2=string.Concat(JToken.Parse(str4).Children()["Name"]);

            string str5="{\"result\":[{\"Name\":\"value1\",\"sAMAccountName\":\"acc1\"},{\"Name\":\"value2\",\"sAMAccountName\":\"acc2\"}]}";
            string res5=string.Concat(JToken.Parse(str5)["result"].Children()["Name"]);
            string str6="{\"news\":[{\"Title\":\"value1\",\"Article\":\"value3\"},{\"Title\":\"value2\",\"Article\":\"value4\"}]}";
            string res6=string.Concat(JToken.Parse(str6)["news"].Children()["Title"]);
            string res7=string.Concat(JToken.Parse(str6)["news"].Children()["Article"]);

            string str7 =
    "{\"news\":[{\"Title\":\"value1\",\"Article\":\"value3\"},{\"Title\":\"value2\",\"Article\":\"value4\",\"tags\":[\"value5\",\"value\"]}]}";
            string res8=string.Concat(JToken.Parse(str7)["news"].Children()["tags"]);

            string str8 =
    "{\"news\":[{\"Title\":\"value1\",\"Article\":\"value3\"},{\"Title\":\"value2\",\"Article\":\"value4\",\"tags\":[{\"Name\":\"value7\"},{\"Name\":\"value8\"}]}]}";

            JSONManager jm2=new JSONManager();

            //Read from 2lvl array to string
            IJEnumerable<JToken> personsJT=JToken.Parse(str8)["news"].Children()["tags"];
            List<string> col=jm2.JTokenToCollection(personsJT);
            string resp4=jm2.CollectionToStringFormat<string>(col,
                new JsonSerializerSettings() {NullValueHandling=NullValueHandling.Ignore, Formatting=Formatting.None}
            );

            var a=JToken.Parse(str8).SelectToken("news", false).SelectToken("Title", false);

            /*{"parentNodeName":[{"Name":"value1","Name2":"Value1"},{"Name":"value2","Name2":"value2"}]} - > model {Name {get;set;}, Name2 {get;set;}} */
            //Extract tokens from JSON response parent Node, convert to collection of model objects
            IJEnumerable<JToken> jte=jm2.ExtractFromParentNode(str5, "result");
            //Extract + convert JSON to collection of model objects
            IEnumerable<Person> res9=jm2.DeserializeFromParentNode<Person>(str5, "result");            
            //to string  Selectable -> ignore nulls, no intending
            string resp0=jm2.CollectionToStringFormat<Person>(res9,
                new JsonSerializerSettings() {NullValueHandling=NullValueHandling.Ignore, Formatting=Formatting.None});

            //extract from JSON parent node
            IJEnumerable<JToken> jte2=jm2.ExtractFromParentChildNode(str8, "news", "Title");
            //convert to collection of strings
            IEnumerable<string> res10=jm2.DeserializeFromParentChildNode<string>(str8, "news", "Title");
            //to string  Selectable -> ignore nulls, no intending
            string resp1=jm2.CollectionToStringFormat<string>(res10
                , new JsonSerializerSettings() {NullValueHandling=NullValueHandling.Include, Formatting=Formatting.None});

            //extract from child nodes
            IJEnumerable<JToken> jte3=jm2.ExtractFromChildNode(str4, "Name");
            //to collection
            IEnumerable<string> res11=jm2.DeserializeFromChildNode<string>(str4, "Name");
            //to string  Selectable -> ignore nulls, no intending
            string resp2=jm2.CollectionToStringFormat<string>(res11
                , new JsonSerializerSettings() {NullValueHandling=NullValueHandling.Include, Formatting=Formatting.None});

            //Back to object 
            IEnumerable<Person> persons=JsonConvert.DeserializeObject<IEnumerable<Person>>(resp0);
       }
       
        public static void PersonApiCheck()
        {

            OrientWebManager wm=new OrientWebManager();

            APItester_sngltn ut=new APItester_sngltn();
            WebResponseReader wr=new WebResponseReader();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json"))
            {
                string res=File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json");
                APItester_sngltn.TestCases=JsonConvert.DeserializeObject<List<APItester_sngltn>>(res);
           }
            else
            {
                APItester_sngltn.TestCases.Add(new APItester_sngltn()
                {
                    URI="http://msk1-vm-ovisp01:8083/api/Person/GetManager/DegterevaSV"
                    ,
                    Expected="[\"filimonovats\",\"kvv\"]"
                    ,
                    OK=false
               });
                APItester_sngltn.TestCases.Add(new APItester_sngltn()
                {
                    URI="http://msk1-vm-ovisp01:8083/api/Person/GetColleges/lobanovamg"
                   ,
                    Expected="[\"stalmakovsm\",\"Bagirovaev\",\"kotovaen\",\"iku\",\"SergiecIG\",\"a.vagin\",\"lobanovamg\",\"Tikhomirovaa\",\"popovalb\"]"
                   ,
                    OK=false
               });
           }


            foreach (APItester_sngltn tc in APItester_sngltn.TestCases)
            {
                try
                {
                    var resp=wm.GetResponse(tc.URI, "GET");
                    string rR=wr.ReadResponse(resp);

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

            string jStr=JsonConvert.SerializeObject(APItester_sngltn.TestCases, Formatting.Indented);
            string dir=AppDomain.CurrentDomain.BaseDirectory;
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json", jStr);

       }
        //>>make trully async
        public static void PersonApiCheckAsync()
        {

            OrientWebManager wm=new OrientWebManager();

            APItester_sngltn ut=new APItester_sngltn();
            WebResponseReader wr=new WebResponseReader();

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json"))
            {
                string res=File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json");
                APItester_sngltn.TestCases=JsonConvert.DeserializeObject<List<APItester_sngltn>>(res);
           }
            else
            {
                APItester_sngltn.TestCases.Add(new APItester_sngltn()
                {
                    URI="http://msk1-vm-ovisp01:8083/api/Person/GetManager/DegterevaSV"
                    ,
                    Expected="[\"filimonovats\",\"kvv\"]"
                    ,
                    OK=false
               });
                APItester_sngltn.TestCases.Add(new APItester_sngltn()
                {
                    URI="http://msk1-vm-ovisp01:8083/api/Person/GetColleges/lobanovamg"
                   ,
                    Expected="[\"stalmakovsm\",\"Bagirovaev\",\"kotovaen\",\"iku\",\"SergiecIG\",\"a.vagin\",\"lobanovamg\",\"Tikhomirovaa\",\"popovalb\"]"
                   ,
                    OK=false
               });
           }


            foreach (APItester_sngltn tc in APItester_sngltn.TestCases)
            {
                try
                {
                    Task<HttpWebResponse> resp=wm.GetResponseAsync(tc.URI, "GET");

                    string rR=wr.ReadResponse(resp.Result);

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

            string jStr=JsonConvert.SerializeObject(APItester_sngltn.TestCases, Formatting.Indented);
            string dir=AppDomain.CurrentDomain.BaseDirectory;
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\Res.json", jStr);

       }

        
        public static void JSONcheck()
        {
            JSONmanager jm=new JSONmanager();

            string input="{\"result\":[{\"@type\":\"d\",\"@rid\":\"#-2:0\",\"@version\":0,\"Name\":\"kapkaev\"},{\"@type\":\"d\",\"@rid\":\"#-2:1\",\"@version\":0,\"Name\":\"kokuevol\"}]}";
            string input2="[{\"@type\":\"d\",\"@rid\":\"#-2:13\",\"@version\":0,\"Name\":[\"tishakovoi\"]}]";
            string input3="[{\"@type\":\"d\",\"@rid\":\"#-2:13\",\"@version\":0,\"Name\":[\"tishakovoi\"]},{\"@type\":\"d\",\"@rid\":\"#-2:13\",\"@version\":0,\"Name\":[\"kapkaev\"]}]";

            string rk=@"result";
            string nk=@"Name";

            var des1=JsonConvert.DeserializeObject(input);
            var des2=JsonConvert.DeserializeObject(input2);
            var des3=JsonConvert.DeserializeObject(input3);

            JToken jt1=null, jt2=null, jt3=null;
         
            JToken r1, r0=null;
            IEnumerable<JToken> r2=null, r3=null;

            try
            {
                jt1=JsonConvert.DeserializeObject<JToken>(input);
           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}
            try
            {
                jt2=JsonConvert.DeserializeObject<JToken>(input2);
           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}
            try
            {
                jt3=JsonConvert.DeserializeObject<JToken>(input3);
           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}

            //Json has parent node
            try {JToken e1=jt1[rk];} catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}
            try {IEnumerable<JToken> e1=jt1[rk].Children();} catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}
            try {IEnumerable<JToken> e1=jt1[rk].Children()[nk];} catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}

            //Json has parent as collection

            //not OK
            try {JToken e1=jt2[rk];} catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}
            try {JToken e1=jt2[nk];} catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}

            //OK
            try {IEnumerable<JToken> e2=jt2.Children();} catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}
            try {IEnumerable<JToken> e3=jt2.Children()[nk];} catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}

            //try {IEnumerable<JToken> e4=jt3.Children()[nk];}catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}

            try
            {
                r1=JToken.Parse(input);
                r0=JToken.Parse(input)[rk];
                r2=JToken.Parse(input2).Children()[nk];
                r3=JToken.Parse(input3).Children()[nk];
           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}

            try
            {
                IEnumerable<Person> z=jm.DeserializeFromNode<Person>(input, "result");
                IEnumerable<JToken> a=jm.ExtractTokens(input3, "Name");

                var r_0=JsonConvert.SerializeObject(a);
                var r_1=JsonConvert.SerializeObject(z);

                string c=jm.CollectionToStringFormat<JToken>(a, null);

                string r5=jm.CollectionToStringFormat<Person>(z, new JsonSerializerSettings() {NullValueHandling=NullValueHandling.Ignore});
                string r6=jm.CollectionToStringFormat<JToken>(a, null);

                var t2=JsonConvert.DeserializeObject(c);
                var t3=JsonConvert.DeserializeObject(r5);
                var t4=JsonConvert.DeserializeObject(r6);
           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e);}


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
                WebRequest AuthRequest=WebRequest.Create(url);
                AuthRequest.Credentials=new NetworkCredential(login, password);
                AuthRequest.Method="GET";
                AuthRequest.ContentType="application/json; charset=utf-8";
                WebResponse response=AuthRequest.GetResponse();
                OSESSIONID=response.Headers.Get("Set-Cookie");
                int status=(int)((HttpWebResponse)response).StatusCode;
                response.Close();
                return status;
           }

            public string Get(string url_)
            {
                WebRequest wr=WebRequest.Create(url_);
                wr.Method="GET";
                wr.ContentType="application/json; charset=utf-8";

                using (HttpWebResponse response=(HttpWebResponse)wr.GetResponse())
                {
                    using (StreamReader reader=new StreamReader(response.GetResponseStream()))
                    {
                        var objText=reader.ReadToEnd();
                        var objClasses=JsonConvert.DeserializeObject(objText);
                        var jsonResult=objClasses.ToString();

                        return jsonResult;
                   }
               }
           }

            public string GetAuthProx(string url_)
            {
                WebRequest wr=WebRequest.Create(url_);
                wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
                wr.Method="GET";
                wr.ContentType="application/json; charset=utf-8";

                using (var response=(HttpWebResponse)wr.GetResponse())
                {
                    using (var reader=new StreamReader(response.GetResponseStream()))
                    {
                        var objText=reader.ReadToEnd();
                        var objClasses=JsonConvert.DeserializeObject(objText);
                        var jsonResult=objClasses.ToString();

                        return jsonResult;
                   }
               }
           }
            public string POSTAuthProx(string url_)
            {
                WebRequest wr=WebRequest.Create(url_);
                wr.Headers.Add(HttpRequestHeader.Cookie, OSESSIONID);
                wr.Method="POST";
                wr.ContentType="application/json; charset=utf-8";

                using (var response=(HttpWebResponse)wr.GetResponse())
                {
                    using (var reader=new StreamReader(response.GetResponseStream()))
                    {
                        var objText=reader.ReadToEnd();
                        var objClasses=JsonConvert.DeserializeObject(objText);
                        var jsonResult=objClasses.ToString();

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
            string selectCommand=String.Empty;
            string functionCommand=String.Empty;

            public string SelectCommandSet(string className_, string filter_)
            {
                this.selectCommand=SelectClause(className_, WhereClause(filter_));
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

            string SelectClause(string classname_, string whereClause=null)
            {
                return String.Format(@"select from {0} {1}", classname_, whereClause);
           }
            string WhereClause(string whereClause_)
            {
                return String.Format(@"where {0}", whereClause_);
           }
            public string FucntionSet(string Fucntion_, string params_)
            {
                this.functionCommand=String.Format(@"{0}/{1}", Fucntion_, params_);
                return functionCommand;
           }
            public string FunctionCommandGet()
            {
                return functionCommand;
           }

       }
        


        //Token managers check
        //check select cluase
       
        //check insert clause
        public static void InsertClausesCheck()
        {
            Person per=new Person() {Name="0", GUID="0", Changed=DateTime.Now, Created=DateTime.Now};
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
            CommandBuilder cb=new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), CreateTokens, cf);
            string CreateCommand=cb.Text.Text;
       }
        //check delete clause
        public static void DeleteClauseCheck()
        {

            List<ITypeToken> wT =
                new List<ITypeToken>() {new OrientWhereToken(), new TextToken() {Text="\"Name\"=\"0\""}};
            OrientWhereClauseFormat cf =
                new OrientWhereClauseFormat();    
            CommandBuilder cb=new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), wT, cf);

            //<<ad where clause builder
            List <ITypeToken> dt =
                new List<ITypeToken>(){new OrientDeleteToken(), new OrientVertexToken(), new OrientPersonToken()
                , new TextToken(){Text="where Name=0"}};
            OrientDeleteVertexCluaseFormat df
               =new OrientDeleteVertexCluaseFormat();
            CommandBuilder dlb=new CommandBuilder(new TokenMiniFactory(), new FormatFactory(), dt, df);
        
            string deleteClause=dlb.Text.Text;
       }
       

        public static void ManagersCheck()
        {

            string url=@"http://10.31.14.76/cleverence_ui/hs/IntraService/location/full";

            OrientWebManager owm=new OrientWebManager();
            JSONmanager jm=new JSONmanager();
            WebResponseReader wr=new WebResponseReader();

            //sample JSON [{},{}] http get from url

            string jres=wr.ReadResponse(owm.GetResponse(url, "GET"));

            //authenticate Orient
            owm.Authenticate(@"http://msk1-vm-ovisp02:2480/connect/news_test3", new NetworkCredential("root", "I9grekVmk5g"));

            //authenticated htp response from command
            url=@"http://msk1-vm-ovisp02:2480/command/news_test3/sql/select from Person";
            jres=wr.ReadResponse(owm.GetResponse(url, "GET"));
            //в коллекцию объектов из модели {node:[{},{}]} -> List<model>({}) для работы в коде
            IEnumerable<Person> persons=jm.DeserializeFromNode<Person>(jres, @"result");
            //в JSON строку List<{}> -> [{},{}] для передачи в API
            string str=jm.CollectionToStringFormat<Person>(persons, null);

            //из функции
            url=@"http://msk1-vm-ovisp02:2480/function/news_test3/SearchByLastNameTst/сав";
            jres=wr.ReadResponse(owm.GetResponse(url, "GET"));
            //{node:[{},{}]} -> List<model>({}) 
            persons=jm.DeserializeFromNode<Person>(jres, @"result");
            str=jm.CollectionToStringFormat<Person>(persons, null);

            //из комманды другой класс
            //authenticated htp response from command
            url=@"http://msk1-vm-ovisp02:2480/command/news_test3/sql/select from Unit";
            jres=wr.ReadResponse(owm.GetResponse(url, "GET"));
            //{node:[{},{}]} -> List<model>({})
            IEnumerable<Unit> units=jm.DeserializeFromNode<Unit>(jres, @"result");
            //в строку [{"name":"a"},..,{"name":"b"}]
            str=jm.CollectionToStringFormat<Unit>(units, null);
            //в строку ["a",..,"b"]
            str=jm.CollectionToStringFormat<JToken>(jm.ExtractTokens(jres, "result", "Name"), null);

       }
        public static void BuilderCheck()
        {

            string login="root";
            string password="I9grekVmk5g";
            string authUrl=@"http://msk1-vm-ovisp02:2480/connect/news_test3";
            string commandUrl=@"http://msk1-vm-ovisp02:2480/command/news_test3/sql/";
            string functionUrl=@"http://msk1-vm-ovisp02:2480/function/news_test3/";

            _OrientApiUrlBuilder ob=new _OrientApiUrlBuilder();

            //<--Out
            //HTTPmanager hm=new HTTPmanager();

            //-->In
            OrientWebManager owm=new OrientWebManager();
            NetworkCredential nc=new NetworkCredential(login, password);
            WebResponseReader wrr=new WebResponseReader();

            JSONmanager jm=new JSONmanager();


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
            //string jsonStringResponse=hm.GetAuthProx(@"http://msk1-vm-ovisp02:2480/command/news_test3/sql/" + ob.SelectCommandGet());
            //new
            WebResponse wr=owm.GetResponse(commandUrl + ob.SelectCommandGet(), "GET");
            string responseRaw=wrr.ReadResponse(wr);
            //десериализуем в модель, проксирование управляется полями POCO класса модели и атрибутами
            IEnumerable<Person> persons=jm.DeserializeFromNode<Person>(responseRaw, @"result");


            //Аналогично для Unit
            //задаем имя класса и фильтр where
            ob.SelectCommandSet("Unit", @"1=1");
            responseRaw=wrr.ReadResponse(owm.GetResponse(commandUrl + ob.SelectCommandGet(), "GET"));
            IEnumerable<Unit> units=jm.DeserializeFromNode<Unit>(responseRaw, @"result");


            //Аналогично для функции
            //имя функции и параметр
            //Так как функция возвразает только одно поле, коллекция заполнится объектами со значением только в 1 поле
            //остальные null
            ob.FucntionSet("SearchByLastNameTst", @"сав");
            responseRaw=wrr.ReadResponse(owm.GetResponse(functionUrl + ob.FunctionCommandGet(), "GET"));
            IEnumerable<Person> persons2=jm.DeserializeFromNode<Person>(responseRaw, @"result");
            //Кручу-верчу для получения "чистых" коллекицй, без полей с null.
            //(когда возращается только часть полей, null values игнорятся, без изменения модели и залезания в строку ответа)
            string jSer=jm.CollectionToStringFormat<Person>(persons2, new JsonSerializerSettings() {NullValueHandling=NullValueHandling.Ignore});
            //Промежуточная коллекция с экстракцией нужного токена, для понимания типа
            IJEnumerable<JToken> jt=jm.ExtractTokens(jSer, "LastName");
            //Формирование "чистой" строки из коллекции токенов
            string res0=jm.CollectionToStringFormat<IJEnumerable<JToken>>(jt, null);


            //Аналогично, без иключения null, oneline          
            res0=jm.CollectionToStringFormat<IJEnumerable<JToken>>(jm.ExtractTokens(
                jm.CollectionToStringFormat<Person>(
                    jm.DeserializeFromNode<Person>(responseRaw, @"result")
                , null)
                , "LastName"), null);

       }
        public static void HTTPcheck()
        {

            WebResponseReader reader=new WebResponseReader();

            //read basic GET response
            string url=@"http://msk1-vm-ovisp01:8083/api/Person/GetCollegesLower/bs";
            OrientWebManager hm=new OrientWebManager();
            WebResponse response=hm.GetResponse(url, "GET");
            string sampleResult=reader.ReadResponse(response);


            //Authentication
            string authUrl=@"http://msk1-vm-ovisp02:2480/connect/news_test3";
            NetworkCredential nc=new NetworkCredential("root", "I9grekVmk5g");

            //read Orient fucntion GET with authentication
            url=@"http://msk1-vm-ovisp02:2480/function/news_test3/GetCollegesLowerByAccount/bs";
            OrientWebManager orm=new OrientWebManager();
            orm.Authenticate(authUrl, nc);
            response=orm.GetResponse(url, "GET");
            string resultOrient=reader.ReadResponse(response);


            //deserializing string request
            JSONmanager mng=new JSONmanager();
            IEnumerable<string> a=mng.DeseializeSample<string>(sampleResult);
            IEnumerable<string> b=mng.DeserializeFromNode<string>(resultOrient, "result", "Name");


            //change equality check
            //they are equal
            bool res=a.Equals(b);

       }       


   }

    public class RepoCheck
    {

        JSONManager jm;
        OrientTokenBuilder ta;
        TypeConverter tc;
        CommandBuilder ocb;
        OrientWebManager wm;
        WebResponseReader wr;

        Repo repo;
        Person p;
        Unit u;
        SubUnit s;

        MainAssignment m;
        List<string> lp, lu;

        UserSettings us;
        CommonSettings cs;
        string guid_;
        IPersonUOW pUOW;

        public RepoCheck()
        {
            pUOW=new PersonUOWold();
            jm=new JSONManager();
            ta=new OrientTokenBuilder();
            tc=new TypeConverter();
            ocb=new OrientCommandBuilder(new TokenMiniFactory(), new FormatFactory());
            wm=new OrientWebManager();
            wr=new WebResponseReader();

            us=new UserSettings() {showBirthday=true};
            cs=new CommonSettings();

            repo=new Repo(jm, ta, tc, ocb, wm, wr);

            s=new SubUnit();

            p =
new Person() {Name="0", GUID="0", Changed=new DateTime(2017, 01, 01, 00, 00, 00), Created=new DateTime(2017, 01, 01, 00, 00, 00)};

            u =
new Unit() {Name="0", GUID="0", Changed=new DateTime(2017, 01, 01, 00, 00, 00), Created=new DateTime(2017, 01, 01, 00, 00, 00)};

            m =
new MainAssignment() {Name="0", GUID="0", Changed=new DateTime(2017, 01, 01, 00, 00, 00), Created=new DateTime(2017, 01, 01, 00, 00, 00)};

            lp=new List<string>();
            lu=new List<string>();

            guid_="ba124b8e-9857-11e7-8119-005056813668";

       }


        public void GO()
        {
            DbCreateDeleteCheck();
            TrackBirthdaysPtP();
            TrackBirthdaysOneToAll();
            AddCheck();
            DeleteCheck();          
            BirthdayConditionAdd();
       }
        public void AddCheck()
        {
            int lim=500;

            for (int i=0; i <= lim; i++)
            {
                lp.Add(jm.DeserializeFromParentNode<Person>(repo.Add(p), new RESULT().Text).Select(s => s.id.Replace(@"#", "")).FirstOrDefault());
                lu.Add(jm.DeserializeFromParentNode<Unit>(repo.Add(u), new RESULT().Text).Select(s => s.id.Replace(@"#", "")).FirstOrDefault());

           }
            for (int i=0; i <= lim / 2; i++)
            {
                repo.Add(m, new TextToken() {Text=lu[i]}, new TextToken() {Text=lp[i + 1]});
           }
            for (int i=0; i <= lim / 2; i++)
            {
                repo.Add(s, new TextToken() {Text=lu[i]}, new TextToken() {Text=lu[i + 1]});
           }

       }
        public void DeleteCheck()
        {
            string str;
            str=repo.Delete(typeof(Person), new TextToken() {Text=@"Name =0"});
            str=repo.Delete(typeof(Unit), new TextToken() {Text=@"Name =0"});
            str=repo.Delete(typeof(MainAssignment), new TextToken() {Text=@"Name =0"});
            str=repo.Delete(typeof(SubUnit), new TextToken() {Text=@"Name =0"});
       }
      
        public void BirthdayConditionAdd()
        {

            List<string> persIds=new List<string>();
            List<string> edgeIds=new List<string>();

            persIds.AddRange(
                jm.DeserializeFromParentNode<Person>(repo.Select(typeof(Person), new TextToken() {Text="1=1 and outE(\"CommonSettings\").inv(\"UserSettings\").showBirthday[0] is null"}), new RESULT().Text).Select(s => s.id.Replace(@"#", ""))
            );

            for (int i=0; i < persIds.Count(); i++)
            {
                string id=jm.DeserializeFromParentNode<UserSettings>(repo.Add(us), new RESULT().Text).Select(s => s.id.Replace(@"#", "")).FirstOrDefault();

                repo.Add(cs, new TextToken() {Text=persIds[i]}, new TextToken() {Text=id});
           }

            repo.Delete(typeof(UserSettings), new TextToken() {Text=@"1 =1"});
            repo.Delete(typeof(CommonSettings), new TextToken() {Text=@"1 =1"});

       }
        public void DbCreateDeleteCheck()
        {

            repo.Add(new TextToken() {Text="testdb"}, new DELETE());
            repo.Add(new TextToken() {Text="testdb"}, new POST());

       }
        public void TrackBirthdaysOneToAll()
        {
            repo.changeAuthCredentials(
                ConfigurationManager.AppSettings["ParentLogin"]
                , ConfigurationManager.AppSettings["ParentPassword"]
                );

            TrackBirthdays tb=new TrackBirthdays();

            Person fromPerson=pUOW.GetObjByGUID(guid_).FirstOrDefault();
            List<Person> personsTo=pUOW.GetAll().ToList();
            List<string> ids=new List<string>() {};

            foreach (Person pt in personsTo)
            {
                ids.Add(repo.Add(tb, fromPerson, pt));
           }

            repo.Delete(typeof(TrackBirthdays), new TextToken() {Text="1=1"});

       }
        public void TrackBirthdaysPtP()
        {
            repo.changeAuthCredentials(
                ConfigurationManager.AppSettings["ParentLogin"]
                , ConfigurationManager.AppSettings["ParentPassword"]
                );

            TrackBirthdays tb=new TrackBirthdays();
            List<Person> personsTo=pUOW.GetAll().Take(3).ToList();
            string personsfrom=null;

            List<string> ids=new List<string>() {};

            foreach (Person pf in personsTo)
            {
                foreach (Person pt in personsTo)
                {
                    ids.Add(repo.Add(tb, pf, pt));
               }
           }

            personsfrom=pUOW.GetTrackedBirthday(personsTo.FirstOrDefault().GUID);

            repo.Delete(typeof(TrackBirthdays), new TextToken() {Text="1=1"});

       }

   }

    //DRIVER scope
    public static class OrientNumToCLRType
    {
        public static string ValuetoString(int value_)
        {
            string result=null;
            switch (value_)
            {

                case 3:
                    result=@"LONG";
                    break;
                case 6:
                    result=@"DATETIME";
                    break;
                case 7:
                    result=@"STRING";
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
            IEnumerable<T> res=null;
            try
            {
                res=JsonConvert.DeserializeObject<IEnumerable<T>>(resp);
           }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}
            return res;
       }
        //For JSON structure {NodeName:[{a:1,..,c:1},{a:10,..,c:10}]}
        public IEnumerable<T> DeserializeFromNode<T>(string jInput, string Node) where T : class
        {
            IEnumerable<T> result=null;
            IEnumerable<JToken> res=JToken.Parse(jInput)[Node].Children();
            result=CollectionConvert<T>(res);
            return result;
       }
        //For parsing not to model but to String for JSON structure {NodeName:[{a:1,..,c:1},{a:10,..,c:10}]}
        //where one item from collection can be parsed
        public IEnumerable<T> DeserializeFromNode<T>(string jInput, string Node, string field) where T : class
        {
            IEnumerable<T> result=null;
            IEnumerable<JToken> res=JObject.Parse(jInput)[Node].Children()[field];
            result=CollectionConvert<T>(res);
            return result;
       }

        //Extracts collection of tokens [{"Tk1":"A"},..,{"Tk31":"Z"}] -> IJEnumerable<JTokens> ["A",..,"Z"]
        public IJEnumerable<JToken> ExtractTokens(string jInput, string field)
        {
            IJEnumerable<JToken> result=null;
            result=JToken.Parse(jInput).Children()[field];
            return result;
       }
        public IJEnumerable<JToken> ExtractTokenChildren(string jInput, string field)
        {
            IJEnumerable<JToken> result=null;
            result=JToken.Parse(jInput).Children()[field].Children();
            return result;
       }
        public IJEnumerable<JToken> ExtractTokens(string jInput, string Node, string field)
        {
            IJEnumerable<JToken> result=null;
            result=JObject.Parse(jInput)[Node].Children()[field];
            return result;
       }

        public IEnumerable<T> CollectionConvert<T>(IEnumerable<JToken> input) where T : class
        {
            IEnumerable<T> result=null;
            result=(from s in input select s.ToObject<T>()).ToList();
            return result;
       }
        public string CollectionToStringFormat<T>(IEnumerable<T> list_, JsonSerializerSettings jss=null) where T : class
        {
            string result=null;
            result=JsonConvert.SerializeObject(list_, jss);
            return result;
       }

   }

    /// <summary>
    /// Char arrays rearrange (arrays gaps error)
    /// </summary>
    public static class FormatRearrange
    {      

        public static void StringsCheck()
        {

            //string r1=Rearrange("{0}{1} {3}"); //OK "{0}{1} {2}"
            //string r1=Rearrange("{0}{2} {3}"); // OK "{0}{1} {2}"
            //string r1=Rearrange("{0}{0} "); // OK "{0}{1}"
            //string r1=Rearrange("{0} {0} {0}"); //OK "{0} {1} {2}"
            //string r1=Rearrange("{0} {3} {2}{5}"); //OK "{0} {1} {2}{3}"            
            //string r1=Rearrange("{1}"); //OK "{0}"
            //string r1=Rearrange("{2} {7}:{0} / {3}"); //OK "{0} {1}:{2} / {3}"
            //string r1=Rearrange("{10}{10}{10}"); //OK "{0}{1}{2}"
            //string r1=Rearrange("{10}{0}{2}");
            //string r1=Rearrange("{0}:{1}/{2}/{3}/{4}/{0} {1} {2} {3} {4}");
            //string r1=Rearrange("{0}:{1}/{2}/{3}/{4}/{0} {1} {2} {3} {0} {1}");

            string rVar=Rearrange("{0}{1} {2} {3}{4}/{5}:{6}{7} {8}"); //OK "{0}{1} {2} {3}{4}/{5}:{6}{7} {8}"
            string rZeroToTen=Rearrange("{0}:{1}/{2}/{3}/{4}/{0} {1} {2} {3} {0} {1}");
            string r40=Rearrange("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15} {16} {17} {18} {19} {20} {21} {22} {23} {24} {25} {26} {27} {28} {29} {30} {31} {32} {33} {34} {35} {36} {37} {38} {39} {40}");

       }

        public static string Rearrange(string input_)
        {

            string result=input_;
            char[] chr=input_.ToCharArray();
            int lng=chr.Length;
            
            char[] prevDigit=null;
            char[] currDigit=null;
            char[] insDigit=null;

            int i2=0;

            for (int i=0; i < lng; i++)
            {
                i2=i;
                if (char.IsDigit(chr[i2]))
                {
                    if (i2 + 1 < lng)
                    {
                        while (char.IsDigit(chr[i2 + 1]))
                        {
                            i2++;
                       }
                   }

                    if (prevDigit==null)
                    {
                        currDigit=ChArrFill(i, i2, chr);
                        if (charArrToInteger(currDigit) != 0)
                        {
                            insDigit=intToCharArr(0);
                            prevDigit=insDigit;
                            char[] chrN=InsertDigitInPosition(insDigit, chr, i, currDigit.Length);
                            result=new string(chrN);
                            chr=chrN;
                            lng=chr.Length;
                       }
                        else {prevDigit=currDigit;}

                   }
                    else
                    {
                        currDigit=ChArrFill(i, i2, chr);
                        if (!check(currDigit, prevDigit))
                        {
                            insDigit=intToCharArr(charArrToInteger(prevDigit) + 1);
                            char[] chrN=InsertDigitInPosition(insDigit, chr, i, currDigit.Length);
                            prevDigit=insDigit;

                            /*
                            char[] chrN=new char[chr.Length + currDigit.Length - prevDigit.Length];

                            for (int i4=0; i4 < i; i4++)
                            {
                                chrN[i4]=chr[i4];
                           }
                            for (int i4=i; i4 <= i2; i4++)
                            {
                                chrN[i4]=currDigit[i4-i];
                           }
                            for (int i4=i2+1; i4 < lng; i4++)
                            {
                                chrN[i4]=chr[i4];
                           }
                            */

                            result=new string(chrN);
                            chr=chrN;
                            lng=chr.Length;
                       }
                        else {prevDigit=currDigit;}
                   }
                    if (prevDigit != null)
                    {
                        i += prevDigit.Length - 1;
                   }
               }
              
           }
        
            return result;
       }     

        static char[] InsertDigitInPosition(char[] insDigit_,char[] fromArr_,int pos_, int curDigitLen_)
        {
            char[] toArr=new char[fromArr_.Length+insDigit_.Length-curDigitLen_];
            int arrGap= toArr.Length-fromArr_.Length;
            //before position copy
            for (int i4=0; i4 < pos_; i4++){
                toArr[i4]=fromArr_[i4];}
            //position copy num length
            for (int i4=pos_; i4 <= (pos_+insDigit_.Length-1); i4++){
                toArr[i4]=insDigit_[i4 - pos_];}
            //after position copy from num length + arrrays gap
            for (int i4=(pos_+insDigit_.Length-arrGap); i4< fromArr_.Length; i4++){
                toArr[i4+arrGap]=fromArr_[i4];}
            return toArr;
       }
        static int charArrToInteger(char[] arr_)
        {
            int res=0;
            int i=1;
            for (int i2=arr_.Length - 1; i2 >= 0; i2--)
            {
                res += (int)(char.GetNumericValue(arr_[i2]) * i);
                i *= 10;
           }
            return res;
       }
        static char[] intToCharArr(int i_)
        {
            return i_.ToString().ToCharArray();
       }
        static char[] intRecount(char[] currDig_, char[] prevDigit_)
        {
            if (charArrToInteger(currDig_) == charArrToInteger(prevDigit_) + 1)
            {
                return currDig_;
           }
            else
            {
                return intToCharArr(charArrToInteger(prevDigit_) + 1);
           }
       }
        static bool check(char[] currDig_, char[] prevDigit_)
        {
            if (charArrToInteger(currDig_) == charArrToInteger(prevDigit_) + 1)
            {
                return true;
           }
            else
            {
                return false;
           }
       }
        static char[] ChArrFill(int i_, int i2_, char[] chFrom_)
        {
            char[] chTo_=new char[(i2_ - i_) + 1];

            for (int i3_=0; i3_ <= (i2_ - i_); i3_++)
            {
                chTo_[i3_]=chFrom_[i_ + i3_];
           }
            return chTo_;
       }
        
   }    

}