using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Repo.DAL.SQL_ent;
using System.Reflection;
using OfficeOpenXml;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

using System.Data.SqlClient;
using System.Data;
using System.Data.Entity;

namespace Presentation_.Models
{

    #region MVCmodel    
    //main select condition values passed/received from view
    public class rParameters
    {
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public string tableSelected { get; set; }
        public bool merchantsFilter { get; set; }

        public rParameters()
        {          
            dateFrom = DateTime.Now.Date.AddDays(-DateTime.Now.Date.Day);
            dateTo = DateTime.Now.Date;
            this.merchantsFilter = false;
        }
        public rParameters(string dateFrom_, string dateTo_,string tableSelected_, bool? listIncluded_)
        {
            dateFrom = DateTime.Parse(dateFrom_);
            dateTo = DateTime.Parse(dateTo_);
            tableSelected = tableSelected_;
            merchantsFilter = listIncluded_ ?? false;
        }
        public void rParametersInit(string dateFrom_ = null, string dateTo_ = null, string tableSelected_ = null, bool? listIncluded_ = false)
        {
            dateFrom = DateTime.Parse(dateFrom_);
            if (dateFrom_ != null)
            {
                dateFrom = DateTime.Parse(dateFrom_);
            }
            dateTo = DateTime.Now.Date;
            if(dateFrom_!= null)
            {
                dateTo = DateTime.Parse(dateTo_);
            }
            tableSelected = tableSelected_;
            merchantsFilter = listIncluded_ ?? false;
        }
    }
    
    //publishes vlues to view
    public class rValues
    {
        public rParameters parameters;
        public int merchantFilterCount { get; set; }
        public List<string> tableNames { get; set; }
        public IEnumerable<SelectListItem> tableNamesToSelect { get; set; }

        //>!!! add from Repo.DAL at runtime
        public void rValuesInit()
        {          
            if (tableNames != null)
            {
                tableNamesToSelect = from s in tableNames select new SelectListItem { Text = s.ToString() };
            }
            //>>!!!add error for empty list
        }
        public void tableNamesSet(List<string> names_)
        {
            tableNames = new List<string>();
            this.tableNames = names_;
            rValuesInit();
        }
        public void loadedAmountCheck()
        {
            SQL_entity ent = new SQL_entity();
            Repo.UtilityRepository ur = new Repo.UtilityRepository(ent);
            this.merchantFilterCount = ur.MerchantsCount();
        }
        public void parametersInit()
        {          
            if(this.parameters != null)
            {
                if (this.merchantFilterCount > 0)
                {
                    this.parameters.merchantsFilter = true;
                }
            }
            else
            {
                this.parameters = new rParameters();
            }
        }
        public rValues()
        {            
            rValuesInit();
            loadedAmountCheck();
            parametersInit();
        }
    }

    public class rUpload
    {         

        public List<string> fileNames = new List<string>();
        public IEnumerable<HttpPostedFileBase> filesPosted { get; set; }
        
        private string currentUser;
        public string folder;

        public String CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
        public rUpload()
        {

        } 

        public void UploadFromFiles(IEnumerable<HttpPostedFileBase> filesPosted_)
        {
            GetPostedFiles(filesPosted_);
            FilesRead();            
        }
        private void GetPostedFiles(IEnumerable<HttpPostedFileBase> filesPosted_)
        {
            if (filesPosted_ != null)
            {
                this.filesPosted = filesPosted_;
                GetFilenames(filesPosted_);
            }
        }
        private void GetFilenames(IEnumerable<HttpPostedFileBase> filesPosted_)
        {
            if (filesPosted_.Count() > 0)
            {
                foreach (HttpPostedFileBase file in filesPosted_)
                {
                    this.fileNames.Add(file.FileName);
                }
            }
        }

        private void FilesRead()
        {
            string path = "";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            foreach (HttpPostedFileBase file in this.filesPosted)
            {

                string propertyType = Mapper.ParseFileName(file.FileName);
                
                //sector name get from file
                int? sectorID = Mapper.ParseBindFileSectorID(file.FileName);

                if(propertyType!=@"" && propertyType != null)
                {
                    Mapper.MapperInitWithNames(@"Repo.DAL", propertyType);
                    Mapper.CurrentEntityFromListSet(propertyType);

                    //>>|| check type then delete
                    SectorDelete((int)sectorID);
                    ByUserDelete(0);

                    FilesParse(file, 1000);
                    path = Path.Combine(folder, file.FileName);
                    file.SaveAs(path);
                }
            }
        }

        private void SectorDelete(int ID_)
        {
            /*
            Repo.DAL.SQL_ent.SQL_entity ent = new Repo.DAL.SQL_ent.SQL_entity();
            Repo.SectorFilterRepo<KEY_CLIENTS> rep = new Repo.SectorFilterRepo<KEY_CLIENTS>(ent);
            rep.DeleteBySector(ID_);
            */
            Repo.Edit_UOF uof = new Repo.Edit_UOF();
            uof.DeleteBySectorID(ID_);
            uof.SaveAll();
            uof.Dispose();
        }
        private void ByUserDelete(int id_)
        {
            Repo.Edit_UOF uof = new Repo.Edit_UOF();
            uof.DeleteByUserID(0);
            uof.SaveAll();
            uof.Dispose();
        }

        private void HeadingsRead(HttpPostedFileBase file_)
                {
                    using (ExcelPackage e = new ExcelPackage(file_.InputStream))
                    {
                        foreach(ExcelWorksheet ws in e.Workbook.Worksheets)
                        {
        OfficeOpenXml.ExcelAddressBase dim = ws.Dimension;
                            if (dim != null)
                            {
                                int rowSt = dim.Start.Row;
                                int colSt = dim.Start.Column;
                                int rowFn = dim.End.Row;
                                int colFn = dim.End.Column;
                                StringBuilder sb = new StringBuilder(100, 500);

                                for(int i = colSt; i <= colFn; i++)
                                {
                                    //ws.Cells[rowSt, i].Value;
                                }
                            }
                        }
                    }
                }
        private void FilesParse(HttpPostedFileBase file_, int partition_)
        {
            string res = @"";
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            System.Diagnostics.Stopwatch watch2 = new System.Diagnostics.Stopwatch();

            List<Repo.IMerchant> Merchants_read = new List<Repo.IMerchant>();
            using (ExcelPackage p = new ExcelPackage(file_.InputStream))
            {
                foreach (ExcelWorksheet ws in p.Workbook.Worksheets)
                {

                    //>>>!! add max row for max column find
                    OfficeOpenXml.ExcelAddressBase dim = ws.Dimension;
                    if (dim != null)
                    {
                        int rowSt = dim.Start.Row;
                        int colSt = dim.Start.Column;
                        int rowFn = dim.End.Row;
                        int colFn = dim.End.Column;

                        StringBuilder sb = new StringBuilder(100, 500);
                        //SQL_entity ent = new SQL_entity();
                        //Repo.EditRepo<Repo.IMerchant> rep = new Repo.EditRepo<Repo.IMerchant>(ent);

                        //parse headers
                        for (int i2 = colSt; i2 <= colFn; i2++)
                        {
                            Mapper.ParseHeader(ws.Cells[rowSt, i2].Value.ToString(), i2);
                        }

                        //Find merchant column number
                        int merchantColumn = Mapper.CurrentEntity.GetColumnNumberByName("MERCHANT");

                            watch.Start();
                            
                            //For each row in merchant column read merchants and collect them
                            for (int i = rowSt + 1; i <= rowFn; i++)
                            {
                                if (merchantColumn != null
                                && ws.Cells[i, merchantColumn].Value != null 
                                && ws.Cells[i, merchantColumn].Value != @"")
                                {
                                    //>> mapper init
                                    for (int i2 = colSt; i2 <= colFn; i2++)
                                    {
                                        Mapper.SetValue(i2, ws.Cells[i, i2].Value.ToString());
                                    }

                                Mapper.SetValue(@"SECTOR_ID", Mapper.sectorID);
                                Merchants_read.Add(Mapper.PropertyValuesBind());                               

                                //ur.Key_Clients_Insert(kk);
                                //if (i % partition_ == 0)
                                //{
                                //    ur.contextSaveChanges();
                                //    ur.Dispose();
                                //    ent.Dispose();
                                //    ent = new SQL_entity();
                                //    ur = new Repo.UtilityRepository(ent);
                                //}
                                }
                            }
                            watch.Stop();
                    }
                            
                            //>>|| to separate type based mwthod with swich for types
                            watch2.Start();
                            Repo.Edit_UOF merchantUOF = new Repo.Edit_UOF();
                            //>>>!! moove to repo
                            List<Repo.IMerchant> Merchants_loaded_kk = merchantUOF.GetKeyClientsMerchants().ToList();

                            List<Repo.IMerchant> a = new List<Repo.IMerchant>();
                            //remove duplicates by se_name and null se_names from List
                            a = (from s in Merchants_read
                            //join c in ent.KEY_CLIENTS on s.MERCHANT equals c.SE_NUMBER
                            join c in Merchants_loaded_kk on s.MERCHANT equals c.MERCHANT select s).ToList();

                            merchantUOF.SaveAll();
                            merchantUOF.Dispose();
                            
                            foreach (Repo.IMerchant kk in a)
                            {
                                Merchants_read.Remove(kk);
                            }

                            a = (from s in Merchants_read where s.MERCHANT == null select s).ToList();
                            foreach (Repo.IMerchant kk in a )
                            {
                                Merchants_read.Remove(kk);
                            }


                            //>>|| to separate method
                            Repo.Edit_UOF uof = new Repo.Edit_UOF();
                            foreach (Repo.IMerchant item_ in Merchants_read)
                            {
                                
                                switch(Mapper.CurrentEntity.EntityType.Name)
                                {
                                    case @"T_ACQ_D":
                                        uof.AddACQ_D(item_);
                                        break;
                                    case @"KEY_CLIENTS":
                                        uof.AddKK(item_);
                                        break;
                                    case @"REFMERCHANTS":
                                        uof.AddREF(item_);
                                    break;
                                }

                                uof.SaveAll();

                                /*
                                rep.AddEntity(item_);
                                rep.Save();
                                rep.Dispose();
                                rep = new Repo.EditRepo<Repo.IMerchant>(ent);
                                */
                            }

                        uof.Dispose();
                     
                        watch2.Stop();
                        //ent.Dispose();
                        res = @"Added: " + watch.ElapsedMilliseconds / 1000 + @" Inserted :" + watch2.ElapsedMilliseconds / 1000;
                        System.Diagnostics.Debug.WriteLine(res);
                   

                }
            }
        }
    }
    #endregion

    #region Export_old
    //Exports entities to excel
    public class rExcelExport
    {
        public rValues values { get; set; }
        SQL_entity entity { get; set; }
        public string FileName { get; private set; }
        ExcelPackage e { get; set; }

        public rExcelExport()
        {
            values = new rValues();
        }
        public void SetEntity(SQL_entity ent_)
        {
            this.entity = ent_;
        }
        public void SetParameters(rParameters parameters_)
        {
            this.values.parameters = parameters_;
        }
        public void GetEntityNames()
        {
            this.values.tableNamesSet((from s in entity.GetType().GetProperties() where s.GetGetMethod().CustomAttributes.Count() != 0 select s.Name).ToList());
        }
        public string ExcelExport()
        {
            string result = "";
            rRepoUnit.SetParameters(this.values.parameters);
            rRepoUnit.TableToRepo(entity);

            if (rRepoUnit.result != null)
            {
                rRepoUnit.QueryByDay();
                //!!! add insert to excel

                //>>>??? change file access
                FileInit();
                ResultToExcel();
            }
            return result;
        }
        public IQueryable<string> ListCheck()
        {
            IQueryable<string> result = null;
            if (this.entity != null)
            {
                result = rRepoUnit.ListReturn(entity);
            }
            return result;
        }
        //public static void ExcelImport(){}

        //>>!!! to EPPLUs proj
        public void SetFile(string fileName_)
        {
            this.FileName = fileName_;
        }
        public void FileInit()
        {
            FileCheck();
        }
        public void FileCheck()
        {
            try
            {
                if (System.IO.File.Exists(FileName))
                {
                    System.IO.File.Delete(FileName);
                }
            }
            catch (Exception e)
            {
                //>>!!! add logging
            }
        }
        public void ResultToExcel()
        {
            int row = 1;
            int col = 1;

            using (e = new ExcelPackage(new FileInfo(FileName)))
            {
                ExcelWorksheet ws = e.Workbook.Worksheets.Add("Sheet");

                //PropertyInfo[] fi = c.GetType().GetProperties();
                //IEnumerable<string> gp = from s in fi select s.Name;
                //IEnumerable<string> properties = from s in rRepoUnit.properties select s.Name;

                ws.Cells[1, 1].Value = "Test export";
                ws.Cells[2, 1].Value = rRepoUnit.result.Count();

                foreach (PropertyInfo pi in rRepoUnit.properties)
                {
                    ws.Cells[1, col].Value = pi.Name;

                    if (pi.Name.Contains("DATE"))
                    {
                        ws.Column(col).Style.Numberformat.Format = "yyyy.mm.dd";
                        //ws.Column(col).Style.Numberformat.Format = @"dd MMM yyyy hh:mm";
                    }
                    col += 1;
                }
                foreach (var item_ in rRepoUnit.result.QueryResult)
                {
                    PropertyInfo[] pInfo = item_.GetType().GetProperties();
                    IEnumerable<string> pNames = from s in pInfo select s.Name;
                    IEnumerable<string> properties = from s in rRepoUnit.properties select s.Name;

                    row += 1;
                    col = 1;
                    for (int i = 0; i < pInfo.Count(); i++)
                    {
                        var a = pInfo[i];
                        ws.Cells[row, col].Value = a.GetValue(item_);
                        col += 1;
                    }
                }
                e.Save();
                e.Dispose();
            }
        }
    }

    //parses string from user input to repo instance
    //materializes query to repo
    //>>!!! change for type init, add constructor
    public static class rRepoUnit
    {
        static rParameters parameters { get; set; }
        public static dynamic result = null;
        public static PropertyInfo[] properties = null;
        public static int count = 0;

        //>>!!! check parameters input
        internal static void SetParameters(rParameters parameters_)
        {
            parameters = parameters_;
        }
        //>>!!! dynamic instantiation, entity to field
        internal static dynamic TableToRepo(SQL_entity ent)
        {
            result = Repo.RepoInit.RepoFromString(parameters.tableSelected, ent);
            return result;
        }
        internal static dynamic QueryByDay()
        {
            DateTime st = parameters.dateFrom;
            DateTime fn = parameters.dateTo;
            bool listInclude_ = parameters.merchantsFilter;

            dynamic res = null;
            if (result != null)
            {
                result.SQLGetByDay(st, fn);
                if (listInclude_)
                {
                    res = result.SQLFilterByMerchants();
                    result = res;
                }
                GetFields();
                GetLength();
            }
            return res;
        }
        internal static IQueryable<string> ListReturn(SQL_entity ent)
        {
            IQueryable<string> res = null;
            Repo.UtilityRepository rep = new Repo.UtilityRepository(ent);
            if (rep != null)
            {
                res = rep.GetMerchants();
            }
            return res;
        }
        public static void GetFields()
        {
            Type t = result.GetType();

            if (t.GenericTypeArguments.Where(s => s.AssemblyQualifiedName.Contains(parameters.tableSelected)).Any())
            {
                properties = t.GenericTypeArguments.Where(s => s.AssemblyQualifiedName.Contains(parameters.tableSelected)).FirstOrDefault().GetProperties();
            }
        }
        public static void GetLength()
        {
            if (result != null)
            {
                count = result.Count();
            }
        }
    }
    #endregion

    #region Parsing
    /// <summary>
    /// Parses txt config with entity names , sector ID and properties and masks.
    /// Reads metadata for namespace and entity.
    /// Stores property names and types.
    /// Parses column name strings  with masks from config and stores column numbers to properties.
    /// </summary>
    public static class Mapper
    {
        public static string sectorID;

        //masks to map file masks to entity classes 
        static EntityNameMask entityMask = new EntityNameMask();
        //masks to map property masks from excel columns to entity properties
        static PropertyNameMask properyMasks = new PropertyNameMask();

        //entity classes collection with properties and types in DB and CLR
        static List<EntityTypeItem> Entities = new List<EntityTypeItem>();
        //Entity wich is parsed from last file
        public static EntityTypeItem CurrentEntity;
          
        static IEnumerable<System.Type> NamespaceTypes;

        //static Repo.Edit_UOF
        //>>||

        internal static void MapperInit()
        {        
            if (Mapper.AssemblyTypesTryGet()) { };
            if (Mapper.EntityTypeTryAdd()) { };
            if (Mapper.PropertyItemsSet()) { };
        }
        internal static void MapperInitWithNames(string assembly_,string type_)
        {           
            if (Mapper.AssemblyTypesTryGet(assembly_)) { };
            if (Mapper.EntityTypeTryAdd(type_)) { };
            if (Mapper.PropertyItemsSet(type_)) { };
        }
        internal static void MasksInit(string path_)
        {
            entityMask.Init(path_);
            properyMasks.Init(path_);
        }
       
        public static bool AssemblyTypesTryGet(string namespace_ = @"Repo.DAL")
        {
            bool result = false;
            if (AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(s => s.Namespace == namespace_).Any())
            {
                NamespaceTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(s => s.Namespace == namespace_);
                result=true;
            }
            return result;
        }
        public static bool EntityTypeTryAdd(string type_ = @"KEY_CLIENTS")
        {

            bool result = false;
            SqlDbType? dbType=null;

            System.Type entityType=null;

            if (NamespaceTypes.Where(s => s.Name == type_).Any() && !Entities.Where(s=>s.EntityType.Name == type_).Any())
            {
                entityType = (from s in NamespaceTypes where s.Name == type_ select s).FirstOrDefault();
                if(entityType!=null && SQL_CLR_mapper.typeslist.Where(s => s.Value == entityType.GetType()).Any())
                {
                    dbType = SQL_CLR_mapper.typeslist.Where(s => s.Value == entityType.GetType()).FirstOrDefault().Key;
                }
                Entities.Add(new EntityTypeItem {EntityType = entityType,SQLtype = dbType});
                result = true;
            }

            return result;

        }
        public static bool PropertyItemsSet(string type_ = @"KEY_CLIENTS")
        {
            bool result = false;
            if (Entities.Where(s => s.EntityType.Name == type_).Select(s => s).Any())
            {
                result = true;
                EntityTypeItem item = Entities.Where(s => s.EntityType.Name == type_).Select(s => s).First();
                System.Type type = item.EntityType;
                foreach (PropertyInfo pi in type.GetProperties())
                {
                    if (!item.Properties.Where(s => s.PropertyName == pi.Name && s.EntityTypeName == type_).Any())
                    {
                        item.Properties.Add(new PropertyItem { EntityTypeName = type_, PropertyInfo = pi, PropertyName = pi.Name, CLR_type = pi.GetType() });
                    }
                }
            }
            return result;
        }

        public static void CurrentEntityFromListSet(string type_ = @"KEY_CLIENTS")
        {          
            CurrentEntity = Mapper.EnityTypeItemGet(type_);
        }
        public static void RepositoryAdd()
        {
            //>>||
        }
        public static void AddMethodSet()
        {

        }

        public static PropertyItem SetValue(int columnNumber_, string value_)
        {
            PropertyItem result = null;
            if (columnNumber_ > 0)
            {
                result = CurrentEntity.GetPropertyByColumnNumber(columnNumber_);
                if (result != null)
                {
                    result.Value = value_;
                }
            }
            return result;
        }
        public static PropertyItem SetValue(string columnName_, string value_)
        {
            PropertyItem result = null;
            if (columnName_ != null && columnName_ != @"")
            {
                result = CurrentEntity.GetPropertyByName(columnName_);
                if(result!=null)
                {
                    result.Value = value_;
                }                
            }
            return result;
        }

        public static EntityTypeItem EnityTypeItemGet(string type_ = null)
        {
            EntityTypeItem result = null;

            if (Entities.Where(s => s.EntityType.Name == type_).Any())
            {                         
                result = Entities.Where(s => s.EntityType.Name == type_).First();
            }

            return result;
        }       
        public static List<PropertyItem> EntityPropertiesGet(string type_ = null)
        {
            List<PropertyItem> result = null;
            if(Entities.Where(s=>s.EntityType.Name == type_).Select(s=>s.Properties).Any())
            {
                result = Entities.Where(s=>s.EntityType.Name==type_).Select(s=>s.Properties).First();
            }
            return result;
        }

        //>>!!! divide return int, bind int
        public static void ParseHeader(string header_, int columnNumber_)
        {
            string name_ = properyMasks.GetNameMasked(header_, properyMasks.namesMaskList);

            if (name_ != null)
            {
                if (CurrentEntity.GetPropertyByName(name_) != null)
                {
                    CurrentEntity.GetPropertyByName(name_).ColumnNumber = columnNumber_;
                }
            }
        }
        //>>!!! replace with interfaces
        public static string ParseFileName(string name_)
        {
            string result = "";
            foreach (NameMaskShadow mask_ in entityMask.namesMaskList)
            {
                System.Text.RegularExpressions.Match match_ = new Regex(mask_.Mask).Match(name_);
                if (match_.Success)
                {
                    result = mask_.Name;
                }
            }
            return result;
        }
        public static int? ParseBindFileSectorID(string name_)
        {
            NameMaskShadow m = new PropertyNameMask();
            
            int? result = null;
            foreach (EntityNameMask mask_ in entityMask.namesMaskList)
            {
                System.Text.RegularExpressions.Match match_ = new Regex(mask_.Mask).Match(name_);
                if (match_.Success)
                {
                    result = Convert.ToInt32(mask_.IndustryFile);
                    sectorID = result.ToString();
                }
            }
            return result;
        }

        //>>!!! change type binding -> Repo.TypeBind
        /// <summary>
        /// instatiates class item from classname
        /// for every property maps value
        /// binds property value to class item
        /// </summary>
        /// <param name="type_">Entity type string name. Default is KEY_CLIENTS</param>
        /// <returns></returns>
        public static dynamic PropertyValuesBind()
        {
            dynamic result=null;
            Type entityType = null;
            EntityTypeItem item = null;
            //item = Mapper.EnityTypeItemGet(type_);
            item = CurrentEntity;
            entityType = item.EntityType;

            if (entityType!=null)
            {
                result = Activator.CreateInstance(entityType);
                foreach(PropertyItem pi in item.Properties)
                {                    
                    dynamic a = null;

                    //if(pi.Value!=null){

                        if(pi.PropertyInfo.PropertyType.FullName.Contains(@"System.String"))
                        {
                            a = pi.Value;
                        }
                        if(pi.PropertyInfo.PropertyType.FullName.Contains(@"System.Int32"))
                        {
                            int t = 0;
                            if(pi.Value!=null){
                                int.TryParse(pi.Value, out t);
                            }
                            a = t;
                        }
                        if (pi.PropertyInfo.PropertyType.FullName.Contains(@"System.Int64"))
                        {
                            long t = 0;
                            if (pi.Value != null){
                                t = Convert.ToInt64(pi.Value);
                            }
                            a = t;
                        }
                        if (pi.PropertyInfo.PropertyType.FullName.Contains(@"System.DateTime"))
                        {
                            if (pi.Value != null)
                            {
                                a = Convert.ToDateTime(pi.Value);
                            }
                        }
                    try
                    {
                        pi.PropertyInfo.SetValue(result, a);
                    }
                    catch(Exception e)
                    {

                    }

                    //}
                }
            }

            return result;
        }

    }

    //Stores file names, paths
    public class FileItem
    {
        public string FilePath { get; set; }
        public string EntityName { get; set; }
        public string FileMask { get; set; }
    }
    //Contain property fields, types, and numbers for mapping
    public class EntityTypeItem
    {
        public System.Type EntityType { get; set; }
        public SqlDbType? SQLtype { get; set; }
        public List<PropertyItem> Properties = new List<PropertyItem>();
        public List<FileItem> Files { get; set; }       

        public PropertyItem GetPropertyByName(string name_)
        {
            PropertyItem result = null;
            if(this.Properties.Where(s => s.PropertyName == name_).Any())
            {
                result = this.Properties.Where(s => s.PropertyName == name_).First();
            }          
            return result;
        }
        public int GetColumnNumberByName(string name_)
        {
            int result = 0;
            if (this.Properties.Where(s => s.PropertyName == name_).Any())
            {
                result = this.Properties.Where(s => s.PropertyName == name_).First().ColumnNumber;
            }
            return result;
        }
        public PropertyItem GetPropertyByColumnNumber(int columNumber_)
        {
            PropertyItem result = null;
            if (this.Properties.Where(s => s.ColumnNumber == columNumber_).Any())
            {
                result = this.Properties.Where(s => s.ColumnNumber == columNumber_).First();
            }
            return result;
        }
    }

    //Property Items
    public class PropertyItem
    {
        public string EntityTypeName { get; set; }
        public string PropertyName { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public int ColumnNumber { get; set; }
        public dynamic Value { get; set; }

        public System.Type CLR_type {get;set;}
        public System.Type SQL_type { get; set; }
    }  
    
    
    /// <summary>
    /// Holds entity names and propertynames to masks.
    /// Checks config file presence , export,import data to/from config with JSON convert.
    /// </summary>
    public abstract class NameMask
    {
        internal virtual string maskFileName { get; set; }

        public string Name { get; set; }
        public string Mask { get; set; }

        public List<NameMaskShadow> namesMaskList = new List<NameMaskShadow>();
     
        public virtual string GetNameMasked(string input_, List<NameMaskShadow> namesMaskList_)
        {
            string result = null;
            Regex r;
            Match m;
            
            if(namesMaskList_.Where(s => new Regex(s.Mask, RegexOptions.IgnoreCase).Match(input_).Success).Any())
            {
                result = namesMaskList_.Where(s => new Regex(s.Mask, RegexOptions.IgnoreCase).Match(input_).Success).Select(s=>s.Name).First();
            }

            //foreach (string str in namesMaskList.Select(s => s.Mask))
            //{
            //    r = new Regex(str, RegexOptions.IgnoreCase);
            //    m = r.Match(input_);
            //    if (m.Success)
            //    {
            //        result = namesMaskList.Where(s => s.Mask == str).Select(s => s.Name).First();
            //    }
            //}

            return result;
        }
        public virtual void Import(string folder_)
        {
            if (FileCheck(folder_))
            {
                string result = File.ReadAllText(folder_ + @"\" + maskFileName);
                namesMaskList = JsonConvert.DeserializeObject<List<NameMaskShadow>>(result);
            }
        }
        public virtual void Export(string folder_)
        {
            if (!FileCheck(folder_))
            {
                string export = JsonConvert.SerializeObject(namesMaskList);
                File.WriteAllText(folder_ + @"\" + maskFileName, export);
            }
        }
        internal bool FileCheck(string folder_)
        {
            bool result = false;
            if (Directory.Exists(folder_)) { if (File.Exists(folder_ + @"\" + maskFileName)) { result = true; } }
            return result;
        }
        public virtual void Generate()
        {
           
        }
        public void Init(string folder_)
        {
            if (FileCheck(folder_))
            {
                Import(folder_);
            }
            else
            {
                Generate();
                Export(folder_);
            }
        }
    }
    public class NameMaskShadow : NameMask
    {

    }
    public class EntityNameMask : NameMaskShadow
    {
        public List<EntityNameMask> namesMaskList = new List<EntityNameMask>();
        public EntityNameMask()
        {
            base.maskFileName = @"entity_types_masks.txt";
        }
        public override void Generate()
        {
            this.namesMaskList.Add(new EntityNameMask { Name = @"KEY_CLIENTS", Mask = @"((key(.*)clients)|(clients(.*)key))\.xlsx", IndustryFile = "1" });
            this.namesMaskList.Add(new EntityNameMask { Name = @"ECOMM_CLIENTS", Mask = @"((ecomm(.*)clients)|(clients(.*)ecomm))\.xlsx", IndustryFile = "2" });
        }
        public string IndustryFile {get;set; }
        public override void Import(string folder_)
        {
            if (FileCheck(folder_))
            {
                string result = File.ReadAllText(folder_ + @"\" + maskFileName);
                this.namesMaskList = JsonConvert.DeserializeObject<List<EntityNameMask>>(result);
            }
        }
    }
    public class PropertyNameMask : NameMaskShadow
    {
        public List<PropertyNameMask> namesMaskList = new List<PropertyNameMask>();
        public PropertyNameMask()
        {
            base.maskFileName = @"properties_masks.txt";
        }
        public string Entity { get; set; }
        public override void Generate()
        {
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"INDUSTRY_FILE", Mask = @"((industry(.*)file)|(file(.*)industry))" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"RESPONSIBILITY_GROUP", Mask = @"((responsibility(.*)group)|(group(.*)responsibility))" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"RESPONSIBILITY_MANAGER", Mask = @"((responsibility(.*)manager)|(manager(.*)responsibility))" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"GROUP_NAME", Mask = @"((group(.*)name)|(name(.*)group))" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"INDUSTRY", Mask = @"\b(INDUSTRY)\b" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"INDUSTRY_SECONDARY", Mask = @"((INDUSTRY(.*)lv.2)|(lv.2(.*)secondary))" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"SE_NAME", Mask = @"((se(.*)name)|(name(.*)se))" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"PHYSICAL_ADDRESS", Mask = @"((physical(.*)address)|(address(.*)physical))" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"CITY", Mask = @"\b(CITY)\b" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"SE_NUMBER", Mask = @"((se(.*)number)|(number(.*)se))" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"LEGAL_ENTITY", Mask = @"((legal(.*)entity)|(entity(.*)legal))" });
this.namesMaskList.Add(new PropertyNameMask { Entity = @"KEY_CLIENTS", Name = @"PROVIDER_NAME", Mask = @"((provider(.*)name)|(name(.*)provider))" });
        }
        public override void Import(string folder_)
        {
            if (FileCheck(folder_))
            {
                string result = File.ReadAllText(folder_ + @"\" + maskFileName);
                this.namesMaskList = JsonConvert.DeserializeObject<List<PropertyNameMask>>(result);
            }
        }
        public string GetNameMasked(string input_, List<PropertyNameMask> namesMaskList_)
        {
            string result = null;
            Regex r;
            Match m;

            if (namesMaskList_.Where(s => new Regex(s.Mask, RegexOptions.IgnoreCase).Match(input_).Success).Any())
            {
                result = namesMaskList_.Where(s => new Regex(s.Mask, RegexOptions.IgnoreCase).Match(input_).Success).Select(s => s.Name).First();
            }

//foreach (string str in namesMaskList.Select(s => s.Mask))
//{
//    r = new Regex(str, RegexOptions.IgnoreCase);
//    m = r.Match(input_);
//    if (m.Success)
//    {
//      result = namesMaskList.Where(s => s.Mask == str).Select(s => s.Name).First();
//    }
//}

            return result;
        }
    }

    //holds mapping of SQL DB types to CLR types
    public static class SQL_CLR_mapper
    {
        public static Dictionary<SqlDbType, Type> typeslist = new Dictionary<SqlDbType, Type>();
        static SQL_CLR_mapper()
        {
            typeslist.Add(SqlDbType.Int, typeof(Int32));
            typeslist.Add(SqlDbType.BigInt, typeof(Int64));
            typeslist.Add(SqlDbType.VarBinary, typeof(String));
            typeslist.Add(SqlDbType.DateTime, typeof(DateTime));
            typeslist.Add(SqlDbType.Date, typeof(DateTime));
        }
    }
    #endregion

    public static class test
    {
        static SQL_entity ent = new SQL_entity();
        static rExcelExport model = new rExcelExport();
        static rParameters params_ = new rParameters();

        public static void exportCheck()
        {
            model.SetEntity(ent);
            model.GetEntityNames();
            model.SetParameters(params_);

            string st = new DateTime(2015, 01,01, 15, 15, 15).ToString();
            string fn = new DateTime(2015, 03, 01, 10, 05, 05).ToString();
            string tablename = "TEMP_ACQ_D";
            bool listInc = true;

            ExcelExportTest(st, fn, tablename, listInc);

            tablename = "TEMP_ACQ_M";
            ExcelExportTest(st, fn, tablename, listInc);

            st = new DateTime(2016, 11, 14, 00, 00, 00).ToString();
            fn = new DateTime(2016, 11, 15, 23, 59, 59).ToString();
            tablename = "TEMP_ACQ";
            listInc = false;
            ExcelExportTest(st, fn, tablename, listInc);
        }
        public static void ExcelExportTest(string dateFrom_, string dateTo_, string tableName_, bool? listInclude_)
        {
            params_.rParametersInit(dateFrom_, dateTo_, tableName_, listInclude_);
            model.SetFile(@"C:\DEBUG\report.xlsx");
            
            model.ExcelExport();
        }
        public static void ParserCheck()
        {
            string path = @"C:\FILES\SHARE";
            string[] files = Directory.GetFiles(path);           
            
            foreach(string file in files)
            {
                
            }           
        }
        public static void AddPropertyCheck(Repo.DAL.KEY_CLIENTS_SQL kk,string propertyName_,object value_)
        {
            var kkDict = kk as IDictionary<string, object>;
            kkDict.Add(propertyName_, value_);
        }
        public static void EntityInstanceCheck()
        {
            int order = 0;
            SQL_entity ent = new SQL_entity();
            Repo.UtilityRepository ur0 = new Repo.UtilityRepository(ent);

            System.Type type =
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(s => s.Namespace == @"Repo.DAL" && s.Name == @"KEY_CLIENTS").First();
            Repo.DAL.KEY_CLIENTS_SQL kk_ = (Repo.DAL.KEY_CLIENTS_SQL)Activator.CreateInstance(type);
            kk_.RESPONSIBILITY_GROUP = @"GROUP_2";
            kk_.SECTOR_ID = 3;

            dynamic item_;
            item_ = new Repo.DAL.KEY_CLIENTS_SQL() { RESPONSIBILITY_GROUP = @"DYN_GR", SECTOR_ID = 5 };
            ur0.Key_Clients_Insert(item_);

            //dynamic property from string
            dynamic item2_;
            item2_ = Activator.CreateInstance(type);
            PropertyInfo p = type.GetProperty(@"SECTOR_ID");
            p.SetValue(item2_, 2);
            ur0.Key_Clients_Insert(item2_);

            if (order == 0)
            {
                ur0.Key_Clients_Insert(kk_);
                ur0.contextSaveChanges();
            }

            List<Repo.MASK_NAME> maskName = ur0.Masks;

            IEnumerable<System.Type> pp = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(s => s.Namespace == @"Repo.DAL");
            System.Type entType = (from s in pp where s.Name == @"KEY_CLIENTS" select s).First();
            PropertyInfo[] pi = entType.GetProperties();
            PropertyItem Properties = new PropertyItem();
            PropertyNameMask propertiesMasks = new PropertyNameMask();
            
            foreach(PropertyInfo pi_ in pi)
            {
                Mapper.EnityTypeItemGet(@"KEY_CLIENTS").Properties.Add(new PropertyItem { EntityTypeName = @"KEY_CLIENTS", PropertyName = pi_.Name, PropertyInfo = pi_ });
            }

            PropertyItem resp = Mapper.EnityTypeItemGet(@"KEY_CLIENTS").GetPropertyByName(propertiesMasks.GetNameMasked(@"RESPONSIBILITY_GROUP", propertiesMasks.namesMaskList));
            resp.ColumnNumber = 5;
            resp.Value = @"RESP_TEST";

            PropertyItem resp2 = Mapper.EnityTypeItemGet(@"KEY_CLIENTS").GetPropertyByName(propertiesMasks.GetNameMasked(@"SECTOR_ID", propertiesMasks.namesMaskList));
            resp2.ColumnNumber = 3;
            resp2.Value = int.Parse(@"2");
            
            Repo.UtilityRepository ur = new Repo.UtilityRepository(ent);
            Repo.DAL.KEY_CLIENTS_SQL kk = new Repo.DAL.KEY_CLIENTS_SQL() { RESPONSIBILITY_GROUP = resp.Value ,SECTOR_ID=resp2.Value};          
            if (order == 1)
            {
                ur.Key_Clients_Insert(kk);
                ur.contextSaveChanges();
            }          
        }

        public static void MapperCheck()
        {
            Mapper.MapperInit();

            Mapper.ParseHeader(@"RESPONSIBILITY_GROUP", 1);
            Mapper.ParseHeader(@"SECTOR_ID", 3);

            Mapper.SetValue(1, @"RESP_check");
            Mapper.SetValue(5, @"2");
            Mapper.SetValue(@"RESPONSIBILITY_MANAGER", @"manager check");
            Mapper.SetValue(@"DATE", DateTime.Now.ToString());

            Repo.DAL.KEY_CLIENTS_SQL kk_ = Mapper.PropertyValuesBind();

            SQL_entity ent = new SQL_entity();
            Repo.UtilityRepository ur = new Repo.UtilityRepository(ent);
            ur.Key_Clients_Insert(kk_);
            ur.contextSaveChanges();
        }

        //whole mapper parse >> init >> insert route
        public static void MapperRegexpCheck(string path_)
        {
            string a = new Regex(@"((file:///)|(/\w+.DLL\b))").Replace(Assembly.GetExecutingAssembly().CodeBase.ToString(),@"");
            Mapper.MasksInit(path_);
            string propertyType = @"";
            string fileName = @"";
            foreach (string file in Directory.GetFiles(path_))
            {
                propertyType = Mapper.ParseFileName(file);
                if (propertyType!=@"")
                {
                    fileName = file;
                    break;
                }
            }
            //Mapper.MapperInit();

            SQL_entity ent = new SQL_entity();
            Repo.UtilityRepository ur = new Repo.UtilityRepository(ent);

            Mapper.MapperInitWithNames(@"Repo.DAL", propertyType);
            Mapper.CurrentEntityFromListSet(propertyType);

            Mapper.ParseHeader(@"RESPONSIBILITY_GROUP", 1);
            Mapper.ParseHeader(@"responsibility group", 2);
            Mapper.ParseHeader(@"SECTOR_ID", 4);
            Mapper.ParseHeader(@"sector motherfucking id", 3);
            Mapper.ParseHeader(@"industry", 7);
            Mapper.ParseHeader(@"id", 5);
            Mapper.ParseHeader(@"AidB", 6);
            Mapper.ParseHeader(@"se number", 8);

            Mapper.SetValue(1,@"6");
            Mapper.SetValue(2,@"7");
            Mapper.SetValue(3,@"2");
            Mapper.SetValue(4,@"3");
            Mapper.SetValue(5,@"10");
            Mapper.SetValue(7,@"IND_CHECK");
            Mapper.SetValue(8, (new Random().Next(1, 9299998 + 1)).ToString());
            
            var kk = Mapper.PropertyValuesBind();
           
            ur.Key_Clients_Insert(kk);
            ur.contextSaveChanges();

        }
        public static void InsertCheckWrite(string path_)
        {
            string a = new Regex(@"((file:///)|(/\w+.DLL\b))").Replace(Assembly.GetExecutingAssembly().CodeBase.ToString(), @"");

            Mapper.MasksInit(path_);
            Mapper.MapperInitWithNames(@"Repo.DAL", @"KEY_CLIENTS");
            Mapper.CurrentEntityFromListSet(@"KEY_CLIENTS");

            Mapper.ParseHeader(@"RESPONSIBILITY_GROUP", 1);
            Mapper.ParseHeader(@"responsibility group", 2);
            Mapper.ParseHeader(@"SECTOR_ID", 4);
            Mapper.ParseHeader(@"sector motherfucking id", 3);
            Mapper.ParseHeader(@"industry", 7);
            Mapper.ParseHeader(@"id", 5);
            Mapper.ParseHeader(@"AidB", 6);
            Mapper.ParseHeader(@"se number", 8);

            Mapper.SetValue(1, @"6");
            Mapper.SetValue(2, @"7");
            Mapper.SetValue(3, @"2");
            Mapper.SetValue(4, @"3");
            Mapper.SetValue(5, @"10");
            Mapper.SetValue(7, @"IND_CHECK");
            Mapper.SetValue(8, (new Random().Next(1, 9299998 + 1)).ToString());

            StringBuilder sb = new StringBuilder();
            StreamWriter sw = new StreamWriter(a + @"\insertCheck_0.txt");

            //sb.AppendLine(InsertCheck(1000, 100));
            //sb.AppendLine(InsertCheck(4000, 100));
            //sb.AppendLine(InsertCheck(6000, 100));

            //sb.AppendLine(InsertCheck(1000, 1000));
            //sb.AppendLine(InsertCheck(4000, 1000));
            //sb.AppendLine(InsertCheck(6000, 1000));

            //sb.AppendLine(InsertCheck(100000, 1000));
            //sb.AppendLine(InsertCheck(10000, 100));

            //sb.AppendLine(InsertCheck(50000, 1000));
            //sb.AppendLine(InsertCheck(100000, 1000));

            //sb.AppendLine(InsertComm(10000));
            //sb.AppendLine(InsertComm(50000));
            //sb.AppendLine(InsertComm(100000));

            //sb.AppendLine(InsertBuilder(200000, 0));

            //sb.AppendLine(InsertEntityCicle(30000, 1000));
            //sb.AppendLine(InsertEntityCicle(30000, 1000));

            sb.AppendLine(InsertCheck(30000,1000));

            sw.WriteLine(sb.ToString());
            sw.Close();
        }

        public static string InsertCheck(int amount_,int partition_)
        {
            List<Repo.DAL.KEY_CLIENTS_SQL> kk_list = new List<Repo.DAL.KEY_CLIENTS_SQL>();
            var watch1 = new System.Diagnostics.Stopwatch();
            var watch2 = new System.Diagnostics.Stopwatch();

            watch1.Start();
            for (int i =0;i<= amount_; i++)
            {
                Mapper.SetValue(8, (new Random().Next(1, 92999999 + 1)).ToString());
                kk_list.Add(Mapper.PropertyValuesBind());
            }
            watch1.Stop();

            watch2.Start();
            SQL_entity ent = new SQL_entity();
            ent.Configuration.AutoDetectChangesEnabled = false;
            Repo.UtilityRepository ur = new Repo.UtilityRepository(ent);
            int count = 0;            

            foreach (var item in kk_list)
            {
                count += 1;
                ur.Key_Clients_Insert(item);
                
                if(count==partition_)
                {
                    count = 0;
                    ur.contextSaveChanges();
                    ent.Dispose();
                    ur.Dispose();
                    ent = new SQL_entity();
                    ur = new Repo.UtilityRepository(ent);
                    ent.Configuration.AutoDetectChangesEnabled = false;
                    ur = new Repo.UtilityRepository(ent);
                }
            }
            
            watch2.Stop();

            string report = String.Format(@"(METHOD|AMOUNT|PARTITION|ADD|INSERT) RepoCicle {0}|{1}|{2}|{3}",
                amount_, partition_, watch1.ElapsedMilliseconds / 1000, watch2.ElapsedMilliseconds / 1000);

            System.Diagnostics.Debug.WriteLine(report);
            return report;
        }
        public static string InsertComm(int amount_, int partition_=0)
        {
            List<Repo.DAL.KEY_CLIENTS_SQL> kk_list = new List<Repo.DAL.KEY_CLIENTS_SQL>();
            var watch1 = new System.Diagnostics.Stopwatch();
            var watch2 = new System.Diagnostics.Stopwatch();
            int count = 0;

            string command = @"insert into dbo.key_clients (se_number,se_name) values (@SE_NUMBER,@SE_NAME);";
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQL_entity"].ToString());
            conn.Open();
            SqlCommand comm = new SqlCommand(command, conn);
            
            comm.Parameters.Add(@"SE_NUMBER", SqlDbType.Int);

            watch1.Start();
            for (int i = 0; i <= amount_; i++)
            {
                Mapper.SetValue(8, (new Random().Next(1, 92999999 + 1)).ToString());
                kk_list.Add(Mapper.PropertyValuesBind());
            }
            watch1.Stop();

            watch2.Start();
            foreach (var item in kk_list)
            {
                comm.Parameters[@"SE_NUMBER"].Value = new Random().Next(1, 92999999 + 1);
                comm.ExecuteNonQuery();
            }            
            conn.Close();
            conn.Dispose();
            comm.Dispose();
            watch2.Stop();

            string report = String.Format(@"SqlConnection {0}, partitioned by {1}, added in {2}, inserted in {3}",
            amount_, partition_, watch1.ElapsedMilliseconds / 1000, watch2.ElapsedMilliseconds / 1000);

            System.Diagnostics.Debug.WriteLine(report);
            return report;
        }
        public static string InsertBuilder(int amount_, int partition_ = 0)
        {
            SQL_entity ent = new SQL_entity();
            var a = from s in ent.KEY_CLIENTS select s;
            ent.KEY_CLIENTS.RemoveRange(a);
            ent.SaveChanges();

            List<Repo.DAL.KEY_CLIENTS_SQL> kk_list = new List<Repo.DAL.KEY_CLIENTS_SQL>();
            var watch1 = new System.Diagnostics.Stopwatch();
            var watch2 = new System.Diagnostics.Stopwatch();
            int count = 0;

            int se_num = 0;

            string result = "";
            string command = @"select * from dbo.key_clients";

            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[@"SQL_entity"].ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(command, connection);
            DataSet ds = new DataSet();
            DataRow dr;           
            DataTable dt ;
            SqlCommandBuilder cm;
            
            watch1.Start();
            for (int i = 0; i <= amount_; i++)
            {
                Mapper.SetValue(8, (new Random().Next(1, 92999999 + 1)).ToString());
                kk_list.Add(Mapper.PropertyValuesBind());
            }
            watch1.Stop();

            adapter.Fill(ds);
            dt=ds.Tables[0];

            watch2.Start();
            for (int i =0;i<amount_;i++)
            {
                count += 1;
                dr = dt.NewRow();
                se_num = new Random().Next(1, 9999999);
                dr[@"SE_NUMBER"] = se_num;
                dr[@"SE_NAME"] = @"NAME1";
                dt.Rows.Add(dr);

                if(count == partition_)
                {
                    count = 0;
                  
                }
            }

            cm = new SqlCommandBuilder(adapter);
            adapter.Update(ds);
            ds.Clear();
            adapter.Fill(ds);
            watch2.Stop();

            result = String.Format(@"Driver {0}, partitioned by {1}, added in {2}, inserted in {3}",
            amount_, partition_, watch1.ElapsedMilliseconds / 1000, watch2.ElapsedMilliseconds / 1000);

            System.Diagnostics.Debug.WriteLine(result);

            return result;            
        }
        public static string InsertEntityCicle(int amount_, int partition_)
        {
            List<Repo.DAL.KEY_CLIENTS_SQL> kk_list = new List<Repo.DAL.KEY_CLIENTS_SQL>();
            var watch1 = new System.Diagnostics.Stopwatch();
            var watch2 = new System.Diagnostics.Stopwatch();

            watch1.Start();
            for (int i = 0; i <= amount_; i++)
            {
                //Mapper.SetValue(8, (new Random().Next(1, 92999999 + 1)).ToString());
                kk_list.Add(new Repo.DAL.KEY_CLIENTS_SQL() { SE_NUMBER = new Random().Next(1, 92999999 + 1) });
            }
            watch1.Stop();

            watch2.Start();                              
            int count = 0;
            SQL_entity ent = new SQL_entity();
            Repo.UtilityRepository ur = new Repo.UtilityRepository(ent);
            foreach (var item in kk_list)
            {
                count += 1;
                ent.Configuration.AutoDetectChangesEnabled = false;
                ur.Key_Clients_Insert(item);
                
                if (count == partition_)
                {
                    count = 0;
                    ur.contextSaveChanges();
                    ent.Dispose();
                    ur.Dispose();
                    ent = new SQL_entity();
                    ur = new Repo.UtilityRepository(ent);
                    ent.Configuration.AutoDetectChangesEnabled = false;
                }
            }
            watch2.Stop();

            string report = String.Format(@"Entities Repo {0}, partitioned by {1}, added in {2}, inserted in {3}",
            amount_, partition_, watch1.ElapsedMilliseconds / 1000, watch2.ElapsedMilliseconds / 1000);

            System.Diagnostics.Debug.WriteLine(report);
            return report;
        }

        //export import property masks
        public static void MaskIOCheck(string folder_)
        {
            PropertyNameMask pm = new PropertyNameMask();
            pm.Export(folder_);
            pm.Import(folder_);
        }

        public static void SQL_CLR_check(string path_)
        {
            string a = new Regex(@"((file:///)|(/\w+.DLL\b))").Replace(Assembly.GetExecutingAssembly().CodeBase.ToString(), @"");

            StringBuilder sb = new StringBuilder();
            StreamWriter sw = new StreamWriter(a + @"\insertCheck.txt");

            Type int32_ = typeof(System.Int32);
            Type int64_ = typeof(System.Int64);
            Type string_ = typeof(System.String);
            Type date_ = typeof(System.DateTime);

            KeyValuePair<SqlDbType, System.Type> dict;
            List<Type> types = new List<Type>()
            {
                typeof(System.Int32),
                typeof(System.Int64),
                typeof(System.String),
                typeof(System.DateTime)
            };

            foreach(Type type_ in types)
            {
                if (SQL_CLR_mapper.typeslist.Where(s => s.Value == type_).Any())
                {
                    dict = SQL_CLR_mapper.typeslist.Where(s => s.Value == type_).FirstOrDefault();
                    sb.AppendLine(type_.FullName + @" ; " + dict.Key + @" ; ");
                }
            }

            sw.WriteLine(sb.ToString());
            sw.Close();
        }

    }

}