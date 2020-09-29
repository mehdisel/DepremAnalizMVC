using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DepremAnalizSistemi.Models.ViewModel
{
    public class UserLoginViewModel
    {
        public int ID { get; set; }
        [Required]
        [StringLength(320)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifreyi girmediniz!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string LoginErrorMessage { get; set; }
    }
}