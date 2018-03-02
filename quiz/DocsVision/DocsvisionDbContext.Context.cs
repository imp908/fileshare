using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using Orient.Client;

namespace Intranet.Models.DocsVision
{
    public partial class DocsvisionDbContext : DbContext, IDatabase
    {
        public DocsvisionDbContext()
            : base("name=docsvisinDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<Nullable<System.Guid>> nspk_BlackFriday_GetEmployeeIdByUserAccountForIntranet(string userAccount)
        {
            var userAccountParameter = userAccount != null ?
                new ObjectParameter("UserAccount", userAccount) :
                new ObjectParameter("UserAccount", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<System.Guid>>("nspk_BlackFriday_GetEmployeeIdByUserAccountForIntranet", userAccountParameter);
        }
    
        public virtual ObjectResult<nspk_GetTaskForCurrentPerformerInfoForIntranet_Result> nspk_GetTaskForCurrentPerformerInfoForIntranet(Nullable<System.Guid> employeeId)
        {
            var employeeIdParameter = employeeId.HasValue ?
                new ObjectParameter("EmployeeId", employeeId) :
                new ObjectParameter("EmployeeId", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<nspk_GetTaskForCurrentPerformerInfoForIntranet_Result>("nspk_GetTaskForCurrentPerformerInfoForIntranet", employeeIdParameter);
        }

        public List<object> GetData(string userName)
        {
            var employeeId = nspk_BlackFriday_GetEmployeeIdByUserAccountForIntranet(userName).FirstOrDefault().Value;
            return nspk_GetTaskForCurrentPerformerInfoForIntranet(employeeId).Select(f => (object)f).ToList();
        }

        public ODatabase GetDataBase<T>()
        {
            throw new NotImplementedException();
        }

        public System.Data.Entity.DbSet<Intranet.Models.DocsVision.nspk_GetTaskForCurrentPerformerInfoForIntranet_Result> nspk_GetTaskForCurrentPerformerInfoForIntranet_Result { get; set; }
    }
}
