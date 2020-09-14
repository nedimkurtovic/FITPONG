using FIT_PONG.Database;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Remotion.Linq.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public class TakmicenjeAutorizator : ITakmicenjeAutorizator
    {
        private readonly MyDb db;
        private readonly ISuspenzijaService suspenzijaService;

        public TakmicenjeAutorizator(MyDb _db, ISuspenzijaService _suspenzijaService)
        {
            db = _db;
            suspenzijaService = _suspenzijaService;
        }

        public bool AuthorizeUpdate(int UserId, int TakmicenjeId)
        {
            return KreatorUslov(UserId, TakmicenjeId);

        }
        public bool AuthorizeDelete(int UserId, int TakmicenjeId)
        {
            return KreatorUslov(UserId, TakmicenjeId);
        }

        public bool AuthorizeInit(int UserId, int TakmicenjeId)
        {
            return KreatorUslov(UserId, TakmicenjeId);
        }


        public bool AuthorizeEvidencijaMeca(int UserId, EvidencijaMeca obj)
        {
            if (!obj.Tim1.Select(x => x.IgracID).Contains(UserId)
                    && !obj.Tim2.Select(x => x.IgracID).Contains(UserId))
                throw new AuthorizeException("Niste autorizovani za ovu radnju!");
            return true;
        }

        public bool AuthorizePrijavaBlok(int UserId, int PrijavaId)
        {
            var takmicenje = db.Prijave.Include(x => x.Takmicenje)
                .Where(x => x.ID == PrijavaId).Select(x => x.Takmicenje).FirstOrDefault();
            return KreatorUslov(UserId, takmicenje.ID);
        }

        public bool AuthorizePrijavaDelete(int UserId, int PrijavaId)
        {
            var prijava = db.PrijaveIgraci.Include(x=>x.Prijava).Include(x=>x.Igrac)
                .Where(x=>x.PrijavaID==PrijavaId).ToList();
            if(!prijava.Select(x=>x.IgracID).Contains(UserId))
                throw new AuthorizeException("Niste autorizovani za ovu radnju!");
            return true;

        }

        public void AuthorizePrijava(int UserId, PrijavaInsert obj)
        {
            if (UserId != obj.Igrac1ID && UserId != obj.Igrac2ID)
                throw new AuthorizeException("Niste autorizovani za takvu radnju.");
            var suspenzija = suspenzijaService.ImaVazecuSuspenziju(UserId, "Prijava na takmičenja");
            if (suspenzija != null)
            {
                UserException ex = new UserException();
                ex.AddError("Suspenzija", $"Suspendovani ste sa prijavom na takmičenja do:  {suspenzija.DatumZavrsetka.ToString()}");
                throw ex;
            }

        }

        public void AuthorizeOtkaziPrijavu(int UserId, Prijave p)
        {   
            if(p!=null && p.Igrac1ID != null && p.Igrac1ID != UserId && p.Igrac2ID != UserId)
                throw new AuthorizeException("Niste autorizovani za takvu radnju.");
        }


        private bool KreatorUslov(int UserId, int TakmicenjeId)
        {
            var takmicenje = db.Takmicenja.Include(x => x.Kreator)
                .Where(x => x.ID == TakmicenjeId).FirstOrDefault();
            if (takmicenje.KreatorID != UserId)
                throw new AuthorizeException("Niste autorizovani za ovu radnju!");
            return true;
        }

        public bool AuthorizeInsert(int UserId)
        {
            var suspenzija = suspenzijaService.ImaVazecuSuspenziju(UserId, "Kreiranje takmičenja");
            if (suspenzija != null)
            {
                UserException ex = new UserException();
                ex.AddError("Suspenzija", $"Suspendovani ste sa kreiranjem takmičenja do:  {suspenzija.DatumZavrsetka.ToString()}");
                throw ex;
            }
            return true;
        }
    }
}
