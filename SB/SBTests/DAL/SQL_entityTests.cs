using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repo.DAL.SQL_ent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Data.Entity;
using TGR;

namespace Repo.DAL.SQL_ent.Tests
{
    [TestClass()]
    public class SQL_entityTests
    {
        [TestMethod()]
        public void SQL_entityTest()
        {

            var mockSet = new Mock<DbSet<T_ACQ_D>>();
            var mockContext = new Mock<SQL_entity>();

            mockContext.Setup(m => m.T_ACQ_D).Returns(mockSet.Object);

            var service = new EditRepo<T_ACQ_D>(mockContext.Object);
            service.AddEntity(new T_ACQ_D { ID = 0, MERCHANT = 9290000001, DATE = DateTime.Now });

            mockSet.Verify(m => m.Add(It.IsAny<T_ACQ_D>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
           
        }
    }
}