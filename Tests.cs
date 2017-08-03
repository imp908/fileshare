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

using nUnit = NUnit.Framework;

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

        KEY_CLIENTS_SQL client1, client2;
        IQueryable<KEY_CLIENTS_SQL> clientsQ;      
        List<KEY_CLIENTS_SQL> clientsL,clientsDel;
        Mock<DbSet<KEY_CLIENTS_SQL>> dbSet;
        Mock<IRepository<KEY_CLIENTS_SQL>> kkRepo;

        int clientsInitialCount, clientAfterAddCount, clientsAfterDeleteCount, clientsAfterListDeleteCount;

        //>!!! Change callback to check methods
        [TestInitialize]
        public void TestInit()
        {

            dbSet = new Mock<DbSet<KEY_CLIENTS_SQL>>();            

            client1 = new KEY_CLIENTS_SQL() { MERCHANT = 900000000, RESPONSIBILITY_MANAGER = @"MNG1", ID = 0 };
            client2 = new KEY_CLIENTS_SQL() { MERCHANT = 100000001, RESPONSIBILITY_MANAGER = @"MNG0", ID = -1 };

            clientsL = new List<KEY_CLIENTS_SQL>() {
            new KEY_CLIENTS_SQL() { MERCHANT = 900000001, RESPONSIBILITY_MANAGER = @"MNG2", ID = 1},
            new KEY_CLIENTS_SQL() { MERCHANT = 900000002, RESPONSIBILITY_MANAGER = @"MNG2", ID = 2},
            new KEY_CLIENTS_SQL() { MERCHANT = 900000003, RESPONSIBILITY_MANAGER = @"MNG2", ID = 3},
            new KEY_CLIENTS_SQL() { MERCHANT = 900000004, RESPONSIBILITY_MANAGER = @"MNG3", ID = 4},
            new KEY_CLIENTS_SQL() { MERCHANT = 900000005, RESPONSIBILITY_MANAGER = @"MNG3", ID = 5}
            };
            
            clientsDel = new List<KEY_CLIENTS_SQL> {
                (from s in clientsL where s.ID == 4 select s).FirstOrDefault(),
               (from s in clientsL where s.ID == 5 select s).FirstOrDefault()
            };

            clientsQ = clientsL.AsQueryable();

            kkRepo = new Mock<IRepository<KEY_CLIENTS_SQL>>();

            clientsInitialCount = clientsL.Count();
            clientAfterAddCount = clientsInitialCount + 1;
            clientsAfterDeleteCount = clientAfterAddCount - 1;
            clientsAfterListDeleteCount = clientsAfterDeleteCount - clientsDel.Count();

            //Arrange
            kkRepo.Setup(m => m.Add(It.IsAny<KEY_CLIENTS_SQL>())).Callback(
                (KEY_CLIENTS_SQL s) => {
                    clientsL.Add(s);
                });
            kkRepo.Setup(m => m.DeleteByID(It.IsAny<int>())).Callback(
                (int s) => {
                    clientsL.Remove(clientsL.Where(r => r.ID == s).FirstOrDefault());
                });
            kkRepo.Setup(m => m.DeleteByList(It.IsAny < IQueryable<KEY_CLIENTS_SQL>>())).Callback(
                (IQueryable<KEY_CLIENTS_SQL> s) => {
                    clientsL = clientsL.Except(clientsDel.AsEnumerable()).ToList();
                });
            kkRepo.Setup(m => m.Save()).Verifiable();
            kkRepo.Setup(m => m.GetByID(It.IsAny<int>())).Returns<int>(id => clientsQ.SingleOrDefault(r => r.ID == id));
            kkRepo.Setup(m => m.GetALL()).Returns(clientsL.AsQueryable());

        }
        
        [TestCleanup]
        public void TestClean()
        {
            client1 = null;
            clientsQ = null;
            kkRepo = null;

            client1 = null;
            client2 = null;
            clientsQ = null;
            clientsL = null;
            clientsDel = null;
            dbSet = null;
            kkRepo = null;
        }

        //>>!!! Allocate to separate methods 
        [TestMethod]
        public void Repo_test()
        {        
            
            //Act
            //Assert

            //ADD
            kkRepo.Object.Add(client1);
            kkRepo.Verify(s => s.Add(It.IsAny<KEY_CLIENTS_SQL>()));
            Assert.AreEqual(clientsL.Count(), clientAfterAddCount);

            //DELTEBYID
            kkRepo.Object.DeleteByID(0);
            kkRepo.Verify(s => s.DeleteByID(It.IsAny<int>()));
            Assert.AreEqual(clientsL.Count(), clientsAfterDeleteCount);

            //DELETEBYLIST
            kkRepo.Object.DeleteByList(clientsDel.AsQueryable());
            kkRepo.Verify(s => s.DeleteByList(It.IsAny<IQueryable<KEY_CLIENTS_SQL>>()));
            Assert.AreEqual(clientsL.Count(), clientsAfterListDeleteCount);

            //SAVE
            kkRepo.Object.Save();
            kkRepo.Verify(s => s.Save(), Times.Once());

            //GETALL
            var a = kkRepo.Object.GetALL().Count();
            kkRepo.Verify(s => s.GetALL(), Times.Once());
            Assert.AreEqual(a, clientsInitialCount);

            //GET BY ID
            int id1 = 1;
            int id2 = 3;
            var item1 = kkRepo.Object.GetByID(id1);
            var item2 = kkRepo.Object.GetByID(id2);
            kkRepo.Verify(s => s.GetByID(It.IsAny<int>()), Times.Exactly(2));
            Assert.AreEqual(id1 , item1.ID );
            Assert.AreEqual(id2, item2.ID);            

        }
                
              
    }

}


