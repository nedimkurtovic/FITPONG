using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.Database.DTOs;
using FIT_PONG.Services.BL;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace FIT_PONG.Services.Services
{
    public class TakmicenjeService:ITakmicenjeService
    {
        private readonly MyDb db;
        private readonly Evidentor evidentor;
        private readonly InitTakmicenja initTakmicenja;
        private readonly TakmicenjeValidator validator;
        private readonly IMapper mapko;

        public TakmicenjeService(MyDb _db, Evidentor _evidentor, InitTakmicenja _initTakmicenja,
            TakmicenjeValidator _validator, IMapper _mapko)
        {
            db = _db;
            evidentor = _evidentor;
            initTakmicenja = _initTakmicenja;
            validator = _validator;
            mapko = _mapko;
        }

        public Takmicenja Add(TakmicenjaInsert objekat, string KreatorUsername)
        {
            
            ValidirajAddTakmicenja(objekat);
            var novo = mapko.Map<Takmicenje>(objekat);
            using (var transakcija = db.Database.BeginTransaction())//sigurnost u opasnim situacijama 
            {
                try
                {
                    //evo belaja, treba skontati mozda da se posalje autorizacijski header pa da se izvuce username iz njega
                    var idUser = db.Users.Where(x => x.UserName == KreatorUsername).FirstOrDefault();
                    novo.KreatorID = idUser.Id;
                    Feed TakmicenjeFeed = new Feed
                    {
                        Naziv = novo.Naziv + " feed",
                        DatumModifikacije = DateTime.Now
                    };
                    db.Feeds.Add(TakmicenjeFeed);
                    db.SaveChanges();
                    novo.FeedID = TakmicenjeFeed.ID;

                    db.Add(novo);
                    db.SaveChanges();

                    //dobaviti igrace iz regexa
                    if (objekat.RucniOdabir)
                    {
                        validator._listaIgraca = db.Igraci.ToList();
                        List<Igrac> svi = validator.GetListaRucnihIgraca(objekat.RucnoOdabraniIgraci);
                        foreach (Igrac i in svi)
                        {
                            Prijava novaPrijava = new Prijava
                            {
                                DatumPrijave = DateTime.Now,
                                isTim = false,
                                Naziv = i.PrikaznoIme,
                                TakmicenjeID = novo.ID
                            };

                            novaPrijava.StanjePrijave = new Stanje_Prijave(novaPrijava.ID);
                            db.Prijave.Add(novaPrijava);
                            db.SaveChanges();

                            Prijava_igrac PrijavaIgracPodatak = new Prijava_igrac
                            {
                                IgracID = i.ID,
                                PrijavaID = novaPrijava.ID
                            };
                            db.PrijaveIgraci.Add(PrijavaIgracPodatak);
                            db.SaveChanges();
                        }
                    }
                    transakcija.Commit();
                    return mapko.Map<Takmicenja>(novo);
                }
                catch (Exception)
                {
                    transakcija.Rollback();
                    throw new UserException("Greška prilikom spašavanja u bazu");
                }
            }
        }

        public Takmicenja Delete(int id)
        {
            // dobro razmisliti da li ce se ovo implementirati, skupa je malo operacija, ili mozda ograniciti samo
            // da se moze obrisati ako nije takmicenje inicirano, tj neko koristio rucni unos ofo ono pa skonto 
            //da je zajebo pa treba ponovo(vidis treba skontat dugme za kreatora, dodaj jos igraca ili nesto)
            Takmicenje obj = db.Takmicenja.Include(x => x.Feed).Include(x => x.Prijave).Where(c => c.ID == id).SingleOrDefault();
            if (obj == null)
            {
                throw new UserException("Takmicenje ne postoji");
            }
            var temp = mapko.Map<Takmicenja>(obj);
            using (var transakcija = db.Database.BeginTransaction())
            {
                try
                {
                    List<FeedObjava> feedObjavas = db.FeedsObjave.Where(x => x.FeedID == obj.FeedID).ToList();
                    foreach (FeedObjava fo in feedObjavas)
                    {
                        List<Objava> objave = db.Objave.Where(x => x.ID == fo.ObjavaID).ToList();
                        db.Objave.RemoveRange(objave);
                        db.FeedsObjave.Remove(fo);
                    }

                    db.Feeds.Remove(obj.Feed);

                    foreach (Prijava i in obj.Prijave)
                    {
                        List<Prijava_igrac> lista = db.PrijaveIgraci.Where(x => x.PrijavaID == i.ID).ToList();
                        List<Stanje_Prijave> stanjaprijave = db.StanjaPrijave.Where(x => x.PrijavaID == i.ID).ToList();
                        db.RemoveRange(lista);
                        db.RemoveRange(stanjaprijave);
                        db.Prijave.Remove(i);
                    }
                    db.Takmicenja.Remove(obj);
                    db.SaveChanges();
                    transakcija.Commit();
                    return temp;

                }
                catch (DbUpdateException)
                {
                    transakcija.Rollback();
                    throw new UserException("Greška prilikom spašavanja u bazu");
                }
            }

        }

        public List<Takmicenja> Get(TakmicenjeSearch obj)
        {
            var qry = db.Takmicenja.AsQueryable();
            if (!string.IsNullOrWhiteSpace(obj.Naziv))
                qry.Where(x => x.Naziv.StartsWith(obj.Naziv));
            
            var TakmicenjaPovratni = qry.ToList();
            return mapko.Map<List<SharedModels.Takmicenja>>(TakmicenjaPovratni);
        }

        public Takmicenja GetByID(int id)
        {
            var obj = db.Takmicenja.Where(x => x.ID == id).FirstOrDefault();
            if(obj == null)
            {
                throw new UserException("Takmicenje ne postoji");
            }
            return mapko.Map<SharedModels.Takmicenja>(obj);
        }

        public Takmicenja Initialize(int id)
        {
            var obj = db.Takmicenja
                .Include(tak => tak.Kategorija)
                .Include(tak => tak.Sistem)
                .Include(tak => tak.Vrsta)
                .Include(tak => tak.Status)
                .Include(tak => tak.Feed)
                .Include(tak => tak.Bracketi)
                .Include(tak => tak.Prijave).SingleOrDefault(y => y.ID == id);
            if (obj == null)
            {
                throw new UserException("Takmicenje ne postoji");
            }
            ValidirajInitTakmicenja(obj);
            try
            {
                initTakmicenja.GenerisiRaspored(obj);
            }
            catch (Exception) // mislim da samo moze biti dbupdate exception ali hajd neka exception
            {
                throw new Exception("Došlo je do greške prilikom inicijalizovanja takmičenja");
            }
            var objPovratni = db.Takmicenja.Where(x => x.ID == id);
            return mapko.Map<Takmicenja>(objPovratni);
        }
        public Takmicenja Update(int id, TakmicenjaUpdate obj)
        {
            var objBaza = db.Takmicenja.Where(x => x.ID == id).FirstOrDefault();
            if (objBaza == null)// neka ovog dijela ovdje, jer kad ga premjestim u validirajUpdatetakmicenja, onda 
                //cu dole pomjeriti ovu prethodnu i naredne 3 linije koda, i onda cu opet morat ako prodje ta funkcija
                //opet ovdje dobavljati objekat baze
            {
                throw new UserException("Takmicenje ne postoji");
            }
            //ako nije inicirano mozes mijenjati odredjene atribute, dodati korisniku mogucnost
            //mijenjanja/ dodavanja / uklanjanja novih/starih igraca?
            ValidirajUpdateTakmicenja(obj, id, objBaza);
            mapko.Map(objBaza, obj);
            db.SaveChanges();
            return mapko.Map<Takmicenja>(objBaza);
        }
        //=============================ZONA VALIDACIJE ISPOD, NASTAVITI S PAŽNJOM !==============================
        #region Validacija
        private bool ValidirajAddTakmicenja(TakmicenjaInsert obj)
        {
            var igraci = db.Igraci.ToList();
            var listaTakmicenja = db.Takmicenja.Select(x => x.Naziv).ToList();
            //ako ce biti prbolema onda ce biti kod provera gdje su hardkodirane 
            //vrijednosti(unutar ovog ove funkcije u validatoru), vrstaID 
            var listaErrora = validator.VratiListuErroraAkcijaDodaj(obj, listaTakmicenja, igraci);
            RegulisiListuErrora(listaErrora);
            return true;
        }
        private bool ValidirajInitTakmicenja(Takmicenje obj)
        {
            var listaErrora = initTakmicenja.VratiListuErrora(obj);
            RegulisiListuErrora(listaErrora);
            return true;

        }
        private bool ValidirajUpdateTakmicenja(TakmicenjaUpdate obj, int _takmicenjeid, Takmicenje objBaza)
        {

            var listaTakmicenja = db.Takmicenja.ToList();
            var listaErrora = validator.VratiListuErroraAkcijaEdit(obj, _takmicenjeid,listaTakmicenja,objBaza);
            RegulisiListuErrora(listaErrora);
            return true;
        }
        private bool ValidirajDeleteTakmicenja(Takmicenje bazaObj)
        {
            var listaErrora = new List<(string key, string msg)>();
            if (bazaObj.Inicirano)
                listaErrora.Add((nameof(bazaObj.Inicirano), "Takmičenje se može obrisati samo ako nije inicirano"));
            RegulisiListuErrora(listaErrora);
            return true;
        }
        //ideja za ime -> private bool CistaListaErrora, to bi genijalno bilo, raja ce poludit kad vidi
        private void RegulisiListuErrora(List<(string key, string msg)> listaErrora)
        {
            if (listaErrora.Count() > 0)
            {
                UserException ex = new UserException();
                foreach ((string key, string msg) in listaErrora)
                    ex.AddError(key, msg);
                throw ex;
            }
        }
#endregion
    }
}
