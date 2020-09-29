namespace DepremAnalizSistemi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Mesajlar")]
    public partial class Mesajlar
    {
        public int ID { get; set; }

        public int DepremID { get; set; }

        public int GonderenID { get; set; }

        public int AliciID { get; set; }

        public string Mesaj { get; set; }

        [StringLength(200)]
        public string MesajBasligi { get; set; }

        public DateTime? GonderimZamani { get; set; }
        public bool GonderenSildiMi { get; set; }
        public bool AliciSildiMi { get; set; }
        public bool OkunduMu { get; set; }

        public virtual Depremler Depremler { get; set; }

        public virtual Kullanicilar Gonderen { get; set; }

        public virtual Kullanicilar Alici { get; set; }
    }
}
