using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Account
{
    public class PasswordPromjena
    {
        [Required]
        public string token { get; set; }
        [Required]
        [Display(Name = "Unesite novi password")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [Display(Name = "Potvrdite novi password")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Password se mora podudarati")]
        public string potvrdaPassword { get; set; }

        [RegularExpression(pattern: "[a-zA-Z0-9.]+@edu\\.fit\\.ba", ErrorMessage = "Email mora biti u obliku ime.prezime@edu.fit.ba")]
        public string Email { get; set; }
    }
}
