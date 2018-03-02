using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Orient.Client;

namespace Intranet.Models.IntraService
{
    public partial class IntraServiceDbContext : DbContext, IDatabase
    {
        public IntraServiceDbContext()
            : base("name=is4_32_5Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<GetTaskCountForIntranet_Result> GetTaskCountForIntranet(string employeeId)
        {
            var employeeIdParameter = employeeId != null ?
                new ObjectParameter("EmployeeId", employeeId) :
                new ObjectParameter("EmployeeId", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetTaskCountForIntranet_Result>("GetTaskCountForIntranet", employeeIdParameter);
        }

        public List<object> GetData(string userName)
        {
            var accountName = userName.Split('\\')[1];

            return GetTaskCountForIntranet(accountName).Select(f => (object)f).ToList() ??
                new List<GetTaskCountForIntranet_Result> { new GetTaskCountForIntranet_Result(0, accountName) }.Select(f => (object)f).ToList();
        }

        public ODatabase GetDataBase<T>()
        {
            throw new NotImplementedException();
        }
    }
}
