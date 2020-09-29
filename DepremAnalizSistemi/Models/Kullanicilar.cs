namespace DepremAnalizSistemi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kullanicilar")]
    public partial class Kullanicilar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kullanicilar()
        {
            GidenMesajlar = new HashSet<Mesajlar>();
            GelenMesajlar = new HashSet<Mesajlar>();
        }
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Ad sayý ve özel karakter içeremez")]
        public string Ad { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Soyad sayý ve özel karakter içeremez")]
        public string Soyad { get; set; }
        
        [Required]
        [StringLength(11)]
        [RegularExpression(@"^[1-9]{1}[0-9]{10}$", ErrorMessage = "Tc Kimlik Numarasý 11 haneli olmalý ve 0 ile baþlayamaz. ")]
        public string TcNo { get; set; }

        [Required]
        [StringLength(320)]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Girilen Email formatý uygun deðil. ")]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [MaxLength(11)]
        [RegularExpression(@"^(05(\d{9}))$", ErrorMessage = "Telefon numarasý 05 ile baþlamalý ve 11 haneli olmalýdýr.")]
        public string Telefon { get; set; }
        
        [Display(Name = "Þifre")]
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,15}$", ErrorMessage = "Þifre en az 6 karakterden oluþmalý.15 karakteri geçemez.En az bir adet büyük harf,bir adet küçük harf ve rakam içermelidir.")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
        
        [Display(Name = "Þifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("Sifre")]
        [NotMapped]
        public string SifreOnay { get; set; }

        [Display(Name = "Yeni Þifre")]
        [DataType(DataType.Password)]
        [Compare("YeniSifreOnay")]
        [NotMapped]
        public string YeniSifre { get; set; }

        [Display(Name = "Yeni Þifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("YeniSifre")]
        [NotMapped]
        public string YeniSifreOnay { get; set; }

        [NotMapped]
        public string ErrorMessage { get; set; }
        [NotMapped]
        public string PasswordErrorMessage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mesajlar> GidenMesajlar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mesajlar> GelenMesajlar { get; set; }
    }
}
