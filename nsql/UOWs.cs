using System;
using System.Collections.Generic;
using System.Linq;

using IUOWs;
using IQueryManagers;
using IWebManagers;
using POCO;
using OrientRealization;
using QueryManagers;
using IRepos;
using JsonManagers;
using IJsonManagers;
using WebManagers;
using Repos;


using Newtonsoft.Json;
using System.Configuration;
using System.Threading.Tasks;


using System.Net;
using System.Text;


namespace PersonUOWs
{
    
    public class PersonUOWold : IPersonUOW
    {
        IRepo_v1 _repo;
        OreintNewsTokenBuilder ob=new OreintNewsTokenBuilder();
        ITypeTokenConverter _typeConverter;
        ICommandBuilder _CommandBuilder;
        IJsonManger _jsonManager;
        ITokenBuilder _tokenAggregator;
        IWebManager wm;
        IResponseReader wr;

        public PersonUOWold()
        {
            _jsonManager=new JSONManager();
            _tokenAggregator=new OrientTokenBuilder();
            _typeConverter=new TypeConverter();
            _CommandBuilder=new OrientCommandBuilder(new TokenMiniFactory(), new FormatFactory());
            wm=new OrientWebManager();
            wr=new WebResponseReader();

            _repo=new Repo(_jsonManager, _tokenAggregator, _typeConverter, _CommandBuilder, wm, wr);
       }


        public IEnumerable<Person> GetObjByGUID(string GUID)
        {
            IEnumerable<Person> result=null;
            TextToken condition_=new TextToken() {Text="1=1 and GUID ='" + GUID + "'"};
            try
            {
                result=_repo.Select<Person>(typeof(Person), condition_);
            }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

            return result;
        }
        public string GetByGUID(string GUID)
        {
            string result=string.Empty;
            IEnumerable<Person> persons=null;
            TextToken condition_=new TextToken() {Text="1=1 and GUID ='" + GUID + "'"};
            try
            {
                persons=_repo.Select<Person>(typeof(Person), condition_);
                result=_jsonManager.SerializeObject(persons);
            }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

            return result;
        }
        public IEnumerable<Person> GetAll()
        {

            IEnumerable<Person> result=null;
            TextToken condition_=new TextToken() {Text="1=1"};
            try
            {
                result=_repo.Select<Person>(typeof(Person), condition_);

            }
            catch (Exception e) {System.Diagnostics.Trace.WriteLine(e.Message);}

            return result;
        }

        public string GetTrackedBirthday(string GUID)
        {
            string result=string.Empty;
            IEnumerable<Person> persons=null;
            TextToken condition_=new TextToken() {Text="1=1 and GUID ='" + GUID + "'"};
            List<ITypeToken> tokens=ob.outEinVExp(new OrientSelectToken(),
                _typeConverter.Get(typeof(Person)), _typeConverter.Get(typeof(TrackBirthdays)), condition_);

            _CommandBuilder.AddTokens(tokens);
            _CommandBuilder.AddFormat(new OrientOutEinVFormat() {});
            string command=_CommandBuilder.Build().GetText();

            persons=_repo.Select<Person>(command);
            result=_jsonManager.SerializeObject(persons);
            return result;
        }

        public string AddTrackBirthday(E edge_, string guidFrom, string guidTo)
        {
            string result=null;
            Person from=GetObjByGUID(guidFrom).FirstOrDefault();
            Person to=GetObjByGUID(guidTo).FirstOrDefault();

            if (from != null && to != null)
            {
                result=_repo.Add(edge_, from, to);
           }
            return result;
        }
        public string DeleteTrackedBirthday(E edge_, string guidFrom, string guidTo)
        {
            string result=null;
            Person from=GetObjByGUID(guidFrom).FirstOrDefault();
            Person to=GetObjByGUID(guidTo).FirstOrDefault();

            List<ITypeToken> condTokens_=ob.outVinVcnd(typeof(Person), new TextToken() {Text="GUID"},
                new TextToken() {Text=from.GUID}, new TextToken() {Text=to.GUID});

            _CommandBuilder.AddTokens(condTokens_);
            _CommandBuilder.AddFormat(new OrientOutVinVFormat() {});
            string command=_CommandBuilder.Build().GetText();
      

            if (from != null && to != null)
            {
                result=_repo.Delete(edge_.GetType(), new TextToken() {Text=command});
           }
            return result;
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
        List<Holiday> holidays=null;
        List<Vacation> vacations=null;
        IEnumerable<GraphRead> graphs=null;
        IEnumerable<GUIDPOCO> guidpocos=null;

        string holidayCommand, vacationCommand, graphCommand,
             holidaysResp=string.Empty, vacationsResp=string.Empty, graphResp=string.Empty;

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

            if (guidpocos==null)
            {
                guidpocos=_jsonManager.DeserializeFromParentNode<GUIDPOCO>(holidaysResp);
            }
            if (holidays==null)
            {

                IEnumerable<List<AdinTce.Holiday>> hl = _jsonManager.DeserializeFromParentChildren<List<Holiday>>(holidaysResp, "Holidays");
                holidays = new List<Holiday>();
                foreach (List<Holiday> lt_ in hl)
                {                   
                    holidays.AddRange(lt_);                                      
                }
                
            }
            if (vacations == null)
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

            if (holidaysResp != null && holidaysResp != string.Empty)
            {
                AdpCheck();
                try
                {
                    adp.holidays=holidays.ToList();
                }
                catch (Exception e) {}
            }

            if (vacationsResp != null && vacationsResp != string.Empty)
            {
                AdpCheck();
                try
                {
                    adp.vacations=vacations.ToList();
                }
                catch (Exception e) {}
            }

            if (graphResp != null && graphResp != string.Empty)
            {

                AdpCheck();
                try
                {
                    graphs=_jsonManager.DeserializeFromParentChildren<GraphRead>(graphResp, "Holidays");
                    adp.Graphs=GrapthReadToWriteDateCheck(graphs.ToList());
                }
                catch (Exception e) {}
            }
        }
        public List<GraphWrite> GrapthReadToWriteDateCheck(List<GraphRead> ghl_)
        {
            List<GraphWrite> gw=new List<GraphWrite>();
            foreach (GraphRead gr in ghl_)
            {
                DateTime? a;
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
            ,ConfigurationManager.AppSettings["ParentHost"]
            ,ConfigurationManager.AppSettings["ParentPort"]
            ,ConfigurationManager.AppSettings["CommandURL"]
            );

            orientDbName=ConfigurationManager.AppSettings["ParentDB"];

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
                    {title="Опросы", href=new QuizHrefNode() {link="http://my.nspk.ru/Quiz/Execute/", target="_self"}, id=50, parentid=1};

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

namespace NewsUOWs
{

    public class NewsUow
    {

        Manager manager;
        string dbName;

        public NewsUow(string databaseName=null)
        {

            string login = ConfigurationManager.AppSettings["orient_login"];
            string password = ConfigurationManager.AppSettings["orient_pswd"];
            string dbHost = string.Format("{0}:{1}"
                , ConfigurationManager.AppSettings["ParentHost"]
                , ConfigurationManager.AppSettings["ParentPort"]);
            if (databaseName == null)
            {
                dbName = ConfigurationManager.AppSettings["ParentDB"];
            }
            else { dbName = databaseName; }

            TypeConverter typeConverter = new TypeConverter();
            JsonManagers.JSONManager jsonMnager = new JSONManager();
            TokenMiniFactory tokenFactory = new TokenMiniFactory();
            UrlShemasExplicit UrlShema = new UrlShemasExplicit(
                new CommandBuilder(tokenFactory, new FormatFactory())
                , new FormatFromListGenerator(new TokenMiniFactory())
                , tokenFactory, new OrientBodyFactory());

            BodyShemas bodyShema = new BodyShemas(new CommandFactory(), new FormatFactory(), new TokenMiniFactory(),
                new OrientBodyFactory());

            UrlShema.AddHost(dbHost);
            WebResponseReader webResponseReader = new WebResponseReader();
            WebRequestManager webRequestManager = new WebRequestManager();
            webRequestManager.SetCredentials(new NetworkCredential(login, password));
            CommandFactory commandFactory = new CommandFactory();
            FormatFactory formatFactory = new FormatFactory();
            OrientQueryFactory orientQueryFactory = new OrientQueryFactory();
            OrientCLRconverter orientCLRconverter = new OrientCLRconverter();

            CommandShemasExplicit commandShema_ = new CommandShemasExplicit(commandFactory, formatFactory,
            new TokenMiniFactory(), new OrientQueryFactory());

            manager = new Manager(typeConverter, jsonMnager, tokenFactory, UrlShema, bodyShema, commandShema_
            , webRequestManager, webResponseReader, commandFactory, formatFactory, orientQueryFactory, orientCLRconverter);

        }

        public Person GetByAccount(string accountName_)
        {
            Person result=null;
            var a=from s in manager.Props<Person>().ToList() where s.Name=="sAMAccountName" select s;
            result=manager.SelectSingle<Person>("sAMAccountName='" + accountName_+"'", dbName);
            return result;
        }
        public Person GetByGUID(string GUID_)
        {
            Person result = null;            
            result = manager.SelectSingle<Person>("GUID='" + GUID_ + "'", dbName);
            return result;
        }
        public Note GetNewsByGUID(string GUID_)
        {
            Note result = null;
            result = manager.SelectSingle<Note>("GUID='" + GUID_ + "'", dbName);
            return result;
        }
        public IEnumerable<Person> SearchByName(string Name_)
        {
            IEnumerable<Person> result=null;
            result=manager
                .Select<Person>("Name like '%"+Name_+"%' or sAMAccountName like '%"+Name_+"%'or mail like '%"+Name_+"%'"
                ,dbName);
            return result;
        }        

        public T GetOrientObjectById<T>(string id_)
            where T: class, IOrientObjects.IOrientEntity
        {
            T result = null;
            result = manager.Select<T>("@rid=" + id_ , dbName).FirstOrDefault();
            return result;
        }
        public T GetOrientObject<T>(T object_)
            where T : class, IOrientObjects.IOrientEntity
        {
            T result = null;
            result = manager.Select<T>("@rid=" + object_.id, dbName).FirstOrDefault();
            return result;
        }
        public IEnumerable<T> GetOrientObjects<T>(string cond_=null)
           where T : class, IOrientObjects.IOrientEntity
        {
            IEnumerable<T> result = null;
            result = manager.Select<T>(cond_, dbName);
            return result;
        }

        public Note CreateCommentary(Person from,string newsId_,string comment_)
        {
            Authorship auth=new Authorship(){type="Printed"};
            Comment commented=new Comment(){type="Printed"};
            Note commentaryTochange_=null;
            Note commentaryToAdd_=null;
            Note newsToComment_=manager.SelectSingle<Note>("@rid="+newsId_,dbName);

            commentaryTochange_=manager.OrientStringToObject<Note>(comment_);

            Note prev=IsComment(newsId_);
            //is comment to comment
            if (prev!=null)
            {             
                commentaryTochange_.commentDepth=(prev.commentDepth + 1);               
            }
            else
            {
                newsToComment_.hasComments=true;
            }
            //commentary Node created and relation from person created
            commentaryToAdd_=CreateNews(from,commentaryTochange_);

            if (commentaryToAdd_!=null)
            {               
                if (newsToComment_!=null)
                {
                    //create relation from commment to news Nodes
                    manager.CreateEdge<Comment>(commented,newsToComment_, commentaryToAdd_);
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
        public Note CreateCommentary(Person from,Note comment_,Note newsId_)
        {
            Authorship auth = new Authorship() { type = "Printed" };
            Comment commented = new Comment() { type = "Printed" };

            Note prev = IsComment(newsId_.id);
            if(prev!=null)
            {
                //comment to comment
                comment_.commentDepth=prev.commentDepth+1;
            }
            else
            {
                //comment to news
                comment_.commentDepth=comment_.commentDepth+1;
            }

            //commentary Node created and relation from person created
            Note commentary_ = CreateNews(from, comment_);

            if (commentary_ != null)
            {
                Note newsToComment_ = manager.SelectSingle<Note>("@rid=" + newsId_.id, dbName);
                if (newsToComment_ != null)
                {
                    //create relation from commment to news Nodes
                    Comment commentedCr=manager.CreateEdge<Comment>(commented, newsToComment_,commentary_);
                }
                else
                {
                    //unsuccesfull news search
                    //manager.Delete<Note>(commentary_);
                }
            }

            return commentary_;
        }    
        public Note CreateNews(Person from,string news_)
        {
            Note note_=manager.CreateVertex<Note>(news_, dbName);
            Note created=CreateNews(from, note_);
            return created;
        }
        public Note CreateNews(Person from,Note note_)
        {
            Authorship auth=new Authorship() {type = "Printed"};
            Note nt_=manager.CreateVertex<Note>(note_, dbName);
            Authorship newAuth=manager.CreateEdge<Authorship>(auth,from,nt_);

            //if unsucceced clean created objects
            if(auth==null||note_==null)
            {
                manager.Delete<Note>(note_,null,dbName);
                manager.Delete<Authorship>(auth,null,dbName);
            }
            return nt_;
        }

        public Note UpdateNews(Note newsObj_)
        {          
            Note nt=manager.UpdateEntity<Note>(newsObj_, dbName);
            return nt;
        }
        public Note UpdateNews(string newsStr_)
        {
            Note result = null;
            Note nt = manager.OrientStringToObject<Note>(newsStr_);
            result = UpdateNews(nt);
            return result;
        }

        public Note PublishNews(string newsId_)
        {
            Note nt = manager.SelectSingle<Note>("@rid=" + newsId_);
            nt.published = DateTime.Now;
            return nt;
        }
        public Note UnPublishNews(string newsId_)
        {
            Note nt = manager.SelectSingle<Note>("@rid=" + newsId_);
            nt.published=null;
            return nt;
        }
        public Note PinNews(string newsId_)
        {
            Note nt = manager.SelectSingle<Note>("@rid=" + newsId_);
            nt.pinned = DateTime.Now;
            return nt;
        }
        public Note UnPinNews(string newsId_)
        {
            Note nt = manager.SelectSingle<Note>("@rid=" + newsId_);
            nt.pinned = null;
            return nt;
        }

        public IEnumerable<Note> GetNews(string accountName_)
        {
            return null;
        }

        public string DeleteNews(Person from, string id_)
        {
            string result = string.Empty;
            Note ntd = GetOrientObjectById<Note>(id_);

            if (ntd != null) {
                string deleteN=manager.DeleteEdge<Authorship,Person,Note>(from,ntd,null,dbName).GetResult();
                string deleteR=manager.Delete<Note>(ntd, null, dbName).GetResult();
                if(deleteN=="Deleted"&&deleteR == "Deleted") { result = "Deleted"; }
            }
            return result;
        }

        public IEnumerable<Note> GetPersonNews(Person p_=null)
        {
            return manager.Select<Person,Authorship, Note>(p_);
        }

        /// <summary>
        /// check inE types on Comment,Authorship. If has inE comment, then returns current Note.
        /// </summary>
        /// <param name="NewsId">Npte which type need to be checked</param>
        /// <returns></returns>
        public Note IsComment(string NewsId)
        {
            Note ret_=null;
            Note nt=manager.SelectSingle<Note>("@rid="+NewsId);
            if (nt!=null)
            {

                Note cm=manager.Select<Note,Comment>(nt,dbName).FirstOrDefault();
                Note auth=manager.Select<Note,Authorship>(nt,dbName).FirstOrDefault();                

                //comment
                if (auth != null && cm != null)
                {
                    //take comment wich iscommented
                    ret_=manager.SelectCommentToComment<Note,Comment,Note>(nt,dbName).FirstOrDefault();
                }
                //news
                if (auth!=null&&cm==null)
                {
                    ret_=null;
                }
            }
            return ret_;
        }

        public string UserAcc()
        {
            return WebManagers.UserAuthenticationMultiple.UserAcc();
        }

        public string NoteToString(Note item_)
        {
            string result=null;
                result=manager.ObjectToString<Note>(item_);
            return result;
        }
        public Note StringToNote(string item_)
        {
            Note result = null;
            result = manager.StringToObject<Note>(item_);
            return result;
        }
    }

}