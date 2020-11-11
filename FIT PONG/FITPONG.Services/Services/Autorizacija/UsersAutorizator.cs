using FIT_PONG.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public class UsersAutorizator : IUsersAutorizator
    {
        private readonly MyDb db;
        private readonly ISuspenzijaService suspenzijaServis;

        public UsersAutorizator(MyDb db, ISuspenzijaService _suspenzijaServis)
        {
            this.db = db;
            suspenzijaServis = _suspenzijaServis;
        }

        public bool AuthorizeEditProfila(int logiraniKorisnikId, int userId)
        {
            return IsLogiraniKorisnik(logiraniKorisnikId, userId);
        }

        public bool AuthorizeEditSlikuProfila(int logiraniKorisnikId, int userId)
        {
            return IsLogiraniKorisnik(logiraniKorisnikId, userId);
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
            if (loggedInUsername != "aldin.talic@edu.fit.ba" && loggedInUsername != "nedim.kurtovic@edu.fit.ba"
                && loggedInUsername != "desktop")
                throw new AuthorizeException("Samo administrator je autorizovan za ovu radnju.");
            return true;
        }

        public bool AuthorizeLogin(int userId)
        {
            var suspenzija = suspenzijaServis.ImaVazecuSuspenziju(userId, "Login");
            if (suspenzija != null)
            {
                UserException ex = new UserException();
                ex.AddError("Suspenzija", $"Suspendovani ste sa loginom do {suspenzija.DatumZavrsetka.ToString()}");
                throw ex;
            }
            return true;
        }
    }
}
