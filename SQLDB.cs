namespace DAL.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Initialization context with create if not exists option
    /// </summary>
    public class SQLDB_INIT : DbContext
    {
        // Your context has been configured to use a 'SQLDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DAL.DAL.SQLDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SQLDB' 
        // connection string in the application configuration file.
        public SQLDB_INIT()
            : base("name=SQLDB")
        {
            Database.SetInitializer<SQLDB_INIT>(new CreateDatabaseIfNotExists<SQLDB_INIT>());
        }

        public SQLDB_INIT(string ConnectionName)
            : base(ConnectionName)
        {
            Database.SetInitializer<SQLDB_INIT>(new CreateDatabaseIfNotExists<SQLDB_INIT>());
            Database.Initialize(true);
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Model.SQLmodel.REFMERCHANTS_SQL> REFMERCHANTS_SQL { get; set; }
        public virtual DbSet<Model.SQLmodel.T_ACQ_M_SQL> T_ACQ_M { get; set; }
        public virtual DbSet<Model.SQLmodel.T_ACQ_D_SQL> T_ACQ_D { get; set; }
        public virtual DbSet<Model.SQLmodel.KEY_CLIENTS_SQL> KEY_CLIENTS { get; set; }

    }

    /// <summary>
    /// Context with recreate if model changes
    /// </summary>
    public class SQLDB_CHANGE : DbContext
    {
        // Your context has been configured to use a 'SQLDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DAL.DAL.SQLDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SQLDB' 
        // connection string in the application configuration file.
        public SQLDB_CHANGE()
            : base("name=SQLDB")
        {
            Database.SetInitializer<SQLDB_CHANGE>(new DropCreateDatabaseIfModelChanges<SQLDB_CHANGE>());
        }

        public SQLDB_CHANGE(string ConnectionName)
            : base(ConnectionName)
        {
            Database.SetInitializer<SQLDB_CHANGE>(new DropCreateDatabaseIfModelChanges<SQLDB_CHANGE>());
            Database.Initialize(true);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {          
            base.OnModelCreating(modelBuilder);
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Model.SQLmodel.REFMERCHANTS_SQL> REFMERCHANTS_SQL { get; set; }
        public virtual DbSet<Model.SQLmodel.T_ACQ_M_SQL> T_ACQ_M { get; set; }
        public virtual DbSet<Model.SQLmodel.T_ACQ_D_SQL> T_ACQ_D { get; set; }
        public virtual DbSet<Model.SQLmodel.KEY_CLIENTS_SQL> KEY_CLIENTS { get; set; }

    }

    /// <summary>
    /// Context recreate always 
    /// </summary>
    public class SQLDB_HARD : DbContext
    {
        // Your context has been configured to use a 'SQLDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DAL.DAL.SQLDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SQLDB' 
        // connection string in the application configuration file.
        public SQLDB_HARD()
            : base("name=SQLDB")
        {
            Database.SetInitializer<SQLDB_HARD>(new DropCreateDatabaseAlways<SQLDB_HARD>());
        }

        public SQLDB_HARD(string ConnectionName)
            : base(ConnectionName)
        {
            Database.SetInitializer<SQLDB_HARD>(new DropCreateDatabaseAlways<SQLDB_HARD>());
            Database.Initialize(true);
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Model.SQLmodel.REFMERCHANTS_SQL> REFMERCHANTS_SQL { get; set; }
        public virtual DbSet<Model.SQLmodel.T_ACQ_M_SQL> T_ACQ_M { get; set; }
        public virtual DbSet<Model.SQLmodel.T_ACQ_D_SQL> T_ACQ_D { get; set; }
        public virtual DbSet<Model.SQLmodel.KEY_CLIENTS_SQL> KEY_CLIENTS { get; set; }

    }

    /// <summary>
    /// Context for testing HR entities
    /// </summary>
    public class SQLHR_CHANGE : DbContext
    {
        // Your context has been configured to use a 'SQLDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DAL.DAL.SQLDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SQLDB' 
        // connection string in the application configuration file.
        public SQLHR_CHANGE()
            : base("name=TEST_CHG_HR_J")
        {
            Database.SetInitializer<SQLHR_CHANGE>(new DropCreateDatabaseIfModelChanges<SQLHR_CHANGE>());
        }

        public SQLHR_CHANGE(string ConnectionName)
            : base(ConnectionName)
        {
            Database.SetInitializer<SQLHR_CHANGE>(new DropCreateDatabaseIfModelChanges<SQLHR_CHANGE>());
            Database.Initialize(true);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Model.SQLmodel.REGIONS> REGIONS_SQL { get; set; }      

    }

}