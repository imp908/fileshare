using Repo_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data.Entity;

using Moq;

using DAL.DAL;
using Model.SQLmodel;

namespace Tests_
{
    [TestClass()]
    public class ReadRepo_TEST
    {
        [TestMethod()]
        public void ReadRepoTest()
        {
            int Notexpected = 0;
            int res = 0;
            try
            {
                var moqSet = new Mock<DbSet<REFMERCHANTS_SQL>>();
                var moqContext = new Mock<SQLDB_CHANGE>();
                moqContext.Setup(m => m.REFMERCHANTS_SQL).Returns(moqSet.Object);
                var service = new ReadRepo<REFMERCHANTS_SQL>(moqContext.Object);
                moqContext.Object.REFMERCHANTS_SQL.Add(
                    new REFMERCHANTS_SQL { MERCHANT = 0000000001, USER_ID =0 }
                    );
                moqContext.Object.SaveChanges();
                //res = service.GetAll<REFMERCHANTS_SQL>().Count();
                res = (from s in moqContext.Object.REFMERCHANTS_SQL select s).Count();
            }
            catch(System.ArgumentNullException e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            catch (System.NotImplementedException e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            Assert.AreNotEqual(Notexpected, res);
        }

        [TestMethod()]
        public void SaveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DisposeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DisposeTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAllTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetByFilterTest()
        {
            Assert.Fail();
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

    [TestClass]
    public class DB_TESTS
    {

        [TestMethod]
        public void SQLDB_Presence_test()
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
            catch(Exception e)
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

        [TestMethod]
        public void SQLDB_TA_Initialized_test()
        {
            SQLDB_CHANGE context = new SQLDB_CHANGE(@"SQLDB_J");

            int cnt = 0;
            try
            {
                cnt = (from s in context.T_ACQ_D select s).Count();
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }

            Assert.AreNotEqual(0, cnt);
        }


    }

    
}
