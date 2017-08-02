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
    class Tests
    {
    }
}



namespace UOW.Tests
{
    [TestClass()]
    public class UOW_tests
    {

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
    public class RepoTests
    {


        KEY_CLIENTS_SQL kk;
        IQueryable<KEY_CLIENTS_SQL> kkList;
        Mock<IRepository<KEY_CLIENTS_SQL>> kkRepo;

        [TestInitialize]
        public void TestInit()
        {        
            kk = new KEY_CLIENTS_SQL() { MERCHANT = 900000001, RESPONSIBILITY_MANAGER = @"MNG1" };

            kkList = new List<KEY_CLIENTS_SQL>() {
            new KEY_CLIENTS_SQL() { MERCHANT = 900000002, RESPONSIBILITY_MANAGER = @"MNG2"},
            new KEY_CLIENTS_SQL() { MERCHANT = 900000003, RESPONSIBILITY_MANAGER = @"MNG2"},
            new KEY_CLIENTS_SQL() { MERCHANT = 900000003, RESPONSIBILITY_MANAGER = @"MNG2"},
         }.AsQueryable();

            kkRepo.Setup(m => m.GetALL()).Returns(kkList);
        }

        [TestCleanup]
        public void TestClean()
        {
            kk = null;
            kkList = null;
            kkRepo = null;
        }


        [TestMethod]
        public void Repo_test()
        {
            
            Assert.Fail();
        }
    }
}
