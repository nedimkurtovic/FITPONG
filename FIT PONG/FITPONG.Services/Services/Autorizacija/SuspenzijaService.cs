using FIT_PONG.Database;
using FIT_PONG.Database.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public class SuspenzijaService : ISuspenzijaService
    {
        private readonly MyDb db;

        public SuspenzijaService(MyDb _db)
        {
            db = _db;
        }
        public Suspenzija ImaVazecuSuspenziju(int UserID, string VrstaSuspenzije)
        {
            var taSuspenzija = db.VrsteSuspenzije.Where(x => x.Opis == VrstaSuspenzije).FirstOrDefault();
            if (taSuspenzija == null)
                throw new Exception("Ne postoji ta vrsta suspenzije");
            var suspenzijeKorisnika = db.Suspenzije
                .Where(x => x.IgracID == UserID && x.VrstaSuspenzijeID == taSuspenzija.ID)
                .OrderByDescending(x=>x.ID)
                .ToList();
            foreach(var i in suspenzijeKorisnika)
            {
                if (i.DatumZavrsetka >= DateTime.Now)
                    return i;
            }
            return null;
        }
    }
}
