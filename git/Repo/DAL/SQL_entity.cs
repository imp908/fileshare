namespace Repo.DAL.SQL_ent
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class SQL_entity : DbContext
    {
        public SQL_entity()
            //: base("SQL_entities")
            : base("SQL_DE")
        {
            Database.SetInitializer<SQL_entity>(new DropCreateDatabaseIfModelChanges<SQL_entity>());           
        }
        public SQL_entity(string entity_) 
            : base (entity_)
        {
            Database.SetInitializer<SQL_entity>(new CreateDatabaseIfNotExists<SQL_entity>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
        }
        
        public virtual DbSet<T_ACQ_D_SQL> T_ACQ_D { get; set; }
        public virtual DbSet<T_ACQ_M_SQL> T_ACQ_M { get; set; }
        public virtual DbSet<T_CTL_D_SQL> T_CTL_D { get; set; }
        public virtual DbSet<T_ECOMM_D_SQL> T_ECOMM_D { get; set; }
        public virtual DbSet<T_ECOMM_M_SQL> T_ECOMM_M { get; set; }

        
        public virtual DbSet<TEMP_ACQ_D_SQL> TEMP_ACQ_D { get; set; }
        public virtual DbSet<TEMP_ACQ_M_SQL> TEMP_ACQ_M { get; set; }
        public virtual DbSet<TEMP_CTL_D_SQL> TEMP_CTL_D { get; set; }
        
     
        public virtual DbSet<TEMP_ACQ_SQL> TEMP_ACQ { get; set; }
        

        public virtual DbSet<REFMERCHANTS_SQL> REFMERCHANTS { get; set; }
        public virtual DbSet<KEY_CLIENTS_SQL> KEY_CLIENTS { get; set; }

        public virtual DbSet<SECTOR_SQL> SECTOR_NAMES { get; set; }
        public virtual DbSet<SECTOR_MASK_SQL> SECTOR_MASKS { get; set; }      

    }
    
}
