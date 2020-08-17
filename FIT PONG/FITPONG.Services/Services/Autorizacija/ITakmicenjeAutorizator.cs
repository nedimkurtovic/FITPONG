using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public interface ITakmicenjeAutorizator
    {
        bool AuthorizeUpdate(int UserId, int TakmicenjeId);
        bool AuthorizeInit(int UserId, int TakmicenjeId);
        bool AuthorizeDelete(int UserId, int TakmicenjeId);
        bool AuthorizePrijavaDelete(int UserId, int PrijavaId);
        bool AuthorizePrijavaBlok(int UserId, int PrijavaId);
        bool AuthorizeEvidencijaMeca(int UserId, EvidencijaMeca obj);
        void AuthorizePrijava(int UserId, PrijavaInsert obj);
        void AuthorizeOtkaziPrijavu(int UserId, Prijave prijava);


    }
}
