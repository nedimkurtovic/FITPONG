using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.AccountVMs
{
    public class RegistracijaVM
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="Morate unijeti email")]
        [RegularExpression(pattern: "[a-zA-Z0-9.]+@edu\\.fit\\.ba"
            , ErrorMessage = "Email mora biti u obliku ime.prezime@edu.fit.ba")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Morate unijeti password")]
        public string Password{ get; set; }

        [Display( Name ="Potvrdite password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Morate potvrditi password")]
        [Compare("Password",ErrorMessage ="Morate unijeti isti password")]
        public string PotvrdaPassword{ get; set; }
    }
}
