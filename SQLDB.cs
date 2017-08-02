namespace DAL.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;

    using Model.SQLmodel;


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
            //Database.SetInitializer<SQLDB_CHANGE>(new DropCreateDatabaseIfModelChanges<SQLDB_CHANGE>());
            Database.SetInitializer(new InitializerAtCreation());

            Database.Initialize(true);
        }


        public class InitializerAtCreation : DropCreateDatabaseIfModelChanges<SQLDB_CHANGE>
        {

            protected override void Seed(SQLDB_CHANGE context)
            {
                IList<KEY_CLIENTS_SQL> kk = new List<KEY_CLIENTS_SQL>()
                {
                    new KEY_CLIENTS_SQL {SECTOR_ID = 1, MERCHANT = 9290000000 },
                    new KEY_CLIENTS_SQL {SECTOR_ID = 2, MERCHANT = 9290000001 },
                      new KEY_CLIENTS_SQL {SECTOR_ID = 2, MERCHANT = 9290000003 }
                };

                context.KEY_CLIENTS.AddRange(kk);
                context.SaveChanges();

                IList<REFMERCHANTS_SQL> rm = new List<REFMERCHANTS_SQL>()
                {
                    new REFMERCHANTS_SQL { MERCHANT = 9290000000 },
                    new REFMERCHANTS_SQL { MERCHANT = 9290000001 },
                    new REFMERCHANTS_SQL { MERCHANT = 9290000002 },
                    new REFMERCHANTS_SQL { MERCHANT = 9290000003 },
                    new REFMERCHANTS_SQL { MERCHANT = 9290000004 },
                };

                context.REFMERCHANTS_SQL.AddRange(rm);
                context.SaveChanges();

                List<MERCHANT_LIST_SQL> ml = new List<MERCHANT_LIST_SQL>() {
                    new MERCHANT_LIST_SQL() { MERCHANT = 9290000000, USER_ID =0, UPDATE_DATE = DateTime.Now },
                    new MERCHANT_LIST_SQL() { MERCHANT = 9290000001, USER_ID =0, UPDATE_DATE = DateTime.Now },
                    new MERCHANT_LIST_SQL() { MERCHANT = 9290000002, USER_ID =0, UPDATE_DATE = DateTime.Now }
                };

                context.MERCHANT_LIST.AddRange(ml);
                context.SaveChanges();

                base.Seed(context);
            }
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
        public virtual DbSet<Model.SQLmodel.MERCHANT_LIST_SQL> MERCHANT_LIST { get; set; }
        public virtual DbSet<Model.SQLmodel.USERS_SQL> USERS { get; set; }
    }

}