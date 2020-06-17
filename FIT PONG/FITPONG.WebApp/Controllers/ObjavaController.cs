using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIT_PONG.ViewModels.ObjavaVMS;
using FIT_PONG.Services;
using Microsoft.EntityFrameworkCore;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.Controllers
{
    public class ObjavaController : Controller
    {
        //readonly je nesto slicno constu ali nije ni blizu,const se moze samo pri deklaraciji incijalizovat dok se readonly
        //varijabla moze incijalizovati samo u konstruktoru i nigdje vise
        private readonly FIT_PONG.Database.MyDb db;
        public ObjavaController(FIT_PONG.Database.MyDb instanca)
        {
            db = instanca;
        }
        public IActionResult Prikaz(int ? id)
        {
            if (id == null)
                return Redirect("/Objava/Neuspjeh");
            Objava obj = db.Objave.Find(id);
            if(obj != null)
            {
                ObjavaPrikazVM novi = new ObjavaPrikazVM
                {
                    ID = obj.ID,
                    Naziv = obj.Naziv,
                    Content = obj.Content,
                    DatumIzmjene = obj.DatumIzmjene,
                    DatumKreiranja = obj.DatumKreiranja,
                    FeedID = db.FeedsObjave.Where(x => x.ObjavaID == obj.ID).Select(s => s.FeedID).SingleOrDefault()
                };
                ViewBag.autorizovan = AutorizovanZaAkciju(obj.ID, HttpContext.User.Identity.Name);
                return PartialView(novi);
            }
            return Redirect("/Objava/Neuspjeh");
        }
        public IActionResult Dodaj(int ID)
        {
            if (!AutorizovanZaAkcijuDodaj(ID, HttpContext.User.Identity.Name))
                return VratiNijeAutorizovan();
            return PartialView(new ObjavaUnosVM {FeedID = ID });
        }
        [HttpPost]
        public IActionResult Dodaj(ObjavaUnosVM obj)
        {
            if (ModelState.IsValid)
            {
                Feed FidObjekat = db.Feeds.Find(obj.FeedID);//mora pripadati objava nekom fidu inace nista
                if (FidObjekat != null)
                {
                    try
                    {
                        Objava nova = new Objava
                        {
                            Naziv = obj.Naziv,
                            Content = obj.Content,
                            DatumKreiranja = DateTime.Now,
                            DatumIzmjene = DateTime.Now
                        };
                        db.Objave.Add(nova);
                        db.SaveChanges();
                        FeedObjava novaFidObjava = new FeedObjava
                        {
                            FeedID = FidObjekat.ID,
                            ObjavaID = nova.ID
                        };
                        db.FeedsObjave.Add(novaFidObjava);
                        db.SaveChanges();
                        return Redirect("/Feed/Prikaz/" + FidObjekat.ID);
                    }
                    catch(DbUpdateException er)
                    {
                        ModelState.AddModelError("","Problem u kreiranju");
                    }
                }
            }
            
            return PartialView(obj);
        }
       

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Objava obj = db.Objave.Find(id);
            if(obj != null)
            {
                if (!AutorizovanZaAkciju(id, HttpContext.User.Identity.Name))
                    return VratiNijeAutorizovan();
                ObjavaUnosVM objVM = new ObjavaUnosVM
                {
                    ID = obj.ID,
                    Content = obj.Content,
                    Naziv = obj.Naziv
                };
                return PartialView(objVM);
            }
            return Redirect("Neuspjeh");
        }
        [HttpPost]
        public IActionResult Edit(ObjavaUnosVM objekat)
        {
            if (ModelState.IsValid)
            {
                Objava obj = db.Objave.Find(objekat.ID);
                if (obj != null)
                {
                    try
                    {
                        obj.Content = objekat.Content;
                        obj.Naziv = objekat.Naziv;
                        obj.DatumIzmjene = DateTime.Now;
                        db.Update(obj);
                        db.SaveChanges();
                        return Redirect("/Objava/Prikaz/" + obj.ID);
                    }
                    catch (DbUpdateException er)
                    {
                        ModelState.AddModelError("", "Greska prilikom updatea provjerite info" + er.Message);
                    }
                }
            }
            return PartialView(objekat);
        }

        public IActionResult Obrisi(int ? id)
        {
            if(id==null)
            {
                return Redirect("Neuspjeh");
            }
            Objava obj = db.Objave.Find(id);
            if(obj != null)
            {
                if (!AutorizovanZaAkciju(id.GetValueOrDefault(), HttpContext.User.Identity.Name))
                    return VratiNijeAutorizovan();
                ObjavaPrikazVM objekat = new ObjavaPrikazVM
                {
                    ID = obj.ID,
                    Content = obj.Content,
                    Naziv = obj.Naziv,
                    DatumIzmjene = obj.DatumIzmjene,
                    DatumKreiranja = obj.DatumKreiranja
                };
                return PartialView(objekat);
            }
            return Redirect("Neuspjeh");

        }
        public IActionResult PotvrdaBrisanja(int id)
        {
            Objava obj = db.Objave.Find(id);
            if(obj != null)
            {
                try
                {
                    FeedObjava FidObj = db.FeedsObjave.Where(x => x.ObjavaID == obj.ID).FirstOrDefault();
                    int fidID = FidObj.FeedID;
                    if (FidObj != null)
                        db.FeedsObjave.Remove(FidObj);
                    db.Objave.Remove(obj);
                    db.SaveChanges();
                    return Redirect("/Feed/Prikaz/" + fidID);
                }
                catch(DbUpdateException er)
                {
                }
            }
            return Redirect("/Objava/Neuspjeh");
        }
        public IActionResult Neuspjeh()
        {
            ViewBag.poruka = "Nesto je krenulo po zlu";
            return PartialView();
        }
        public IActionResult Uspjeh()
        {
            return PartialView();
        }
        private bool AutorizovanZaAkciju(int id, string pozivatelj)
        {
            Feed obj = db.FeedsObjave.Include(x=>x.Feed).Where(x => x.ObjavaID == id).Select(x => x.Feed).FirstOrDefault();
            if (obj != null)
            {
                Takmicenje tak = db.Takmicenja.Where(x => x.FeedID == obj.ID).FirstOrDefault();
                if (tak != null)
                {
                    var idUser = db.Users.Where(x => x.UserName == pozivatelj).FirstOrDefault();
                    if (tak.KreatorID == idUser.Id)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }
        private bool AutorizovanZaAkcijuDodaj(int id, string pozivatelj)
        {
            Takmicenje tak = db.Takmicenja.Where(x => x.FeedID == id).FirstOrDefault();
            if (tak != null)
            {
                var idUser = db.Users.Where(x => x.UserName == pozivatelj).FirstOrDefault();
                if (tak.KreatorID == idUser.Id)
                    return true;
                else
                    return false;
            }
            return false;
        }
            private IActionResult VratiNijeAutorizovan()
        {
            ViewBag.poruka = "Niste autorizovani za takvu radnju";
            return PartialView("Neuspjeh");
        }

    }
}