using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FIT_PONG.ViewModels.ObjavaVMS
{
    public class ObjavaUnosVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Ovo polje je obavezno.")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Ovo polje je obavezno.")]
        public string Content { get; set; }
    }
}
