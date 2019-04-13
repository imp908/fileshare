using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Repo.DAL;
using Repo.DAL.SQL_ent;

namespace Repo.Tests
{
    [TestClass()]
    public class UnitOfWorkGenericTests
    {

        protected static DateTime st = new DateTime(2016, 01, 14, 00, 00, 00, DateTimeKind.Utc);
        protected static DateTime fn = new DateTime(2016, 03, 15, 23, 59, 59);

        [TestMethod()]
        public void CallDisconnectedProcedureTest()
        {

            SQL_entity ent = new SQL_entity();
            UnitOfWorkGeneric uof = new UnitOfWorkGeneric(ent);
            uof.RefreshValues();

            /*
             * 
            //12
            var a = uof.GetByDate<T_ACQ_D>(st, fn).Count();
            //5
            var b = uof.GetByMerchants<KEY_CLIENTS>().Count();
            //10
            var c = uof.MerchantListCount();


            ReadRepo<KEY_CLIENTS> kkRead = new ReadRepo<KEY_CLIENTS>(ent);
            //5
            var d = uof.GetByMerchants<KEY_CLIENTS>().Count();
            uof.DeleteBySector(2);
            uof.SaveAll();
            //4
            var e = uof.GetByMerchants<KEY_CLIENTS>().Count();


            EditRepo<REFMERCHANTS> rfEdit = new EditRepo<REFMERCHANTS>(ent);
            //10
            var f = uof.MerchantListCount();
            uof.DeleteByUserID(0);
            uof.SaveAll();
            //9
            var g = uof.MerchantListCount();

            uof.RefreshValues();

            //27
            var h =
            uof.GetByDate<T_ACQ_D>(st, fn).Count()
            + uof.GetByMerchants<KEY_CLIENTS>().Count()
            + uof.MerchantListCount();

            */

            Assert.Fail();
        }

    }
}