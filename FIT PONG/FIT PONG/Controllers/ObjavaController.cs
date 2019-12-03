using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIT_PONG.ViewModels.ObjavaVMS;
using FIT_PONG.Models;
using Microsoft.EntityFrameworkCore;

namespace FIT_PONG.Controllers
{
    public class ObjavaController : Controller
    {
        public IActionResult Prikaz(int ? id)
        {
            if (id == null)
                return Redirect("/Objava/Neuspjeh");
            MyDb db = new MyDb();
            Objava obj = db.Objave.Find(id);
            if(obj != null)
            {
                ObjavaPrikazVM novi = new ObjavaPrikazVM
                {
                    ID = obj.ID,
                    Naziv = obj.Naziv,
                    Content = obj.Content,
                    DatumIzmjene = obj.DatumIzmjene,
                    DatumKreiranja = obj.DatumKreiranja
                };
                return View(novi);
            }
            return Redirect("/Objava/Neuspjeh");
        }
        public IActionResult Dodaj(int ID)
        {
            return View(new ObjavaUnosVM {FeedID = ID });
        }
        [HttpPost]
        public IActionResult Dodaj(ObjavaUnosVM obj)
        {
            if (ModelState.IsValid)
            {
                MyDb db = new MyDb();
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
                        db.Dispose();
                        return Redirect("/Feed/Prikaz/" + FidObjekat.ID);
                    }
                    catch(DbUpdateException er)
                    {
                        ModelState.AddModelError("","PRoblem u kreiranju");
                    }
                }
            }

            return View(obj);
        }
        [HttpPost]
        public IActionResult Edit(ObjavaUnosVM objekat)
        {
            if(ModelState.IsValid)
            {
                MyDb db = new MyDb();
                Objava obj = db.Objave.Find(objekat.ID);
                if(obj != null)
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
                    catch(DbUpdateException er)
                    {
                        ModelState.AddModelError("", "Greska prilikom updatea provjerite info" + er.Message);
                    }
                }
            }
            return View(objekat);
        }

        public IActionResult Edit(int id)
        {
            MyDb db = new MyDb();
            Objava obj = db.Objave.Find(id);
            db.Dispose();
            if(obj != null)
            {
                ObjavaUnosVM objVM = new ObjavaUnosVM
                {
                    ID = obj.ID,
                    Content = obj.Content,
                    Naziv = obj.Naziv
                };
                return View(objVM);
            }
            return Redirect("Neuspjeh");
        }
        public IActionResult Obrisi(int ? id)
        {
            if(id==null)
            {
                return Redirect("Neuspjeh");
            }
            MyDb db = new MyDb();
            Objava obj = db.Objave.Find(id);
            if(obj != null)
            {
                ObjavaPrikazVM objekat = new ObjavaPrikazVM
                {
                    ID = obj.ID,
                    Content = obj.Content,
                    Naziv = obj.Naziv,
                    DatumIzmjene = obj.DatumIzmjene,
                    DatumKreiranja = obj.DatumKreiranja
                };
                return View(objekat);
            }
            return Redirect("Neuspjeh");

        }
        public IActionResult PotvrdaBrisanja(int id)
        {
            MyDb db = new MyDb();
            Objava obj = db.Objave.Find(id);
            if(obj != null)
            {
                try
                {
                    FeedObjava FidObj = db.FeedsObjave.Where(x => x.ObjavaID == obj.ID).FirstOrDefault();
                    if (FidObj != null)
                        db.FeedsObjave.Remove(FidObj);
                    db.Objave.Remove(obj);
                    db.SaveChanges();
                    db.Dispose();
                    return Redirect("/Objava/Uspjeh");
                }
                catch(DbUpdateException er)
                {
                }
            }
            db.Dispose();
            return Redirect("/Objava/Neuspjeh");
        }
        public IActionResult Neuspjeh()
        {
            return View();
        }
        public IActionResult Uspjeh()
        {
            return View();
        }
    }
}