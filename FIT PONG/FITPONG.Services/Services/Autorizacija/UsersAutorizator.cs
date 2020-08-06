using FIT_PONG.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public class UsersAutorizator : IUsersAutorizator
    {
        private readonly MyDb db;

        public UsersAutorizator(MyDb db)
        {
            this.db = db;
        }

        public bool AuthorizeEditProfila(int logiraniKorisnikId, int userId)
        {
            return IsLogiraniKorisnik(logiraniKorisnikId, userId);
        }

        public bool AuthorizeEditSlikuProfila(int logiraniKorisnikId, int userId)
        {
            return IsLogiraniKorisnik(logiraniKorisnikId, userId);
        }

        public bool AuthorizePostovanje()
        {
            throw new NotImplementedException();
        }

        public bool AuthorizePromjenaPasswordaMail()
        {
            throw new NotImplementedException();
        }

        public bool AuthorizePromjenaPasswordaPotvrda()
        {
            throw new NotImplementedException();
        }

        public bool AuthorizeSuspenziju(string loggedInUsername)
        {
            return IsAdministrator(loggedInUsername);
        }

        public bool AuthorizeUkloniSlikuProfila(int logiraniKorisnikId, int userId)
        {
            return IsLogiraniKorisnik(logiraniKorisnikId, userId);
        }

        private bool IsLogiraniKorisnik(int logiraniKorisnikId, int userId)
        {
            if (logiraniKorisnikId != userId)
                throw new AuthorizeException("Niste autorizovani za takvu radnju.");
            return true;
        }

        private bool IsAdministrator(string loggedInUsername)
        {
            if (loggedInUsername != "aldin.talic@edu.fit.ba" && loggedInUsername != "nedim.kurtovic@edu.fit.ba")
                throw new AuthorizeException("Samo administrator je autorizovan za ovu radnju.");
            return true;
        }
    }
}
