using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data.Entity;

using DAL.DAL;
using Repo_;
using Model.SQLmodel;

using Moq;

namespace Tests_
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    [TestClass]
    public class DB_TESTS
    {

        [TestMethod]
        public void SQLDB_Creation_test()
        {
            bool result = false;
            string connection_ = @"SQLDB_J";
            try
            {
                SQLDB_CHANGE ent = new SQLDB_CHANGE(connection_);
                result = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SQLHR_Creation_test()
        {
            bool result = false;
            string connection_ = @"SQLHR_J";
            try
            {
                SQLHR_CHANGE ent = new SQLHR_CHANGE(connection_);
                result = true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SQLDB_KK_Initialized_test()
        {
            SQLDB_CHANGE context = new SQLDB_CHANGE(@"SQLDB_J");
            int cnt = 0;
            try
            {
                cnt = (from s in context.KEY_CLIENTS select s).Count();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            Assert.AreNotEqual(0, cnt);
        }

        [TestMethod]
        public void SQLDB_RM_Initialized_test()
        {
            SQLDB_CHANGE context = new SQLDB_CHANGE(@"SQLDB_J");

            int cnt = 0;
            try
            {
                cnt = (from s in context.REFMERCHANTS_SQL select s).Count();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            Assert.AreNotEqual(0, cnt);
        }

    }

    [TestClass]
    public class Repo_FUNCTIONAL_TEST
    {

        [TestMethod]
        public void ReadRepo_TEST()
        {
            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            ReadRepo<REFMERCHANTS_SQL> ref_repo = new ReadRepo<REFMERCHANTS_SQL>(ent);
            int a = ref_repo.GetAll().Count();
            Assert.AreNotEqual(0,a);
        }
        [TestMethod]
        public void EditRepo_ADD_GET_TEST()
        {
            int cntSt = 0;
            int cntFN = 0;

            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var mock = new Mock<EditRepo<REFMERCHANTS_SQL>>();
            //mock.Setup(s => s.GetAll());
            //ReadRepo<REFMERCHANTS_SQL> ref_repo = new ReadRepo<REFMERCHANTS_SQL>(ent);
            REFMERCHANTS_SQL rm = new REFMERCHANTS_SQL() { MERCHANT = 9000000001, USER_ID = 1 };
            mock.Object.BindContext(ent);
            if (mock.Object.GetAll().Any())
            {
                cntSt = mock.Object.GetAll().Count();
            }
            mock.Object.AddEntity(rm);
            mock.Object.Save();
            if (mock.Object.GetAll().Any())
            {
                cntFN = mock.Object.GetAll().Count();
            }
            mock.Object.Delete(rm);
            mock.Object.Save();
            Assert.AreNotEqual(cntSt, cntFN);
        }
        [TestMethod]
        public void ReadRepo_FILTER_TEST()
        {
            int a = 0;

            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var mock = new Mock<EditRepo<REFMERCHANTS_SQL>>();   
            mock.Object.BindContext(ent);
            if (mock.Object.GetAll().Any())
            {
                REFMERCHANTS_SQL rm = mock.Object.GetAll().FirstOrDefault();              
                a = mock.Object.GetByFilter<REFMERCHANTS_SQL>(s => s.ID == rm.ID).Count();
            }
            Assert.AreNotEqual(0, a);
        }
        [TestMethod]
        public void SectorRepo_TEST()
        {
            int cntSt = 0;
            int cntFn = 0;

            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var mock = new Mock<SectorFilterRepo<KEY_CLIENTS_SQL>>();           
            mock.Object.BindContext(ent);
            List<KEY_CLIENTS_SQL> kk = new List<KEY_CLIENTS_SQL>() {

                new KEY_CLIENTS_SQL() { MERCHANT = 9290000020, SECTOR_ID = 101 },
                new KEY_CLIENTS_SQL() { MERCHANT = 9290000021, SECTOR_ID = 101 },
                new KEY_CLIENTS_SQL() { MERCHANT = 9290000022, SECTOR_ID = 101 },         

            };
            if (mock.Object.GetAll().Any())
            {
                cntSt = mock.Object.GetAll().Count();
            }
            mock.Object.AddEntities(kk);
            mock.Object.Save();
            mock.Object.DeleteBySector(101);           
            mock.Object.Save();
            if (mock.Object.GetAll().Any())
            {
                cntFn = mock.Object.GetAll().Count();
            }

            Assert.AreEqual(cntSt,cntFn);
        }
        [TestMethod]
        public void Merchants_TEST()
        {
            int cntSt = 0;
            int cntFn = 0;

            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var mock = new Mock<MerchantFilterRepo<REFMERCHANTS_SQL,KEY_CLIENTS_SQL>>();
            mock.Object.BindContext(ent);
            List<KEY_CLIENTS_SQL> kk = new List<KEY_CLIENTS_SQL>() {

                new KEY_CLIENTS_SQL() { MERCHANT = 9290000020, SECTOR_ID = 101 },
                new KEY_CLIENTS_SQL() { MERCHANT = 9290000021, SECTOR_ID = 101 },
                new KEY_CLIENTS_SQL() { MERCHANT = 9290000022, SECTOR_ID = 101 },
                new KEY_CLIENTS_SQL() { MERCHANT = 9290000023, SECTOR_ID = 101 },
                new KEY_CLIENTS_SQL() { MERCHANT = 9290000024, SECTOR_ID = 101 }

            };
            List<REFMERCHANTS_SQL> rm = new List<REFMERCHANTS_SQL>() {

                new REFMERCHANTS_SQL() { MERCHANT = 9290000020, USER_ID = 101 },
                new REFMERCHANTS_SQL() { MERCHANT = 9290000021, USER_ID = 101 },
                new REFMERCHANTS_SQL() { MERCHANT = 9290000022, USER_ID = 101 },
                new REFMERCHANTS_SQL() { MERCHANT = 9290000023, USER_ID = 101 },
                new REFMERCHANTS_SQL() { MERCHANT = 9290000024, USER_ID = 101 }

            };
            if (mock.Object.GetAll().Any())
            {
                cntSt = mock.Object.GetAll().Count();
            }

            mock.Object.AddEntities(rm);

            if (mock.Object.GetAll().Any())
            {
                cntFn = mock.Object.GetAll().Count();
            }

            Assert.AreEqual(cntSt, cntFn);
        }
    }

}
