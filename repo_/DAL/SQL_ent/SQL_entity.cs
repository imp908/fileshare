namespace Repo.DAL.SQL_ent
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SQL_entity : DbContext
    {
        public SQL_entity()
            : base("SQL_entities")
        {
            Database.SetInitializer<SQL_entity>(new DropCreateDatabaseIfModelChanges<SQL_entity>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
        }
        
        public virtual DbSet<T_ACQ_D> T_ACQ_D { get; set; }
        public virtual DbSet<T_ACQ_M> T_ACQ_M { get; set; }
        public virtual DbSet<T_CTL_D> T_CTL_D { get; set; }
        public virtual DbSet<T_ECOMM_D> T_ECOMM_D { get; set; }
        public virtual DbSet<T_ECOMM_M> T_ECOMM_M { get; set; }
        
        public virtual DbSet<TEMP_ACQ_D> TEMP_ACQ_D { get; set; }
        public virtual DbSet<TEMP_ACQ_M> TEMP_ACQ_M { get; set; }
        public virtual DbSet<TEMP_CTL_D> TEMP_CTL_D { get; set; }

        public virtual DbSet<TEMP_ECOMM_D> TEMP_ECOMM_D { get; set; }
     
        public virtual DbSet<TEMP_ACQ> TEMP_ACQ { get; set; }

        public virtual DbSet<REFMERCHANTS> REFMERCHANTS { get; set; }
        public virtual DbSet<KEY_CLIENTS> KEY_CLIENTS { get; set; }

        public virtual DbSet<SECTOR> SECTOR_NAMES { get; set; }
        public virtual DbSet<SECTOR_MASK> SECTOR_MASKS { get; set; }      

    }
}
