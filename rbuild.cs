using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.IO.Packaging;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

using xlNS = Microsoft.Office.Interop.Excel;
using pptNS = Microsoft.Office.Interop.PowerPoint;

using Oracle.DataAccess.Client;
using System.Data.OleDb;
using System.Data;


using System.Xml.Linq;


using Outlook = Microsoft.Office.Interop.Outlook;

using System.Globalization;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

using OfficeOpenXml;

namespace r_build
{
   
    public class rObject : IDisposable
    {
        internal static xlNS.Application apl;
        internal static pptNS.Application papl;
        internal static pptNS.Presentations pts;
        internal static pptNS.Presentation presentation;

        public  xlNS.Workbook wb;
        public  xlNS.Worksheet ws;
        public  xlNS.Range rng;


        public List<rObject> globalList = new List<rObject>();

        public string objectName;
        public string objectParameter;
        public rObject objectParent;
        public string objectType;

        public int level;

        public string FileName()
        {
            return Path.GetFileName(this.objectParameter);
        }

        public bool objectInit = false;

        public rObject()
        {

        }

        public rObject(string objectName_, string objectParam_, int level_, rObject objectParent_ = null, string objectType_ = null)
        {

            this.objectName = objectName_;
            this.objectParameter = objectParam_;
            this.level = level_;
            this.objectParent = objectParent_;
            this.objectType = objectType_;

            //globalList.Add(this);
        }
     
        public void workBookInit(string filepath)
        {
            if (apl==null)
            {
                apl = new xlNS.Application();
                ProcessHandler_ih.processGet(apl);
            }
         
            if(File.Exists(filepath))
            {               
                wb=apl.Workbooks.Open(filepath);
                objectInit = true;
            }   
            else
            {
                wb=apl.Workbooks.Add();
                wb.SaveAs(filepath);
                objectInit = true;
            }
        }
        public void worksheetInit()
        {
            if(objectInit)
            {
                if (wb.Worksheets[1]!=null)
                {
                    ws = wb.Worksheets[1];
                }
                else
                {
                    ws = wb.Worksheets.Add();
                }
            }
        }
        public void workbookCreateInit(string path)
        {

        }
        public void Dispose()
        {
            ProcessHandler_ih.processesKill();
        }
    }

    //class represents line from parameter .txt and list of parsed hierarhical
    //object type names and parameters
    public class LineList
    {
        public List<LineList> hierarhyList = new List<LineList>();

        public LineList()
        {

        }

      
        public int level;
        public string ObjectName;
        public string ObjectParameter;
        public string ObjectType;
           
        public LineList(int level_, string objName_, string objParam_, string objType_)
            {
                level = level_;
                ObjectName = objName_;
                ObjectParameter = objParam_;
                ObjectType = objType_;
            }        
   
    }

    public class NameParameter
    {

        public string Name;
        public string Parameter;

        public List<NameParameter> hierarhyList = new List<NameParameter>();

        public NameParameter()
        {

        }
        public NameParameter(string name_,string parameter_)
        {
            Name = name_; Parameter = parameter_;
        }

    }

    //contains linq from query name to export file
    public class QueryToFile
    {
        public List<QueryToFile> hierarhyList = new List<QueryToFile>();

        public string wbName;
        public string wbParam;
        public string qrName;
        public string qrParam;

        public QueryToFile ()
        {

        }
        public QueryToFile ( string wbName_, string wbParam_, string qrName_,string qrParam_)
        {
            wbName = wbName_; wbParam = wbParam_; qrName = qrName_; qrParam = qrParam_;
        }

        public void listAdd()
        {
            this.hierarhyList.Add(this);
        }
        public string getPath(string queryname)
        {
            string result = "";
            foreach (QueryToFile qt in this.hierarhyList)
            {
                if (qt.qrParam==queryname)
                {
                    result = qt.wbParam;
                }
            }
            return result;
        }
    }

    //parses and contains query names to query texts
    public class QueryTexts
    {

        public string queryName;
        public string queryText;
        public string queryType;

        internal string qyeryPath;
        public List<QueryTexts> queryList = new List<QueryTexts>();

        public QueryTexts()
        {

        }
        public QueryTexts(string queryName, string queryText)
        {
            this.queryName = queryName;
            this.queryText = queryText;
        }
        public QueryTexts(string queryName, string queryText, string queryType)
        {
            this.queryName = queryName;
            this.queryText = queryText;
            this.queryType = queryType;
        }

        public void queryParse()
        {
            if (File.Exists(qyeryPath))
            {
               
                string read = File.ReadAllText(qyeryPath);
                read = Regex.Replace(read, @"\r\n\r\n\r\n", "");
                string[] groups = read.Split(';');
               
                foreach (string str in groups)
                {
                    if (str != "" && str != null && str != "\r\n")
                    {
                        Regex reg = new Regex(@"::");
                        Regex regPr = new Regex(@"\[.*\]");
                      

                        string[] groups2 = reg.Split(str);

                        Match queryParam = regPr.Match(groups2[1]);

                        string qyeryname = Regex.Replace(groups2[0], @"QUERY_NAME=|\[.*\]|\s", @"");
                        string querytext = Regex.Replace(groups2[1], @"QUERY_TEXT(\[PARAMETRIZED\])=|QUERY_TEXT=|\[.*\]", @"");
                        if (queryParam.Value != "")
                        {
                            queryList.Add(new QueryTexts(queryName = qyeryname, queryText = querytext, Regex.Replace(queryParam.Value, @"\[|\]", "")));
                        }
                        else
                        {
                            queryList.Add(new QueryTexts(queryName = qyeryname, queryText = querytext));
                        }

                    }

                }

            }
        }
        public void queryStore()
        {
            if (queryList.Count != 0)
            {
                InitializationParams.JSON_Serialize(this.queryList, qyeryPath);
            }
        }
        public void queryLoad()
        {
            if (File.Exists(qyeryPath))
            {
                InitializationParams.JSON_Deserialize(this.queryList, qyeryPath);
            }
        }

    }   

    public class FileToMail
    {

        public string[] filename;
        public string[] maillist;

        public List<FileToMail> hierarhyList = new List<FileToMail>();

        public string paramPath;

        public FileToMail()
        {

        }
        public FileToMail(string[] file_,string [] mail_)
        {
            filename = file_; maillist = mail_;
        }

        public void paramParse()
        {
            if(File.Exists(paramPath))
            {
                string[] read = File.ReadAllLines(paramPath);
                foreach( string str in read)
                {
                    if (str != "" && str[0].ToString() != @"#")
                    {

                        string[] block = new Regex(@";").Split(str);

                        string[] wbPath = null;
                        string[] mlList = null;

                        foreach (string str2 in block)
                        {
                            if (new Regex(@"WB=").Match(str2).ToString() != "")
                            {
                                wbPath = new Regex(@"WB=").Replace(str2, "").Split(',');
                            }
                            if (new Regex(@"MAIL=").Match(str2).ToString() != "")
                            {
                                mlList = new Regex(@",").Split(new Regex(@"MAIL=").Replace(str2, "").ToString());
                            }
                        }

                        if (wbPath.Count() > 0 && mlList.Count() > 0)
                        {
                            hierarhyList.Add(new FileToMail(wbPath, mlList));
                        }
                    }
                }
            }
        }

    }

    public class Connections
    {
        OleDbConnection connection;
        OleDbCommand command = new OleDbCommand();

        protected string ConnectionString_;
        bool connectionAvailable_;
        string LoginReplace = "LoginReplacePatch";
        string PasswordReplace = "PasswordReplacePatch";
        string dateParameter = @":reportDate";
        public Connections(string Login, string Password)
        {
            ConnectionStringSet();
            LoginPasswordInsert(Login, Password);
            ConnectionTest();
        }
        public Connections(string Login, string Password, string connString)
        {
            ConnectionString = connString;
            LoginPasswordInsert(Login, Password);
            ConnectionTest();
        }

        public bool connIsAvailable
        {
            get { return connectionAvailable_; }
            set { connectionAvailable_ = value; }
        }
        public string ConnectionString
        {
            get { return ConnectionString_; }
            set { ConnectionString_ = value; }
        }
        protected virtual void ConnectionStringSet()
        {
            if (ConnectionString_ == null) { ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["DefaultConnectionString"]; };
        }
        public virtual void Open()
        {
            connection = new OleDbConnection(ConnectionString_);
            connection.Open();
        }

        protected void LoginPasswordInsert(string Login, string Password)
        {
            ConnectionString_ = ConnectionString_.Replace(LoginReplace, Login).Replace(PasswordReplace, Password);
        }
        protected virtual void ConnectionTest()
        {
            connection = new OleDbConnection(ConnectionString_);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {

            }
            connection.Close();
        }
        public virtual void addCommand(string commandText)
        {
            command.CommandText = commandText;
        }
        //useful to see if any new connections created
        public void testExecuteReader()
        {
            connection.Open();
            command.Connection = connection;
            command.ExecuteReader();
        }

        public virtual void testDsFill(DataSet ds, OleDbConnection oc)
        {
            OleDbDataAdapter od = new OleDbDataAdapter(command.CommandText, oc);
            od.Fill(ds);
        }
        public virtual void DatasetEraseFill(DataSet ds)
        {
            if (!command.Parameters.Contains(InitializationParams.paramterName))
            {
                command.Parameters.Add(InitializationParams.paramterName, OleDbType.Date);
            }

            command.CommandText = command.CommandText.Replace(InitializationParams.paramterName, @"'" + InitializationParams.reportDate.ToString() + @"'");

            OleDbDataAdapter od = new OleDbDataAdapter(command.CommandText, connection);
            
            ds.Clear();
            
            while (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (ds.Tables.CanRemove(dt))
                {
                    ds.Tables.Remove(dt);
                }
            }
            
            od.Fill(ds);
        
        }
        public virtual void DatasetTablesFill(DataSet ds, string tableName)
        {                  
            OleDbDataAdapter od = new OleDbDataAdapter(command.CommandText , connection);
            DataTable dt = new DataTable();

            
            if(datatablePresenceCheck(ds,tableName))
            {
                dt=ds.Tables[tableName];
                dt.Clear();
            }
            else
            {
                dt = ds.Tables.Add(tableName);
            }          
            

            od.Fill(dt);
        }

        private bool datatablePresenceCheck(DataSet ds, string tableName_)
        {
            bool result = false;

            foreach (DataTable dt in ds.Tables)
            {
                if (dt.TableName==tableName_)
                {
                    result = true;
                }
            }

            return result;
        }
    }
    public class OracleConnections : Connections
    {
        public OracleConnection connection;
        public OracleCommand command = new OracleCommand();

        public OracleConnections(string Login, string Password)
            : base(Login, Password)
        {

        }
        public OracleConnections(string Login, string Password, string connString)
            : base(Login, Password, connString)
        {
            ConnectionString = connString;
            LoginPasswordInsert(Login, Password);
            ConnectionTest();
        }

        protected override void ConnectionTest()
        {
            connection = new OracleConnection(ConnectionString_);
            try
            {
                connection.Open();
            }
            catch (Exception e)
            {

            }
            connection.Close();
        }
        protected override void ConnectionStringSet()
        {
            if (ConnectionString_ == null) { ConnectionString = System.Configuration.ConfigurationSettings.AppSettings["OracleConnectionString"]; };
        }
        public override void Open()
        {
            connection = new OracleConnection(ConnectionString_);
            connection.Open();
        }
        public void testDsFill_(DataSet ds)
        {
            OracleConnection oc = connection;
            OracleDataAdapter od = new OracleDataAdapter(command.CommandText, oc);
            od.Fill(ds);
        }

        public override void addCommand(string commandText)
        {
            command.CommandText = commandText;
        }
    }

    public static class Extensions
    {
        public static string ToStringEx(this DateTime date,string format)
        {
            string result = "";
            if(new string[] {"q","Q"}.Contains(format))
            {                
                result = Math.Ceiling((double)date.Month / 4).ToString();
            }
            if (new string[] { "qq", "QQ" }.Contains(format))
            {
               
                result = @"0" + Math.Ceiling((double)date.Month / 4).ToString();
                
            }
            return result;
        }
    }
    
    //class parses parameter .txt stores values in List of lineList classes
    //removes duplicated lines if any
    public static class InitializationParams
    {
        private static List<LineList> lineList = new List<LineList>();

        public static rObject rObjectPrent = new rObject();     
        public static QueryToFile queryToFile = new QueryToFile();
        public static QueryTexts queryTexts = new QueryTexts();
        public static FileToMail fileToMail = new FileToMail();

        public static Connections connection;
        public static DataSet dataset;

        public static string login;
        public static string password;

        private static StringBuilder sb = new StringBuilder();
        private static string line;
        private static string[] lines;

        private static string match;
        private static string wbName;
        private static string wbParam;
        private static string qrName;
        private static string qrParam;
        public static string dateString;

        private static Dictionary<string, string> TypeParamPairs = new Dictionary<string, string>();

        static int objectLevel;
        private static string[] objects;
        public static string paramterName = ":reportdate";
        public static DateTime reportDate = System.DateTime.Now;
        public static int lineCompare = 0;

        public static void SetDateString(DateTime? reportDate, string type_)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            string monthName = @"";

            if (reportDate == null)
            {
                reportDate = DateTime.Now;
            }

            InitializationParams.reportDate = (DateTime)reportDate;

            if (type_== @"W")
            {
                int weekDay = (int)InitializationParams.reportDate.DayOfWeek;
                //first day of previous week always
                InitializationParams.reportDate = InitializationParams.reportDate.Date.AddDays(-weekDay-6).Date;
                int weekNum = dfi.Calendar.GetWeekOfYear(
                   InitializationParams.reportDate, System.Globalization.CalendarWeekRule.FirstFourDayWeek,
                   DayOfWeek.Monday
                );
                //Year of first day of week (for correct year swich)
                dateString = "Week_" + weekNum + "_" + InitializationParams.reportDate.AddDays(-7).Date.ToString("yyyy");
                if (weekNum==1)
                {
                    //Year of last day of week (for correct year swich)
                    dateString = "Week_" + weekNum + "_" + InitializationParams.reportDate.Date.ToString("yyyy");
                }
            }

            if (type_ == @"M")
            {
                //previous month always
                InitializationParams.reportDate = InitializationParams.reportDate.Date.AddDays(-(InitializationParams.reportDate.Day-1)).AddMilliseconds(-1);
                int monthNum = InitializationParams.reportDate.Month;
                monthName = InitializationParams.reportDate.AddDays(-7).Date.ToString("MMM", CultureInfo.CreateSpecificCulture("en-US"));
                dateString = "Month_" + monthName + "_" + InitializationParams.reportDate.AddDays(-7).Date.ToString("yyyy");
            }
            if (type_ == @"D")
            {
                InitializationParams.reportDate = InitializationParams.reportDate.Date.AddMilliseconds(-1);
                dateString =  InitializationParams.reportDate.ToString(@"yyyyMMdd");
            }
            if (type_ == @"MTR")
            {
                InitializationParams.reportDate = InitializationParams.reportDate.Date.AddDays(1).AddMilliseconds(-1);
                dateString = InitializationParams.reportDate.ToString(@"yyyyMMdd");
            }
            if (type_ == @"MTR_PREV")
            {
                InitializationParams.reportDate = InitializationParams.reportDate.Date.AddDays(-(InitializationParams.reportDate.Date.Day-1)).AddMilliseconds(-1);
                dateString = InitializationParams.reportDate.ToString(@"yyyyMMdd");
            }
            if (type_ == @"KKB")
            {
                InitializationParams.reportDate = InitializationParams.reportDate.Date.AddHours(-5);
                dateString = InitializationParams.reportDate.ToString(@"yyyyMMdd");
            }

        }

        public static void InitializeLines(string paramFilePath)
        {
            if (File.Exists(paramFilePath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(paramFilePath))
                    {
                        rObjectPrent = new r_build.rObject();
                        line = sr.ReadToEnd();
                        lines = line.Split('\n');
                        sb.Append(lines[0]);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("File could not be read");
                }
            }
        }
        public static void ParseFileToLines()
        {

            foreach (string objectline in lines)
            {
                if (objectline != "\r")
                {

                    LineList insideLineList = new LineList();
                    lineList.Add(insideLineList);
                    objectLevel = 0;
                    objects = new Regex(@";").Split(objectline);

                    foreach(string object_ in objects)
                    {
foreach(Enum ot in Enum.GetValues(typeof(objectTypeNames)))
{
    match = new Regex(ot.ToString()).Match(object_).ToString();

    if (match == "WB")
    {      
        wbName = match;
        wbParam = new Regex(wbName + "|=").Replace(object_, "").ToString();
    }

    if (match == "QUERY")
    {        
        qrName = match;
        qrParam = new Regex(qrName + "|=").Replace(object_, "").ToString();
    }
}

                    }
                }

if (wbName != "" && wbParam != "" && qrName != "" && qrParam != ""
&& wbName != null && wbParam != null && qrName != null && qrParam != null){
queryToFile.hierarhyList.Add(new QueryToFile(wbName, wbParam, qrName, qrParam));
}
            }

        }     

        public static void QueriesRead()
        {
            queryTexts.queryParse();
        }

        public static void JSON_Serialize<T>(T object_, string path_)
        {
            string json = JsonConvert.SerializeObject(object_);
            File.WriteAllText(path_, json);
        }
        public static void JSON_Deserialize<T>(T object_, string path_)
        {
            string json = File.ReadAllText(path_);
            object_ = JsonConvert.DeserializeObject<T>(json);
        }

        public static void connectionInit(string login, string password)
        {
            connection = new Connections(login, password);
        }

        public static void datasetAllFill()
        {
            foreach(QueryTexts qt in queryTexts.queryList)
            {
                foreach (QueryToFile wq in queryToFile.hierarhyList)
                {
                    if(qt.queryName==wq.qrParam)
                    {
                        connection.addCommand(qt.queryText);
                        dataset = new DataSet();
                        connection.DatasetTablesFill(dataset, qt.queryName);                      
                    }
                }
            }            
        }
        public static void datasetParametrizedFill(string QueryName,List<NameParameter> parametersList)
        {
            foreach (QueryTexts qt in queryTexts.queryList)
            {
                foreach (QueryToFile wq in queryToFile.hierarhyList)
                {
                    if (qt.queryName == wq.qrParam && qt.queryName == QueryName)
                    {
                        string querytext_ = qt.queryText;
                        foreach (NameParameter param in parametersList)
                        {
                            querytext_ = querytext_.Replace(param.Name, param.Parameter);
                        }

                        connection.addCommand(querytext_);
                        //dataset = new DataSet();
                        connection.DatasetTablesFill(dataset, qt.queryName);
                     }
                }
            }
        }
        
        public static void dataSetCreate()
        {
            dataset = new DataSet();
        }
        public static void dataSetDispose()
        {
            dataset.Dispose();
        }
        public static void datatableExport()
        {
           foreach(QueryTexts qt in queryTexts.queryList)
            {
                foreach (QueryToFile wq in queryToFile.hierarhyList)
                {                    
                    foreach(DataTable dt in dataset.Tables)
                    {
                        if (qt.queryName == wq.qrParam && qt.queryName == dt.TableName)
                        {
                            datatableToXML(dt, wq.wbParam);
                            datatableToXLSX(dt,wq.wbParam);
                        }        
                    }
                }
            }         
        }
        public static void datatableParametrizedExport(string QueryName,string filepath)
        {            
            foreach (DataTable dt in dataset.Tables)
            {
                if ( QueryName == dt.TableName)
                {
                    if (filepath.Contains(@".xlsx"))
                    {
                        datatableToXLSX(dt, filepath);
                    }
                    if (filepath.Contains(@".xml"))
                    {
                        datatableToXML(dt, filepath);
                    }
                                       
                }
            }               
        }
        public static void OpenXmlExport(string QueryName)
        {
             foreach (DataTable dt in dataset.Tables)
            {
                if (QueryName == dt.TableName && queryToFile.getPath(QueryName) != "")
                {
                    OpenXmlToXLSX(dt, queryToFile.getPath(QueryName));
                 }
             }

        }
        public static void StreamBuilderExport(string QueryName)
        {
            foreach (DataTable dt in dataset.Tables)
            {
                if (QueryName == dt.TableName && queryToFile.getPath(QueryName) != "")
                {
                    StreamBuilderToFile(dt, queryToFile.getPath(QueryName));
                }
            }
        }
        public static void StreamWriterExportEPPL(string QueryName, string FileName)
        {
            foreach (DataTable dt in dataset.Tables)
            {
                if (QueryName == dt.TableName && queryToFile.getPath(QueryName) != "")
                {
                    StreamBuilderToEPPL(dt, queryToFile.getPath(QueryName));
                }
            }
        }
        public static void StreamWriterExport(string QueryName,string FileName)
        {
            foreach (DataTable dt in dataset.Tables)
            {
                if (QueryName == dt.TableName && queryToFile.getPath(QueryName) != "")
                {
                    StreamWriterToFile(dt, queryToFile.getPath(QueryName));
                }
            }
        }
        public static void xmlTree(string path ,string[] tableNames, DateTime date)
        {            
            datasetToXMLtree(path,tableNames,date);
        }

        private static void datatableToXLSX(DataTable dt, string path)
        {
            xlNS.Workbook wb;
            xlNS.Worksheet ws;
            rObjectPrent.workBookInit(path);
            rObjectPrent.worksheetInit();
            wb = rObjectPrent.wb;
            ws = rObjectPrent.ws;

            //ExportDataSetOPENXML(dt, Path.GetDirectoryName(path) + @"\tst.xlsx");

            object[,] array = new object[dt.Rows.Count,dt.Columns.Count];

            for(int i =0;i<dt.Rows.Count; i++)
            {
                for (int i2 = 0; i2 < dt.Columns.Count; i2++)
                {
                    if(dt.Columns[i2].DataType==typeof(System.DateTime))
                    {
                        DateTime date = new DateTime();                        
                        if (dt.Rows[i][i2].ToString() != @"")
                        {                           
                            date = (DateTime)dt.Rows[i][i2];
                            array[i, i2] = date;
                        }
                        else
                        {
                            array[i, i2] = null;
                        }
                    }
                    else
                    {
                        array[i, i2] = dt.Rows[i][i2];
                    }
                }
            }

            xlNS.Range rn;           

            int columnToCopy=1;            
            foreach(DataColumn dc in dt.Columns)
            {
                if (dc.DataType == typeof(System.DateTime))
                {
                    ws.get_Range((xlNS.Range)ws.Cells[1, columnToCopy], (xlNS.Range)ws.Cells[1 + dt.Rows.Count, columnToCopy]).NumberFormat = "DD.MM.YYYY";
                }
                ws.Cells[1, columnToCopy] = dc.Caption;
                columnToCopy++;
            }

            rn = ws.get_Range((xlNS.Range)ws.Cells[2, 1], (xlNS.Range)ws.Cells[1+dt.Rows.Count, dt.Columns.Count]);
           
            //rn.Value2 = array;

            
            for (int i = 1; i <= dt.Rows.Count; i++)
            {
                for (int i2 = 1; i2 <= dt.Columns.Count; i2++)
                {
                    rn[i, i2].Value2 = array[i - 1, i2 - 1];

                }
            }
            

            ws.Application.DisplayAlerts = false;
            wb.Save();
            wb.Close();         
                                 
        }
        private static void OpenXmlToXLSX(DataTable dt, string path)
        {
            ExportDataSetOPENXML(dt, Path.GetDirectoryName(path) + @"\" + dt.TableName + ".xlsx");
        }
        private static void ExportDataSetOPENXML(DataTable table, string destination)
        {
            
            using (var workbook = SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();

                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();
               
                    
                    var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                    var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                    sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                    DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                    string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                    uint sheetId = 1;
                    if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                    {
                        sheetId =
                            sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                    }

                    DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                    sheets.Append(sheet);

                    DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                    List<String> columns = new List<string>();
                    foreach (System.Data.DataColumn column in table.Columns)
                    {
                        columns.Add(column.ColumnName);

                        DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                        headerRow.AppendChild(cell);
                    }


                    sheetData.AppendChild(headerRow);

                    foreach (System.Data.DataRow dsrow in table.Rows)
                    {
                        DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                        foreach (String col in columns)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString()); //
                            newRow.AppendChild(cell);
                        }

                        sheetData.AppendChild(newRow);
                    }
                
            }
        }
        private static void StreamBuilderToFile(DataTable table, string destination)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < table.Columns.Count;i++ )
            {
                if (i != table.Columns.Count-1)
                {
                    sb.Append(table.Columns[i].ColumnName.ToString() + ',');
                }
                else
                {
                    sb.Append(table.Columns[i].ColumnName.ToString() + ';');
                }
            }

            sb.AppendLine();
            //sb.Remove(sb.Length - 1, 1);

            foreach(DataRow row in table.Rows)
            {
                for(int i = 0; i < table.Columns.Count;i++)
                {
                    if (table.Columns.Count != table.Columns.Count)
                    {
                        sb.Append(row[i].ToString() + ",");
                    }
                    else
                    {
                        sb.Append(row[i].ToString() + ";");
                    }
                }
                sb.AppendLine();
            }            

            File.WriteAllText(destination, sb.ToString(),Encoding.UTF8);
        }
        private static void StreamBuilderToEPPL(DataTable table, string destination)
        {
            destination = Path.GetDirectoryName(destination) + @"\" + Path.GetFileNameWithoutExtension(destination) + @".xlsx";

            FileInfo fi = new FileInfo(destination);
            ExcelPackage ep = new ExcelPackage(fi);
            ExcelWorksheet ws = null;

            if(!epplWSExists(ep,table.TableName))
            {
                ws = ep.Workbook.Worksheets.Add(table.TableName);
            }

            ws = ep.Workbook.Worksheets[table.TableName];

            for (int i = 0; i < table.Columns.Count; i++)
            {
                ws.Cells[1, i+1].Value = table.Columns[i].ColumnName.ToString();
            }          

            for (int i0 = 0; i0 < table.Rows.Count; i0++)
            {
                DataRow row = table.Rows[i0];
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    ws.Cells[i0+2, i+1].Value = row.ItemArray[i].ToString();
                }               
            }

            ep.Save();

            ws.Dispose();
            ep.Dispose();
            table.Dispose();
        }
        private static void datatableToXML(DataTable dt, string path)
        {           
            dt.WriteXml(Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + @".xml");
        }
        private static void StreamWriterToFile(DataTable dt, string path)
        {

            //FileStream fs = File.Create(path);
           //byte[] info = new UTF8Encoding(true).GetBytes(sb.ToString());
            if (File.Exists(path)) File.Delete(path);

            path = Path.GetDirectoryName(path) + @"\" + Path.GetFileNameWithoutExtension(path) + @".csv";

           using (StreamWriter sw = new StreamWriter(new FileStream(path,FileMode.OpenOrCreate,FileAccess.Write),Encoding.GetEncoding(1251)))
           {

                StringBuilder sb = new StringBuilder(1, 1000);

                for(int i3 = 0;i3<dt.Columns.Count;i3++)
                {
                    if (i3 != dt.Columns.Count)
                    {
                        sb.Append(dt.Columns[i3].ColumnName + "|");
                    }
                    else
                    {
                        sb.Append(dt.Columns[i3].ColumnName);
                    }                   
                }

                sw.WriteLine(sb.ToString());
                sb.Clear();

                string line;
                for (int i2 = 0; i2 < dt.Rows.Count; i2++)
                {
                    line = "";
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        
                        if (i != dt.Columns.Count-1)
                        {
                            sb.Append(Regex.Replace(dt.Rows[i2][i].ToString(), "\r|\n|\f|\t|\v", @"").ToString() + '|');
                            line += Regex.Replace(dt.Rows[i2][i].ToString(), "\r|\n|\f|\t|\v", @"").ToString() + '|';
                        }
                        else
                        {
                            sb.Append(Regex.Replace(dt.Rows[i2][i].ToString(), "\r|\n|\f|\t|\v", @"").ToString());
                            line += Regex.Replace(dt.Rows[i2][i].ToString(), "\r|\n|\f|\t|\v", @"").ToString();
                        }
                    }


                    line = line.Replace(@"\n", @"")
                        .Replace(@"\f", @"")
                        .Replace(@"\n", @"")
                        .Replace(@"\r", @"")
                        .Replace(@"\t", @"")
                        .Replace(@"\v", @"");

                    sw.WriteLine(sb.ToString());
                    //sw.WriteLine(line);
                    //sb.Remove(sb.Length - 1, 1);
                    sb.Clear();
                }
            }

           dt.Dispose();

        }

        public static void csvToXlsx(string QueryName)
        {
            foreach (QueryToFile qtf in queryToFile.hierarhyList)
            {
                if (queryToFile.getPath(QueryName) != "" && QueryName ==qtf.qrParam)
                {
                    rObject.apl = new xlNS.Application();
                    ProcessHandler_ih.processGet(rObject.apl);
                    rObject.apl.DisplayAlerts = false;

                    rObject.apl.Visible = true;

                    xlNS.Worksheet ws;
                    xlNS.Range rn;
                    xlNS.Range rn2;

                    string pathOld = queryToFile.getPath(QueryName);
                    string pathNew = pathOld.Replace(@".csv",@".xlsx");

                    xlNS.Workbook wb = rObject.apl.Workbooks.Open(pathOld, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    ws = wb.Worksheets[1];
                    rn = (xlNS.Range)ws.Range["A1"];
                    rn2 = (xlNS.Range)ws.Range["B1"];
                    char del = '|';

//vba macro tamplate
/*
Columns("A:A").Select
Selection.TextToColumns Destination:=Range("A1"), DataType:=xlDelimited, _
TextQualifier:=xlNone, ConsecutiveDelimiter:=False, Tab:=True, Semicolon _
:=False, Comma:=False, Space:=False, Other:=True, OtherChar:="|", _
FieldInfo:=Array(Array(1, 1), Array(2, 1)), TrailingMinusNumbers:=True
*/

                    rn.EntireColumn.Select();

rn.EntireColumn.TextToColumns(
rn, xlNS.XlTextParsingType.xlDelimited, xlNS.XlTextQualifier.xlTextQualifierDoubleQuote,
Type.Missing, Type.Missing,true,
Type.Missing, Type.Missing, Type.Missing,
Type.Missing,Type.Missing, Type.Missing,
Type.Missing, Type.Missing);

//Delimeter '|' try
/*
rn.EntireColumn.TextToColumns(
rn2, xlNS.XlTextParsingType.xlDelimited, xlNS.XlTextQualifier.xlTextQualifierDoubleQuote,
Type.Missing,false,false,false,false,true,'|',
Type.Missing, Type.Missing, Type.Missing, Type.Missing);
*/

                    wb.SaveAs(pathNew, xlNS.XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, xlNS.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //wb.SaveAs(pathNew);
                    wb.Close();

                    ProcessHandler_ih.processesKill();
                }
            }
            
        }
        public static void csvToXlsx(string QueryName, string Postfix)
        {
            foreach (QueryToFile qtf in queryToFile.hierarhyList)
            {
                if (queryToFile.getPath(QueryName) != "" && QueryName == qtf.qrParam)
                {
                    rObject.apl = new xlNS.Application();
                    ProcessHandler_ih.processGet(rObject.apl);
                    rObject.apl.DisplayAlerts = false;

                    rObject.apl.Visible = true;

                    xlNS.Worksheet ws;
                    xlNS.Range rn;
                    xlNS.Range rn2;

                    string pathOld = queryToFile.getPath(QueryName);
                    string pathNew = Path.GetDirectoryName(pathOld) + @"\" + Postfix + @".xlsx";
                   
                    xlNS.Workbook wb = rObject.apl.Workbooks.Open(pathOld, Type.Missing, Type.Missing, 5 , Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    ws = wb.Worksheets[1];
                    rn = (xlNS.Range)ws.Range["A1"];
                    rn2 = (xlNS.Range)ws.Range["B1"];
                    char del = '|';

                    //vba macro tamplate
                    /*
                    Columns("A:A").Select
                    Selection.TextToColumns Destination:=Range("A1"), DataType:=xlDelimited, _
                    TextQualifier:=xlNone, ConsecutiveDelimiter:=False, Tab:=True, Semicolon _
                    :=False, Comma:=False, Space:=False, Other:=True, OtherChar:="|", _
                    FieldInfo:=Array(Array(1, 1), Array(2, 1)), TrailingMinusNumbers:=True
                    */

                    rn.EntireColumn.Select();

                    rn.EntireColumn.TextToColumns(
                    rn, xlNS.XlTextParsingType.xlDelimited, xlNS.XlTextQualifier.xlTextQualifierNone,
                    Type.Missing, Type.Missing, false,
                    Type.Missing, Type.Missing, true,
                    "|", Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing);


                    //Delimeter '|' try
                    /*
                    rn.EntireColumn.TextToColumns(
                    rn2, xlNS.XlTextParsingType.xlDelimited, xlNS.XlTextQualifier.xlTextQualifierDoubleQuote,
                    Type.Missing,false,false,false,false,true,'|',
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    */

                    try
                    {
                        wb.SaveAs(pathNew, xlNS.XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing, xlNS.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                    catch(Exception e)
                    {

                    }
                    finally
                    {
                        wb.Close();

                        ProcessHandler_ih.processesKill();
                    }
                    //wb.SaveAs(pathNew);
                   
                }
            }
        }

        private static void datasetToXMLtree(string path, string[] queryNames, DateTime date)
        {
            
            TimeSpan dateTiff = new TimeSpan(10, 0, 0);
            DateTime dateFn = date.Date.AddHours(19);
            DateTime dateSt = dateFn.Date.AddDays(-1).AddHours(19);

            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();    
            
            foreach(string table  in queryNames )
            {
               foreach(DataTable dt in dataset.Tables)
               {
                   if(table==dt.TableName )
                   {
                       if(table.Contains(@"CNT"))
                       {
                            dt1 = dt;
                       }
                       else
                       {
                           dt2 = dt;
                       }                          
                   }
               }
            }          

            XElement xmlTree = new XElement("w3s.request",
                new XElement("retval", 0),
                new XElement("retdesc"),
                new XElement("startdate", dateSt.ToString("yyyyMMdd HH:mm:ss")),
                new XElement("enddate", dateFn.ToString("yyyyMMdd HH:mm:ss")),
                new XElement("reestr", new XAttribute("cnt", dt1.Rows[0][0]), new XAttribute("price", dt1.Rows[0][1]),
                    
from t in dt2.AsEnumerable() select new XElement("payment", new XAttribute("id", (decimal)t["PAYMENT"]),new XAttribute("test",0)
    //new linq elements add
    , new XElement("Price", t["PRICE"])
    , new XElement("Amount", t["AMOUNT"])
    , new XElement("Purse", t["PURSE"])
    , new XElement("Cheque", t["CHEQUE"])
    , new XElement("date", t["DATE"])
    , new XElement("KIOSK_ID", t["KIOSK_ID"])
        )
                     )
                
                );

            xmlTree.Save(path);
           
        }

        public static void fileBulkSend()
        {
            foreach (FileToMail ftm in fileToMail.hierarhyList)
            {
                string to="";
                string bodySubject = @"";               
                
                /*
                MailMessage msg = new MailMessage(@"ia-neprintsev@rsb.ru", @"ia-neprintsev@rsb.ru");

                SmtpClient client = new SmtpClient(@"EX-MB-05.rs.ru");
                client.UseDefaultCredentials = true;
                SmtpClient client2 = new SmtpClient(@"web.bank.rs.ru");
                client2.UseDefaultCredentials = true;

                msg.Body = "aaa";

                client2.Send(msg);
                 * */

                Outlook.Application apl = new Outlook.Application();
                Outlook.MailItem mail = (Outlook.MailItem)apl.CreateItem(Outlook.OlItemType.olMailItem);
                mail.Body = @"test mail";

                String sDisplayName = "Attachments";
                int iPosition = (int)mail.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;

                
                foreach(string filename in ftm.filename)
                {
                    string[] files=Directory.GetFiles(Path.GetDirectoryName(filename), @"*" + Path.GetFileNameWithoutExtension(filename) + @"*");

                    foreach(string file_ in files)
                    {
                        Outlook.Attachment oAttach = mail.Attachments.Add(file_, iAttachType, iPosition, sDisplayName);                      
                    }                    
                }               
                


                Outlook.Recipients oRecips = (Outlook.Recipients)mail.Recipients;
                Outlook.Recipient oRecip = null;
                mail.Subject = @"test_mail";

                foreach (string nm in ftm.maillist)
                {
                    oRecip = (Outlook.Recipient)oRecips.Add(nm);
                    /*
                    if (to == "")
                    {
                        to = to + nm;
                    }
                    else
                    {
                        to = to + @"," + nm;
                    }
                    */
                }
                
                oRecip.Resolve();
                mail.Send();
               
            }
        }
        public static void fileTagetSend(string filemask,string subject)
        {
          
            string bodySubject = subject;
            bool attachmentsCheck = false;
            bool file_presence = false;

            Outlook.Application apl = new Outlook.Application();
            Outlook.MailItem mail = (Outlook.MailItem)apl.CreateItem(Outlook.OlItemType.olMailItem);
            Outlook.Recipients oRecips = null;
            Outlook.Recipient oRecip = null;

            String sDisplayName = "Attachments";
            int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
            //int iPosition = (int)mail.Body.Length + 1;
            
            string to = "";
            mail.Body = @" ";

            //fileToMail = configMaskCheck(filemask);

            file_presence = configMaskCheck(filemask);

            //if (fileToMail != null){
            if (file_presence) { 

                List<string> files = configsCheck(fileToMail, filemask);
                List<string> mails = mailsCheck(fileToMail, filemask);
                List<string> filesOnDisk = filesCheck(files, filemask);

                if (files.Count!=0 && mails.Count!=0)
                {                   
                    foreach(string file_ in filesOnDisk)
                    {
                        attachmentsCheck = true;
                        Outlook.Attachment oAttach = mail.Attachments.Add(file_, iAttachType, Type.Missing, Type.Missing);
                    }                 

                    foreach (string nm in mails)
                    {
                        oRecip = mail.Recipients.Add(nm);
                    }
                }

                if(attachmentsCheck)
                {
                    mail.Subject = subject;
                    mail.Recipients.ResolveAll();              
                    mail.Send();
                }

            }

#region reg
            /*
            foreach (FileToMail ftm in fileToMail.hierarhyList)
            {               
              

                foreach (string filename in ftm.filename)
                {                                     
                    List<string> files=new List<string>();

                    if (new Regex(filemask.Replace(@"\", @"|")).Match(filename).ToString() != "")
                    {
                        if(!File.Exists(filename))
                        {
                            foreach(string str in Directory.GetFiles(Path.GetDirectoryName(filename), Path.GetFileName(filemask)))
                            {
                                files.Add(str);
                            }
                        }
                        else
                        {                            
                            files.Add(filename);
                        }

                        foreach (string file_ in files)
                        {                        

                            if (new Regex(filemask.Replace(@"\", @"|")).Match(file_.Replace(@"\", @"|")).ToString() != "")
                            {
                                attachmentsCheck = true;
                                Outlook.Attachment oAttach = mail.Attachments.Add(file_, iAttachType, iPosition, sDisplayName);
                            }
                        }
                    }                   
                }


                mail.Subject = bodySubject;

                if (attachmentsCheck)
                {
                    foreach (string nm in ftm.maillist)
                    {
                        oRecip = (Outlook.Recipient)oRecips.Add(nm);
                        
                    }

                    oRecip.Resolve();
                    mail.Send();
                }            
            } 

            */
#endregion

        }

        public static void FilesErase(string path_)
        {
            foreach(string filename in Directory.GetFiles(path_) )
            {
                File.Delete(filename);
            }
        }
        private static bool configMaskCheck(string mask)
        {
            foreach(FileToMail ftm in fileToMail.hierarhyList)
            {
                foreach(string file in ftm.filename)
                {
                    if(Regex.IsMatch(file,mask))
                    {
                        return true;
                    }
                }
            }
            return false;
        }            
        private static List<string> configsCheck(FileToMail ftm,string mask)
        {
            List<string> files = new List<string>();
            
            foreach(FileToMail fileToMail in ftm.hierarhyList)
            {
                foreach (string file in fileToMail.filename)
                {
                    if (Regex.IsMatch(file, mask))
                    {
                        files.Add(file);
                    }
                }
            }

            return files;
        }
        private static List<string> mailsCheck(FileToMail ftm, string mask)
        {
            List<string> mailList = new List<string>();
          

            foreach (FileToMail fileToMail in ftm.hierarhyList)
            {               
                foreach (string file in fileToMail.filename)
                {
                    if (Regex.IsMatch(file, mask))
                    {
                        mailList = fileToMail.maillist.ToList();
                    }
                }                        
            }

            return mailList;
        }
        private static List<string> filesCheck(List<string> files_, string mask)
        {
            List<string> filelist = new List<string>();
            foreach(string str in files_)
            {               
                foreach (string str2 in Directory.GetFiles(Path.GetDirectoryName(str)))
                {
                    if(Regex.IsMatch(str2,mask) && !filelist.Contains(str2))
                    {
                        filelist.Add(str2);
                    }
                }
                
            }

            return filelist;
        }

        private static bool epplWSExists(ExcelPackage ep_, string name_)
        {
            bool result = false;

            foreach (ExcelWorksheet ws_ in ep_.Workbook.Worksheets)
            {
                if (ws_.Name == name_)
                {
                    result = true;
                }
            }

            return result;
        }
    }
    
    public static class ProcessHandler_ih
    {
        static List<ProcessName> processName = new List<ProcessName>();

        static internal int processID;
        static internal string windowTitle;
     

        static public void processGet(xlNS.Application apl)
        {
            apl.Visible = true;
            processID = 0;
            foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcessesByName("Excel"))
            {
                if (proc.MainWindowTitle == apl.Caption)
                    processName.Add(new ProcessName(proc.Id, proc.MainWindowTitle));
            }
            apl.Visible = false;
        }
        static public void processGet(pptNS.Application apl)
        {
            processID = 0;
            foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcesses()) //.GetProcessesByName("PowerPoint"))
            {
                if (proc.MainWindowTitle == apl.Caption)
                    processName.Add(new ProcessName(proc.Id, proc.MainWindowTitle));
            }
        }

        static public void processesKill()
        {
            foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcesses())
            {
                foreach(ProcessName process in processName)
                {
                    if (process.processID == proc.Id)
                        proc.Kill();
                }
               
            }
        }
        static public void processKill(string ProcessName_)
        {                                   
            foreach (ProcessName process in processName)
            {
                if (process.processName == ProcessName_)
                {
                    foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcesses())
                    {
                        if (process.processID == processID)
                            proc.Kill();
                    }
                }
            }            
        }
        static public void processKill(int ProcessID_)
        {
            foreach (ProcessName process in processName)
            {
                if (process.processID == ProcessID_)
                {
                    foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcesses())
                    {
                        if (process.processID == processID)
                            proc.Kill();
                    }
                }
            }
        }
      
        internal class ProcessName
        {
            internal int processID;
            internal string processName;

            internal ProcessName(int processID_, string processName_)
            {
                processID = processID_;
                processName = processName_;
            }
        }
    }
  
    public enum objectTypeNames
    {
        REPORT, WB, WS, RANGE, QUERY
    }    

    public class testClass
    {
        internal int rows_;
        internal int columns_;

        public testClass()
        {
          
            //WebMoneyQiwiExp();           
            testPrimary();
        }
  
        private void testPrimary()
        {
            //OPENxmlCreate();
            //DateTime dt = new DateTime(2017, 05, 17, 00, 00, 00);
            WebMoneyQiwiExp();
        }

        private void OPENxmlCreate()
        {

            string fileName = @"C:\111\test.xlsx";

            using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Test Sheet" };

                sheets.Append(sheet);
               
                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                for (int i =3;i<=5;i++)
                {
                    row = new Row();
                    Cell cell = new Cell()
                    {
                        CellValue = new CellValue(i.ToString()),
                        DataType = new EnumValue<CellValues>(CellValues.String)
                    };

                    row.Append(cell);
                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
            }
        }

        private void WebMoneyQiwiExp(DateTime? reportDate = null)
        {

            InitializationParams.SetDateString(reportDate, @"M");

            //InitializationParams.FilesErase(@"C:\test\repbuild\TEST");
            
            InitializationParams.queryTexts.qyeryPath =
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + @"queries.txt";
            InitializationParams.queryTexts.queryParse();

            InitializationParams.InitializeLines(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + @"paths.txt");
            InitializationParams.ParseFileToLines();
            
            InitializationParams.fileToMail.paramPath =
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\" + @"mails.txt";
            InitializationParams.fileToMail.paramParse();

            InitializationParams.connectionInit(@"neprintsev_ia", @"awsedrDRSEAW");
            //InitializationParams.connectionInit(InitializationParams.login, InitializationParams.password);

            NameParameter nameParameter = new NameParameter();

            nameParameter.hierarhyList.Add(new NameParameter(@":param1", InitializationParams.reportDate.ToString()));

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            //export to EPPLUS
            //InitializationParams.datasetParametrizedFill(@"TEST_QR", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExportEPPL(@"TEST_QR", @"TEST_QR");

            //MONITORING NEW

            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"MONITORING_ROS", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"MONITORING_ROS", @"MONITORING_ROS");
            //InitializationParams.dataSetDispose();

            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"MONITORING_VOR", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"MONITORING_VOR", @"MONITORING_VOR");
            //InitializationParams.dataSetDispose();

            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"MONITORING_NOV", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"MONITORING_NOV", @"MONITORING_NOV");
            //InitializationParams.dataSetDispose();

            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"MONITORING_MSC", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"MONITORING_MSC", @"MONITORING_MSC");
            //InitializationParams.dataSetDispose();

            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"MONITORING_KAZ", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"MONITORING_KAZ", @"MONITORING_KAZ");
            //InitializationParams.dataSetDispose();

            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"MONITORING_EKT", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"MONITORING_EKT", @"MONITORING_EKT");
            //InitializationParams.dataSetDispose();

            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"MONITORING_SPB", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"MONITORING_SPB", @"MONITORING_SPB");
            //InitializationParams.dataSetDispose();

            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"MONITORING_DE", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"MONITORING_DE", @"MONITORING_DE");
            //InitializationParams.dataSetDispose();


            //InitializationParams.csvToXlsx(@"MONITORING_DE", @"MTR_DE");
            //InitializationParams.csvToXlsx(@"MONITORING_SPB", @"MTR_SPB");
            //InitializationParams.csvToXlsx(@"MONITORING_EKT", @"MTR_EKT");
            //InitializationParams.csvToXlsx(@"MONITORING_KAZ", @"MTR_KAZ");
            //InitializationParams.csvToXlsx(@"MONITORING_MSC", @"MTR_MSC");
            //InitializationParams.csvToXlsx(@"MONITORING_VOR", @"MTR_VOR");
            //InitializationParams.csvToXlsx(@"MONITORING_ROS", @"MTR_ROS");
            //InitializationParams.csvToXlsx(@"MONITORING_NOV", @"MTR_NOV");


            //ACQ Weekly
            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"ACQ_FULL_WEEKLY", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"ACQ_FULL_WEEKLY", @"ACQ_FULL_WEEKLY");
            //InitializationParams.csvToXlsx(@"ACQ_FULL_WEEKLY", @"ACQ_FULL_WEEKLY" + @"_" + InitializationParams.dateString);
            ////InitializationParams.fileTagetSend(@"ACQ_FULL_WEEKLY" + @"(.+)xlsx", "ACQ_FULL_WEEKLY_" + InitializationParams.dateString);

            ////ACQ Monthly
            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"ACQ_FULL_MONTHLY", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"ACQ_FULL_MONTHLY", @"ACQ_FULL_MONTHLY");
            //InitializationParams.csvToXlsx(@"ACQ_FULL_MONTHLY", @"ACQ_FULL_MONTHLY_" + InitializationParams.dateString);
            //InitializationParams.fileTagetSend(@"ACQ_FULL_MONTHLY(.*).xlsx", "ACQ_FULL_MONTHLY_" + InitializationParams.dateString);
            //InitializationParams.dataSetDispose();

            //QIWI
            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"QIWI", nameParameter.hierarhyList);
            //InitializationParams.datatableParametrizedExport(@"QIWI", @"C:\test\repbuild\TEST\QIWI_" + InitializationParams.dateString + @"_.xlsx");
            //InitializationParams.fileTagetSend(@"QIWI", "QIWI");

            //webmoney
            ////fills dataset with qurey with parameter list
            //InitializationParams.dataSetCreate();
            //nameParameter.hierarhyList.Add(new NameParameter(@":param2", @"V"));
            //InitializationParams.datasetParametrizedFill(@"WEB_MONEY_CNT_PS", nameParameter.hierarhyList);
            //InitializationParams.datasetParametrizedFill(@"WEB_MONEY_PS", nameParameter.hierarhyList);
            //InitializationParams.xmlTree(@"C:\test\repbuild\TEST\VISA_" + InitializationParams.dateString + "_.xml", new string[] { @"WEB_MONEY_CNT_PS", @"WEB_MONEY_PS" }, InitializationParams.reportDate);
            //nameParameter.hierarhyList[nameParameter.hierarhyList.FindIndex(c => c.Name == @":param2")].Parameter = @"M";
            //InitializationParams.datasetParametrizedFill(@"WEB_MONEY_CNT_PS", nameParameter.hierarhyList);
            //InitializationParams.datasetParametrizedFill(@"WEB_MONEY_PS", nameParameter.hierarhyList);
            //InitializationParams.xmlTree(@"C:\test\repbuild\TEST\MC_" + InitializationParams.dateString + "_.xml", new string[] { @"WEB_MONEY_CNT_PS", @"WEB_MONEY_PS" }, InitializationParams.reportDate);
            //nameParameter.hierarhyList[nameParameter.hierarhyList.FindIndex(c => c.Name == @":param2")].Parameter = @"A";
            //InitializationParams.datasetParametrizedFill(@"WEB_MONEY_CNT_PS", nameParameter.hierarhyList);
            //InitializationParams.datasetParametrizedFill(@"WEB_MONEY_PS", nameParameter.hierarhyList);
            //InitializationParams.xmlTree(@"C:\test\repbuild\TEST\AMEX_" + InitializationParams.dateString + "_.xml", new string[] { @"WEB_MONEY_CNT_PS", @"WEB_MONEY_PS" }, InitializationParams.reportDate);
            //InitializationParams.fileTagetSend(@"AMEX|VISA|MC", "Web money");

            //ACQ_template
            /*
            InitializationParams.datasetParametrizedFill(@"ACQ_TEMPLATE", nameParameter.hierarhyList);
            InitializationParams.datatableParametrizedExport(@"ACQ_TEMPLATE", @"C:\test\repbuild\TEST\ACQ_TEMPLATE" + dateString + @".xlsx");
            InitializationParams.fileTagetSend(@"ACQ_TEMPLATE", "МТМ Шаблон " + dateString);
            */

            //ACQ_template
            /*
            InitializationParams.datasetParametrizedFill(@"MCD_CTL", nameParameter.hierarhyList);
            InitializationParams.datatableParametrizedExport(@"MCD_CTL", @"C:\test\repbuild\TEST\MCD_CTL_" + dateString + @".xlsx");

            InitializationParams.datasetParametrizedFill(@"MCD_SLIP", nameParameter.hierarhyList);
            InitializationParams.datatableParametrizedExport(@"MCD_SLIP", @"C:\test\repbuild\TEST\MCD_SLIP_" + dateString + @".xlsx");

            InitializationParams.datasetParametrizedFill(@"MCD_ACQ", nameParameter.hierarhyList);
            InitializationParams.datatableParametrizedExport(@"MCD_ACQ", @"C:\test\repbuild\TEST\MCD_ACQ_" + dateString + @".xlsx");            

            InitializationParams.fileTagetSend(@"MCD_", "Macdonalds " + dateString);
            InitializationParams.rObjectPrent.Dispose();
            */

            //CHANNELS
            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"CHANNELS_M", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"CHANNELS_M", @"CHANNELS_M");
            //InitializationParams.csvToXlsx(@"CHANNELS_M", @"CHANNELS_M_" + InitializationParams.dateString);
            //InitializationParams.fileTagetSend(@"CHANNELS_M.*xlsx", "CHANNELS_M");
            //InitializationParams.dataSetDispose();

            ////ECOMM FIRST
            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"ECOMM_DEBUT", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"ECOMM_DEBUT", @"ECOMM_DEBUT");
            //InitializationParams.csvToXlsx(@"ECOMM_DEBUT", @"ECOMM_DEBUT_" + InitializationParams.dateString);
            //InitializationParams.fileTagetSend(@"ECOMM_DEBUT(.*).xlsx", "ECOMM_DEBUT");
            //InitializationParams.dataSetDispose();

            //BANKS_ULM
            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"BANK_ULM", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"BANK_ULM", @"BANK_ULM");
            //InitializationParams.csvToXlsx(@"BANK_ULM", @"BANK_ULM_" + InitializationParams.dateString);
            //InitializationParams.fileTagetSend(@"BANK_ULM(.*).xlsx", "BANK_ULM");
            //InitializationParams.dataSetDispose();

            //BANKS_BOSCO
            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"BANK_BOSCO", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"BANK_BOSCO", @"BANK_BOSCO");
            //InitializationParams.csvToXlsx(@"BANK_BOSCO", @"BANK_BOSCO_" + InitializationParams.dateString);
            //InitializationParams.fileTagetSend(@"BANK_BOSCO(.*).xlsx", "BANK_BOSCO");
            //InitializationParams.dataSetDispose();

            //INTES
            InitializationParams.dataSetCreate();
            InitializationParams.datasetParametrizedFill(@"OD_INTES", nameParameter.hierarhyList);
            InitializationParams.StreamWriterExport(@"OD_INTES", @"");
            InitializationParams.csvToXlsx(@"OD_INTES", @"INTES_" + InitializationParams.dateString);
            InitializationParams.fileTagetSend(@"INTES_(.*).xlsx", "INTES");
            InitializationParams.dataSetDispose();

            //GNS
            //InitializationParams.dataSetCreate();
            //InitializationParams.datasetParametrizedFill(@"OD_GNS", nameParameter.hierarhyList);
            //InitializationParams.StreamWriterExport(@"OD_GNS", @"");
            //InitializationParams.csvToXlsx(@"OD_GNS", @"GNS_" + InitializationParams.dateString);
            //InitializationParams.fileTagetSend(@"GNS_(.*).xlsx", "GNS");
            //InitializationParams.dataSetDispose();


            watch.Stop();

            long res = watch.ElapsedMilliseconds / 1000;
        }



    }

}
