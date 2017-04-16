using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;

namespace SB
{
    public partial class BI_copy : DbContext
    {
        public BI_copy() : base ("name=BI_copy")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<table1> table1_copy { get; set; }
    }
}
