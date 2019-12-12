using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Report
    {
        public int ID { get; set; }
        public string Naslov { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public string Sadrzaj { get; set; }
        public string Email { get; set; }
        public List<Attachment> Prilozi{ get; set; }
    }
}
