
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Configuration;

namespace NSQLManager
{

    #region AdinTce

    //adin tce repository
    public class AdinTceRepo
    {

        IQueryManagers.ITextBuilder _textBuilder;
        IWebManagers.IWebManager _webManager;
        IWebManagers.IResponseReader _responseReader;
        IJsonManagers.IJsonManger _jsonManager;

        IQueryManagers.ITypeToken GUIDtoken;

        AdinTceExplicitTokenBuilder tokenBuilder;

        public AdinTceRepo(
            IQueryManagers.ITextBuilder textBuilder_,
            IWebManagers.IWebManager webManager_,
            IWebManagers.IResponseReader responseReader_,
            IJsonManagers.IJsonManger jsonManager_)
        {
            this._textBuilder = textBuilder_;
            this._webManager = webManager_;
            this._responseReader = responseReader_;
            this._jsonManager = jsonManager_;

            GUIDtoken = new AdinTceGUIDToken();
            tokenBuilder = new AdinTceExplicitTokenBuilder();
        }

        public AdinTceRepo()
        {
            this._textBuilder = new AdinTceTextBuilder();
            this._webManager = new AdinTceWebManager();
            this._responseReader = new AdinTceResponseReader();
            this._jsonManager = new AdinTceJsonManager();

            _webManager.addCredentials(new System.Net.NetworkCredential(
               ConfigurationManager.AppSettings["AdinTceLogin"], ConfigurationManager.AppSettings["AdinTcePassword"]));

            GUIDtoken = new AdinTceGUIDToken();
            tokenBuilder = new AdinTceExplicitTokenBuilder();
        }
        public string HoliVation(string GUID)
        {
            AdinTcePOCO2 adp = new AdinTcePOCO2();
            IEnumerable<Holiday> dhl;
            IEnumerable<Vacation> vhl;
            IEnumerable<GUIDPOCO> gpl;

            string result = string.Empty;
            string holidayCommand, vacationCommand, holidaysResp, vacationsResp;
            GUIDtoken.Text = GUID;

            _textBuilder.SetText(tokenBuilder.HolidaysCommand(GUIDtoken), new AdinTcePartformat());
            holidayCommand = _textBuilder.GetText();
            _textBuilder.SetText(tokenBuilder.VacationsCommand(GUIDtoken), new AdinTcePartformat());
            vacationCommand = _textBuilder.GetText();

            //Task.Run(() => {
            holidaysResp = _responseReader.ReadResponse(_webManager.addRequest(holidayCommand, "GET").GetResponse());
            gpl = _jsonManager.DeserializeFromParentNode<GUIDPOCO>(holidaysResp);
            dhl = _jsonManager.DeserializeFromParentChildren<Holiday>(holidaysResp,"Holidays");           

            vacationsResp = _responseReader.ReadResponse(_webManager.addRequest(vacationCommand, "GET").GetResponse());
            vhl = _jsonManager.DeserializeFromParentChildren<Vacation>(vacationsResp, "Holidays");

            adp.holidays = dhl.ToList();
            adp.vacations = vhl.ToList();
            adp.GUID_ = gpl.Select(s => s).FirstOrDefault().GUID_;
            adp.Position = gpl.Select(s => s).FirstOrDefault().Position;

            //});

            result = _jsonManager.SerializeObject(adp);

            return result;
        }

    }

    //Command buil from Tokens,with explicit sytax for repo call
    public class AdinTceExplicitTokenBuilder
    {

        public List<IQueryManagers.ITypeToken> HolidaysCommand(IQueryManagers.ITypeToken GUID)
        {
            List<IQueryManagers.ITypeToken> result = new List<IQueryManagers.ITypeToken>()
            { new AdinTceURLToken(), new AdinTceHolidatyToken(), new AdinTcePartToken(), GUID};
            return result;
        }
        public List<IQueryManagers.ITypeToken> VacationsCommand(IQueryManagers.ITypeToken GUID)
        {
            List<IQueryManagers.ITypeToken> result = new List<IQueryManagers.ITypeToken>()
            { new AdinTceURLToken(), new AdinTceVacationToken(), new AdinTcePartToken(), GUID};
            return result;
        }

    }


    //AdinTce Tokens
    public class AdinTceURLToken : IQueryManagers.ITypeToken
    {
        public string Text { get; set; } = ConfigurationManager.AppSettings["AdinTceUrl"];
    }
    public class AdinTceHolidatyToken : IQueryManagers.ITypeToken
    {
        public string Text { get; set; } = @"holiday";
    }
    public class AdinTceVacationToken : IQueryManagers.ITypeToken
    {
        public string Text { get; set; } = @"vacation";
    }
    public class AdinTceFullToken : IQueryManagers.ITypeToken
    {
        public string Text { get; set; } = @"full";
    }
    public class AdinTcePartToken : IQueryManagers.ITypeToken
    {
        public string Text { get; set; } = @"part";
    }
    public class AdinTceGUIDToken : IQueryManagers.ITypeToken
    {
        public string Text { get; set; }
    }

    //AdinTce formats
    public class AdinTceURLformat : IQueryManagers.ITypeToken
    {
        public string Text { get; set; } = @"{0}/{1}/{2}";
    }
    public class AdinTcePartformat : IQueryManagers.ITypeToken
    {
        public string Text { get; set; } = @"{0}/{1}/{2}/{3}";
    }



    ///<summary>AdinTce realization of Base builder,web,reader,json
    ///</summary>
    public class AdinTceTextBuilder : QueryManagers.TextBuilder
    {

    }

    public class AdinTceWebManager : WebManagers.WebManager
    {

    }

    public class AdinTceResponseReader : WebManagers.WebResponseReader
    {

    }

    public class AdinTceJsonManager : JsonManagers.JSONManager
    {

    }

    public class AdinTceCommands
    {

    }



    //AdinTce POCOs
    class MonthDayYearDateConverter : IsoDateTimeConverter
    {
        public MonthDayYearDateConverter()
        {
            DateTimeFormat = "dd.MM.yyyy";
        }
    }

    public class Holiday
    {
        [JsonProperty("LeaveType")]
        public string LeaveType { get; set; }
        [JsonProperty("Days")]
        public double Days { get; set; }
    }
   
    public class Vacation
    {
        [JsonProperty("LeaveType")]
        public string LeaveType { get; set; }
        [JsonProperty("DateStart"), JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime DateStart { get; set; }
        [JsonProperty("DateFinish"), JsonConverter(typeof(MonthDayYearDateConverter))]
        public DateTime DateFinish { get; set; }
        [JsonProperty("DaysSpent")]
        public int DaysSpent { get; set; }
    }
   
    public class GUIDPOCO
    {
        [JsonProperty("GUID")]
        public string GUID_ { get; set; }
        [JsonProperty("Position")]
        public string Position { get; set; }
    }
  
    public class AdinTcePOCO2
    {
        [JsonProperty("GUID")]
        public string GUID_ { get; set; }
        [JsonProperty("Position")]
        public string Position { get; set; }
        [JsonProperty("Holidays")]
        public List<Holiday> holidays { get; set; }
        [JsonProperty("Vacations")]
        public List<Vacation> vacations { get; set; }
    }
    #endregion

}