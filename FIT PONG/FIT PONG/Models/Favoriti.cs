using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models
{
    public class Favoriti
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public IdentityUser<int> User { get; set; }

        public int UtakmicaId { get; set; }
        public Utakmica Utakmica { get; set; }
    }
}
