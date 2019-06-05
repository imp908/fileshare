namespace TransactionSite_
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using TransactionSite_.Models;

    public class SQL_CONTEXT : DbContext
    {
        // Your context has been configured to use a 'SQL_MODEL' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the
        // 'TransactionSite_.SQL_MODEL' database on your LocalDb instance.
        // 
        // If you wish to target a different database and/or database provider, modify the 'SQL_MODEL'
        // connection string in the application configuration file.
        public SQL_CONTEXT()
            : base("name=SQL_CONN")
        {
            
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public DbSet<REFMERCHANT> REFMERCHANTS { get; set; }
        public DbSet<FD_ACQ_D> FD_ACQ_D { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

}