using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.ViewModels.TakmicenjeVMs
{
    public class TabelaStavkaVM
    {
        public string Naziv { get; set; }
        public int Pobjeda{ get; set; }
        public int Poraza{ get; set; }
        public int UkupnoOdigrano{ get; set; }
    }
}
