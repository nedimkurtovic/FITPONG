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

        public TakmicenjeAutorizator(MyDb _db)
        {
            db = _db;
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
        }

        public void AuthorizeOtkaziPrijavu(int UserId, Prijave p)
        {   
            if(p!=null && p.Igrac1ID != null && p.Igrac1ID != UserId && p.Igrac2ID != -1 && p.Igrac2ID != UserId)
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
    }
}
