using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.AccountVMs
{
    public class ResetPasswordEmailVM
    {
        [Required]
        [EmailAddress]
        [RegularExpression(pattern: "[a-zA-Z0-9.]+@edu\\.fit\\.ba"
            , ErrorMessage = "Email mora biti u obliku ime.prezime@edu.fit.ba")]
        public string Email{ get; set; }
    }
}
