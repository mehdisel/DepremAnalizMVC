namespace DepremAnalizSistemi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Depremler")]
    public partial class Depremler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Depremler()
        {
            Mesajlar = new HashSet<Mesajlar>();
        }

        public int Id { get; set; }

        public DateTime? DepremTarihi { get; set; }

        [StringLength(100)]
        public string DepremYeri { get; set; }

        [Column(TypeName = "money")]
        public decimal? Enlem { get; set; }

        [Column(TypeName = "money")]
        public decimal? Boylam { get; set; }

        [Column(TypeName = "money")]
        public decimal? Siddet { get; set; }

        [Column(TypeName = "money")]
        public decimal? Derinlik { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mesajlar> Mesajlar { get; set; }
    }
}
