namespace DAL.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SQLDB : DbContext
    {
        // Your context has been configured to use a 'SQLDB' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DAL.DAL.SQLDB' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'SQLDB' 
        // connection string in the application configuration file.
        public SQLDB()
            : base("name=SQLDB")
        {
            Database.SetInitializer<SQLDB>(new CreateDatabaseIfNotExists<SQLDB>());
        }

        public SQLDB(string ConnectionName)
            : base(ConnectionName)
        {
            Database.SetInitializer<SQLDB>(new CreateDatabaseIfNotExists<SQLDB>());
            Database.Initialize(true);
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Model.SQLmodel.REFMERCHANTS_SQL> REFERCHANTS_SQL { get; set; }
        public virtual DbSet<Model.SQLmodel.T_ACQ_M_SQL> T_ACQ_M { get; set; }
        public virtual DbSet<Model.SQLmodel.T_ACQ_D_SQL> T_ACQ_D { get; set; }
        public virtual DbSet<Model.SQLmodel.KEY_CLIENTS_SQL> KEY_CLIENTS { get; set; }



    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}