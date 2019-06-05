using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq.Expressions;
using System.IO;
using Presentation_.DAL;
using Newtonsoft.Json;
using System.Reflection;
using OfficeOpenXml;

using System.Net;
using System.Net.Mail;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Presentation_.DAL;

using Repo;

using Repo.DAL;

namespace Presentation_.Models
{

    //>>!!! delete/ Methods -> to execution, IQueryable -> to Execution; Execution -> to Unit Of Work;
    public class MainModel
    {
        public List<FD_ACQ_DTO> FD_ACQ { get; set; }
        public IQueryable<IDates> entity { get; set; }

        public List<string> paramsSelected = new List<string>();

        //>>!!! remove - not model methods? all rewrote in Execution class
        //Get data from DB with predefine LINQ by paramteters from JS POSTED parameters
        public void GetJSONbyParams(DWH_REPLICAEntities db, DateTime dateFrom, DateTime dateTo, string dateType, bool checkBox, string selectedList)
        {
            DateTime fromDate = dateFrom.Date;
            DateTime toDate = dateTo.Date;
            try {
                paramsSelected = JsonConvert.DeserializeObject<List<string>>(selectedList);
            }
            catch (Exception e)
            {
                
            }
            FD_ACQ = new List<FD_ACQ_DTO>();

            FIELDS_REF.paramsInit();
            FIELDS_REF.paramsCheck(paramsSelected);

            LINQ_QUERIES.db = db;
            LINQ_QUERIES.fromDate = dateFrom;
            LINQ_QUERIES.toDate = dateTo;

            switch (dateType)
            {
                case "DAY":
                    this.FD_ACQ = LINQ_QUERIES.func_d.Invoke(db).ToList();
                    break;
                case "MONTH":
                    this.FD_ACQ = LINQ_QUERIES.func_m.Invoke(db).ToList();
                    break;
                case "YEAR":
                    this.FD_ACQ = LINQ_QUERIES.func_y.Invoke(db).ToList();
                    break;
            }

            /*
            this.FD_ACQ_D = a.ToList();
            this.FD_ACQ_D = compiledQuery_1.Invoke(db, "9290572320").ToList();
            this.FD_ACQ_D = compiledQuery_m.Invoke(db).ToList();

            foreach (var b in a)
            {
                this.FD_ACQ_D.Add(new FD_RES { DT_REG = b.DT_REG, TYPE_TRANSACTION = b.TYPE_TRANSACTION, AMT = b.AMT, PAY_SYS = b.PAY_SYS });
            }
            */

        }
        //Get data from DB with predefine LINQ by paramteters from MVC POSTED parameters class
        public void GetJSONbyParams(DWH_REPLICAEntities db, DateTime dateFrom, DateTime dateTo, string dateType, bool checkBox, List<string> selectedList, string entityType)
        {
            DateTime fromDate = dateFrom.Date;
            DateTime toDate = dateTo.Date;
            try
            {
                paramsSelected = selectedList;
            }
            catch (Exception e)
            {

            }

            FD_ACQ = new List<FD_ACQ_DTO>();

            FIELDS_REF.paramsInit();
            FIELDS_REF.paramsCheck(paramsSelected);

            LINQ_QUERIES.db = db;
            LINQ_QUERIES.fromDate = dateFrom;
            LINQ_QUERIES.toDate = dateTo;

            Parameters pm = new Parameters();

            pm.dateFrom = dateFrom;
            pm.dateTo = dateTo;
            pm.formatSelcted = dateType;
            pm.listInclude = checkBox;
            pm.ParametersPublish = selectedList;

            switch (dateType)
            {
                case "DAY":
                    this.FD_ACQ = LINQ_QUERIES.func_d.Invoke(db).ToList();
                    break;
                case "MONTH":
                    this.FD_ACQ = LINQ_QUERIES.func_m.Invoke(db).ToList();
                    break;
                case "YEAR":
                    this.FD_ACQ = LINQ_QUERIES.func_y.Invoke(db).ToList();
                    break;
            }

            int count = FD_ACQ_GROUP.list_group(pm).Count();

            /*
            this.FD_ACQ_D = a.ToList();
            this.FD_ACQ_D = compiledQuery_1.Invoke(db, "9290572320").ToList();
            this.FD_ACQ_D = compiledQuery_m.Invoke(db).ToList();

            foreach (var b in a)
            {
                this.FD_ACQ_D.Add(new FD_RES { DT_REG = b.DT_REG, TYPE_TRANSACTION = b.TYPE_TRANSACTION, AMT = b.AMT, PAY_SYS = b.PAY_SYS });
            }
            */

        }
        //export request result (list of model class) to Excel
        public void ExportToExcel(FileInfo newFile)
        {
            using (ExcelPackage p = new ExcelPackage(newFile))
            {
                Dictionary<string, object> attribs = new Dictionary<string, object>();
                OfficeOpenXml.ExcelWorksheet ws = p.Workbook.Worksheets.Add("Sheet1");
                Type t = typeof(FD_ACQ_DTO); //FD_ACQ_D_ent.GetType();
                PropertyInfo[] propertyInfo = t.GetProperties();
                int row = 1;
                int col = 1;
                ws.Cells[1, 1].Value = 1;

                //headers 
                foreach (PropertyInfo propertyInfo_ in propertyInfo)
                {
                    //if not standard aggregated column, defined by parameters
                    if (!GetAttrValue(propertyInfo_))
                    {
                        //if selected by user for display by POSTED var
                        if (paramsSelected.Contains(propertyInfo_.Name))
                        {
                            ws.Cells[row, col].Value = propertyInfo_.Name;
                            col += 1;
                        }
                    }
                    else
                    {
                        ws.Cells[row, col].Value = propertyInfo_.Name;
                        col += 1;
                    }
                }

                //rows
                foreach (FD_ACQ_DTO r in FD_ACQ)
                {
                    col = 1;
                    row += 1;
                    foreach (PropertyInfo propertyInfo_ in propertyInfo)
                    {
                        //if not standard aggregated column, defined by parameters
                        if (!GetAttrValue(propertyInfo_))
                        {
                            //if selected by user for display by POSTED var
                            if (paramsSelected.Contains(propertyInfo_.Name))
                            {
                                ws.Cells[row, col].Value = r[propertyInfo_.Name];
                                col += 1;
                            }
                        }else{
                            ws.Cells[row, col].Value = r[propertyInfo_.Name];
                            col += 1;
                        }
                    }
                }

                p.Save();
            }
        }
        //check if class property has attribute with true value
        public bool GetAttrValue(System.Reflection.PropertyInfo propertyInfo_)
        {
            bool result = false;

            IList<System.Reflection.CustomAttributeData> attributeData = propertyInfo_.GetCustomAttributesData();
            foreach (System.Reflection.CustomAttributeData attributeData_ in attributeData)
            {
                string typeName = attributeData_.Constructor.DeclaringType.Name;
                if (typeName.EndsWith("Attribute")) typeName = typeName.Substring(0, typeName.Length - 9);
                IList<CustomAttributeNamedArgument> namedArgument = attributeData_.NamedArguments;
                //attribs[typeName]
                foreach (System.Reflection.CustomAttributeNamedArgument namedArgument_ in namedArgument)
                {
                    if (namedArgument_.TypedValue.Value.GetType().Equals(typeof(Boolean)))
                    {
                        result = (bool)namedArgument_.TypedValue.Value;
                    }
                }
            }

            return result;
        }

        [Obsolete]
        public void GetACQ(DWH_REPLICAEntities db)
        {

            DateTime fromDate = new DateTime(2016, 09, 02).Date;
            DateTime toDate = new DateTime(2016, 09, 03).Date;

            FD_ACQ = new List<FD_ACQ_DTO>();

            //test check
            //IEnumerable<FD_ACQ_D> a = from s in db.FD_ACQ_D where s.ID >= 496912 && s.ID <= 496977 select s;

            //aggregate sum
            //db.FD_ACQ_D.Where(s => DbFunctions.TruncateTime(s.DT_REG) >= fromDate
            //&& DbFunctions.TruncateTime(s.DT_REG) <= toDate)
            //.GroupBy(s => new { s.DT_REG, s.TYPE_TRANSACTION })
            //.Select(s => new FD_RES { AMT = s.Sum(g => g.AMT), FEE = s.Sum(g => g.FEE) }).ToList();

            var a =
                from s in db.FD_ACQ_D
                where DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, 01, 0, 0, 0) ==
                DbFunctions.CreateDateTime(fromDate.Year, fromDate.Month, 01, 0, 0, 0)
                group s by new
                {
                    DT_REG = DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, 01, 0, 0, 0),
                    TYPE_TRANSACTION = s.TYPE_TRANSACTION,
                    PAY_SYSTEM = s.PAY_SYS
                } into g
                select new
                {
                    DT_REG = g.Key.DT_REG,
                    TYPE_TRANSACTION = g.Key.TYPE_TRANSACTION,
                    PAY_SYSTEM = g.Key.PAY_SYSTEM,
                    AMT = g.Sum(s => s.AMT)
                };


            foreach (var b in a)
            {
                this.FD_ACQ.Add(new FD_ACQ_DTO { DT_REG = b.DT_REG, TYPE_TRANSACTION = b.TYPE_TRANSACTION, AMT = b.AMT, PAY_SYS = b.PAY_SYSTEM });
            }

        }
        public void GetACQbyMonth(DWH_REPLICAEntities db, DateTime dateFrom, DateTime dateTo)
        {

            DateTime fromDate = dateFrom.Date;
            DateTime toDate = dateTo.Date;

            FD_ACQ = new List<FD_ACQ_DTO>();

            var a =
                from s in db.FD_ACQ_D
                where DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, 01, 0, 0, 0) ==
                DbFunctions.CreateDateTime(fromDate.Year, fromDate.Month, 01, 0, 0, 0)
                group s by new
                {
                    DT_REG = DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, 01, 0, 0, 0)
                    ,
                    TYPE_TRANSACTION = s.TYPE_TRANSACTION,
                    PAY_SYSTEM = s.PAY_SYS
                } into g
                select new
                {
                    DT_REG = g.Key.DT_REG,
                    TYPE_TRANSACTION = g.Key.TYPE_TRANSACTION,
                    PAY_SYSTEM = g.Key.PAY_SYSTEM,
                    AMT = g.Sum(s => s.AMT)
                };

            foreach (var b in a)
            {
                this.FD_ACQ.Add(new FD_ACQ_DTO { DT_REG = b.DT_REG, TYPE_TRANSACTION = b.TYPE_TRANSACTION, AMT = b.AMT, PAY_SYS = b.PAY_SYSTEM });
            }

        }
    }

    //paramteters for page DOM elements
    public class Parameters
    {
        [Column(TypeName = "Date"), DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        public DateTime dateFrom { get; set; }
        [Column(TypeName = "Date"), DisplayFormat(DataFormatString = "{0:dd/MM/yy}")]
        public DateTime dateTo { get; set; }

        public IEnumerable<SelectListItem> selectedDateFormat { get; set; }
        public IEnumerable<SelectListItem> selectedParameters { get; set; }
        public IEnumerable<SelectListItem> selectedEntity { get; set; }

        public List<string> dateFormatPublish { get; set; }
        public List<string> ParametersPublish { get; set; }
        public List<string> entityPublish { get; set; }

        public string entitySelected { get; set; }
        public bool listInclude { get; set; }
        public string formatSelcted { get; set; }

        public string listCount { get; set; }

        public Parameters()
        {
            dateFrom = DateTime.Now;
            dateTo = DateTime.Now;
            dateFormatPublish = new List<string> { "DAY", "MONTH", "YEAR" }; //{ "d.m.y", "dd.mm.yy", "dd/mm/yy", "M-yy" };
            entityPublish = new List<string> { "ACQ", "MT", "ECOMM" };
            PropertyRepo.propertyInitilize(typeof(FD_ACQ_D));
            entitySelected = typeof(FD_ACQ_D).ToString();
            ParametersPublish = (from s in PropertyRepo.properyList select s.propertyName).ToList(); //new List<string> { "MERCHANT", "PAY_SYS", "TYPE_TRANSACTION", "DT_REG", "ALL", "none" };
            listCount = QueriesToDbSet.GetList();

            selectedDateFormat = from s in dateFormatPublish select new SelectListItem { Text = s.ToString() };
            selectedParameters = from s in ParametersPublish select new SelectListItem { Text = s.ToString() };
            selectedEntity = from s in entityPublish select new SelectListItem { Text = s.ToString() };
        }

        //get selected entity, if not - default entity ACQ
        public string EntityReturn {
            get {
                string result = "ACQ";
                if((from s in selectedEntity where s.Selected == true select s).Any())
                {
                    result = (from s in selectedEntity where s.Selected == true select s).First().ToString();
                }
                return result;
            }
        }

        //return selected date format, if not - default DAY
        public string DateFormatReturn {
            get {
                string result = "DAY";
                if ((from s in selectedDateFormat where s.Selected == true select s).Any())
                {
                    result = (from s in selectedDateFormat where s.Selected == true select s).First().ToString();
                }
                return result;
            }
        }
    }

    //>>!!! -> to Unit Of Work
    public class Execution
    {
        //>>!!! replace with IQueryable, parameters and context create in every query;
        MainModel mm=null;
        Parameters pm = null;
        DWH_REPLICAEntities db = null;
        Type queriedType = null;

        public MainModel mainmodel { get { return mm; } set {mm=value; } }
        public Parameters parameters { get { return pm; } set { pm = value; } }

        public Execution()
        {

        }
        public Execution(DWH_REPLICAEntities db_=null, MainModel mm_ = null,Parameters pm_ = null)
        {
            if(db_!=null)
            {
                this.db = db_;
            }
            if (mm_ != null)
            {
                mainmodel = mm_;
            }
            if(pm_!=null)
            {
                parameters = pm_;
            }
        }


        /// <summary>
        /// passes context and parameters input from view to parsing method
        /// stores result in MainModel entity
        /// </summary>
        /// <param name="db_"></param>
        /// <param name="pm_"></param>
        public void QueryFromParameters(DWH_REPLICAEntities db_, Parameters pm_)
        {
            this.mm.entity = ConditionalQuering(db_, pm_);
        }
        public void ExcelExport(string filename_, MainModel mm_)
        {
            
            using (ExcelPackage e = new ExcelPackage(new FileInfo(filename_)))
            {
                queriedType = mm_.entity.GetType();
                int amount = mm_.entity.Count();
                int sheetVolume = 100000; //1048575;
                int wsCount = (int)Math.Ceiling((double)(amount / sheetVolume));

                for (int i = 0; i <= wsCount; i++)
                {
                    //ws presence check
                    if(e.Workbook.Worksheets.Where(s=>s.Name== "Sheet" + i).Any())
                    {                      
                        e.Workbook.Worksheets.Delete(e.Workbook.Worksheets.Where(s => s.Name == "Sheet" + i).FirstOrDefault());
                    }
                    ExcelWorksheet ws = e.Workbook.Worksheets.Add("Sheet" + i);
                    ws.Cells[1, 1].Value = "Test export";
                    QueryPaging(ws, mm_.entity, i, sheetVolume);
                }

                e.Save();
            }
        }
        //>>!!! to methods
        public void QueryPaging(ExcelWorksheet ws_, IQueryable<Ientity> item, int sheet, int volume)
        {
            //print body
            IQueryable<Ientity> gItem = item.Select(s => s).OrderBy(s => s.ID).Skip(sheet * volume).Take(volume);
            QueryPopulate(gItem, ws_);
        }
        public void QueryPopulate(IQueryable<Ientity> item_, ExcelWorksheet ws_)
        {
            int i = 2;

            switch (this.pm.entitySelected)
            {              
                case "ACQ":
                    foreach(FD_ACQ_D ent in item_)
                    {
                        ws_.Cells[i, 1].Value = ent.DT_REG;
                        ws_.Cells[i, 2].Value = ent.MERCHANT;
                        ws_.Cells[i, 3].Value = ent.AMT;
                        i +=1;
                    }
                break;
                case "ECOMM":
                    foreach (FD_ACQ_M ent in item_)
                    {
                        ws_.Cells[i, 2].Value = ent.DT_REG;
                        ws_.Cells[i, 3].Value = ent.MERCHANT;
                        ws_.Cells[i, 4].Value = ent.AMT;
                        i += 1;
                    }
                    break;
            }                       
        }

        public Type GetIqueryableType<T>(IQueryable<T> item)
        {
            Type type = null;
            if (item.Select(s => s).Any())
            {
                type = item.Select(s => s).First().GetType();
            }
            return type;
        }
        public PropertyInfo[] GetTypeProperties(Type item)
        {
            PropertyInfo[] result = null;
            result = item.GetProperties();
            return result;
        }
        //>>??? generic list create
        public IList<T> CreateList<T>(Type type)
        {
            IList<T> result = null;

            Type listGenericType = typeof(IQueryable<>);
            Type list = listGenericType.MakeGenericType(type);
            ConstructorInfo ci = list.GetConstructor(new Type[] { });
            result = (IList<T>)ci.Invoke(new object[] { });

            return result;
        }
        public void CreateGenericList(string type_)
        {
            Type elementType = Type.GetType(type_);
            Type listType = typeof(IEnumerable<>).MakeGenericType(new Type[] { elementType });
            object list = Activator.CreateInstance(listType);
        }

        //
        public string GO(Parameters pm)
        {
            this.QueryFromParameters(db,pm);
            return this.ObjectToJSON(mm.entity);
        }

        //Exports class as JSON string
        public string ObjectToJSON<T>(T item) where T : class
        {
            string result = "";
            JsonConvert.SerializeObject(item);
            return result;
        }

        //Quering agains DB according to user input
        //>>??? Find GP realization of different queries in model  according to user select in view
        private IQueryable<IDates> ConditionalQuering(DWH_REPLICAEntities db, Parameters pm)
        {
            IQueryable<IDates> result = null;

            switch (pm.entitySelected)
            {
                case "ACQ":
                    result = QueriesToDbSet.GetByDate(db.FD_ACQ_D, pm.dateFrom, pm.dateTo);
                    break;
                case "ECOMM":
                    result = QueriesToDbSet.GetByDate(db.FD_ACQ_M, pm.dateFrom, pm.dateTo);
                    break;
                case "MT":
                    
                break;
            }

            if (pm.listInclude)
            {
                result = QueriesToDbSet.GetByMerchants(result);
            }

            return result;
        }

    }

    //
    public static class QueriesToDbSet
    {
        public static DWH_REPLICAEntities db_;

        #region obsolette


        public static Func<FD_ACQ_D, DateTime, DateTime, bool> ACQ_D_bool = (s, st, fn) =>
        DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 00, 00, 00)
        >= DbFunctions.CreateDateTime(st.Year, st.Month, st.Day, 00, 00, 00)
        &&
        DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 00, 00, 00)
        <= DbFunctions.CreateDateTime(fn.Year, fn.Month, fn.Day, 00, 00, 00);       

        public static Expression<Func<DWH_REPLICAEntities, DateTime, DateTime, IQueryable<FD_ACQ_D>>> ACQ_DATE_D
        = (DWH_REPLICAEntities db, DateTime st, DateTime fn) =>
        from s in db.FD_ACQ_D
        where DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 00, 00, 00)
        >= DbFunctions.CreateDateTime(st.Year, st.Month, st.Day, 00, 00, 00)
        &&
        DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 00, 00, 00)
        <= DbFunctions.CreateDateTime(fn.Year, fn.Month, fn.Day, 00, 00, 00)
        select s;

        public static Expression<Func<DWH_REPLICAEntities, IQueryable<FD_ACQ_D>>> ACQ_LIST_bool = (db) =>
        from s in db.FD_ACQ_D
        join f in db.parsedValues on s.MERCHANT equals f.ITEM_ID
        select s;
 
        public static Func<DWH_REPLICAEntities, DateTime, DateTime, IQueryable<FD_ACQ_D>> ACQ_DAY
        = (DWH_REPLICAEntities db, DateTime st, DateTime fn) =>
        from s
        in db.FD_ACQ_D
        select s;
#endregion

        //Date grouping  for interface
        public static IQueryable<IDates> GetByDate(IQueryable<IDates> item, DateTime dateFrom, DateTime dateTo)
        {
            return from s in item
                    where s.DT_REG >= dateFrom && s.DT_REG <= dateTo
                    select s;            

            //Linq to entities syntax
            /*
            DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 00, 00, 00) >=
            DbFunctions.CreateDateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 00, 00, 00)
            &&
            DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 00, 00, 00) <=
            DbFunctions.CreateDateTime(dateTo.Year, dateTo.Month, dateTo.Day, 00, 00, 00)
            );
            */
        }
        //merchans filter for interface

        public static IQueryable<IDates> GetByMerchants(IQueryable<IDates> item)
        { 
            return from s in item
                    join f in db_.parsedValues on s.MERCHANT equals f.ITEM_ID
                    select s;
        }
        public static string GetList()
        {
            string result = "";
            using (DWH_REPLICAEntities db = new DWH_REPLICAEntities())
            {
                try
                {
                    result = (from s in db.REFMERCHANTS select s).Count().ToString();
                }
                catch (Exception e)
                {
                    result = "Error : " + e.Message;
                }
            }               
           
            return result;
        }
    }

    public interface IDates : Ientity
    {
        System.DateTime DT_REG { get; set; }
        System.String MERCHANT { get; set; }
    }    
    public interface Ientity
    {
        int ID { get; set; }
    }

    #region OBSOLETE

        //model class
        //!nullable datetime, datetime formulas in LINQ issue
        public class FD_ACQ_DTO
        {
            public string PAY_SYS { get; set; }
            public DateTime? DT_REG { get; set; }
            public string TYPE_TRANSACTION { get; set; }
            public string MERCHANT { get; set; }
            [DefaultAggregateColumn(type = true)]
            public double? AMT { get; set; }
            [DefaultAggregateColumn(type = true)]
            public double? FEE { get; set; }
            [DefaultAggregateColumn(type = true)]
            public int? CNT { get; set; }

            internal object this[string propName]
            {
                get { if (this.GetType().GetProperty(propName).GetValue(this, null) != null)
                    { return this.GetType().GetProperty(propName).GetValue(this, null); }
                    else { return null; }
                }
            }
        }
        //Methods for query grouping
        public static class FD_ACQ_GROUP
        {
            public static DWH_REPLICAEntities db = new DWH_REPLICAEntities();

            public static IQueryable<FD_ACQ_DTO> list_bool(Parameters parameters_)
            {
                if (parameters_.listInclude)
                {
                    return from s in db.FD_ACQ_D
                           join f in db.parsedValues on s.MERCHANT equals f.ITEM_ID
                           select new FD_ACQ_DTO
                           {
                               DT_REG = s.DT_REG,
                               TYPE_TRANSACTION = s.TYPE_TRANSACTION,
                               MERCHANT = s.MERCHANT,
                               PAY_SYS = s.PAY_SYS,
                               AMT = s.AMT
                           };
                }
                else
                {
                    return from s in db.FD_ACQ_D
                           select new FD_ACQ_DTO
                           {
                               DT_REG = s.DT_REG,
                               TYPE_TRANSACTION = s.TYPE_TRANSACTION,
                               MERCHANT = s.MERCHANT,
                               PAY_SYS = s.PAY_SYS,
                               AMT = s.AMT
                           };
                }
            }
            public static IQueryable<FD_ACQ_DTO> list_d(Parameters parameters_)
            {
                return from s in list_bool(parameters_)
                       where DbFunctions.CreateDateTime(s.DT_REG.Value.Year, s.DT_REG.Value.Month, s.DT_REG.Value.Day, 00, 00, 00) >=
                DbFunctions.CreateDateTime(parameters_.dateFrom.Year, parameters_.dateFrom.Month, parameters_.dateFrom.Day, 00, 00, 00)
                &&
                DbFunctions.CreateDateTime(s.DT_REG.Value.Year, s.DT_REG.Value.Month, s.DT_REG.Value.Day, 00, 00, 00) <=
                DbFunctions.CreateDateTime(parameters_.dateTo.Year, parameters_.dateTo.Month, parameters_.dateTo.Day, 00, 00, 00)
                       select new FD_ACQ_DTO
                       {
                           DT_REG = s.DT_REG,
                           TYPE_TRANSACTION = s.TYPE_TRANSACTION,
                           MERCHANT = s.MERCHANT,
                           PAY_SYS = s.PAY_SYS,
                           AMT = s.AMT
                       };
            }
            public static IQueryable<FD_ACQ_DTO> list_group(Parameters parameters_)
            {
                bool cond = PropertyRepo.GetPropertyCondition("DT_REG");

                return
                from s in list_d(parameters_)
                group s by new
                {
                    DT_REG = FIELDS_REF.dt_reg != false ? s.DT_REG : null,
                    TYPE_TRANSACTION = FIELDS_REF.tran_type != null ? s.TYPE_TRANSACTION : null,
                    MERCHANT = FIELDS_REF.merchant != null ? s.MERCHANT : null,
                    PAY_SYS = FIELDS_REF.pay_sys != null ? s.PAY_SYS : null
                } into g
                select new FD_ACQ_DTO
                {
                    DT_REG = g.Key.DT_REG != null ? g.Key.DT_REG : null,
                    TYPE_TRANSACTION = g.Key.TYPE_TRANSACTION != null ? g.Key.TYPE_TRANSACTION : null,
                    MERCHANT = g.Key.MERCHANT != null ? g.Key.MERCHANT : null,
                    PAY_SYS = g.Key.PAY_SYS != null ? g.Key.PAY_SYS : null,
                    AMT = g.Sum(s => s.AMT)
                };

            }

            /*
            static IQueryable<FD_ACQ_D> listRes = listReturn(db,true);
            public static int count = listRes.Count();
        
            from s in db.FD_ACQ_D
            join s2 in db.parsedValues on s.MERCHANT equals s2.ITEM_ID
            select s;
            new FD_ACQ_D { DT_REG =s.DT_REG,TYPE_TRANSACTION = s.TYPE_TRANSACTION,ISSUER_TYPE =s.ISSUER_TYPE,
            AMT =s.AMT,CNT=s.CNT, FEE=s.FEE};
            */

        }
        //List of propertynames from type with condition
        public static class PropertyRepo
        {
            public static Type classType { get; set; }
            public static string propertyName { get; set; }
            public static bool propertyCondition { get; set; }

            public static List<PropertyConditions> properyList = new List<PropertyConditions>();
            public static Func<string, bool> GetPropertyCondition = (name_) => properyList.Where(s => s.propertyName == name_).First().propertyCondition;

            public static void propertyInitilize(Type type)
            {
                properyList.Clear();
                var propertiesList = type.GetProperties();
                foreach (var b in propertiesList)
                {
                    properyList.Add(new PropertyConditions { classType = type, propertyName = b.Name, propertyCondition = false });
                }
            }
            public static void propertyCheck(List<string> selectedParameters)
            {
                foreach (string parameter in selectedParameters)
                {
                    foreach (PropertyConditions pc in properyList)
                    {
                        if (parameter == pc.propertyName)
                        {
                            pc.propertyCondition = true;
                        }
                    }
                }
            }
            public static void propertyConditionDescard()
            {
                foreach (PropertyConditions pc in properyList)
                {
                    pc.propertyCondition = false;
                }
            }
        }
        public class PropertyConditions
        {
            public Type classType { get; set; }
            public string propertyName { get; set; }
            public bool propertyCondition { get; set; }
        }

        //Precompiled LINQ aggregate fields conditions
        ///!change to one class with list of parameters with NULL status
        public static class FIELDS_REF
        {
            public static bool tran_type = false;
            public static bool merchant = false;
            public static bool pay_sys = false;
            public static bool dt_reg = false;

            public static void paramsCheck(List<string> params_selected)
            {
                if (params_selected != null)
                {
                    if (params_selected.Contains("MERCHANT"))
                    {
                        merchant = true;
                    }
                    if (params_selected.Contains("PAY_SYS"))
                    {
                        pay_sys = true;
                    }
                    if (params_selected.Contains("TYPE_TRANSACTION"))
                    {
                        tran_type = true;
                    }
                    if (params_selected.Contains("DT_REG"))
                    {
                        dt_reg = true;
                    }
                    if (params_selected.Contains("ALL"))
                    {
                        merchant = true;
                        pay_sys = true;
                        tran_type = true;
                        dt_reg = true;
                    }
                    if (params_selected.Contains("none"))
                    {
                        merchant = false;
                        pay_sys = false;
                        tran_type = false;
                        dt_reg = false;
                    }
                }

            }
            public static void paramsInit()
            {
                tran_type = false;
                merchant = false;
                pay_sys = false;
                dt_reg = false;
            }
        }
        //Precompiled LINQ dates
        public static class LINQ_DATES
        {
            public static Func<FD_ACQ_D, DateTime, DateTime, bool> datesMbool = (s, fd, td) =>
            DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, 01, 0, 0, 0) >=
            DbFunctions.CreateDateTime(fd.Year, fd.Month, 01, 0, 0, 0)
            && DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, 01, 0, 0, 0) <=
            DbFunctions.CreateDateTime(td.Year, td.Month, 01, 0, 0, 0);

            public static Func<DWH_REPLICAEntities, DateTime, DateTime, IQueryable<FD_ACQ_D>> datesY = (b, fd, td) =>
            from s in b.FD_ACQ_D
            where DbFunctions.CreateDateTime(s.DT_REG.Year, 01, 01, 0, 0, 0) >=
            DbFunctions.CreateDateTime(fd.Year, 01, 01, 0, 0, 0)
            && DbFunctions.CreateDateTime(s.DT_REG.Year, 01, 01, 0, 0, 0) <=
            DbFunctions.CreateDateTime(td.Year, 01, 01, 0, 0, 0)
            select s;

            public static Func<DWH_REPLICAEntities, DateTime, DateTime, IQueryable<FD_ACQ_D>> datesM = (b, fd, td) =>
            from s in b.FD_ACQ_D
            where DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, 01, 0, 0, 0) >=
            DbFunctions.CreateDateTime(fd.Year, fd.Month, 01, 0, 0, 0)
            && DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, 01, 0, 0, 0) <=
            DbFunctions.CreateDateTime(td.Year, td.Month, 01, 0, 0, 0)
            select s;

            public static Func<DWH_REPLICAEntities, DateTime, DateTime, IQueryable<FD_ACQ_D>> datesD = (b, fd, td) =>
            from s in b.FD_ACQ_D
            where DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 0, 0, 0) >=
            DbFunctions.CreateDateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0)
            && DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 0, 0, 0) <=
            DbFunctions.CreateDateTime(td.Year, td.Month, td.Day, 0, 0, 0)
            select s;
        }
        //Precompiled LINQ aggregation
        public static class LINQ_QUERIES
        {
            public static DWH_REPLICAEntities db;
            public static DateTime fromDate;
            public static DateTime toDate;

            //Precompiled LINQ Query for compile query parameter
            public static Func<DWH_REPLICAEntities, IQueryable<FD_ACQ_DTO>> func_d =
            ((DWH_REPLICAEntities dwh) =>
                from s in LINQ_DATES.datesD.Invoke(db, fromDate, toDate)
                group s by new
                {
                    DT_REG = FIELDS_REF.dt_reg ? DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 0, 0, 0) : null,
                    TYPE_TRANSACTION = FIELDS_REF.tran_type ? s.TYPE_TRANSACTION : null,
                    MERCHANT = FIELDS_REF.merchant ? s.MERCHANT : null,//s.MERCHANT,
                    PAY_SYSTEM = FIELDS_REF.pay_sys ? s.PAY_SYS : null
                } into g
                select new FD_ACQ_DTO
                {
                    DT_REG = FIELDS_REF.dt_reg ? g.Key.DT_REG : null,
                    TYPE_TRANSACTION = FIELDS_REF.tran_type ? g.Key.TYPE_TRANSACTION : null,
                    MERCHANT = FIELDS_REF.merchant ? g.Key.MERCHANT : null,
                    PAY_SYS = FIELDS_REF.pay_sys ? g.Key.PAY_SYSTEM : null,
                    AMT = g.Sum(s => s.AMT),
                    FEE = g.Sum(s => s.FEE),
                    CNT = g.Sum(s => s.CNT)
                }
            );

            //Precompiled LINQ Query for compile query parameter
            public static Func<DWH_REPLICAEntities, IQueryable<FD_ACQ_DTO>> func_m =
            ((DWH_REPLICAEntities dwh) =>
                from s in LINQ_DATES.datesM.Invoke(db, fromDate, toDate)
                group s by new
                {
                    DT_REG = FIELDS_REF.dt_reg ? DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, 01, 0, 0, 0) : null,
                    TYPE_TRANSACTION = FIELDS_REF.tran_type ? s.TYPE_TRANSACTION : null,
                    MERCHANT = FIELDS_REF.merchant ? s.MERCHANT : null,//s.MERCHANT,
                    PAY_SYSTEM = FIELDS_REF.pay_sys ? s.PAY_SYS : null
                } into g
                select new FD_ACQ_DTO
                {
                    DT_REG = FIELDS_REF.dt_reg ? g.Key.DT_REG : null,
                    TYPE_TRANSACTION = FIELDS_REF.tran_type ? g.Key.TYPE_TRANSACTION : null,
                    MERCHANT = FIELDS_REF.merchant ? g.Key.MERCHANT : null,
                    PAY_SYS = FIELDS_REF.pay_sys ? g.Key.PAY_SYSTEM : null,
                    AMT = g.Sum(s => s.AMT),
                    FEE = g.Sum(s => s.FEE),
                    CNT = g.Sum(s => s.CNT)
                }
            );

            //Precompiled LINQ Query for compile query parameter
            public static Func<DWH_REPLICAEntities, IQueryable<FD_ACQ_DTO>> func_y =
            ((DWH_REPLICAEntities dwh) =>
                from s in LINQ_DATES.datesY.Invoke(db, fromDate, toDate)
                group s by new
                {
                    DT_REG = FIELDS_REF.dt_reg ? DbFunctions.CreateDateTime(s.DT_REG.Year, 01, 01, 0, 0, 0) : null,
                    TYPE_TRANSACTION = FIELDS_REF.tran_type ? s.TYPE_TRANSACTION : null,
                    MERCHANT = FIELDS_REF.merchant ? s.MERCHANT : null,//s.MERCHANT,
                    PAY_SYSTEM = FIELDS_REF.pay_sys ? s.PAY_SYS : null
                } into g
                select new FD_ACQ_DTO
                {
                    DT_REG = FIELDS_REF.dt_reg ? g.Key.DT_REG : null,
                    TYPE_TRANSACTION = FIELDS_REF.tran_type ? g.Key.TYPE_TRANSACTION : null,
                    MERCHANT = FIELDS_REF.merchant ? g.Key.MERCHANT : null,
                    PAY_SYS = FIELDS_REF.pay_sys ? g.Key.PAY_SYSTEM : null,
                    AMT = g.Sum(s => s.AMT),
                    FEE = g.Sum(s => s.FEE),
                    CNT = g.Sum(s => s.CNT)
                }
            );
        }

        //attribute for default aggregate columns check
        public class DefaultAggregateColumnAttribute : Attribute
        {
            public bool type = false;
        }

        public class Repo<T> where T : class
        {
            public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navProp)
            {
                List<T> list = new List<T>();
                using (var context = new DWH_REPLICAEntities())
                {
                    IQueryable<T> dbQ = context.Set<T>();

                    foreach (Expression<Func<T, object>> navProp_ in navProp)
                    {
                        dbQ = dbQ.Include<T, object>(navProp_);
                    }
                }
                return list;
            }
        }

        public class Repo2<T>
        {
            public IQueryable<T> Qlist;

            public List<Type> list;

            public void GetAll(IQueryable<T> list_)
            {
                this.Qlist = list_;
            }
            public void GetFromType(Type item)
            {
                this.list.Add(item);
            }

        }

    #endregion   

    //delegate, lambda,LINQ examples
    public class Test
    {
        Del delegateMethod, delegateMethod2;
        Del handler, handler2, handler3;

        public Test(DWH_REPLICAEntities db)
        {
            /*
            delegateTest();
            lambdaTest();
            linqTest(db);
            */
        }
        public Test()
        {
            //RefTest();
        }

        public delegate void Del(string input);
        public void method(string message)
        {
            System.Diagnostics.Trace.WriteLine("Method 1 :" + message);
        }
        public void method2(string message)
        {
            System.Diagnostics.Trace.WriteLine("Method 2 :" + message);
        }
        public void delegateBinding()
        {
            delegateMethod = method;
            handler = delegateMethod;
        }
        public void delegateBinding2()
        {
            delegateMethod2 = method2;
            handler2 = delegateMethod2;
        }
        public void delegateBinding3()
        {
            handler3 = handler + handler2;
        }
        public void delegateTest()
        {
            delegateBinding();
            delegateBinding2();
            delegateBinding3();

            handler("delegate test");
            handler3("test 3");
        }
        delegate int del(int i, int i2);
        public void lambdaTest()
        {
            Func<int, int> func1 = x => x + 1;
            Func<int, int> func2 = x => { return x + 1; };
            Func<int, int> func3 = (int x) => x + 1;
            Func<int, int, int> func4 = (x, y) => x * y;
            Func<string, string> func5 = x => { Console.WriteLine("A" + x); return "A" + x; };

            System.Diagnostics.Trace.WriteLine(func1.Invoke(1));
            System.Diagnostics.Trace.WriteLine(func2.Invoke(1));
            System.Diagnostics.Trace.WriteLine(func3.Invoke(1));
            System.Diagnostics.Trace.WriteLine(func4.Invoke(1, 2));
            System.Diagnostics.Trace.WriteLine(func5.Invoke("B"));

            del mydel = (x, y) => { for (int i = 0; i < y; i++) { x = x * x; System.Diagnostics.Trace.WriteLine(x); } return x; };
            System.Diagnostics.Trace.WriteLine(mydel(2, 3));
        }
        public void linqTest(DWH_REPLICAEntities db)
        {
            db.Database.CommandTimeout = 180;

            var a =
                from s
                in db.FD_ACQ_D
                where DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 00, 00, 00)
                == DbFunctions.CreateDateTime(2016, 09, 01, 00, 00, 00)
                group s by new
                {
                    DATE = DbFunctions.CreateDateTime(s.DT_REG.Year, s.DT_REG.Month, s.DT_REG.Day, 01, 01, 01)
                    ,
                    PAY_SYS = s.PAY_SYS
                    ,
                    TRAN_TYPE = s.TYPE_TRANSACTION
                    ,
                    MERCH = s.MERCHANT
                } into g
                select new
                {
                    DATE = g.Key.DATE
                    ,
                    TRAN_TYPE = g.Key.TRAN_TYPE
                    ,
                    PAY_SYS = g.Key.PAY_SYS
                    ,
                    MERCH = g.Key.MERCH
                    ,
                    AMT = g.Sum(t => t.AMT)
                };

            var b = a.Count();
        }
        private class nestedTest
        {
            internal DateTime DATE { get; set; }
            internal string PAY_SYS { get; set; }
            internal string MERCHANT { get; set; }
            internal string TRAN_TYPE { get; set; }
            internal double AMT { get; set; }
        }
        public void RefTest()
        {
            //SendMail(@"10.30.33.154", @"adm2@dc03rs-vm.rs.ru", @"", @"ia-neprintsev@rsb.ru", @"A", @"test", null);

            FD_ACQ_DTO r = new FD_ACQ_DTO();
            Type type_ = r.GetType();
            FieldInfo[] fields = type_.GetFields();
            List<string> properties = new List<string>();
            Dictionary<string, object> attribs = new Dictionary<string, object>();

            Type type = typeof(FD_ACQ_DTO);
            PropertyInfo[] propertiesInfo = type.GetProperties();
            foreach (System.Reflection.PropertyInfo propertyInfo_ in propertiesInfo)
            {
                properties.Add(propertyInfo_.Name);

                IList<System.Reflection.CustomAttributeData> attributeData = propertyInfo_.GetCustomAttributesData();
                foreach (System.Reflection.CustomAttributeData attributeData_ in attributeData)
                {
                    string typeName = attributeData_.Constructor.DeclaringType.Name;
                    if (typeName.EndsWith("Attribute")) typeName = typeName.Substring(0, typeName.Length - 9);
                    IList<CustomAttributeNamedArgument> namedArgument = attributeData_.NamedArguments;
                    //attribs[typeName] 
                    foreach (System.Reflection.CustomAttributeNamedArgument namedArgument_ in namedArgument)
                    {
                        if (namedArgument_.TypedValue.Value.GetType().Equals(typeof(Boolean)))
                        {
                            bool bl = (bool)namedArgument_.TypedValue.Value;
                        }
                    }
                }
            }

            int a = 1;
        }

        public static void SendMail(string smtpServer, string from, string password,
        string mailto, string caption, string message, string attachFile = null)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;
                if (!string.IsNullOrEmpty(attachFile))
                    mail.Attachments.Add(new Attachment(attachFile));
                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }

        public void excelExportTest()
        {
            DWH_REPLICAEntities db = new DWH_REPLICAEntities();
            MainModel model = new MainModel();
            Parameters parameters_ = new Parameters();
            Execution execution = new Execution();

            parameters_.entitySelected = "ACQ";

            execution.parameters = parameters_;

            model.entity = QueriesToDbSet.GetByDate(db.FD_ACQ_D, new DateTime(2016,09, 01, 00, 00, 00), new DateTime(2016, 09, 03, 00, 00, 00));
            string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Directory.GetParent( Directory.GetParent(uri.Path).ToString()).ToString();
            path = path + "\\App_Data\\report.xlsx";
            ExcelPackage e = new ExcelPackage(new FileInfo(path));
            execution.ExcelExport(path, model);
        }
      
    }

}
