namespace VeriCek.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DepremModel : DbContext
    {
        public DepremModel()
            : base("VeriCek.Properties.Settings.Setting")
        {
        }

        public virtual DbSet<Depremler> Depremler { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Depremler>()
                .Property(e => e.Enlem)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Depremler>()
                .Property(e => e.Boylam)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Depremler>()
                .Property(e => e.Siddet)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Depremler>()
                .Property(e => e.Derinlik)
                .HasPrecision(19, 4);
        }
    }
}
