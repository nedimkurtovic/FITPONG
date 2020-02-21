using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Models.BL
{
    public interface iEmailServis
    {
        public void PosaljiMejlReport(Report novi);
        public void PosaljiKonfirmacijskiMejl(string linkzaklik, string email);
        public void PosaljiResetPassword(string linkzaklik, string email);
    }
}
