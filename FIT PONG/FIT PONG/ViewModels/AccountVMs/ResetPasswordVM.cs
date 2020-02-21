using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.AccountVMs
{
    public class ResetPasswordVM
    {
        public string token { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Compare("password",ErrorMessage = "Password se mora podudarati")]
        public string potvrdaPassword { get; set; }
    }
}
