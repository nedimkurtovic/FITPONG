using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Account
{
    public class Email_Password_Request
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Morate unijeti email")]
        [RegularExpression(pattern: "[a-zA-Z0-9.]+@edu\\.fit\\.ba"
            , ErrorMessage = "Email mora biti u obliku ime.prezime@edu.fit.ba")]
        public string Email { get; set; }
    }
}
