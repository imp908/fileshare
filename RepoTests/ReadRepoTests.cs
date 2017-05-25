using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Repo.DAL;

namespace Repo.Tests
{
    [TestClass()]
    public class ReadRepoTests
    {
       

        [TestMethod()]
        public void ReadRepoTest()
        {
            DWH_entities ent = new DWH_entities();
            var a = ent.Database.Connection;
            var b = a.State;
            ReadRepo<REFMERCHANTS> rr = new ReadRepo<REFMERCHANTS>(ent);

            Assert.Fail();
        }
    }
}