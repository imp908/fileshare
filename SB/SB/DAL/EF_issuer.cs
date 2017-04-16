namespace SB.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using SB.Entities;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using System.Collections.Generic;

    public class EF_issuer : DbContext
    {
        // Your context has been configured to use a 'EF_issuer' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'SB.DAL.EF_issuer' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'EF_issuer' 
        // connection string in the application configuration file.
        public EF_issuer()
            : base("name=EF_issuer")
        {
            Database.SetInitializer( new MyInit());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<MyEntity>().ToTable(@"MyEntity");
            base.OnModelCreating(modelBuilder);
        }
       
        public DbSet<MyEntity> ENTITY { get; set; }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    public class MyEntity
    {
        [Key,Column(Order =1)]       
        public int Id { get; set; }

        //[Key, Column(Order = 2)]       
        //public string newID2 { get; set; }

        public string Name { get; set; }
        public string SerName { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class MyInit : CreateDatabaseIfNotExists<EF_issuer>
    {
        protected override void Seed(EF_issuer context)
        {
            EF_issuer ent = new EF_issuer();
            List<MyEntity> entList = new List<MyEntity>()
            {
                new MyEntity() { Name = "A", SerName = "B", BirthDate = DateTime.Now, Id = 3 },
                new MyEntity() { Name = "C", SerName = "D", BirthDate = DateTime.Now, Id = 4 },
                new MyEntity() { Name = "E", SerName = "F", BirthDate = DateTime.Now, Id = 5 }
            };

            ent.ENTITY.ForEachAsync(s => ent.ENTITY.Add(s));

            ent.SaveChanges();

            base.Seed(context);
        }
    }
}