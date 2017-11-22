
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Configuration;
using IQueryManagers;
using IFormats;

namespace AdinTce
{

    #region AdinTce

    //adin tce repository
    public class AdinTceRepo
    {

        IQueryManagers.ICommandBuilder _CommandBuilder;
        IWebManagers.IWebManager _webManager;
        IWebManagers.IResponseReader _responseReader;
        IJsonManagers.IJsonManger _jsonManager;

        IQueryManagers.ITypeToken GUIDtoken;

        AdinTceExplicitTokenBuilder tokenBuilder;

        public AdinTceRepo(
            IQueryManagers.ICommandBuilder CommandBuilder_,
            IWebManagers.IWebManager webManager_,
            IWebManagers.IResponseReader responseReader_,
            IJsonManagers.IJsonManger jsonManager_)
        {
            this._CommandBuilder = CommandBuilder_;
            this._webManager = webManager_;
            this._responseReader = responseReader_;
            this._jsonManager = jsonManager_;

            GUIDtoken = new AdinTceGUIDToken();
            tokenBuilder = new AdinTceExplicitTokenBuilder();
        }

        public AdinTceRepo()
        {
            this._CommandBuilder = new AdinTceCommandBuilder();
            this._webManager = new AdinTceWebManager();
            this._responseReader = new AdinTceResponseReader();
            this._jsonManager = new AdinTceJsonManager();

            _webManager.AddCredentials(new System.Net.NetworkCredential(
               ConfigurationManager.AppSettings["AdinTceLogin"], ConfigurationManager.AppSettings["AdinTcePassword"]));

            GUIDtoken = new AdinTceGUIDToken();
            tokenBuilder = new AdinTceExplicitTokenBuilder();
        }
        public string HoliVation(string GUID)
        {
            
            AdinTcePOCO adp = new AdinTcePOCO();
            IEnumerable<Holiday> dhl=null;
            IEnumerable<Vacation> vhl = null;
            IEnumerable<GUIDPOCO> gpl = null;

            string result = string.Empty;
            string holidayCommand, vacationCommand, holidaysResp, vacationsResp;
            GUIDtoken.Text = GUID;

            _CommandBuilder.SetText(tokenBuilder.HolidaysCommand(GUIDtoken), new AdinTcePartformat());
            holidayCommand = _CommandBuilder.GetText();
            _CommandBuilder.SetText(tokenBuilder.VacationsCommand(GUIDtoken), new AdinTcePartformat());
            vacationCommand = _CommandBuilder.GetText();

            //Task.Run(() => {
            _webManager.AddRequest(holidayCommand);
            holidaysResp = _responseReader.ReadResponse(_webManager.GetResponse("GET"));
            if (holidaysResp != null && holidaysResp != string.Empty)
            {
                gpl = _jsonManager.DeserializeFromParentNode<GUIDPOCO>(holidaysResp);
                dhl = _jsonManager.DeserializeFromParentChildren<Holiday>(holidaysResp, "Holidays");
                adp.GUID_ = gpl.Select(s => s).FirstOrDefault().GUID_;
                adp.Position = gpl.Select(s => s).FirstOrDefault().Position;
                adp.holidays = dhl.ToList();
            }
            _webManager.AddRequest(holidayCommand);
            vacationsResp = _responseReader.ReadResponse(_webManager.GetResponse( "GET"));
            if (vacationsResp!=null && vacationsResp != string.Empty)
            {
                vhl = _jsonManager.DeserializeFromParentChildren<Vacation>(vacationsResp, "Holidays");
                adp.vacations = vhl.ToList();
            }

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
    public class AdinTceCommandBuilder : QueryManagers.CommandBuilder
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
  
    public class AdinTcePOCO
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