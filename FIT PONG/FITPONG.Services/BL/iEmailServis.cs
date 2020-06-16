using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Database.DTOs;
namespace FIT_PONG.Services.BL
{
    public interface iEmailServis
    {
        void PosaljiMejlReport(Report novi);
        void PosaljiKonfirmacijskiMejl(string linkzaklik, string email);
        void PosaljiResetPassword(string linkzaklik, string email);
        void PosaljiTwoFactorCode(int code, string email);
    }
}
