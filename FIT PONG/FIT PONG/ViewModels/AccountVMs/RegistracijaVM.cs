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
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Morate unijeti password")]
        public string Password{ get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Morate potvrditi password")]
        [Compare("Password",ErrorMessage ="Morate unijet isti password jebo ga vi")]
        public string PotvrdaPassword{ get; set; }
    }
}
