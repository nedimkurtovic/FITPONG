using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.AccountVMs
{
    public class LoginVM
    {
        [Required(ErrorMessage ="Morate unijeti email")]
        [Display( Name ="Email")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Morate unijeti password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }
    }
}
