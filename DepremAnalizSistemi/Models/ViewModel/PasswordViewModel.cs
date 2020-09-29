using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DepremAnalizSistemi.Models.ViewModel
{
    public class PasswordViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Şifre")]
        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,15}$", ErrorMessage = "Şifre en az 6 karakterden oluşmalı.15 karakteri geçemez.En az bir adet büyük harf,bir adet küçük harf ve rakam içermelidir.")]
        [Compare("YeniSifreOnay")]
        public string YeniSifre { get; set; }

        [Display(Name = "Yeni Şifre Tekrar")]
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,15}$", ErrorMessage = "Şifre en az 6 karakterden oluşmalı.15 karakteri geçemez.En az bir adet büyük harf,bir adet küçük harf ve rakam içermelidir.")]
        [DataType(DataType.Password)]
        [Compare("YeniSifre")]
        public string YeniSifreOnay { get; set; }
    }
}