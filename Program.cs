using UOW;
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

namespace UOW.Tests
{
    [TestClass()]
    public class UOW_tests
    {
        [TestMethod()]
        public void UOW_merchantsTest()
        {
            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var readrepo = new Mock<IReadRepo<KEY_CLIENTS_SQL>>();
            var mock = new Mock<IUOW_sectors<KEY_CLIENTS_SQL>>();
            mock.Object.BindContext(ent);
            mock.Verify(s => s.BindContext(It.IsAny<SQLDB_CHANGE>()), Times.Once());
                                
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void UOW_BindRepo_TEST()
        {
            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var readrepo = new Mock<IReadRepo<KEY_CLIENTS_SQL>>();
            var mock = new Mock<IUOW_sectors<KEY_CLIENTS_SQL>>();           
            mock.Object.BindRepo(readrepo.Object);
            mock.Verify(s => s.BindRepo(It.IsAny<IReadRepo<KEY_CLIENTS_SQL>>()), Times.Once());

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetBySectorTest()
        {
            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var readrepo = new Mock<IReadRepo<KEY_CLIENTS_SQL>>();
            var mock = new Mock<IUOW_sectors<KEY_CLIENTS_SQL>>();
            mock.Object.BindContext(ent);
            mock.Object.BindRepo(readrepo.Object);
            mock.Object.GetBySector(1);
            mock.Verify(s => s.GetBySector(It.IsAny<int>()), Times.Once());

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void GetByUserTest()
        {
            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var readrepo = new Mock<IReadRepo<KEY_CLIENTS_SQL>>();
            var mock = new Mock<IUOW_sectors<KEY_CLIENTS_SQL>>();
            mock.Object.BindContext(ent);
            mock.Object.BindRepo(readrepo.Object);
            mock.Object.GetByUser(1);
            mock.Verify(s => s.GetByUser(It.IsAny<int>()), Times.Once());

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void DeleteByMerchantListTest()
        {
            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var readrepo = new Mock<IReadRepo<KEY_CLIENTS_SQL>>();
            var mock = new Mock<IUOW_sectors<KEY_CLIENTS_SQL>>();
            mock.Object.BindContext(ent);
            mock.Object.BindRepo(readrepo.Object);

            IQueryable<KEY_CLIENTS_SQL> kk = new List<KEY_CLIENTS_SQL>() {
                new KEY_CLIENTS_SQL() {MERCHANT = 900000010, SECTOR_ID = 1 },
                new KEY_CLIENTS_SQL() {MERCHANT = 900000011, SECTOR_ID = 1 },
                new KEY_CLIENTS_SQL() {MERCHANT = 900000012, SECTOR_ID = 1 }
            }.AsQueryable();

            //mock.Object.AddMerchantList(kk);
            //mock.Verify(s => s.AddMerchantList(It.IsAny<IQueryable<KEY_CLIENTS_SQL>>()), Times.Once());

            mock.Object.DeleteByMerchantList(kk);
            mock.Verify(s => s.DeleteByMerchantList(It.IsAny<IQueryable<KEY_CLIENTS_SQL>>()), Times.Once());

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void AddMerchantListTest()
        {
            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var readrepo = new Mock<IReadRepo<KEY_CLIENTS_SQL>>();
            var mock = new Mock<IUOW_sectors<KEY_CLIENTS_SQL>>();
            mock.Object.BindContext(ent);
            mock.Object.BindRepo(readrepo.Object);

            IQueryable<KEY_CLIENTS_SQL> kk = new List<KEY_CLIENTS_SQL>() {
                new KEY_CLIENTS_SQL() {MERCHANT = 900000010, SECTOR_ID = 1 },
                new KEY_CLIENTS_SQL() {MERCHANT = 900000011, SECTOR_ID = 1 },
                new KEY_CLIENTS_SQL() {MERCHANT = 900000012, SECTOR_ID = 1 }
            }.AsQueryable();

            //mock.Object.AddMerchantList(kk);
            //mock.Verify(s => s.AddMerchantList(It.IsAny<IQueryable<KEY_CLIENTS_SQL>>()), Times.Once());

            mock.Object.AddMerchantList(kk);
            mock.Verify(s => s.AddMerchantList(It.IsAny<IQueryable<KEY_CLIENTS_SQL>>()), Times.Once());

            Assert.IsTrue(true);
        }
    }
}

namespace DAL.Tests
{
    [TestClass]
    public class DB_INTEGRATION_TESTS
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
}

namespace Repo_.Tests
{
    [TestClass]
    public class Repo_FUNCTIONAL_TEST
    {

        [TestMethod]
        public void ReadRepo_GetAll_TEST()
        {
            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            ReadRepo<REFMERCHANTS_SQL> ref_repo = new ReadRepo<REFMERCHANTS_SQL>(ent);
            int a = ref_repo.GetAll().Count();
            Assert.AreNotEqual(0,a);
        }
        [TestMethod]
        public void EditRepo_ADD_TEST()
        {
        
            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var mock = new Mock<IEditRepo<REFMERCHANTS_SQL>>();
            //mock.Setup(s => s.GetAll());
            //ReadRepo<REFMERCHANTS_SQL> ref_repo = new ReadRepo<REFMERCHANTS_SQL>(ent);
            REFMERCHANTS_SQL rm = new REFMERCHANTS_SQL() { MERCHANT = 9000000001, USER_ID = 1 };
            mock.Object.BindContext(ent);          
            mock.Object.AddEntity(rm);
            mock.Verify(s => s.AddEntity(It.IsAny<REFMERCHANTS_SQL>()), Times.Once());            

            Assert.IsTrue(true);
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

            Assert.AreNotEqual(0,a);
        }
        [TestMethod]
        public void SectorRepo_DeleteBySector_TEST()
        {
            int cntSt = 0;
            int cntFn = 0;

            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var mock = new Mock<ISectorFilterRepo<KEY_CLIENTS_SQL>>();           
            mock.Object.BindContext(ent);
            IQueryable<KEY_CLIENTS_SQL> kk = (from s in (new List<KEY_CLIENTS_SQL>() {

                new KEY_CLIENTS_SQL() { MERCHANT = 9290000020, SECTOR_ID = 101 },
                new KEY_CLIENTS_SQL() { MERCHANT = 9290000021, SECTOR_ID = 101 },
                new KEY_CLIENTS_SQL() { MERCHANT = 9290000022, SECTOR_ID = 101 },         

            }) select s).AsQueryable();
          

            if (mock.Object.GetAll().Any())
            {
                cntSt = mock.Object.GetAll().Count();
            }
            mock.Object.AddEntities(kk);
            mock.Verify(s => s.AddEntities(It.IsAny<IQueryable<KEY_CLIENTS_SQL>>()), Times.Once());
                       
        }
        [TestMethod]
        public void Merchants_TEST()
        {
       
            SQLDB_CHANGE ent = new SQLDB_CHANGE(@"SQLDB_J");
            var mock = new Mock<IMerchantFilterRepo<REFMERCHANTS_SQL,KEY_CLIENTS_SQL>>();
            mock.Object.BindContext(ent);     
            IQueryable<REFMERCHANTS_SQL> rm = new List<REFMERCHANTS_SQL>() {

                new REFMERCHANTS_SQL() { MERCHANT = 9290000020, USER_ID = 101 },
                new REFMERCHANTS_SQL() { MERCHANT = 9290000021, USER_ID = 101 },
                new REFMERCHANTS_SQL() { MERCHANT = 9290000022, USER_ID = 101 },
                new REFMERCHANTS_SQL() { MERCHANT = 9290000023, USER_ID = 101 },
                new REFMERCHANTS_SQL() { MERCHANT = 9290000024, USER_ID = 101 }

            }.AsQueryable();

            mock.Object.AddEntities(rm);
            mock.Verify(s => s.AddEntities(It.IsAny<IQueryable<REFMERCHANTS_SQL>>()),Times.Once());

            Assert.IsTrue(true);
        }
    }
}

namespace Tests_
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
