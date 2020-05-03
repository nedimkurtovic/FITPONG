using FIT_PONG.Database.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels
{
    public class Utakmice
    {
    
        public int ID { get; set; }

        public int BrojUtakmice { get; set; }
        public DateTime DatumVrijeme { get; set; }
        
        public string Rezultat { get; set; }

        public Runda Runda { get; set; }
        public int RundaID { get; set; }

        public Tip_Utakmice TipUtakmice { get; set; }
        public int TipUtakmiceID { get; set; }

        public Status_Utakmice Status { get; set; }
        public int StatusID { get; set; }

        public bool IsEvidentirana { get; set; }
        public List<Igrac_Utakmica> UcescaNaUtakmici { get; set; }

    }
}
