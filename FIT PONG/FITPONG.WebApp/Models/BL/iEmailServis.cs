using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Database.DTOs;
namespace FIT_PONG.Models.BL
{
    public interface iEmailServis
    {
        public void PosaljiMejlReport(Report novi);
        public void PosaljiKonfirmacijskiMejl(string linkzaklik, string email);
        public void PosaljiResetPassword(string linkzaklik, string email);
        void PosaljiTwoFactorCode(int code, string email);
    }
}
