using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Account
{
    public class Login
    {
        [Required(ErrorMessage = "Morate unijeti email")]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Morate unijeti password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public string Code { get; set; } //2F code, nisam siguran da li ovo treba biti u ovom requestu...
    }
}
