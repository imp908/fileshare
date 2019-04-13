using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Data;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TransactionSite_.Models
{
    public class ServiceLayer
    {

        public bool initialized = false;

        public ServiceReference1.Service1Client client;
     
        public List<QueryState> tableList = new List<QueryState>();
        public IEnumerable<QueryState> tableEnum { get; set; }
        public QueryState tableState;

        public ServiceLayer()
        {
            init();
        }

        public void init_() 
        {
            var builder = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
        }

        public void init()
        {
            
            result = "";
            client = new ServiceReference1.Service1Client();
            DateFrom = DateTime.Now.AddDays(-3).Date;
            DateTo = DateFrom.AddDays(2).AddMilliseconds(-1);

            tableList.Add(new QueryState
            {
                ID = 0,
                TableName = TableNames.tablenames["FD"],
                DateFrom = this.DateFrom.AddDays(-10),
                DateTo = this.DateTo.AddDays(-5),
                Status = status.IDLE
            });

            tableList.Add(new QueryState
            {
                ID = 1,
                TableName = TableNames.tablenames["REF"],
                DateFrom = this.DateFrom,
                DateTo = this.DateTo,
                Status = status.IDLE
            });

            tableEnum = tableList.AsEnumerable();
            this.initialized = true;
        }

        public string result { get; set; }
        
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
       
        public void migrate(string tablename, DateTime dateFrom, DateTime dateTo)
        {
            if (tableList.Select(s => s.TableName == tablename) != null)
            {
                Connections.migrate(tablename, dateFrom, dateTo);
            }
        }
        public string GetTableName (int ID)
        {
            if (this.tableList.Count != 0)
            {
                return this.tableList.Where(s => s.ID == ID).First().TableName;
            }
            else { return "Lsit is empty"; }     
        }
        public void UpdateModelEnum(int ID,DateTime? dateFrom, DateTime? dateTo)
        {
            this.tableEnum.Where(s => s.ID == ID).First().DateFrom = dateFrom;
            this.tableEnum.Where(s => s.ID == ID).First().DateTo = dateTo;
        }
        public void UpdateModel(int ID, DateTime? dateFrom, DateTime? dateTo)
        {
            this.tableList.Where(s => s.ID == ID).First().DateFrom = dateFrom;
            this.tableList.Where(s => s.ID == ID).First().DateTo = dateTo;
        }

    }

    public static class Connections
    {
        public static OracleConnection ORCLconn = new OracleConnection();
        public static SqlConnection SQLconn = new SqlConnection();

        public static List<StringsToCommand> stringsToCommand = new List<StringsToCommand>();

        public static string ORCLconnectionString;
        private static string SQLconnectionString;

        private static string ORCLdeleteCommand;
        private static string SQLdeleteCommand;

        private static string ORCLselectCommand;
        private static string SQLselectCommand;

        static Connections()
        {

            ORCLconnectionString = @"User Id=:userID; password = :password; Data Source = dwh.rs.ru; Pooling=false;";
            SQLconnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=DWH_replica;Integrated Security=SSPI;";

            stringsToCommand.Add(new StringsToCommand
            {
                TableName = TableNames.tablenames["FD"]
                ,SQLDeleteCommand = @"delete from DWH_replica.dbo.FD_ACQ_D
where dt_reg between DATEADD(dd,DATEDIFF(dd,0,@DateFrom),0)  and DATEADD(dd,DATEDIFF(dd,0,@DateTo),0)"
                ,SQLInsertCommand = @"insert into  DWH_replica.dbo.FD_ACQ_D 
                    (dt_trn ,dt_reg ,acquire_bank ,pay_sys , 
                    issuer_type ,type_transaction ,tran_name ,
                    merchantID ,terminal_type , is_linked  , 
                    amt , fee ,cnt )values(
                    @dt_trn ,@dt_reg ,@acquire_bank ,@pay_sys , 
                    @issuer_type ,@type_transaction ,@tran_name ,
                    @merchantID ,@terminal_type , @is_linked  , 
                    @amt , @fee ,@cnt)"
                ,
                ORCLSelectCommand = @"select * FROM vitr.V$SLIP_ACQ_d@db_link S 
where trunc(s.dt_reg,'dd') between trunc(:DateFrom,'dd') and trunc(:DateTo,'dd')"
            });

            stringsToCommand.Add(new StringsToCommand
            {
                TableName = TableNames.tablenames["REF"]
                ,SQLDeleteCommand = @"delete from DWH_replica.dbo.REFMERCHANTS 
where dt_reg between DATEADD(dd,DATEDIFF(dd,0,@DateFrom),0)  and DATEADD(dd,DATEDIFF(dd,0,@DateTo),0)"
                ,SQLInsertCommand = @"insert into  DWH_replica.dbo.REFMERCHANTS
                    (merchant ,parent ,abrv_name ,full_name,
                    mcc ,RC ,street ,city,reg_nr,region)
                    values (@merchant ,@parent ,@abrv_name ,@full_name ,
                    @mcc ,@RC ,@street ,@city,@reg_nr,@region)"
               ,ORCLSelectCommand = @"select * FROM neprintsev_ia.ref_merchants"
            });
        }

        public static void migrate( string tableName,DateTime dateFrom,DateTime dateTo)
        {
            StringsToCommand st = Connections.stringsToCommand.Where(s => s.TableName == tableName).FirstOrDefault();
            ORCLconn.ConnectionString=Connections.ORCLconnectionString
                .Replace(@":userID", @"neprintsev_ia")
                .Replace(@":password", @"awsedrDRSEAW");
            ORCLconn.Open();

            SQLconn.ConnectionString = @"data source=.\SQLEXPRESS;initial catalog=DWH_REPLICA;integrated security=True;";
            SQLconn.Open();

            OracleCommand oSelcomm = new OracleCommand(st.ORCLSelectCommand, ORCLconn);
            SqlCommand sDelcomm = new SqlCommand(st.SQLDeleteCommand,SQLconn);
            SqlCommand sInscomm = new SqlCommand(st.SQLInsertCommand, SQLconn);

            oSelcomm.Parameters.Add("DateFrom", OracleDbType.Date);
            oSelcomm.Parameters.Add("DateTo", OracleDbType.Date);
            oSelcomm.Parameters["DateFrom"].Value = dateFrom;
            oSelcomm.Parameters["DateTo"].Value = dateTo;

            sDelcomm.Parameters.Add("DateFrom", SqlDbType.DateTime);
            sDelcomm.Parameters.Add("DateTo", SqlDbType.DateTime);
            sDelcomm.Parameters["DateFrom"].Value = dateFrom;
            sDelcomm.Parameters["DateTo"].Value = dateTo;
          
            if (Connections.stringsToCommand.Select(s => s.TableName == tableName) != null)
            {

                sDelcomm.ExecuteNonQuery();
                OracleDataReader reader = oSelcomm.ExecuteReader();
                SQL_CONTEXT db = new SQL_CONTEXT();
                

                if (tableName == "FD_ACQ_D"){

                    sInscomm.Parameters.Add("dt_trn", SqlDbType.Date);
                    sInscomm.Parameters.Add("dt_reg", SqlDbType.Date);
                    sInscomm.Parameters.Add("acquire_bank", SqlDbType.Char);
                    sInscomm.Parameters.Add("pay_sys", SqlDbType.Char);
                    sInscomm.Parameters.Add("issuer_type", SqlDbType.Char);
                    sInscomm.Parameters.Add("type_transaction", SqlDbType.Char);
                    sInscomm.Parameters.Add("tran_name", SqlDbType.Char);
                    sInscomm.Parameters.Add("merchantID", SqlDbType.Char);
                    sInscomm.Parameters.Add("terminal_type", SqlDbType.Char);
                    sInscomm.Parameters.Add("is_linked", SqlDbType.Char);
                    sInscomm.Parameters.Add("amt", SqlDbType.Decimal);
                    sInscomm.Parameters.Add("fee", SqlDbType.Decimal);
                    sInscomm.Parameters.Add("cnt", SqlDbType.BigInt);
                    
                }
                while (reader.Read())
                {
                    var b = reader[12].GetType();

                    sInscomm.Parameters["dt_trn"].Value=(DateTime)reader[0];
                    sInscomm.Parameters["dt_reg"].Value = (DateTime)reader[1];
                    sInscomm.Parameters["acquire_bank"].Value = (string)reader[2];
                    sInscomm.Parameters["pay_sys"].Value = (string)reader[3];
                    sInscomm.Parameters["issuer_type"].Value = (string)reader[4];
                    sInscomm.Parameters["type_transaction"].Value = (string)reader[5];
                    sInscomm.Parameters["tran_name"].Value = (string)reader[6];
                    sInscomm.Parameters["merchantID"].Value = (string)reader[7];
                    sInscomm.Parameters["terminal_type"].Value = (string)reader[8];
                    sInscomm.Parameters["is_linked"].Value = (string)reader[9];
                    sInscomm.Parameters["amt"].Value = (double)reader[10];
                    sInscomm.Parameters["fee"].Value = (double)reader[11];
                    sInscomm.Parameters["cnt"].Value = (Int64)reader[12];
                    sInscomm.ExecuteNonQuery();
                }
            }
            
        }

    }

    public class QueryState
    {
        [Required, DisplayName("ID")]
        public int ID { get; set; }
        public string TableName { get; set; }
        [Required, DisplayName("DateFrom")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DateFrom { get; set; }
        [Required, DisplayName("DateTo")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DateTo { get; set; }
        public status Status { get; set; }
    }
    public class StringsToCommand
    {
        public string TableName { get; set; }
        public string SQLDeleteCommand { get; set; }
        public string SQLInsertCommand { get; set; }
        public string ORCLSelectCommand { get; set; }
    }

    public static class TableNames
    {
        public static Dictionary<string, string> tablenames = new Dictionary<string, string>();
        static TableNames()
        {
            tablenames.Add("FD", "FD_ACQ_D");
            tablenames.Add("REF", "REFMERCHANT");
        }
    }

        
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }
        
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            bool isValidName = false;
            string keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
            {
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }
    
    public enum status { IDLE,UPDATING,STOPPED }
}