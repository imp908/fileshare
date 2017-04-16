namespace SB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using SB.Entities;

    /// <summary>
    /// Models for oracle - from database 
    /// and for sql- with connection string change
    /// </summary>
    public partial class ORCL_model : DbContext
    {
        public ORCL_model()
            : base("name=ORCL_model")
        {

        }

        public virtual DbSet<EMPLOYEES> EMPLOYEES { get; set; }
        public virtual DbSet<JOBS> JOBS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.FIRST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.LAST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.PHONE_NUMBER)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.JOB_ID)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.SALARY)
                .HasPrecision(8, 2);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.COMMISSION_PCT)
                .HasPrecision(2, 2);

            modelBuilder.Entity<EMPLOYEES>()
                .HasMany(e => e.EMPLOYEES1)
                .WithOptional(e => e.EMPLOYEES2)
                .HasForeignKey(e => e.MANAGER_ID);

            modelBuilder.Entity<JOBS>()
                .Property(e => e.JOB_ID)
                .IsUnicode(false);

            modelBuilder.Entity<JOBS>()
                .Property(e => e.JOB_TITLE)
                .IsUnicode(false);

            modelBuilder.Entity<JOBS>()
                .HasMany(e => e.EMPLOYEES)
                .WithRequired(e => e.JOBS)
                .WillCascadeOnDelete(false);
        }
    }

    /// <summary>
    /// model for SQL with connection string change   
    /// </summary>
    public partial class ORCL_SQL_model : DbContext
    {
        public ORCL_SQL_model()
            : base("name=ORCL_SQL_model")
        {
        }

        public virtual DbSet<EMPLOYEES> EMPLOYEES { get; set; }
        public virtual DbSet<JOBS> JOBS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.FIRST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.LAST_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.PHONE_NUMBER)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.JOB_ID)
                .IsUnicode(false);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.SALARY)
                .HasPrecision(8, 2);

            modelBuilder.Entity<EMPLOYEES>()
                .Property(e => e.COMMISSION_PCT)
                .HasPrecision(2, 2);

            modelBuilder.Entity<EMPLOYEES>()
                .HasMany(e => e.EMPLOYEES1)
                .WithOptional(e => e.EMPLOYEES2)
                .HasForeignKey(e => e.MANAGER_ID);

            modelBuilder.Entity<JOBS>()
                .Property(e => e.JOB_ID)
                .IsUnicode(false);

            modelBuilder.Entity<JOBS>()
                .Property(e => e.JOB_TITLE)
                .IsUnicode(false);

            modelBuilder.Entity<JOBS>()
                .HasMany(e => e.EMPLOYEES)
                .WithRequired(e => e.JOBS)
                .WillCascadeOnDelete(false);
        }
    }
}
