using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Repo.DAL;
using Repo.DAL.SQL_ent;

using System.Data.Entity;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;

using System.Linq.Expressions;

using System.Data;


namespace Repo
{
    //Testing Repositories and Units of Work
    class REPO
    {
        protected static DateTime st = new DateTime(2016, 01, 14, 00, 00, 00, DateTimeKind.Utc);
        protected static DateTime fn = new DateTime(2016, 03, 15, 23, 59, 59);

        public REPO()
        {
            test();
        }

        public void test()
        {
            //to test
            /*
            UOWcheck();
            UOWinsertCheck();                      
            InstantiateFromType();
            */

            MigrateCheck();
       
            
        }

        public void DWH_check()
        {

            using (DWH_entities ds = new DWH_entities())
            {
                ds.Database.Initialize(true);
                ds.Database.Log = LogWrite;

                var a_ = from s in ds.T_ACQ_D
                         where s.DATE.Value >= st && s.DATE.Value <= fn
                         select s;

                int b_ = a_.Count();

                foreach (var c_ in a_)
                {

                }

                //Repository<T_ACQ_D> rp2 = new Repository<T_ACQ_D>(ds);
                //IQueryable<T_ACQ_D> q2 = rp2.DWHGetDateMerchant(st, fn, "9290570092");
                //int c2 = q2.Count();

                Repository<T_ECOMM_D_SQL> rp = new Repository<T_ECOMM_D_SQL>(ds);
                IQueryable<T_ECOMM_D_SQL> q = rp.DWHGetByDate(st, fn);
                int c = q.Count();
            }

            //9290570092

        }
        public void SQL_check()
        {
            SQL_entity ent = new SQL_entity();
            Repository<TEMP_ACQ_D_SQL> rp = new Repository<TEMP_ACQ_D_SQL>(ent);

            var a = from s in rp.SQLGetByMonth(st, fn) select s;
            int b = a.Count();

            var a_ = from s in ent.TEMP_ACQ_D
                     where DbFunctions.CreateDateTime(s.DATE.Value.Year, s.DATE.Value.Month, s.DATE.Value.Day, 00, 00, 00) == st
                     select s;

            int b_ = a_.Count();
        }
        public void DatesCheck()
        {
            SQL_entity ds = new SQL_entity();
            Repository<TEMP_ACQ_D_SQL> rp = new Repository<TEMP_ACQ_D_SQL>(ds);

            int stY = st.Year;
            int stm = st.Month;

            int fnY = fn.Year;
            int fnm = fn.Month;

            //test select from temp_acq by days
            var a = from s in ds.TEMP_ACQ_D
                    where
                    s.DATE >= st
                    &&
                    s.DATE <= fn
                    select s;

            int cnt = a.Count();

            var a2 = from s in ds.TEMP_ACQ_M
                     where
                     s.DATE >= st
                     &&
                     s.DATE <= fn
                     select s;

            int cnt2 = a2.Count();

            foreach (var b in a)
            {
                var c = b;
            }

        }
        public void MerchantsCheck()
        {
            SQL_entity ent = new SQL_entity();
            Repository<TEMP_ACQ_D_SQL> rep = new Repository<TEMP_ACQ_D_SQL>(ent);

            var a = rep.SQLGetList();
            int cnt = a.Count();
        }
        public void MigrateCheck()
        {
            // I
            //Migration.Migrate<TEMP_CTL_D>();
            //Migration.Migrate<TEMP_ACQ_D>();
            //Migration.Migrate<TEMP_ACQ_M>();
            //Migration.Migrate<TEMP_ECOMM_D>();

            //// II
            //Migration.Migrate<TEMP_ACQ>();

            Migration.Migrate_T_ACQ_M_bd(new DateTime(2017, 04, 01, 00, 00, 00)
                , new DateTime(2017, 04, 30, 23, 59, 59));

        }
        public void Chainingcheck()
        {
            SQL_entity ent = new SQL_entity();
            Repository<TEMP_ACQ_D_SQL> rep = new Repository<TEMP_ACQ_D_SQL>(ent);
            rep.SQLGetByDay(st, fn);
            

            //chn.GetByMerchant();
            var a = rep.GetResult();
            int c = a.Count();
        }
        public void TypeCheck(string typename_)
        {
            SQL_entity ent = new SQL_entity();

            var a = ent.GetType();
            PropertyInfo[] b = a.GetProperties();
            foreach (PropertyInfo c in b)
            {
                if (c.Name == typename_)
                {
                    //Repository<a> rep = new Repository<a>();
                }
            }
        }
        public void LogWrite(string input_)
        {
            System.Diagnostics.Debug.WriteLine(input_);
        }
        public void KeyClientsInsertCheck()
        {
            SQL_entity ent = new SQL_entity();
            UtilityRepository ur = new UtilityRepository(ent);

            KEY_CLIENTS_SQL kk = new DAL.KEY_CLIENTS_SQL { SECTOR_ID = 2, MERCHANT = 929001 };
            ur.MerchantsCount();
            ur.Key_Clients_Insert(kk);
            ur.contextSaveChanges();
            ur.Key_Clients_DeleteAll();
            ur.contextSaveChanges();
        }
        public static void RegexpCheck()
        {
            int maskNum = 0, resultNum = 0;

            string[] masks = new string[100];
            string[] results = new string[1000];

            //masks[0] = "responsibility[_ ]?group";
            //masks[1] = @"((responsibility(.*)group)|(group(.*)responsibility))";
            //masks[2] = @"(responsibility[.*]group)";
            //masks[3] = @"\u005F";
            masks[4] = @"((ecomm(.*)clients)|(clients(.*)ecomm))(.*)\.xlsx";
            //masks[5] = @"((key(.*)clients)|(clients(.*)key))(.*)\.xlsx";

            //results[0] = "RESPONSIBILITY GROUP";
            //results[1] = "RESPONSIBILITY_GROUP";
            //results[2] = "RESPONSIBILITYGROUP";
            //results[3] = "aaaRESPONSIBILITYxxxGROUPzzz";
            //results[4] = "aaaRESPONSIBILITxxxGROUPzzz";
            //results[5] = "aaaRESPONSIBILITYxxxGROUzzz";
            //results[6] = "aaaRESPONSIBILITxxxGROUzzz";
            //results[7] = "responsibility";
            //results[8] = "group";
            //results[9] = "qwsRESPONSIBILITYerGROUPtyx";
            //results[10] = "qwsGROUPerRESPONSIBILITYtas";
            //results[11] = @"aB_c D";
            //results[12] = @"clients.ecomm.xlsx.xlsx";
            //results[13] = @"key_clients_tst.xlsx";
            //results[14] = @"clients_key_tst.xlsx.xlsx";
            //results[15] = @"key_clients_tst.xlsx";
            results[16] = @"clients.ecomm.xlsx.xlsx";
            results[17] = @"ecomm.clients.xlsx.xlsx";
            results[18] = @"clients_ecomm.xlsx";

            foreach (string mask in masks)
            {
                maskNum += 1;
                resultNum = 0;
                foreach (string result in results)
                {
                    resultNum += 1;
                    if (mask != null && result != null)
                    {
                        Regex r = new Regex(mask, RegexOptions.IgnoreCase);
                        Match m = r.Match(result);
                        string toWrite = string.Format("({3},{4}) {2}-'{0}','{1}' | (result-'mask','value')", mask, result, m.Success.ToString(), maskNum.ToString(), resultNum.ToString());
                        System.Diagnostics.Trace.WriteLine(toWrite);
                    }
                }
            }
        }    
        public static void Initialize()
        {
            SQL_entity context = new SQL_entity();
            SECTOR_SQL sector;
            List<SECTOR_MASK_SQL> masks = new List<SECTOR_MASK_SQL>();
            Random rand = new Random();

            context.SECTOR_NAMES.RemoveRange(from s in context.SECTOR_NAMES select s);
            context.SECTOR_MASKS.RemoveRange(from s in context.SECTOR_MASKS select s);
            context.REFMERCHANTS.RemoveRange(from s in context.REFMERCHANTS select s);

            List<SECTOR_SQL> names = new List<SECTOR_SQL>();
            sector = new SECTOR_SQL { SECTOR_NAME = @"Retail&Telecom" };
            masks.Add(new SECTOR_MASK_SQL { MASK = @"((retail(.*)telecom)|(telecom(.*)retail))", SECTOR = sector });
            sector = new SECTOR_SQL { SECTOR_NAME = @"Travel$Restaurants" };
            masks.Add(new SECTOR_MASK_SQL { MASK = @"((travel(.*)restaurants)|(restaurants(.*)travel))", SECTOR = sector });
            sector = new SECTOR_SQL { SECTOR_NAME = @"ECOMM" };
            masks.Add(new SECTOR_MASK_SQL { MASK = @"((.*)ECOMM(.*))", SECTOR = sector });
            sector = new SECTOR_SQL { SECTOR_NAME = @"Hotels" };
            masks.Add(new SECTOR_MASK_SQL { MASK = @"((.*)hotels(.*))", SECTOR = sector });
            foreach (SECTOR_MASK_SQL ms in masks)
            {
                context.SECTOR_MASKS.Add(ms);
            }
            context.SaveChanges();

            List<REFMERCHANTS_SQL> rf = new List<REFMERCHANTS_SQL>();
            int upper = rand.Next(1000, 3000);
            int merch_gen = rand.Next(29000000, 59999999);
            long merch_solid = 9000000000;
            string merch = merch_solid.ToString();

            for (int i = 0; i < upper; i++)
            {
                merch_solid = 9000000000;
                int type1 = rand.Next(290000000, 299999999);
                int type2 = rand.Next(590000000, 599999999);

                if (type1 % 2 == 0)
                {
                    merch_solid += type1;
                }
                else
                {
                    merch_solid += type2;
                }

                merch = merch_solid.ToString();
                context.REFMERCHANTS.Add(new REFMERCHANTS_SQL { ITEM_ID = merch_solid.ToString(), USER_ID = 0 });
            }
            context.SaveChanges();
        }
        public static void ReadRepoCheck()
        {
            SQL_entity ent = new SQL_entity();
            ReadRepo<T_ACQ_D_SQL> readRep = new ReadRepo<T_ACQ_D_SQL>(ent);
            MerchantFilterRepo<KEY_CLIENTS_SQL,REFMERCHANTS_SQL> filtRep = new MerchantFilterRepo<KEY_CLIENTS_SQL, REFMERCHANTS_SQL>(ent);
            DateFilterRepo<T_ACQ_D_SQL> dateFilter = new DateFilterRepo<T_ACQ_D_SQL>(ent);
            ChainingRepo<TEMP_ACQ_D_SQL> chainRep = new ChainingRepo<TEMP_ACQ_D_SQL>(ent);

            DateTime stDt = new DateTime(2016, 08, 21, 00, 00, 00);
            DateTime fnDt = new DateTime(2016, 08, 22, 00, 00, 00);

            DateTime st = new DateTime(2016, 02, 01, 00, 00, 00);
            DateTime fn = new DateTime(2016, 06, 01, 00, 00, 00);

            //48
            int cnt_1 = readRep.GetByFilter<T_ACQ_M_SQL>(s => s.MERCHANT == 9294109921).Count();

            //50830
            int cnt_2 = dateFilter.GetByDate(stDt, fnDt).Count();
            //128
            int cnt_3 = filtRep.GetByMerchantFilter<T_ACQ_D_SQL>().Count();

            //2042
            int cnt_4 = chainRep.FilterByDate(st, fn)._result.Count();
            //597
            int cnt_5 = chainRep.FilterByMerchants<REFMERCHANTS_SQL>()._result.Count();

        }
        public static void UOFfromStringCheck()
        {
            
            var a = (from s in AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(s => s.Namespace == "Repo.DAL")
                     where s.Name == "KEY_CLIENTS"
                     select s).FirstOrDefault();
                     
            List<IMerchant> SectorEntityList = new List<IMerchant>();          

            DateTime st = new DateTime(2016, 01, 01, 00, 00, 00);
            DateTime fn = new DateTime(2016, 08, 22, 00, 00, 00);

            Edit_UOF t_uof = new Edit_UOF();

            dynamic dyn=null;
            dynamic dynmc = null;

            int cnt = 0;
            string type_ = @"T_ACQ_D";
            switch(type_)
            {
                case @"T_ACQ_D":
                    dyn = t_uof.GetAllACQ_D();
                break;
                case @"KEY_CLIENTS":
                    dyn = t_uof.GetAllKK();
                break;
            }

            //dynmc = InstantiateFromString(type_);
            LoopThrougt(SectorEntityList, dyn);
            AddFrom(SectorEntityList,t_uof, type_);
            t_uof.Dispose();
        }
        public static void UOF_one_another_check()
        {
            Edit_UOF eof = new Edit_UOF();
            Edit_UOF eof2 = new Edit_UOF();           
            
            var a = eof.GetAllKK();
            foreach(var b in a)
            {
                eof2.AddKK(b);
            }

            eof2.SaveAll();
            eof.Dispose();
            eof2.Dispose();
        }
        public static dynamic InstantiateFromString()
        {
            string type_ = null;
            dynamic result_ = null;

            var a =  AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(s => s.Namespace == @"Repo.DAL");
            var b = (from s in a where s.Name == type_ select s).FirstOrDefault();
        
            Type genericType = typeof(Repository<>);
            
            Type uofType = genericType.MakeGenericType(b);           
            SQL_entity ent = new SQL_entity();
            result_ = Activator.CreateInstance(uofType);
            result_.dbContext = ent;

            result_.GetResult().Count();

            return result_;
        }
        public static void LoopThrougt(List<IMerchant> SectorEntityList, IQueryable<IMerchant> item)
        {
            var b = item.GetType();
            var c = b.GetProperties();
            foreach(var item_ in item)
            {
                //T_ACQ_D itm = new T_ACQ_D() { AMT = 123456789, MERCHANT = item_.MERCHANT, DATE = DateTime.Now };
                /*T_ACQ_D itm = new T_ACQ_D();
                itm = (T_ACQ_D)item_;*/
                SectorEntityList.Add(item_);
            }
        }
        public static void AddFrom(List<IMerchant> SectorEntityList,Edit_UOF t_uof_, string type_)
        {
            foreach (var item_ in SectorEntityList)
            {
                //T_ACQ_D_UOF.AddEntity(item_);
                switch (type_)
                {
                    case @"T_ACQ_D":
                        t_uof_.AddACQ_D(item_);
                    break;
                    case @"KEY_CLIENTS":
                        t_uof_.AddKK(item_);
                    break;
                }
                t_uof_.SaveAll();
            }
        }
        public static void AddFrom(List<IEntity> SectorEntityList, dynamic type_)
        {
            foreach (var item_ in SectorEntityList)
            {
                type_.AddEntity(item_);
            }
        }
        
        /// <summary>
        /// Instatiate and insert entity from string with string to property link with switch
        /// to unit of work MOTHERFUCK!
        /// For excel parsing Column - PropertyName - value reference needed
        /// like in PropertyItem class
        /// </summary>
        public static void InstantiateFromType()
        {

            SQL_entity ent = new SQL_entity();
            UnitOfWorkGeneric uow = new UnitOfWorkGeneric(ent);
           

            uow.CallDisconnectedProcedure(@"VALUES_INSERT");

            KEY_CLIENTS_SQL kk = new KEY_CLIENTS_SQL { MERCHANT = 9290000009, SECTOR_ID = 9 };
            REFMERCHANTS_SQL rm = new REFMERCHANTS_SQL { ID = 9, MERCHANT = 9290000010 };

            TypeMethodNameContainer tc = new TypeMethodNameContainer();
            string name_ = @"REFMERCHANTS_SQL";
            var types_ = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(s => s.Namespace == @"Repo.DAL");
            Type type_ = (from s in types_ where s.Name == name_ select s).FirstOrDefault();
            PropertyInfo propertyInf = null;
            IMerchant inst = null;
            PropertyInfo[] properties = null;


            switch (name_)
            {
                case @"KEY_CLIENTS_SQL":
                    inst = (KEY_CLIENTS_SQL)Activator.CreateInstance(type_);
                    properties = inst.GetType().GetProperties();
                    tc.PropertyInfoValueSet(properties, propertyInf, "SE_NUMBER", inst, 9290000010);
                    tc.PropertyInfoValueSet(properties, propertyInf, "MERCHANT", inst, 9290000010);
                    tc.PropertyInfoValueSet(properties, propertyInf, "SECTOR_ID", inst, 9);
                break;
                case @"REFMERCHANTS_SQL" :
                    inst = (REFMERCHANTS_SQL)Activator.CreateInstance(type_);
                    properties = inst.GetType().GetProperties();
                    tc.PropertyInfoValueSet(properties, propertyInf, "MERCHANT", inst, 9290000010);
                    tc.PropertyInfoValueSet(properties, propertyInf, "ID", inst, 9);
                break;
                case @"T_ACQ_D_SQL":
                    inst = (T_ACQ_D_SQL)Activator.CreateInstance(type_);
                    properties = inst.GetType().GetProperties();
                    tc.PropertyInfoValueSet(properties, propertyInf, "MERCHANT", inst, 9290000010);
                    tc.PropertyInfoValueSet(properties, propertyInf, "ID", inst, 9);
                    break;
            }
       
            tc.MethodGet(name_);
            tc.MethodInit(inst);
            tc.Save();

        }        
        //>>!!! To Unit Test
        public static void UOWcheck()
        {
            SQL_entity ent = new SQL_entity();
            UnitOfWorkGeneric uof = new UnitOfWorkGeneric(ent);
            uof.RefreshValues();

            //12
            var a = uof.GetByDate<T_ACQ_D_SQL>(st, fn).Count();
            //5
            var b = uof.GetByMerchants<KEY_CLIENTS_SQL>().Count();
            //10
            var c = uof.MerchantListCount();


            ReadRepo<KEY_CLIENTS_SQL> kkRead = new ReadRepo<KEY_CLIENTS_SQL>(ent);
            //5
            var d = uof.GetByMerchants<KEY_CLIENTS_SQL>().Count();
            uof.DeleteBySector(2);
            uof.SaveAll();
            //4
            var e = uof.GetByMerchants<KEY_CLIENTS_SQL>().Count();


            EditRepo<REFMERCHANTS_SQL> rfEdit = new EditRepo<REFMERCHANTS_SQL>(ent);
            //10
            var f = uof.MerchantListCount();
            uof.DeleteByUserID(0);
            uof.SaveAll();
            //9
            var g = uof.MerchantListCount();

            uof.RefreshValues();

            //27
            var h =
            uof.GetByDate<T_ACQ_D_SQL>(st, fn).Count()
            + uof.GetByMerchants<KEY_CLIENTS_SQL>().Count()
            + uof.MerchantListCount();

        }
        //>>!!! To Unit Test
        public static void UOWinsertCheck()
        {

            SQL_entity ent = new SQL_entity();
            EditRepo<KEY_CLIENTS_SQL> editRep = new EditRepo<KEY_CLIENTS_SQL>(ent);
            UnitOfWorkGeneric uof = new UnitOfWorkGeneric(ent);
            uof.RefreshValues();

            KEY_CLIENTS_SQL kk = new KEY_CLIENTS_SQL { MERCHANT = 9290000009, SECTOR_ID = 9 };

            //5
            var a = editRep.GetByFilter<KEY_CLIENTS_SQL>(s => s.ID != null).Count();
            uof.Add<KEY_CLIENTS_SQL>(kk);
            //6
            var b = editRep.GetByFilter<KEY_CLIENTS_SQL>(s => s.ID != null).Count();
            uof.RefreshValues();
            //5
            var c = editRep.GetByFilter<KEY_CLIENTS_SQL>(s => s.ID != null).Count();         

        }        
        //>>!!! To Unit Test
        public static void TypeInsertCheck()
        {
            SQL_entity ent = new SQL_entity();
            EditRepo<KEY_CLIENTS_SQL> editRep = new EditRepo<KEY_CLIENTS_SQL>(ent);
            UnitOfWorkGeneric uof = new UnitOfWorkGeneric(ent);
            uof.RefreshValues();
            EntityInitialization entityInit = new EntityInitialization();

            entityInit.KEY_CLIENTS_add(uof, 9290000009, 9, "ABCD");
            var a = editRep.GetByFilter<KEY_CLIENTS_SQL>(s => s.ID != null).Count();            
        }    
        
    }

    #region OldRepo
    public class BaseEntity
    {

    }
    public class DenormalEntity : BaseEntity
    {
        public virtual Nullable<System.DateTime> DATE { get; set; }
        public virtual string MERCHANT { get; set; }
    }

    //>>!!! split to two repositories in UOW for DWH and SQL context stores
    public class Repository<T> where T : class, IEntity, IDate
    {
        IDbSet<T> dbSet { get; set; }
        IDbSet<REFMERCHANTS_SQL> merchantSet { get; set; }
        DbContext dbContext { get; set; }

        public IQueryable<T> QueryResult = null;
        public IQueryable<REFMERCHANTS_SQL> ListResult = null;

        public Repository()
        {

        }
        public Repository(DbContext context_)
        {
            this.dbContext = context_;
            this.dbSet = dbContext.Set<T>();
            this.merchantSet = dbContext.Set<REFMERCHANTS_SQL>();
        }

        public IQueryable<T> GetResult()
        {
            return this.QueryResult;
        }

        public virtual T Add(T entity)
        {
            return dbSet.Add(entity);
        }
        public virtual IQueryable<T> SelectAll()
        {
            IQueryable<T> result = null;
            result = from s in this.dbSet select s;
            return result;
        }
        public virtual void ClearAll()
        {
            foreach (T item in this.SelectAll())
            {
                this.dbSet.Remove(item);
            }
        }
        public virtual void Save()
        {
            dbContext.SaveChanges();
        }
        public virtual void Dispose()
        {
            this.dbContext.Dispose();
        }
        public virtual int Count()
        {
            return this.dbSet.Count();
        }
        public int QueryResultCount()
        {
            int res = 0;
            if (QueryResult != null)
            {
                res = QueryResult.Count();
            }
            return res;
        }

        public IQueryable<T> DWHGetByDate(DateTime DateFrom_, DateTime DateTo_)
        {
            var a = from s in this.dbSet
                    where
                    s.DATE >= DateFrom_ && s.DATE <= DateTo_
                    select s;
            return a;
        }
       
        public IQueryable<T> DWHGetDateMerchant(DateTime DateFrom_, DateTime DateTo_, string merchant)
        {
            var a = from s in this.dbSet
                    where s.DATE.Value >= DateFrom_
                    &&
                    s.DATE.Value <= DateTo_
                    select s;
            return a;
        }
       

        public IQueryable<T> SQLGetByDay(DateTime DateFrom_, DateTime DateTo_)
        {
            var a =
                from s in this.dbSet
                where DbFunctions.CreateDateTime(s.DATE.Value.Year, s.DATE.Value.Month, s.DATE.Value.Day, 00, 00, 00) >=
                DbFunctions.CreateDateTime(DateFrom_.Year, DateFrom_.Month, DateFrom_.Day, 00, 00, 00)
                &&
                DbFunctions.CreateDateTime(s.DATE.Value.Year, s.DATE.Value.Month, s.DATE.Value.Day, 00, 00, 00) <=
                DbFunctions.CreateDateTime(DateTo_.Year, DateTo_.Month, DateTo_.Day, 00, 00, 00)
                select s;
            this.QueryResult = a;
            return a;
        }
        public IQueryable<T> SQLGetByMonth(DateTime DateFrom_, DateTime DateTo_)
        {
            var a =
                from s in this.dbSet
                where DbFunctions.CreateDateTime(s.DATE.Value.Year, s.DATE.Value.Month, 01, 00, 00, 00) >=
                DbFunctions.CreateDateTime(DateFrom_.Year, DateFrom_.Month, 01, 00, 00, 00)
                &&
                DbFunctions.CreateDateTime(s.DATE.Value.Year, s.DATE.Value.Month, 01, 00, 00, 00) <=
                DbFunctions.CreateDateTime(DateTo_.Year, DateTo_.Month, 01, 00, 00, 00)
                select s;
            this.QueryResult = a;
            return a;
        }
        public IQueryable<T> SQLGetByYear(DateTime DateFrom_, DateTime DateTo_)
        {
            var a =
                from s in this.dbSet
                where DbFunctions.CreateDateTime(s.DATE.Value.Year, 01, 01, 00, 00, 00) >=
                DbFunctions.CreateDateTime(DateFrom_.Year, 01, 01, 00, 00, 00)
                &&
                DbFunctions.CreateDateTime(s.DATE.Value.Year, 01, 01, 00, 00, 00) <=
                DbFunctions.CreateDateTime(DateTo_.Year, 01, 01, 00, 00, 00)
                select s;
            this.QueryResult = a;
            return a;
        }
        public IQueryable<REFMERCHANTS_SQL> SQLGetList()
        {
            var a = (from s in this.merchantSet select s);
            this.ListResult = a;
            return a;
        }

        public Repository<T> SQLFilterByDay(DateTime DateFrom_, DateTime DateTo_)
        {
            QueryResult = from s in QueryResult
                          where DbFunctions.CreateDateTime(s.DATE.Value.Year, s.DATE.Value.Month, s.DATE.Value.Day, 00, 00, 00) >=
                             DbFunctions.CreateDateTime(DateFrom_.Year, DateFrom_.Month, DateFrom_.Day, 00, 00, 00)
                             &&
                             DbFunctions.CreateDateTime(s.DATE.Value.Year, s.DATE.Value.Month, s.DATE.Value.Day, 00, 00, 00) <=
                             DbFunctions.CreateDateTime(DateTo_.Year, DateTo_.Month, DateTo_.Day, 00, 00, 00)
                          select s;
            return this;
        }
        public Repository<T> SQLFilterByMonth(DateTime DateFrom_, DateTime DateTo_)
        {
            QueryResult = from s in QueryResult
                          where DbFunctions.CreateDateTime(s.DATE.Value.Year, s.DATE.Value.Month, 01, 00, 00, 00) >=
                             DbFunctions.CreateDateTime(DateFrom_.Year, DateFrom_.Month, 01, 00, 00, 00)
                             &&
                             DbFunctions.CreateDateTime(s.DATE.Value.Year, s.DATE.Value.Month, 01, 00, 00, 00) <=
                             DbFunctions.CreateDateTime(DateTo_.Year, DateTo_.Month, 01, 00, 00, 00)
                          select s;
            return this;
        }
      
       
    }
    public class UtilityRepository
    {
        //>>!!! to Unit of work
        IDbSet<REFMERCHANTS_SQL> REFMERCHANTS { get; set; }
        IDbSet<KEY_CLIENTS_SQL> KEY_CLIENTS { get; set; }

        IDbSet<SECTOR_SQL> SECTOR_NAMES { get; set; }
        IDbSet<SECTOR_MASK_SQL> SECTOR_MASKS { get; set; }

        DbContext context { get; set; }

        public List<MASK_NAME> Masks = new List<MASK_NAME>();

        public UtilityRepository()
        {
            Repo.DAL.SQL_ent.SQL_entity ent = new SQL_entity();
            this.context = ent;
            REFMERCHANTS = context.Set<REFMERCHANTS_SQL>();
            KEY_CLIENTS = context.Set<KEY_CLIENTS_SQL>();
            SECTOR_NAMES = context.Set<SECTOR_SQL>();
            SECTOR_MASKS = context.Set<SECTOR_MASK_SQL>();
            //Masks_Init();
            Entity_check();
        }
        public UtilityRepository(SQL_entity ent)
        {
            this.context = ent;
            REFMERCHANTS = context.Set<REFMERCHANTS_SQL>();
            KEY_CLIENTS = context.Set<KEY_CLIENTS_SQL>();
            SECTOR_NAMES = context.Set<SECTOR_SQL>();
            SECTOR_MASKS = context.Set<SECTOR_MASK_SQL>();
            //Masks_Init();
            Entity_check();
        }
        public int MerchantsCount()
        {
            int result = 0;
            result = (from s in REFMERCHANTS select s).Count();
            return result;
        }
        public IQueryable<String> GetMerchants()
        {
            IQueryable<string> result = null;
            result = from s in REFMERCHANTS select s.ITEM_ID.ToString();
            return result;
        }

        //>>!!! to Unit of work/ Doubles Generic Methods
        public void Key_Clients_DeleteAll()
        {
            context.Set<KEY_CLIENTS_SQL>().RemoveRange(context.Set<KEY_CLIENTS_SQL>());
        }
        public void Key_Clients_DeleteByMerchant(int ID_)
        {
            KEY_CLIENTS = context.Set<KEY_CLIENTS_SQL>();
            var a = from s in this.KEY_CLIENTS where s.MERCHANT == ID_ select s;

            foreach (var b in a)
            {
                KEY_CLIENTS.Remove((KEY_CLIENTS_SQL)b);
            }
        }
        public void Key_Clients_DeleteBySectorID(int ID_)
        {
            DbSet<KEY_CLIENTS_SQL> KEY_CLIENTS = context.Set<KEY_CLIENTS_SQL>();
            var a = from s in this.KEY_CLIENTS where s.SECTOR_ID == ID_ select s;
            KEY_CLIENTS.RemoveRange(a);

            contextSaveChanges();
        }
        public void Key_Clients_Insert(KEY_CLIENTS_SQL kk)
        {
            var a = KEY_CLIENTS.Where((Expression<Func<KEY_CLIENTS_SQL, bool>>)(s => s.MERCHANT == kk.MERCHANT));
            //distinct check
            if (!KEY_CLIENTS.Where(s => s.MERCHANT == kk.MERCHANT).Any())
            {
                KEY_CLIENTS.Add(kk);
            }
        }

        public void Refmerchants_DeleteAll()
        {
            context.Set<REFMERCHANTS_SQL>().RemoveRange(context.Set<REFMERCHANTS_SQL>());
        }
        public void Refmerchants_DeleteByUserID(int UserID_)
        {
            REFMERCHANTS = context.Set<REFMERCHANTS_SQL>();
            var b = from s in REFMERCHANTS where s.USER_ID == UserID_ select s;
            foreach (var c in b)
            {
                REFMERCHANTS.Remove((REFMERCHANTS_SQL)c);
            }
        }
        public void Refmerchants_Insert(REFMERCHANTS_SQL rm)
        {
            REFMERCHANTS.Add(rm);
        }

        public List<MASK_NAME> Masks_Init()
        {
            List<MASK_NAME> result = new List<MASK_NAME>();
            if (SECTOR_NAMES != null)
            {
                foreach (SECTOR_SQL sn in SECTOR_NAMES.Include(s => s.SECTOR_MASKS))
                {
                    if (sn.SECTOR_MASKS != null)
                    {
                        foreach (SECTOR_MASK_SQL sm in sn.SECTOR_MASKS)
                        {
                            Masks.Add(new MASK_NAME { Sector = sn.SECTOR_NAME, Mask = sm.MASK });
                        }
                    }
                }
            }
            return result;
        }
        public void Entity_check()
        {
            var a = from s in this.SECTOR_MASKS select s;
            var b = from s in this.SECTOR_NAMES select s;

            int cnt = a.Count();
            cnt = b.Count();
        }

        public void contextSaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }
        public void Dispose()
        {
            this.context.Dispose();
        }
    }    

    public class MASK_NAME
    {
        public string Sector { get; set; }
        public string Mask { get; set; }
    }
    
    public static class RepoInit
    {
        public static dynamic RepoFromString(string name_,SQL_entity ent)
        {
            dynamic result = null;

            switch (name_)
            {
                case "T_ACQ_D_SQL":
                    result = new Repo.Repository<T_ACQ_D_SQL>(ent);
                    break;
                case "T_ACQ_M_SQL":
                    result = new Repo.Repository<T_ACQ_M_SQL>(ent);
                    break;              
                default:
                    result = null;
                    break;                
            }
            return result;
        }               
    }
    #endregion

    //migrate any table from DWH to SQL
    //>>!!! to repo for Idate, to UOF
    public static class Migration
    {
        static DateTime st = new DateTime(2015, 01, 01, 00, 00, 00);
        static DateTime fn = new DateTime(2016, 11, 15, 23, 59, 59);

        static IDbSet<REFMERCHANTS_SQL> merchantSet { get; set; }

        public static void Migrate<T>() where T : class, IEntity, IDate
        {
            try
            {
                DWH_entities dwh = new DWH_entities();
                SQL_entity sql = new SQL_entity();

                sql.Database.Initialize(true);

                Repository<T> acq_rep_dwh = new Repository<T>(dwh);
                Repository<T> acq_rep_sql = new Repository<T>(sql);

                IQueryable<T> a = acq_rep_dwh.DWHGetByDate(st, fn);
                int cnt = a.Count();

                foreach (var t in a)
                {
                    acq_rep_sql.Add(t);
                    acq_rep_sql.Save();
                }
            }
            catch (Exception e)
            {

            }
        }
        public static void MigrateByDate<T>(DateTime st_, DateTime fn_) where T : class, IEntity, IDate
        {
            try
            {
                DWH_entities dwh = new DWH_entities();
                SQL_entity sql = new SQL_entity();

                try
                {
                    sql.Database.Initialize(true);
                }
                catch(Exception e)
                {

                }

                Repository<T> acq_rep_dwh = new Repository<T>(dwh);
                Repository<T> acq_rep_sql = new Repository<T>(sql);

                IQueryable<T> a = acq_rep_dwh.DWHGetByDate(st_, fn_);

                int cnt = a.Count();

                foreach (var t in a)
                {
                    acq_rep_sql.Add(t);
                    acq_rep_sql.Save();
                }
            }
            catch (Exception e)
            {

            }
        }
        public static void Migrate_T_ACQ_M_bd(DateTime st_, DateTime fn_)
        {
            DWH_entities dwh = new DWH_entities();
            SQL_entity ent = new SQL_entity();

            var c = from s in dwh.Set < T_ECOMM_D >()  where s.DATE >= st && s.DATE <= fn select s ;

            ReadRepo<T_ACQ_M> readRep = new DateFilterRepo<T_ACQ_M>(dwh);
            EditRepo<T_ACQ_M_SQL> insRep = new EditRepo<T_ACQ_M_SQL>(ent);

            var a = readRep.GetByFilter<T_ACQ_M>(s => s.DATE >= st && s.DATE <= fn);

            var b = a.Count();
            int i = 0;

            foreach(T_ACQ_M m in a)
            {
                i += 1;
                insRep.AddEntity(ACQ_M_CONVERT(m));
                insRep.Save();
            }
        }

        /// <summary>
        /// Convert from DWH type to SQL type
        /// </summary>
        /// <param name="input_"></param>
        /// <returns></returns>
        public static T_ACQ_M_SQL ACQ_M_CONVERT(T_ACQ_M input_)
        {
            long val_ = 0;

            T_ACQ_M_SQL result_ = new T_ACQ_M_SQL();
            result_.ID = input_.ID;
            result_.DATE = input_.DATE;
            long.TryParse(input_.MERCHANT, out val_);
            result_.MERCHANT = val_;
            result_.AMT = input_.AMT;
            return result_;
        }
      
    }

    //>>!!! every method can be used on separate entity, check DbSet initialize composition
    /// <summary>
    /// Truly generic repository
    /// </summary>
    #region TGR
    public interface IEntity { int ID { get; set; } }
    
    public interface IEntity<T> : IEntity { }
    //public abstract class Entity<T> : IEntity<T> where T : DbContext { public int ID { get; set; } }

    public interface IDate : IEntity { Nullable<System.DateTime> DATE { get; set; } }
    public interface IByDate { IQueryable<T> GetByDate<T> (DateTime st,DateTime fn) where T : class, IDate; }

    public delegate void IMerchantMethod(IMerchant ent_);
    public interface IMerchant : IEntity { long? MERCHANT { get; set; } }
    public interface IByMerchant { IQueryable<T> GetByMerchantFilter<T>() where T : class, IMerchant; }

    public interface ISector : IMerchant { int? SECTOR_ID { get; set; } }
    public interface IBySector { void DeleteBySector(int id); }

    public interface IUser : IMerchant { int USER_ID { get; set; } }
    public interface IByUser { void DeleteByUserID(int id); }

    public interface IChainable
       : IDate, IMerchant
    {

    }
    public interface IChain
    {
        ChainingRepo<IChainable> FilterByDate(DateTime st, DateTime fn);
        ChainingRepo<IChainable> FilterByMerchants<K>() where K : class, IMerchant;
    }   
    
    public interface IRead
    {
        IQueryable<T> GetByFilter<T>(Expression<Func<T,bool>> filter = null,
        Func<IQueryable<T>,IOrderedQueryable<T>> OrderBy=null) where T : class, IEntity;
    }
    public interface IGetFilter : IByDate, IByMerchant
    {
        IQueryable<T> GetByMerchantFilter<T>() where T : class, IMerchant;
        new IQueryable<T> GetByDate<T>(DateTime st, DateTime fn) where T : class, IDate;
    }

    /// <summary>
    /// Repository to readonly context, get all, get by specific Expression<Func<T, bool>> filter, save and dispose
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadRepo<T> : IRead  where T: class, IEntity
    {
        internal DbContext _context { get; set; }
        internal DbSet<T> _dbSet { get; set; }
        public IQueryable<T> _result { get; set; }

        public ReadRepo(DbContext context_)

        {
            this._context = context_;
            this._dbSet = this._context.Set<T>();
        }

        private bool dispose = false;

        public void Save()
        {
            try
            {
                this._context.SaveChanges();
            }
            catch(DbEntityValidationException e)
            {
                foreach(var ev in e.EntityValidationErrors)
                {

                }
            }
        }
        public void Dispose(bool disposing_)
        {
            if (!this.dispose)
            {
                if (disposing_)
                {                   
                    this._context.Dispose();
                }
            }
            this.dispose = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }      

        public IQueryable<T> GetAll() 
        {
            IQueryable<T> result = this._dbSet;
            return result;
        }
        public IQueryable<T> GetByFilter<T>(Expression<Func<T, bool>> filter = null
        ,Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy = null) where T : class, IEntity
        {
            IQueryable<T> result = this._context.Set<T>();
            if(filter!= null)
            {
                result = result.Where(filter);
            }
            return result;
        }                       
    }
    /// <summary>
    /// Repository inherited from ReadRepo to addEntity,GetByID and Edit (as atach) entity
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EditRepo<T> : ReadRepo<T> where T : class, IEntity
    {
        public EditRepo(DbContext context_) : base(context_)
        {

        }
        public void AddEntity(T item_)
        {
            this._dbSet.Add(item_);
        }
        public T GetById(int ID_)
        {
            T result = null;
            if ((from s in this._dbSet where s.ID == ID_ select s).Any())
            {
                result = (from s in this._dbSet where s.ID == ID_ select s).FirstOrDefault();
            }
            return result;
        }
        public void Edit(T item_)
        {
            this._dbSet.Attach(item_);
        }       
    }
    /// <summary>
    /// Repository for filtering by merchantlist entity inherited from IMerchant interface, 
    /// with sector_id , which not needed to be chained (no data and merchant simultaneous filter)
    /// >>!!! used for entities with different properties
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SectorFilterRepo<T> : EditRepo<T>, IBySector
        where T : class, ISector
    {
        public SectorFilterRepo(DbContext context_) : base (context_)
        {

        }
        public void DeleteBySector(int id_)
        {                   
            //this._dbSet.RemoveRange(from s in this._dbSet where s.SECTOR_ID == id_ select s );
            foreach (var item_ in this._dbSet)
            {
                if (item_.SECTOR_ID != null)
                {
                    if (item_.SECTOR_ID.Value == id_)
                    {
                        this._dbSet.Remove(item_);
                    }
                }
            }            
        }      
    }
    public class UserFilterRepo<T> : EditRepo<T>, IByUser
        where T : class , IUser
    {
        public UserFilterRepo(DbContext context_) : base(context_)
        {

        }
        public void DeleteByUserID(int id_)
        {
            foreach (var item_ in this._dbSet.Where(s => s.USER_ID == id_))
            {
                this._dbSet.Remove(item_);
            }           
        }
    }
    /// <summary>
    /// Repository for entity with merchant, filtering by refmerchants and merchant count
    /// compares K merchants in T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MerchantFilterRepo<T, K> : EditRepo<T>, IByMerchant
        where T : class, IMerchant
        where K : class, IMerchant
    {
        public MerchantFilterRepo(DbContext context_) : base (context_)
        {

        }
        public IQueryable<T> GetByMerchantFilter<T>() where T : class, IMerchant
        {
            IQueryable<T> result = null;
            IDbSet<K> merchant_entity = this._context.Set<K>();
            var merchs = from s in merchant_entity select s;
            DbSet<T> filtering_entity = this._context.Set<T>();
            result = from s in filtering_entity join k in merchs on s.MERCHANT equals k.MERCHANT select s;
            return result;
        }      
        public int GetMerchantFilterAmount<T>()  where T : class, IMerchant
        {
            int result_ = 0;
            IDbSet<T> merchant_entity = this._context.Set<T>();
            result_ = (from s in merchant_entity select s).Count();
            return result_;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DateFilterRepo<T> : EditRepo<T> 
        where T: class, IDate
    {
        public DateFilterRepo(DbContext context_) : base (context_)
        {

        }
        public IQueryable<T> GetByDate(DateTime st, DateTime fn) 
        {
            IQueryable<T> result = _context.Set<T>().Where(s => s.DATE >= st && s.DATE <= fn);
            return result;
        }
    }
    /// <summary>
    /// Repository for entities with chaining, entities with date and merchants fields
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ChainingRepo<T> : EditRepo<T> , IChain where T : class, IChainable
    {
        public ChainingRepo(DbContext context_) : base (context_)
        {

        }

        public ChainingRepo<T> FilterByExpression(Expression<Func<T, bool>> filter_)
        {
            if (this._dbSet != null)
            {
                if (this._result == null)
                {
                    this._result = this._dbSet;
                }
                try
                {
                    this._result = this._result.Where(filter_);
                }
                catch(Exception e)
                {

                }
            }
            return this;
        }
        public new ChainingRepo<IChainable> GetAll()
        {
            ChainingRepo<IChainable> _result = new ChainingRepo<IChainable>(this._context);
            if (this._result == null)
            {
                this._result = from s in this._dbSet select s;
            }
            _result._result = from s in this._result select s;
            return _result;
        }
        public ChainingRepo<IChainable> FilterByDate(DateTime st, DateTime fn)
        {
            ChainingRepo<IChainable> _result = new ChainingRepo<IChainable> (this._context);
            if(this._result == null)
            {
                this._result = from s in this._dbSet where s.DATE >= st && s.DATE <= fn select s;
            }
            _result._result = from s in this._result where s.DATE >= st && s.DATE <= fn select s;
            return _result;
        }
        public ChainingRepo<IChainable> FilterByMerchants<K>() where K: class,IMerchant
        {
            ChainingRepo<IChainable> _result = new ChainingRepo<IChainable>(this._context);
            DbSet<K> filter_mercahnt = this._context.Set<K>();
            if (this._result == null)
            {
                this._result = from s in this._dbSet where s.MERCHANT == 929 select s;
            }
            _result._result = from s in this._result join t in filter_mercahnt on s.MERCHANT equals t.MERCHANT select s;
            return _result;
        }
    }
    #endregion

    /// <summary>
    /// Unit of Work implements repositories and contains methods to getall, add entity (IEntity) to repo, save and dispose
    /// can be extended with generic get, add for TGR interfaces
    /// </summary>
    #region UOF                

    public class Edit_UOF
    {
        SQL_entity ent;
        EditRepo<T_ACQ_D_SQL> ACQ_D;       
        UserFilterRepo<REFMERCHANTS_SQL> REFMERCHANTS;
        SectorFilterRepo<KEY_CLIENTS_SQL> KEYCLIENTS;

        public Edit_UOF()
        {
            ent = new SQL_entity();
            ACQ_D = new EditRepo<T_ACQ_D_SQL> (ent);            
            REFMERCHANTS = new UserFilterRepo<REFMERCHANTS_SQL>(ent);
            KEYCLIENTS = new SectorFilterRepo<KEY_CLIENTS_SQL>(ent);
        }

        ~Edit_UOF()
        {
            this.Dispose();
        }

        //>>!!! to read UOF
        public IQueryable<Repo.IMerchant> GetAllACQ_D()
        {
            IQueryable<Repo.IMerchant> result_ = null;
            result_ = ACQ_D.GetAll();
            return result_;
        }
        public IQueryable<KEY_CLIENTS_SQL> GetAllKK()
        {
            IQueryable<KEY_CLIENTS_SQL> result_ = null;
            result_ = KEYCLIENTS.GetAll();
            return result_;
        }

        public IQueryable<IMerchant> GetKeyClientsMerchants()
        {
            IQueryable<IMerchant> result_ = null;           
            result_ = KEYCLIENTS.GetAll();
            return result_;
        }
        public IQueryable<IMerchant> FilterByMerchant<T>() where T : class, IMerchant
        {
            IQueryable<IMerchant> result_ = null;
            MerchantFilterRepo<T, REFMERCHANTS_SQL> MerchantFilter = new MerchantFilterRepo<T, REFMERCHANTS_SQL>(ent);
            result_ = MerchantFilter.GetByMerchantFilter<T>();
            return result_;
        }
        public int GetMerchantFilterAmount()
        {
            int result_ = 0;
            MerchantFilterRepo<KEY_CLIENTS_SQL, REFMERCHANTS_SQL> MerchantFilter = new MerchantFilterRepo<KEY_CLIENTS_SQL,REFMERCHANTS_SQL>(ent);
            result_ = MerchantFilter.GetMerchantFilterAmount < KEY_CLIENTS_SQL>();
            return result_;
        }

        public void AddACQ_D(IMerchant entity_)
        {
            ACQ_D.AddEntity((T_ACQ_D_SQL)entity_);
        }

        public void AddKK(IMerchant entity_)
        {
            KEYCLIENTS.AddEntity((KEY_CLIENTS_SQL)entity_);
        }
        public void DeleteBySectorID(int id_)
        {
            KEYCLIENTS.DeleteBySector(id_);
        }

        public void AddREF(IMerchant entity_)
        {
            REFMERCHANTS.AddEntity((REFMERCHANTS_SQL)entity_);
        }
        public void DeleteByUserID(int id_)
        {
            REFMERCHANTS.DeleteByUserID(id_);
        }

        private bool dispose = false;

        public void SaveAll()
        {           
            try
            {
                ent.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var ev in e.EntityValidationErrors)
                {

                }
            }
        }

        public void Dispose(bool disposing_)
        {
            if(!this.dispose)
            {
                if(disposing_)
                {
                    KEYCLIENTS.Dispose();
                    ACQ_D.Dispose();
                }
            }
            this.dispose = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
       
    }
   
    /// <summary>
    /// Generic Unit Of work. Receives DbContext uses one of Generic Repo for operations
    /// Contains Generic and Item specific methods
    /// </summary>
    public class UnitOfWorkGeneric
    {
        DbContext ent;

        public UnitOfWorkGeneric(DbContext input_) 
        {
            ent = input_;           
        }
        ~UnitOfWorkGeneric()
        {
            this.Dispose();
        }
      
        public void Add<T>(T item_) where T: class, IEntity
        {
            EditRepo<T> et = new EditRepo<T>(ent);
            et.AddEntity(item_);
            et.Save();            
        }
        public IEnumerable<T> GetByDate<T>(DateTime dateStart,DateTime dateFinal) where T: class, IDate
        {
            IEnumerable<T> _result = null;
            DateFilterRepo<T> rr = new DateFilterRepo<T>(ent);
                _result = rr.GetByDate(dateStart, dateFinal).ToList();          
            return _result;
        }
        //bind K of REFMERCHANTS with method
        public IQueryable<T> GetByMerchants<T>()where T: class, IMerchant          
        {
            IQueryable<T> _result = null;
            MerchantFilterRepo<T,REFMERCHANTS_SQL> merchantRepo = new MerchantFilterRepo<T, REFMERCHANTS_SQL>(ent);
            _result = merchantRepo.GetByMerchantFilter<T>();
            return _result;
        }
        public int MerchantListCount()
        {
            int _result = 0;
            MerchantFilterRepo<REFMERCHANTS_SQL,REFMERCHANTS_SQL> merchantRepo = new MerchantFilterRepo<REFMERCHANTS_SQL,REFMERCHANTS_SQL>(ent);
            _result = merchantRepo.GetMerchantFilterAmount<REFMERCHANTS_SQL>();
            return _result;
        }
        public void DeleteBySector(int id_) 
        {
            SectorFilterRepo<KEY_CLIENTS_SQL> sector = new SectorFilterRepo<KEY_CLIENTS_SQL>(ent);
            sector.DeleteBySector(id_);
        }
        public void DeleteByUserID(int id_)
        {
            UserFilterRepo<REFMERCHANTS_SQL> repo = new UserFilterRepo<REFMERCHANTS_SQL>(ent);
            repo.DeleteByUserID(id_);
        }

        public void RefreshValues()
        {
            //var a = ent.Database.Connection.CreateCommand();
            CallDisconnectedProcedure(@"VALUES_INSERT");
        }
        public void CallDisconnectedProcedure(string input_)
        {
            //var c = ent.Database.SqlQuery<FakePOCO>(@"VALUES_INSERT").ToArray();
            var a = from s in ent.Set<KEY_CLIENTS_SQL>() select s;
            var c = ent.Database.SqlQuery<FakePOCO_SQL>(input_).ToArray();
        }

        private bool dispose = false;

        public void SaveAll()
        {           
            try
            {
                ent.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var ev in e.EntityValidationErrors)
                {

                }
            }
        }
        public void Dispose(bool disposing_)
        {
            if(!this.dispose)
            {
                if(disposing_)
                {
                    ent.Dispose();
                }
            }
            this.dispose = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }       
    }
    #endregion

    /// <summary>
    /// Class and container to bind Edit_UOF methods to types and type names
    /// </summary>
    #region TypeBind

    public class TypeMethodName
    {
        public TypeMethodName(Type entityType_, IMerchantMethod method_, string typeName_)
        {
            entityType = entityType_;
            method = method_;
            typeName = typeName_;
        }
        public Type entityType { get; set; }
        public IMerchantMethod method { get; set; }
        public string typeName { get; set; }
    }
    public class TypeMethodNameContainer
    {
        Edit_UOF uof;

        public List<TypeMethodName> AddMethods;

        public IMerchantMethod am;

        public TypeMethodNameContainer()
        {

            uof = new Edit_UOF();

            this.AddMethods = new List<TypeMethodName>();
            this.AddMethods.Add(new TypeMethodName(typeof(KEY_CLIENTS_SQL), uof.AddKK, @"KEY_CLIENTS_SQL"));
            this.AddMethods.Add(new TypeMethodName(typeof(REFMERCHANTS_SQL), uof.AddREF, @"REFMERCHANTS_SQL"));
            this.AddMethods.Add(new TypeMethodName(typeof(T_ACQ_D_SQL), uof.AddACQ_D, @"T_ACQ_D_SQL"));

        }
        public void MethodGet(Type type_)
        {
            this.am = (from s in this.AddMethods where s.entityType == type_ select s.method).FirstOrDefault();
        }
        public void MethodGet(string name_)
        {
            this.am = (from s in this.AddMethods where s.typeName == name_ select s.method).FirstOrDefault();
        }
        public void MethodInit(IMerchant input_)
        {
            am.Invoke(input_);
        }
        public void Save()
        {
            uof.SaveAll();
        }
        public void PropertyInfoValueSet(PropertyInfo[] piArr_, PropertyInfo pi_, string name_, object obj_, object val_)
        {

            if ((from s in piArr_ where s.Name == name_ select s).Any())
            {
                pi_ = (from s in piArr_ where s.Name == name_ select s).FirstOrDefault();
            }

            pi_.SetValue(obj_, val_);
        }
    }

    #endregion

    public class EntityInitialization
    {
        public IEntity entityItem { get; set; }

        public void KEY_CLIENTS_init(IEntity ent_)
        {
            entityItem = ent_;
        }
        public void KEY_CLIENTS_add(UnitOfWorkGeneric uof_, long senumber, int sectorid, string providername = null)
        {
            entityItem = new KEY_CLIENTS_SQL() {  MERCHANT = senumber, SECTOR_ID = sectorid, PROVIDER_NAME = providername };
            List<IEntity> entList = new List<IEntity>();

            entList.Add(entityItem);

            uof_.Add<KEY_CLIENTS_SQL>((KEY_CLIENTS_SQL)entityItem);
        }
        public void ExcelExport()
        {

        }
    }

}

