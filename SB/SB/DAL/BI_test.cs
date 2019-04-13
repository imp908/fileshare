namespace SB
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BI_test : DbContext
    {
        public BI_test()
            : base("name=BI_model")
        {
        }

        public virtual DbSet<table1> table1 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<table1>()
                .Property(e => e.value2)
                .HasPrecision(18, 0);

            modelBuilder.Entity<table1>()
                .Property(e => e.range1)
                .IsUnicode(false);

            modelBuilder.Entity<table1>()
                .Property(e => e.range2)
                .IsUnicode(false);
        }
    }
}
