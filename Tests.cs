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

using nUnit=NUnit.Framework;



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
            bool result=false;
            string connection_=@"SQLDB_J";
            try
            {
                SQLDB_CHANGE ent=new SQLDB_CHANGE(connection_);
                result=true;
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
            SQLDB_CHANGE context=new SQLDB_CHANGE(@"SQLDB_J");
            int cnt=0;
            try
            {
                cnt=(from s in context.KEY_CLIENTS select s).Count();
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
            SQLDB_CHANGE context=new SQLDB_CHANGE(@"SQLDB_J");

            int cnt=0;
            try
            {
                cnt=(from s in context.REFMERCHANTS_SQL select s).Count();
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

            dbSet=new Mock<DbSet<KEY_CLIENTS_SQL>>();            

            client1=new KEY_CLIENTS_SQL(){MERCHANT=900000000, RESPONSIBILITY_MANAGER=@"MNG1", ID=0};
            client2=new KEY_CLIENTS_SQL(){MERCHANT=100000001, RESPONSIBILITY_MANAGER=@"MNG0", ID=-1};

            clientsL=new List<KEY_CLIENTS_SQL>(){
            new KEY_CLIENTS_SQL(){MERCHANT=900000001, RESPONSIBILITY_MANAGER=@"MNG2", ID=1},
            new KEY_CLIENTS_SQL(){MERCHANT=900000002, RESPONSIBILITY_MANAGER=@"MNG2", ID=2},
            new KEY_CLIENTS_SQL(){MERCHANT=900000003, RESPONSIBILITY_MANAGER=@"MNG2", ID=3},
            new KEY_CLIENTS_SQL(){MERCHANT=900000004, RESPONSIBILITY_MANAGER=@"MNG3", ID=4},
            new KEY_CLIENTS_SQL(){MERCHANT=900000005, RESPONSIBILITY_MANAGER=@"MNG3", ID=5}
           };
            
            clientsDel=new List<KEY_CLIENTS_SQL> {
                (from s in clientsL where s.ID == 4 select s).FirstOrDefault(),
               (from s in clientsL where s.ID == 5 select s).FirstOrDefault()
           };

            clientsQ=clientsL.AsQueryable();

            kkRepo=new Mock<IRepository<KEY_CLIENTS_SQL>>();

            clientsInitialCount=clientsL.Count();
            clientAfterAddCount=clientsInitialCount + 1;
            clientsAfterDeleteCount=clientAfterAddCount - 1;
            clientsAfterListDeleteCount=clientsAfterDeleteCount - clientsDel.Count();

            //Arrange
            kkRepo.Setup(m => m.Add(It.IsAny<KEY_CLIENTS_SQL>())).Callback(
                (KEY_CLIENTS_SQL s) => {
                    clientsL.Add(s);
               });
            kkRepo.Setup(m => m.DeleteByID(It.IsAny<int>())).Callback(
                (int s) => {
                    clientsL.Remove(clientsL.Where(r => r.ID == s).FirstOrDefault());
               });
            kkRepo.Setup(m => m.DeleteByList(It.IsAny < List<KEY_CLIENTS_SQL>>())).Callback(
                (IQueryable<KEY_CLIENTS_SQL> s) => {
                    clientsL=clientsL.Except(clientsDel.AsEnumerable()).ToList();
               });
            kkRepo.Setup(m => m.Save()).Verifiable();
            kkRepo.Setup(m => m.GetByID(It.IsAny<int>()))
                .Returns<int>(id => clientsQ.SingleOrDefault(r => r.ID == id));
            kkRepo.Setup(m => m.GetALL()).Returns(clientsL.AsQueryable());

       }
        
        [TestCleanup]
        public void TestClean()
        {
            client1=null;
            clientsQ=null;
            kkRepo=null;

            client1=null;
            client2=null;
            clientsQ=null;
            clientsL=null;
            clientsDel=null;
            dbSet=null;
            kkRepo=null;
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
            kkRepo.Object.DeleteByList(clientsDel);
            kkRepo.Verify(s => s.DeleteByList(It.IsAny<List<KEY_CLIENTS_SQL>>()));
            Assert.AreEqual(clientsL.Count(), clientsAfterListDeleteCount);

            //SAVE
            kkRepo.Object.Save();
            kkRepo.Verify(s => s.Save(), Times.Once());

            //GETALL
            var a=kkRepo.Object.GetALL().Count();
            kkRepo.Verify(s => s.GetALL(), Times.Once());
            Assert.AreEqual(a, clientsInitialCount);

            //GET BY ID
            int id1=1;
            int id2=3;
            var item1=kkRepo.Object.GetByID(id1);
            var item2=kkRepo.Object.GetByID(id2);
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
        Repository<USERS_SQL> usersRepo;
        Repository<KEY_CLIENTS_SQL> clientsRepo;
        Repository<MERCHANT_LIST_SQL> merchantsRepo;
        List<MERCHANT_LIST_SQL> merchantsToInsert;
        string UserNameToGet, UserSernameToGet, UserSernameSetted;
        int SetUserID, GetUSerID, merchantsForUserCnt;

        [nUnit.OneTimeSetUp]
        public void UOW_init()
        {

            context =new SQLDB_CHANGE(@"SQLDB_J");

            usersRepo=new Repository<USERS_SQL>(context);
            clientsRepo=new Repository<KEY_CLIENTS_SQL>(context);
            merchantsRepo=new Repository<MERCHANT_LIST_SQL>(context);

            uow_CUT =new UOW();

            uow_CUT.BindRepoUsers(usersRepo);
            uow_CUT.BindRepoClients(clientsRepo);
            uow_CUT.BindRepoMerchants(merchantsRepo);

            UserNameToGet = @"NAME2";
            UserSernameToGet = @"SERNAME2";
         
            SetUserID = 3;
            GetUSerID = -1;

            UserSernameSetted = @"SERNAME3";

            merchantsForUserCnt = 3;

            merchantsToInsert = new List<MERCHANT_LIST_SQL>() {
new MERCHANT_LIST_SQL() { MERCHANT = 9290000090, USER_ID = 3, UPDATE_DATE = new DateTime(2017, 08, 05, 00, 00, 08) }
,new MERCHANT_LIST_SQL() { MERCHANT = 9290000091, USER_ID = 3, UPDATE_DATE = new DateTime(2017, 08, 05, 00, 00, 08) }
,new MERCHANT_LIST_SQL() { MERCHANT = 9290000081, USER_ID = 4, UPDATE_DATE = new DateTime(2017, 08, 05, 00, 00, 08) }
,new MERCHANT_LIST_SQL() { MERCHANT = 9290000082, USER_ID = 4, UPDATE_DATE = new DateTime(2017, 08, 05, 00, 00, 08) }
,new MERCHANT_LIST_SQL() { MERCHANT = 9290000083, USER_ID = 4, UPDATE_DATE = new DateTime(2017, 08, 05, 00, 00, 08) }
            };

        }
        [nUnit.OneTimeTearDown]
        public void UOW_cleanUp()
        {
            clientsRepo.Dispose();            
            usersRepo.Dispose();
            context.Dispose();
            uow_CUT.Dispose();
        }

        [nUnit.Test]
        public void GetIDByCredentialsTest()
        {            
            //GetIDByCredentials
            int IDAct =uow_CUT.GetIDByCredentials(UserNameToGet, UserSernameToGet);
            int IDExp = (from s in context.Set<USERS_SQL>() where s.Name== UserNameToGet && s.Sername == UserSernameToGet select s).FirstOrDefault().ID;
            nUnit.Assert.AreEqual(IDExp, IDAct);

        }
        [nUnit.Test()]
        public void SetCurrentUserTest()
        {
          
            int ExpId = SetUserID;

            uow_CUT.SetCurrentUser(SetUserID);

            var a = uow_CUT.GetType()
                .GetField(@"currentUserId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(uow_CUT);

            if(a is int)
            {
                GetUSerID = (int)a;
            }            
        
            Assert.AreEqual(ExpId, GetUSerID);
        }
        [nUnit.Test()]
        public void GetCurrentUserSernameTest()
        {
            uow_CUT.SetCurrentUser(SetUserID);
            string ActUsername =  uow_CUT.GetCurrentUserSername();
            Assert.AreEqual(UserSernameSetted, ActUsername);
        }

        [nUnit.Test()]
        public void GetMerchantListByUserIdTest()
        {
            uow_CUT.SetCurrentUser(SetUserID);
            int MerchantsAct = uow_CUT.GetMerchantListByUserId().Count();
            Assert.AreEqual(merchantsForUserCnt,MerchantsAct);
        }
        [nUnit.Test()]
        public void InsertMerchatListTest()
        {

            var a = uow_CUT.GetType();
            var b = a.GetField("merchantList_repository",
                 System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var c = b.GetValue(uow_CUT);

            IRepository <MERCHANT_LIST_SQL> mlRepo = (IRepository < MERCHANT_LIST_SQL >)uow_CUT.GetType().GetField("merchantList_repository",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .GetValue(uow_CUT);

            int merchantsBefore = mlRepo.GetALL().Count();
            int merchantsExpected = merchantsBefore + merchantsToInsert.Count();
            uow_CUT.InsertMerchatList(merchantsToInsert);
            
            int merchantsAfter = mlRepo.GetALL().Count();
           
            Assert.AreEqual(merchantsExpected, merchantsAfter);
            Assert.AreNotEqual(merchantsBefore, merchantsAfter);

            mlRepo.DeleteByList(merchantsToInsert);
        }

    }

    [nUnit.TestFixture]
    public class UOW_tests
    {

        Mock<IUOW> iuow_CUT;
        List<USERS_SQL> users;
        List<KEY_CLIENTS_SQL> keyClients, keyClientsToInsert;
        List<MERCHANT_LIST_SQL> merchants, merchantsToInsert, merchantsToFilter;
        IQueryable<MERCHANT_LIST_SQL> merchantsGetExp, merchantsGetAct;
        string name, sername, UserSernameRes;
        int userBinded, userIDToGetSername, userPased, UserIDforKeyClients;

        private bool MerchantsListCompare(IQueryable<MERCHANT_LIST_SQL> item1, IQueryable<MERCHANT_LIST_SQL> item2)
        {           
            bool result=false;
            result=item1.OrderBy(s => s.ID).SequenceEqual(item2.OrderBy(s => s.ID));
            return result;
        }

        List<T_ACQ_M_SQL> acq_result;

        DateTime st=new DateTime(2017, 08, 06, 00, 00, 14);
        DateTime fn=new DateTime(2017, 08, 07, 00, 00, 15);

        [nUnit.OneTimeSetUp]
        public void UOW_init()
        {
          
            userBinded=-1;
            name=@"NAME2";
            sername=@"SERNAME2";
            userIDToGetSername=3;
            userPased=10;
            UserIDforKeyClients = 1;

            users = new List<USERS_SQL>(){
                new USERS_SQL(){ID=1, Name=@"NAME1", Sername=@"SERNAME1", mail=@"NAME1@rsb.ru"},
                new USERS_SQL(){ID=2, Name=@"NAME2", Sername=@"SERNAME2", mail=@"NAME2@rsb.ru"},
                new USERS_SQL(){ID=3, Name=@"NAME3", Sername=@"SERNAME3", mail=@"NAME3@rsb.ru"},
                new USERS_SQL(){ID=4, Name=@"NAME4", Sername=@"SERNAME4", mail=@"NAME4@rsb.ru"},
                new USERS_SQL(){ID=5, Name=@"NAME5", Sername=@"SERNAME5", mail=@"NAME5@rsb.ru"}

            };

            keyClients=new List<KEY_CLIENTS_SQL>(){
                new KEY_CLIENTS_SQL(){ID=0,MERCHANT=9290000000, RESPONSIBILITY_MANAGER=@"SERNAME1", SECTOR_ID=1},
                new KEY_CLIENTS_SQL(){ID=1,MERCHANT=9290000002, RESPONSIBILITY_MANAGER=@"SERNAME1", SECTOR_ID=1},
                new KEY_CLIENTS_SQL(){ID=3,MERCHANT=9290000001, RESPONSIBILITY_MANAGER=@"SERNAME2",SECTOR_ID=2},
                new KEY_CLIENTS_SQL(){ID=4,MERCHANT=9290000003, RESPONSIBILITY_MANAGER=@"SERNAME3",SECTOR_ID=3},
                new KEY_CLIENTS_SQL(){ID=5,MERCHANT=9290000005, RESPONSIBILITY_MANAGER=@"SERNAME1",SECTOR_ID=1},
                new KEY_CLIENTS_SQL(){ID=6,MERCHANT=9290000007, RESPONSIBILITY_MANAGER=@"SERNAME3",SECTOR_ID=3}
            };

            keyClientsToInsert = new List<KEY_CLIENTS_SQL>(){
                new KEY_CLIENTS_SQL(){ID=7,MERCHANT=9290000008, RESPONSIBILITY_MANAGER=@"SERNAME2",SECTOR_ID=2},
                new KEY_CLIENTS_SQL(){ID=8,MERCHANT=9290000009, RESPONSIBILITY_MANAGER=@"SERNAME1",SECTOR_ID=1}               
            };

            merchants =new List<MERCHANT_LIST_SQL>(){
                new MERCHANT_LIST_SQL(){MERCHANT=9290000000, USER_ID =1, UPDATE_DATE=new DateTime(2017,08,03,00,00,01)},
                new MERCHANT_LIST_SQL(){MERCHANT=9290000001, USER_ID =1, UPDATE_DATE=new DateTime(2017,08,03,00,00,02)},                
                new MERCHANT_LIST_SQL(){MERCHANT=9290000003, USER_ID =1, UPDATE_DATE=new DateTime(2017,08,03,00,00,03)},
                new MERCHANT_LIST_SQL(){MERCHANT=9290000005, USER_ID =1, UPDATE_DATE=new DateTime(2017,08,03,00,00,03)},
                new MERCHANT_LIST_SQL(){MERCHANT=9290000007, USER_ID =3, UPDATE_DATE=new DateTime(2017,08,03,00,00,04)},
                new MERCHANT_LIST_SQL(){MERCHANT=9290000008, USER_ID =3, UPDATE_DATE=new DateTime(2017,08,03,00,00,05)},
                new MERCHANT_LIST_SQL(){MERCHANT=9290000009, USER_ID =3, UPDATE_DATE=new DateTime(2017,08,03,00,00,06)} 
            };

            merchantsToInsert=new List<MERCHANT_LIST_SQL>(){
                new MERCHANT_LIST_SQL(){MERCHANT=9290000010, USER_ID =4, UPDATE_DATE=new DateTime(2017,08,04,00,00,07)},
                new MERCHANT_LIST_SQL(){MERCHANT=9290000011, USER_ID =4, UPDATE_DATE=new DateTime(2017,08,04,00,00,08)}
            };

            merchantsToFilter=new List<MERCHANT_LIST_SQL>(){
                new MERCHANT_LIST_SQL(){MERCHANT=9290000001, USER_ID =1, UPDATE_DATE=new DateTime(2017,08,03,00,00,02)},
                new MERCHANT_LIST_SQL(){MERCHANT=9290000008, USER_ID =3, UPDATE_DATE=new DateTime(2017,08,03,00,00,05)}
            };

            acq_result=new List<T_ACQ_M_SQL>(){
                new T_ACQ_M_SQL(){MERCHANT=9290000000, AMT=1, DATE=new DateTime(2017,01,05,00,00,13)},
                new T_ACQ_M_SQL(){MERCHANT=9290000000, AMT=2, DATE=new DateTime(2017,02,06,00,00,14)},
                new T_ACQ_M_SQL(){MERCHANT=9290000000, AMT=3, DATE=new DateTime(2017,03,07,00,00,15)},
                new T_ACQ_M_SQL(){MERCHANT=9290000000, AMT=4, DATE=new DateTime(2017,04,08,00,00,15)},

                new T_ACQ_M_SQL(){MERCHANT=9290000001, AMT=5, DATE=new DateTime(2017,01,05,00,00,17)},
                new T_ACQ_M_SQL(){MERCHANT=9290000001, AMT=6, DATE=new DateTime(2017,02,06,00,00,18)},
                new T_ACQ_M_SQL(){MERCHANT=9290000001, AMT=7, DATE=new DateTime(2017,03,07,00,00,19)},
                new T_ACQ_M_SQL(){MERCHANT=9290000001, AMT=8, DATE=new DateTime(2017,04,08,00,00,20)},

                new T_ACQ_M_SQL(){MERCHANT=9290000003, AMT=9, DATE=new DateTime(2017,01,05,00,00,21)},
                new T_ACQ_M_SQL(){MERCHANT=9290000003, AMT=10, DATE=new DateTime(2017,02,06,00,00,22)},
                new T_ACQ_M_SQL(){MERCHANT=9290000003, AMT=11, DATE=new DateTime(2017,03,07,00,00,23)},
                new T_ACQ_M_SQL(){MERCHANT=9290000003, AMT=12, DATE=new DateTime(2017,04,08,00,00,24)},

                new T_ACQ_M_SQL(){MERCHANT=9290000008, AMT=13, DATE=new DateTime(2017,01,05,00,00,01)},
                new T_ACQ_M_SQL(){MERCHANT=9290000008, AMT=13, DATE=new DateTime(2017,02,06,00,00,01)},
                new T_ACQ_M_SQL(){MERCHANT=9290000008, AMT=13, DATE=new DateTime(2017,03,07,00,00,01)},
                new T_ACQ_M_SQL(){MERCHANT=9290000008, AMT=13, DATE=new DateTime(2017,04,08,00,00,01)}
            };

            //Arrange
            iuow_CUT=new Mock<IUOW>();

            iuow_CUT.Setup(s => s.GetIDByCredentials(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>(                          
                    (T0, T1) => users.Where(R => R.Name == T0 && R.Sername == T1).FirstOrDefault().ID
                );
            iuow_CUT.Setup(s => s.SetCurrentUser(It.IsAny<int>()))
                .Callback((int c) =>{userBinded=c;});
            iuow_CUT.Setup(s=>s.GetUserSernameByID())
                .Returns(
                    users.FirstOrDefault(s => s.ID == userIDToGetSername).Sername                    
                );

            iuow_CUT.Setup(s => s.GetMerchantListByUserId())
                .Returns(
                merchants.Where(s => s.USER_ID == userIDToGetSername).AsQueryable()
                );
            iuow_CUT.Setup(s => s.InsertMerchatList(It.IsAny<List<MERCHANT_LIST_SQL>>()))
                .Callback((List<MERCHANT_LIST_SQL> c) => merchants.AddRange(c) );

            iuow_CUT.Setup(s=>s.GetAcqByDate(It.IsAny<DateTime>(),It.IsAny<DateTime>()))
                .Returns< DateTime ,DateTime >( (T0,T1) => acq_result.Where(s=>s.DATE >= T0 && s.DATE <= T1).AsQueryable() );

            iuow_CUT.Setup(s => s.GetAcqByMerchants(It.IsAny<List<MERCHANT_LIST_SQL>>()))
                .Returns<List<MERCHANT_LIST_SQL>>((c) =>
                    (from s in acq_result join z in c on s.MERCHANT equals z.MERCHANT select s).AsQueryable()
                );


            iuow_CUT.Setup(s => s.GetKKByUserId())
                .Returns(
                    (from s in keyClients where s.RESPONSIBILITY_MANAGER 
                        == (from z in users where z.ID == UserIDforKeyClients select z.Sername ).FirstOrDefault() select s )
                        .AsQueryable()
                );

            iuow_CUT.Setup(s => s.InsertKKFromList(It.IsAny<List<KEY_CLIENTS_SQL>>()))
                .Callback((List<KEY_CLIENTS_SQL> s)=>keyClients.AddRange(s));

        }
        [nUnit.OneTimeTearDown]
        public void UOW_cleanUp()
        {
            iuow_CUT=null;
        }
        [nUnit.Test]
        public void UOW_check()
        {

            //Act
            //Assert           
            bool ListsCompareResult= false;

            int idExpected=users.SingleOrDefault(s => s.Name == name && s.Sername == sername).ID;
            int idActual=iuow_CUT.Object.GetIDByCredentials(name, sername);
            iuow_CUT.Verify(s => s.GetIDByCredentials(name,sername), Times.Exactly(1));
            Assert.AreEqual(idExpected,idActual);
            
            iuow_CUT.Object.SetCurrentUser(userPased);
            iuow_CUT.Verify(s => s.SetCurrentUser(It.IsAny<int>()));
            Assert.AreEqual(userPased, userBinded);
          
            UserSernameRes=users.FirstOrDefault(s => s.ID == userIDToGetSername).Sername;
            iuow_CUT.Object.GetUserSernameByID();
            iuow_CUT.Verify(s => s.GetUserSernameByID(),Times.Once());
            Assert.AreEqual(UserSernameRes, iuow_CUT.Object.GetUserSernameByID());
        
            merchantsGetExp=iuow_CUT.Object.GetMerchantListByUserId().AsQueryable();
            merchantsGetAct=merchants.Where(s => s.USER_ID == userIDToGetSername).AsQueryable();
            iuow_CUT.Verify(s => s.GetMerchantListByUserId(), Times.Once());
            ListsCompareResult=MerchantsListCompare(merchantsGetExp, merchantsGetAct);
            Assert.IsTrue(ListsCompareResult);

            int beforeinsert=merchants.Count();
            iuow_CUT.Object.InsertMerchatList(merchantsToInsert);
            iuow_CUT.Verify(s => s.InsertMerchatList(merchantsToInsert), Times.Once());
            int afterinsert=merchants.Count();
            Assert.AreNotEqual(beforeinsert, afterinsert);

            int acqByDateResult=iuow_CUT.Object.GetAcqByDate(st,fn).Count();
            iuow_CUT.Verify(s => s.GetAcqByDate(st, fn), Times.Once());
            Assert.AreEqual(acq_result.Where(s=>s.DATE >= st && s.DATE <= fn).Count(), acqByDateResult);      

        }

        [nUnit.Test]
        public void UOW_GetACQbyMerchantList()
        {
            decimal? amtExp =
            (from z in acq_result join x in merchantsToFilter on z.MERCHANT equals x.MERCHANT select z)
            .Sum(s => s.AMT);
            decimal? amtAct=
                iuow_CUT.Object.GetAcqByMerchants(merchantsToFilter).Sum(s=>s.AMT);
            iuow_CUT.Verify(s => s.GetAcqByMerchants(It.IsAny<List<MERCHANT_LIST_SQL>>()), Times.Once());
            Assert.AreEqual(amtExp, amtAct);
        }

        [nUnit.Test]
        public void UOW_GetKKByUserId()
        {
            int clientsExp = 
                (from s in keyClients where s.RESPONSIBILITY_MANAGER ==
                (from z in users where z.ID == UserIDforKeyClients select z.Sername).FirstOrDefault()
                select s).Count();
            int clientsAct = iuow_CUT.Object.GetKKByUserId().Count();
            iuow_CUT.Verify(s => s.GetKKByUserId(), Times.Once());
            Assert.AreEqual(clientsExp,clientsAct);
        }

        [nUnit.Test]
        public void UOW_InsertKKFromList()
        {
            int clientsAfterInsertExp = keyClients.Count() + keyClientsToInsert.Count();
            int clientsBeforeInsert = keyClients.Count();
            iuow_CUT.Object.InsertKKFromList(keyClientsToInsert);
            iuow_CUT.Verify(s => s.InsertKKFromList(keyClientsToInsert), Times.Once);
            int clientsAfterInsert = keyClients.Count();            
            Assert.AreNotEqual(clientsBeforeInsert, clientsAfterInsert);
            Assert.AreEqual(clientsAfterInsertExp, clientsAfterInsert);
        }


    }

}