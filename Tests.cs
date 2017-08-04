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
            kkRepo.Setup(m => m.GetByID(It.IsAny<int>()))
                .Returns<int>(id => clientsQ.SingleOrDefault(r => r.ID == id));
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

namespace UOW.Tests
{

    [nUnit.TestFixture]
    public class UOW_Integration_tests
    {
        DbContext context;
        UOW uow_CUT;
        Repository<USERS_SQL> repo;

        [nUnit.OneTimeSetUp]
        public void UOW_init()
        {
            context = new SQLDB_CHANGE(@"SQLDB_J");
            repo = new Repository<USERS_SQL>(context);
            uow_CUT = new UOW();
            uow_CUT.BindRepoUsers(repo);
        }
        [nUnit.OneTimeTearDown]
        public void UOW_cleanUp()
        {
            repo.Dispose();
            uow_CUT.Dispose();
        }
        [nUnit.Test]
        public void UOW_integration_check()
        {
            int ID = uow_CUT.GetIDByCredentials("NAME2", "SERNAME2");
            nUnit.Assert.AreEqual(2, ID);
        }
    }

    [nUnit.TestFixture]
    public class UOW_tests
    {
        Mock<IUOW> iuow_CUT;
        List<USERS_SQL> users;
        List<MERCHANT_LIST_SQL> merchants;
        IQueryable<MERCHANT_LIST_SQL> merchantsGetExp, merchantsGetAct;
        string name, sername, UserSernameRes;
        int userBinded, userIDToGetSername, userPased;

        [nUnit.OneTimeSetUp]
        public void UOW_init()
        {
          
            userBinded = -1;
            name = @"NAME2";
            sername = @"SERNAME2";
            userIDToGetSername = 3;
            userPased = 10;

            users = new List<USERS_SQL>() {
                new USERS_SQL() { ID=1, Name = @"NAME1", Sername = @"SERNAME1", mail = @"NAME1@rsb.ru"},
                new USERS_SQL() { ID=2, Name = @"NAME2", Sername = @"SERNAME2", mail = @"NAME2@rsb.ru"},
                new USERS_SQL() { ID=3, Name = @"NAME3", Sername = @"SERNAME3", mail = @"NAME3@rsb.ru"}
            };

            merchants = new List<MERCHANT_LIST_SQL>() {
                    new MERCHANT_LIST_SQL() { MERCHANT = 9290000000, USER_ID =1, UPDATE_DATE = new DateTime(2017,08,03,00,00,01) },
                    new MERCHANT_LIST_SQL() { MERCHANT = 9290000001, USER_ID =1, UPDATE_DATE = new DateTime(2017,08,03,00,00,02) },
                    new MERCHANT_LIST_SQL() { MERCHANT = 9290000002, USER_ID =1, UPDATE_DATE = new DateTime(2017,08,03,00,00,03)} ,
                    new MERCHANT_LIST_SQL() { MERCHANT = 9290000007, USER_ID =3, UPDATE_DATE = new DateTime(2017,08,03,00,00,04)} ,
                    new MERCHANT_LIST_SQL() { MERCHANT = 9290000008, USER_ID =3, UPDATE_DATE = new DateTime(2017,08,03,00,00,05)} ,
                    new MERCHANT_LIST_SQL() { MERCHANT = 9290000009, USER_ID =3, UPDATE_DATE = new DateTime(2017,08,03,00,00,06)} 
                };

            //Arrange
            iuow_CUT = new Mock<IUOW>();
            iuow_CUT.Setup(s => s.GetIDByCredentials(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>(                          
                    (T0, T1) => users.Where(R => R.Name == T0 && R.Sername == T1).FirstOrDefault().ID
                );
            iuow_CUT.Setup(s => s.SetCurrentUser(It.IsAny<int>()))
                .Callback((int c) => { userBinded = c; } );
            iuow_CUT.Setup(s=>s.GetUserSernameByID())
                .Returns(
                    users.FirstOrDefault(s => s.ID == userIDToGetSername).Sername                    
                );
            iuow_CUT.Setup(s => s.GetMerchantListByUserId())
                .Returns(
                merchants.Where(s => s.USER_ID == userIDToGetSername).AsQueryable()
                );
           
        }
        [nUnit.OneTimeTearDown]
        public void UOW_cleanUp()
        {
            iuow_CUT = null;
        }
        [nUnit.Test]
        public void UOW_check()
        {

            //Act
            //Assert           
            
            int idExpected = users.SingleOrDefault(s => s.Name == name && s.Sername == sername).ID;
            int idActual=iuow_CUT.Object.GetIDByCredentials(name, sername);
            iuow_CUT.Verify(s => s.GetIDByCredentials(name,sername), Times.Exactly(1));
            Assert.AreEqual(idExpected,idActual);
            
            iuow_CUT.Object.SetCurrentUser(userPased);
            iuow_CUT.Verify(s => s.SetCurrentUser(It.IsAny<int>()));
            Assert.AreEqual(userPased, userBinded);
          
            UserSernameRes = users.FirstOrDefault(s => s.ID == userIDToGetSername).Sername;
            iuow_CUT.Object.GetUserSernameByID();
            iuow_CUT.Verify(s => s.GetUserSernameByID(),Times.Once());
            Assert.AreEqual(UserSernameRes, iuow_CUT.Object.GetUserSernameByID());


            merchantsGetExp = iuow_CUT.Object.GetMerchantListByUserId();
            merchantsGetAct = merchants.Where(s => s.ID == userIDToGetSername).AsQueryable();
            iuow_CUT.Verify(s => s.GetMerchantListByUserId(), Times.Once());
            Assert.AreEqual(merchantsGetAct, merchantsGetExp);
        }

    }

}