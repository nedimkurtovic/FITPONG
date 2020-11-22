using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using FIT_PONG.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FIT_PONG.Services.Services.Autorizacija;
using Microsoft.EntityFrameworkCore;

namespace FIT_PONG.Services.Services
{
    public class PrijaveService : IPrijaveService
    {
        private readonly MyDb db;
        private readonly IMapper mapper;
        private readonly ISuspenzijaService suspenzijaService;

        public PrijaveService(MyDb db, IMapper mapper, ISuspenzijaService _suspenzijaService)
        {
            this.db = db;
            this.mapper = mapper;
            suspenzijaService = _suspenzijaService;
        }

        public Prijave Add(int TakmicenjeID, PrijavaInsert obj)
        {
            ValidirajDodavanje(TakmicenjeID, obj);

            var prijava = new Database.DTOs.Prijava
            {
                DatumPrijave = DateTime.Now,
                TakmicenjeID = TakmicenjeID,
                isTim = obj.isTim,
                Naziv = obj.Naziv
            };

            prijava.StanjePrijave = new Database.DTOs.Stanje_Prijave(prijava.ID);
            if (!obj.isTim)
                prijava.Naziv = db.Igraci.Find(obj.Igrac1ID).PrikaznoIme;

            db.Add(prijava);
            db.SaveChanges();

            KreirajPrijavuIgrac(obj, prijava.ID);

            return mapper.Map<SharedModels.Prijave>(prijava);
        }

        public Prijave Delete(int id)
        {
            var prijava = db.Prijave.Include(x=>x.Takmicenje).Where(x=>x.ID == id).FirstOrDefault();

            if (prijava == null)
                throw new UserException("Prijava ne postoji u bazi.");
            if(prijava.Takmicenje.Inicirano)
                throw new UserException("Nemoguće otkazati prijavu. Raspored je već generisan!");

            var sp = db.StanjaPrijave.Where(x => x.PrijavaID == id).SingleOrDefault();
            if (sp != null)
                db.Remove(sp);
            
            var pi = db.PrijaveIgraci.Where(x => x.PrijavaID == id).ToList();
            if (pi != null && pi.Count > 1)
            {
                db.Remove(pi[1]);
            }
            db.Remove(pi[0]);

            db.Remove(prijava);
            db.SaveChanges();
            ObrisiPrijavuIgrac(id);

            return mapper.Map<SharedModels.Prijave>(prijava);
        }

        public List<Prijave> Get(int TakmicenjeID)
        {
            var takmicenje = db.Takmicenja.Find(TakmicenjeID);

            if (takmicenje == null)
                throw new UserException("Takmicenje ne postoji u bazi.");

            var prijave = db.Prijave.Where(x => x.TakmicenjeID == TakmicenjeID).ToList();

            return mapper.Map<List<SharedModels.Prijave>>(prijave);
        }

        public Prijave GetByID(int id)
        {
            var prijava = db.Prijave.Find(id);

            if (prijava == null)
                throw new UserException("Prijava ne postoji u bazi.");

            var p = mapper.Map<SharedModels.Prijave>(prijava);
            var pi = db.PrijaveIgraci.Where(d => d.PrijavaID == id).Select(d => d.IgracID).ToList();
            p.Igrac1ID = pi[0];
            p.Igrac2ID = pi.Count > 1 ? pi[1] : -1;

            return p;
        }


        //*****************************************************
        //              POMOCNE FUNKCIJE
        //*****************************************************
        private void ValidirajDodavanje(int takmicenjeId, PrijavaInsert obj)
        {
            var exception = new UserException();

            var takmicenje = db.Takmicenja.Where(x => x.ID == takmicenjeId).Include(x => x.Vrsta).SingleOrDefault();
            if (takmicenje == null)
                exception.AddError("", "Takmicenje ne postoji u bazi.");
            
            var igrac1 = db.Igraci.Find(obj.Igrac1ID);
            var igrac2 = db.Igraci.Find(obj.Igrac2ID);

            if (igrac1 == null || (takmicenje != null && takmicenje.Vrsta.Naziv == "Double" && igrac2 == null))
                exception.AddError("", "Igrac ne postoji u bazi.");


            if (takmicenje.RokZavrsetkaPrijave >= DateTime.Now)
            {
                var pi = db.PrijaveIgraci.Where(p => p.Prijava.TakmicenjeID == takmicenjeId && p.IgracID == obj.Igrac1ID).SingleOrDefault();

                if (pi != null)
                    exception.AddError("", "Igrač je već prijavljen na takmičenje.");

                if (obj.Igrac1ID == null)
                    exception.AddError("", "Polje igrač1 je obavezno.");

                if (obj.isTim)
                {
                    var pi2 = db.PrijaveIgraci.Where(p => p.Prijava.TakmicenjeID == takmicenjeId && p.IgracID == obj.Igrac2ID).SingleOrDefault();

                    if (pi2 != null)
                        exception.AddError("", "Igrač je već prijavljen na takmičenje.");

                    if (obj.Igrac2ID == null)
                        exception.AddError("", "Polje igrač2 je obavezno.");

                    if (obj.Naziv == null)
                        exception.AddError("", "Polje naziv je obavezno.");

                    var blokListaIgrac2 = db.BlokListe.Where(x => x.IgracID == obj.Igrac2ID && x.TakmicenjeID == takmicenjeId).SingleOrDefault();
                    if (blokListaIgrac2 != null)
                        exception.AddError("", "Ovaj igrač je blokiran na ovom takmičenju.");
                }

                if (obj.Igrac1ID == obj.Igrac2ID && obj.Igrac2ID != null)
                    exception.AddError("", "Ne možete dodati istog igrača kao saigrača.");

                var blokListaIgrac1 = db.BlokListe.Where(x => x.IgracID == obj.Igrac1ID && x.TakmicenjeID == takmicenjeId).SingleOrDefault();

                if (blokListaIgrac1 != null)
                    exception.AddError("", "Blokirani ste na ovom takmičenju.");

                var naziv = obj.isTim ? obj.Naziv : igrac1.PrikaznoIme;

                if (db.Prijave.Where(x => x.TakmicenjeID == takmicenjeId && x.Naziv == naziv).SingleOrDefault() != null)
                    exception.AddError("", "Ime je zauzeto.");

                if (exception.Errori.Count > 0)
                    throw exception;
            }
        }
        private void KreirajPrijavuIgrac(PrijavaInsert prijava, int id)
        {

            var prijava_Igrac1 = new Database.DTOs.Prijava_igrac
            {
                IgracID = prijava.Igrac1ID ?? default(int),
                PrijavaID = id
            };

            db.Add(prijava_Igrac1);

            if (prijava.isTim)
            {
                var prijava_Igrac2 = new Database.DTOs.Prijava_igrac
                {
                    IgracID = prijava.Igrac2ID ?? default(int),
                    PrijavaID = id
                };
                db.Add(prijava_Igrac2);
            }
            db.SaveChanges();
        }
        private void ObrisiPrijavuIgrac(int id)
        {
            var prijaveIgraci = db.PrijaveIgraci.Where(x => x.PrijavaID == id).ToList();
            foreach (var item in prijaveIgraci)
            {
                db.Remove(item);
                db.SaveChanges();
            }
        }
    }
}
