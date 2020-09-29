namespace DepremAnalizSistemi.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DepremModel : DbContext
    {
        public DepremModel()
            : base("DepremAnalizSistemi.Properties.Settings.ConnString")
        {
        }

        public virtual DbSet<Depremler> Depremler { get; set; }
        public virtual DbSet<Kullanicilar> Kullanicilar { get; set; }
        public virtual DbSet<Mesajlar> Mesajlar { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }

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

            modelBuilder.Entity<Depremler>()
                .HasMany(e => e.Mesajlar)
                .WithRequired(e => e.Depremler)
                .HasForeignKey(e => e.DepremID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanicilar>()
                .HasMany(e => e.GidenMesajlar)
                .WithRequired(e => e.Gonderen)
                .HasForeignKey(e => e.GonderenID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanicilar>()
                .HasMany(e => e.GelenMesajlar)
                .WithRequired(e => e.Alici)
                .HasForeignKey(e => e.AliciID)
                .WillCascadeOnDelete(false);
        }

        public System.Data.Entity.DbSet<DepremAnalizSistemi.Models.ViewModel.UserLoginViewModel> UserLoginViewModels { get; set; }
    }
}
