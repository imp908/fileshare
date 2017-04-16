using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;


using System.Linq.Expressions;

using SB.Entities;
using SB.DAL;
using SB_;

using System.Threading;

//DAL folder - > dbcontexts for SQL database and table copy from to; simmilar for ORACLE 
//1)code first, 2) generate DB from connection string from ORCL to SQL change
//Entities folder -> generated classes from sql and oracle base; DB first

namespace SB
{
    class Check1
    {

        public Check1()
        {

            //BtOwfl();
            //ParametersCheck();

            //copy table from oracle to sql with contexts
            //tableCopyCheck();

            //DB_play db = new DB_play();
            //db.Lmbda();
            //db.RepoCheck();

            //check usage of generic method with funct delegate BubbleSort
            //genMethCheck();

            //check usage of event publisher and subscriber
            //EventsCheck.check();

            //delegates invokation - simple, action,array
            //delegates assigning
            //DelegateCheck.Check();


            //LinkedListCheck();
            //LinkedListSwitchCheck();

            //PropertyBind.RepoBind();

            //Parse pr = new Parse();

            //DB_play db = new DB_play();
            //db.EF_issue_check();
            //db.EF_del();
            //db.ORCL_to_SQL_copy_repo();

            //LINQ_play lp = new LINQ_play();




            //check for generic bubblesort with delegaate
            //BubbleSort.BubblesortCheck();            

            //linked lists check
            //LinkedLists.Check();

            //DelegateAsyncCheck.delAsyncCheck_0();
            //DelegateAsyncCheck.delAsyncCheck_1();
            //DelegatesCheck.Check();

            //Async check
            //AsyncFoundation.Check();

            //Parallel check
            //ParallelTasks.Check();

            //Reflections
            //Reflections_.Check();

            PoliParseCheck pp = new PoliParseCheck();
            pp.Check();
        }
  
    }


    #region DB_Play
    //Entity framework for database copy   
    public class DB_play
    {
        public BI_test db = new BI_test();
        public DB_play()
        {

        }

        //full copy table from oracle to sql using contexts
        public void tableCopyCheck()
        {
            DB_play bi = new DB_play();
            //copy with code first migration
            //bi.ORCL_to_SQL_copy();

            //copy with 2 db first contexts
            bi.ORCL_to_SQL_copy_FULL();
        }

        public void genMethCheck()
        {            
            Item item = new Item();
            item.ItemsInit();
            item.itemsPrint();
            BubbleSort.Sort<Item>(item.ListOfItems, item.itemsCompare);
            System.Diagnostics.Debug.WriteLine("After sort");
            item.itemsPrint();
        }

        //copy from existing sql database to new created sql database
        public void SQL_to_SQL_copy()
        {
            //existing database
            BI_test db = new BI_test();
            IQueryable<table1> table = from s in db.table1 select s;

            //new database
            BI_copy db_copy = new BI_copy();

            foreach (table1 table1_ in table)
            {
                db_copy.table1_copy.Add(table1_);
                db_copy.SaveChanges();
            }

        }

        //copy from existing ORACLE database to new created sql database
        public void ORCL_to_SQL_copy()
        {
            //existing database
            ORCL_model db = new ORCL_model();
            IQueryable< SB.Entities.JOBS> table = from s in db.JOBS select s;
            
            //new database
            ORCL_SQL_model db_copy = new ORCL_SQL_model();


            foreach (var table1_ in db.JOBS)
            {
                //instantiating new entity class to prevent multiple context reference error
                db_copy.JOBS.Add(new JOBS { JOB_ID = table1_.JOB_ID, JOB_TITLE = table1_.JOB_TITLE });
                db_copy.SaveChanges();
            }

        }

        //copy from ORacle db to sql with new contexts
        public void ORCL_to_SQL_copy_FULL()
        {
            ORCL_HR ORCL_DB = new ORCL_HR();
            SQL_HR_COPY SQL_DB = new SQL_HR_COPY();
            
            IQueryable<SB.Entities.COUNTRIES> regions = from s in ORCL_DB.COUNTRIES select s;

            foreach (SB.Entities.COUNTRIES rg in regions)
            {
                SQL_DB.COUNTRIES.Add(new SB.Entities.COUNTRIES { COUNTRY_ID = rg.COUNTRY_ID, COUNTRY_NAME = rg.COUNTRY_NAME, REGION_ID = rg.REGION_ID });
                SQL_DB.SaveChanges();
            }

        }

        public void ORCL_to_SQL_copy_repo()
        {
            ORCL_HR ORCL_DB = new ORCL_HR();
            SQL_HR_COPY SQL_DB = new SQL_HR_COPY();         

            Repository<LOCATIONS> orcl_jobs = new Repository<LOCATIONS>(ORCL_DB);
            Repository<LOCATIONS> sql_jobs = new Repository<LOCATIONS>(SQL_DB);
           
            IEnumerable<LOCATIONS> jobs_ = orcl_jobs.GetAll().AsNoTracking();
            
            //works for entity with no navigation properties
            foreach (LOCATIONS jb in jobs_)
            {
                
                //sql_jobs.Add(new JOBS { JOB_ID=jb.JOB_ID, JOB_TITLE=jb.JOB_TITLE,MIN_SALARY=jb.MIN_SALARY,MAX_SALARY=jb.MAX_SALARY} );
                sql_jobs.Add(jb);
                sql_jobs.SaveChnages();
            }                
        }

        /*
        public static Func<BI_test, DateTime, DateTime, IQueryable<table1>> del = (b, ds, df) =>
        from s in b.table1
        where DbFunctions.CreateDateTime(s.date1.Value.Year, s.date1.Value.Month, 01, 00, 00, 00) >= DbFunctions.CreateDateTime(2016, 01, 01, 00, 00, 00)
        && DbFunctions.CreateDateTime(s.date1.Value.Year, s.date1.Value.Month, 01, 00, 00, 00) <= DbFunctions.CreateDateTime(2016, 02, 01, 00, 00, 00)
        select s;

        public static Func<BI_test> delFN = (db) =>
        from s in del.Invoke(db, new DateTime(2016, 01, 01, 00, 00, 00), new DateTime(2016, 02, 01, 00, 00, 00))
        group s by new { DATE = DbFunctions.CreateDateTime(s.date1.Value.Year, s.date1.Value.Month,01,00,00,00), range = s.range1 } into g
        select new table1DTO { DATE = g.Key.DATE, RANGE = g.Key.range, AMT = g.Sum(s => s.value1) };
        */

        public delegate IQueryable<table1> del_table1();

        public IQueryable<table1> del_date()
        {
            return from s in db.table1
                   where DbFunctions.CreateDateTime(s.date1.Value.Year, s.date1.Value.Month, 01, 00, 00, 00) >= DbFunctions.CreateDateTime(2016, 01, 01, 00, 00, 00)
                   && DbFunctions.CreateDateTime(s.date1.Value.Year, s.date1.Value.Month, 01, 00, 00, 00) <= DbFunctions.CreateDateTime(2016, 02, 01, 00, 00, 00)
                   select s;
        }

        public IQueryable<table1DTO> del_table1_group()
        {
            del_table1 dt1 = del_date;

            return from s in dt1()
                   group s by new { DATE = DbFunctions.CreateDateTime(s.date1.Value.Year, s.date1.Value.Month, 01, 00, 00, 00), range = s.range1 } into g
                   select new table1DTO { DATE = g.Key.DATE, RANGE = g.Key.range, AMT = g.Sum(s => s.value1) };
        }

        ORCL_HR orcl_db = new ORCL_HR();

        //
        public void Lmbda()
        {

            DateTime dt = DateTime.Now;
            BI_test db = new BI_test();

            //var a = GetByRegionID(orcl_db.COUNTRIES, 1);

            //var arr = del.Invoke(db, new DateTime(2016, 01, 01, 00, 00, 00), new DateTime(2016, 02, 01, 00, 00, 00));


            IQueryable<table1DTO> arr3 =
from s in db.table1
where DbFunctions.CreateDateTime(s.date1.Value.Year, s.date1.Value.Month, 01, 00, 00, 00) >= DbFunctions.CreateDateTime(2016, 01, 01, 00, 00, 00)
&& DbFunctions.CreateDateTime(s.date1.Value.Year, s.date1.Value.Month, 01, 00, 00, 00) <= DbFunctions.CreateDateTime(2016, 02, 01, 00, 00, 00)
group s by new { DATE = DbFunctions.CreateDateTime(s.date1.Value.Year, s.date1.Value.Month, 01, 00, 00, 00), range = s.range1 } into g
select new table1DTO { DATE = g.Key.DATE, RANGE = g.Key.range, AMT = g.Sum(s => s.value1) };

            var arr4 = del_table1_group();

            int cnt = arr3.Count();
            table1Condition.addProperties(arr3.GetType());
            List<string> str = table1Condition.propertiesList;


        }

        public Expression<Func<IEntity, bool>> GetByID<Ientity>(decimal id)
        {
            return s => s.REGION_ID == id;
        }
        public IQueryable<IEntity> GetByRegionID(IQueryable<IEntity> item, decimal id_)
        {
            return from s in item where s.REGION_ID == id_ select s;
        }
        public IEnumerable<IEntity> GetData<IEntity>() where IEntity : class
        {
            return orcl_db.Set<IEntity>();
        }

        //checking of generic repository from .DAL.Repository.cs
        public void RepoCheck()
        {
            ORCL_HR orcl_db = new ORCL_HR();
            SB.Entities.COUNTRIES ctr = new SB.Entities.COUNTRIES();

            Repository<SB.Entities.COUNTRIES> repo = new Repository<SB.Entities.COUNTRIES>(orcl_db);

            IQueryable<SB.Entities.COUNTRIES> items = repo.GetAll();
            int item_count = items.Count();

            RepositoryCountries repoCountries = new RepositoryCountries(orcl_db);
            items = repoCountries.countriesByRegion(1);
            item_count = items.Count();
            repoCountries.display(items);
        }

        public void EF_issue_check()
        {
            EF_issuer ent = new EF_issuer();

            IEnumerable<MyEntity> a = from s in ent.ENTITY select s;

            //ent.ENTITY.Add(new MyEntity() { Name = "A", SerName = "B", BirthDate = DateTime.Now, Id=3 });
            //ent.ENTITY.Add(new MyEntity() { Name = "C", SerName = "D", BirthDate = DateTime.Now, newID2 = @"929004" });
            //ent.ENTITY.Add(new MyEntity() { Name = "E", SerName = "F", BirthDate = DateTime.Now, newID2 = @"929005" });
            try
            {
                //ent.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public void EF_del()
        {
            EF_issuer ent = new EF_issuer();           
           
                try
                {
                    ent.ENTITY.RemoveRange(ent.ENTITY);
                    ent.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                            
        }
    }

    public class IEntity
    {
        public decimal REGION_ID { get; set; }
    }
    #endregion

    #region PropertyBinder
    public class PropertyBind
    {
        internal SQL_HR_COPY db ;
        internal Repository<REGIONS> regRepo ;

        public PropertyBind()
        {
            db = new SQL_HR_COPY();
            regRepo = new Repository<REGIONS>(db);
            
            
        }
        public static void RepoBind()
        {
            var a = Assembly.GetEntryAssembly().GetTypes().First(s=>s.Name=="REGIONS");
            var b = Activator.CreateInstance(a);
            PropertyInfo[] pi = b.GetType().GetProperties();
            
        }
    }       
    public class Masks
    {
        internal string propertyName { get; set; }
        internal string mask { get; set; }
        internal string value { get; set; }
        public int colNum { get; set; }   

    }
    public static class RegionMask 
    {
        public static List<Masks> maskList = new List<Masks>();
        public static void RegionMaskInit()
        {
            maskList = new List<Masks>();
            maskList.Add(new Masks { propertyName = @"REGION_ID", mask = @"region[_]id" });
            maskList.Add(new Masks { propertyName = @"REGION_NAME", mask = @"region[_]name" });
        }
     
        public static string GetValueByName(string name_)
        {
            string result = null;
            if (maskList.Where(s => s.propertyName == name_).Any())
            {
                result=maskList.Where(s => s.propertyName == name_).First().value;
            }
            return result;
        }
    }

    public class Parse
    {
        public Parse()
        {
            RegionMask.RegionMaskInit();
            RegionMask.maskList.Where(s => s.propertyName == "REGION_ID").First().colNum = 0;
            RegionMask.maskList.Where(s => s.propertyName == "REGION_ID").First().value = "0";
            RegionMask.maskList.Where(s => s.propertyName == "REGION_NAME").First().colNum = 1;
            RegionMask.maskList.Where(s => s.propertyName == "REGION_NAME").First().value = "REG_0";

            SQL_HR_COPY sql = new SQL_HR_COPY();
            decimal id = 5;
            REGIONS rg = new REGIONS { REGION_ID = id, REGION_NAME = "A" };
            //sql.REGIONS.Add(new REGIONS { REGION_ID = decimal.Parse(rm.GetValueByName(@"REGION_ID")), REGION_NAME = rm.GetValueByName(@"REGION_NAME") });
            sql.REGIONS.Add(rg);
            try
            {
                sql.SaveChanges();
            }
            catch(Exception e )
            {

                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            
        }
    }
    #endregion

    #region TGR
    public class TrullyGenericRepo<T>
    {

    }
    #endregion

}


namespace SB.DAL
{

    #region genericRepository
    //generic interface with collection and ID method
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetByID(int ID);
    }
    //interface for concrete Entity
    public interface IRepoCountries : IRepository<SB.Entities.COUNTRIES>
    {
        IQueryable<SB.Entities.COUNTRIES> countriesByRegion(int ID);
    }

    //generic class implements interface
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        //initializer for DbContext with dbset type T
        public Repository(DbContext dbContext_)
        {
            dbContext = dbContext_;
            dbSet = dbContext.Set<T>();
        }

        public DbContext dbContext { get; set; }
        public DbSet<T> dbSet { get; set; }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }
        public T GetByID(int ID)
        {
            return dbSet.Find(ID);
        }
        public void Add(T item_)
        {
            this.dbSet.Add(item_);
        }
        public void SaveChnages()
        {
            this.dbContext.SaveChanges();
        }
        public void Dispose()
        {
            this.dbContext.Dispose();
            this.dbSet = null;
        }
    }
    //Repository for specific entity implementing according Interface
    public class RepositoryCountries : Repository<SB.Entities.COUNTRIES>, IRepoCountries
    {
        public RepositoryCountries(DbContext dbContext_) : base(dbContext_)
        {

        }

        public IQueryable<SB.Entities.COUNTRIES> countriesByRegion(int ID_)
        {
            return GetAll().Where(s => s.REGION_ID == ID_);
        }

        //display for country constraint
        public void display<Tdoc>(IQueryable<Tdoc> items_) where Tdoc : ICountry
        {
            foreach (Tdoc item in items_)
            {
                System.Diagnostics.Trace.WriteLine(
                        item.COUNTRY_NAME
                    );
            }
        }
    }
    #endregion

}

