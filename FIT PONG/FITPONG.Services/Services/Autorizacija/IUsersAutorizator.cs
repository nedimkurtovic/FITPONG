using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public interface IUsersAutorizator
    {
        
        bool AuthorizePromjenaPasswordaMail();
        bool AuthorizePromjenaPasswordaPotvrda();
        bool AuthorizePostovanje();
        bool AuthorizeUkloniSlikuProfila(int logiraniKorisnikId, int userId);
        bool AuthorizeEditSlikuProfila(int logiraniKorisnikId, int userId);

    }
}
