using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DepremAnalizSistemi.Models.ViewModel
{
    public class AccountSettingsViewModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(320)]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Girilen Email formatı uygun değil. ")]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^(05(\d{9}))$", ErrorMessage = "Telefon numarası 05 ile başlamalı ve 11 haneli olmalıdır.")]
        public string Telefon { get; set; }
    }
}