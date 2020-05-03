using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Objave
{
    public class ObjaveInsertUpdate
    {
        [Required(ErrorMessage = "Ovo polje je obavezno.")]
        public string Naziv { get; set; }
        [Display(Name = "Sadržaj objave")]
        [Required(ErrorMessage = "Ovo polje je obavezno.")]
        public string Content { get; set; }
    }
}
