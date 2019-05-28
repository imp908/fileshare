using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace mvccoresb.Infrastructure.EF
{
    using mvccoresb.Domain.TestModels;
    using mvccoresb.Domain.GeoModel;

    public class TestContext : DbContext
    {

        public TestContext(DbContextOptions<TestContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //registration in startup.cs
            //optionsBuilder.UseSqlServer(@"Servedotnet buildr=AAAPC;Database=testdb;User Id=tl;Password=QwErT123;");
        }

        public DbSet<BlogEF> Blogs { get; set; }
        public DbSet<PostEF> Posts { get; set; }


        public DbSet<GeoCategory> GeoCategory { get; set; }
        public DbSet<GeoFacility> GeoFacility { get; set; }
        public DbSet<GeoLayout> GeoLayout { get; set; }


        public DbSet<ServiceType> ServiceTypes { get; set; }

        
        public DbSet<PersonEF> Persons { get; set; }
        public DbSet<StudentEF> Students { get; set; }
        public DbSet<InstructorEF> Instructors { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceType>()
            .HasKey(p => p.ServiceId);

            modelBuilder.Entity<ServiceType>()
            .Property(p => p.ServiceId).HasColumnName("ServiceID");
            
            modelBuilder.Entity<ServiceType>()
            .HasIndex(p => p.ServiceId);
            


            modelBuilder.Entity<PostEF>()
            .HasOne(p => p.Blog)
            .WithMany(p => p.Posts).OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<BlogEF>()
            .HasOne(p => p.BlogImage)
            .WithOne(p => p.Blog)
            .HasForeignKey<BlogImage>(p => p.BlogId);



            //many-to-many via join table 1->oo
            modelBuilder.Entity<TagEF>()
            .HasKey(p => p.TagId);

            modelBuilder.Entity<PostTagEF>()
            .HasKey(p => new { p.PostId , p.TagId});

            modelBuilder.Entity<PostTagEF>()
            .HasOne(p => p.Post)
            .WithMany(p => p.PostTags)            
            .HasForeignKey(p => p.PostId);

            modelBuilder.Entity<PostTagEF>()
            .HasOne(p => p.Tag)
            .WithMany(p => p.PostTags)
            .HasForeignKey(p => p.TagId);

            /**some Fluent API examples like :
            ToTable,HasColumnName,ValueGeneratedOnAdd,IsRequired,IsConcurrencyToken*/
            modelBuilder.Entity<PersonEF>().HasKey(p => p.Id);
            //table mapping
            modelBuilder.Entity<PersonEF>().ToTable("Persons");
            //column mapping
            modelBuilder.Entity<PersonEF>().Property(p => p.Id).HasColumnName("PersonID")
            //if generated in code tries to insert, else tries to generate from DB
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<PersonEF>().Property(p => p.Name)
            //not null
                .IsRequired();
            //concurency token
            modelBuilder.Entity<PersonEF>().Property(p => p.Name)
                .IsConcurrencyToken();

            //complex join queries
            /** Table per hierarhy (TPH)- 
                adds discriminator column for inherited Student class in Person table*/
            modelBuilder.Entity<StudentEF>().HasBaseType<PersonEF>();

            /**TPT and TPC - not implemented yet, ToTable not working for inherited classes
            manual implementation via migrations */
            modelBuilder.Entity<InstructorEF>().HasKey(p => p.Id);
            
            //Ignore
            //Entity
            modelBuilder.Ignore<ExludeFromDBentity>();
            //Property
            modelBuilder.Entity<BlogEF>().Ignore(p=>p.LoadedFromDatabase);

        }
    
    }
}