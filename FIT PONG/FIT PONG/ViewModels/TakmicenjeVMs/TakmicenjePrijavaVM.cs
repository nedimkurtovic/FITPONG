using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.TakmicenjeVMs
{
    public class TakmicenjePrijavaVM
    {
        public bool isTim { get; set; }
        [StringLength(50,ErrorMessage ="Naziv ne smije biti duži od 50 karaktera.")]
        //[Required(ErrorMessage ="Ovo je obavezno polje.")]
        public string Naziv { get; set; }
        public int Igrac1ID { get; set; }
        public int Igrac2ID { get; set; }
        public int takmicenjeID { get; set; }
    }
}
