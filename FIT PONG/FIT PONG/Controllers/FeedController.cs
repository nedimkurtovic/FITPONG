using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIT_PONG.Models;
using FIT_PONG.ViewModels.FeedVMs;
using FIT_PONG.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FIT_PONG.Controllers
{
    public class FeedController : Controller
    {
        /*
         TODOs::
         1.Implementirati migraciju da takmicenje ima svoj feed napomena :
         feed se automatski kreira
         prilikom kreiranja takmicenja,ne mora se pozivati akcija drugog kontrolera,dovoljno je napraviti feed objekat unutar
         prilikom kreiranja takmicenja

         */

        private readonly MyDb db;
        public FeedController(MyDb instanca)
        {
            db = instanca;
        }
        public IActionResult Index()
        {
            List < DisplayFeedVM > feeds = db.Feeds.Select(s => new DisplayFeedVM
            {
                ID = s.ID,
                naziv = s.Naziv,
                DatumModifikacije = s.DatumModifikacije
            }).ToList();
            ViewBag.feeds = feeds;
            return View();
        }
        [HttpPost]
        public IActionResult Dodaj(CreateFeedVM novi)
        {
            if(PostojiIsti(novi.Naziv))
            {
                return View("Postoji");
            }
            if(ModelState.IsValid)
            {
                Feed obj = new Feed
                {
                    Naziv = novi.Naziv,
                    DatumModifikacije = DateTime.Now
                };
                db.Feeds.Add(obj);
                db.SaveChanges();
                return Redirect("/Feed/Index");
            }
            return View(novi);
        }
        public IActionResult Dodaj()
        {
            ViewBag.takmicenja = db.Takmicenja.Select(s=> new ComboBoxVM
            {
                ID= s.ID,
                Opis = s.Naziv
            }).ToList();
            return View();
        }
        public IActionResult Prikaz(int id)
        {
            Feed x = db.Feeds.Find(id);
            if(x != null)
            {
                DisplayFeedVM obj = new DisplayFeedVM
                {
                    ID = x.ID,
                    naziv = x.Naziv,
                    DatumModifikacije = x.DatumModifikacije
                };
                obj.Objave = (from o in db.Objave
                              join fo in db.FeedsObjave
                              on o.ID equals fo.ObjavaID
                              where fo.FeedID == x.ID
                              select fo.Objava).ToList();
                ViewBag.fid = obj;
                return View();
            }
            return Redirect("/Feed/Neuspjeh");
        }
        [HttpPost]
        public IActionResult Edit(Feed novi)
        {
            if(PostojiIsti(novi.Naziv))
            {
                return View("Postoji");
            }

            if(ModelState.IsValid)
            {
                Feed obj = db.Feeds.Find(novi.ID);
                obj.Naziv = novi.Naziv;
                obj.DatumModifikacije = DateTime.Now;
                db.Update(obj);
                db.SaveChanges();
                return Redirect("/Feed/Prikaz/"+obj.ID);
            }
            return View("Neuspjeh");
        }
        public IActionResult Edit(int id)
        {
            Feed obj = db.Feeds.Find(id);
            if(obj != null)
            {
                return View(obj);
            }
            return View("/Feed");
        }
        private bool PostojiIsti(string naziv)
        {
            if(db.Feeds.Where(s=> s.Naziv == naziv).Count() > 0)
            {
                return true;
            }
            return false;
        }
        public IActionResult Obrisi(int id)
        {
            Feed obj = db.Feeds.Find(id);
            if(obj != null)
            {
                DisplayFeedVM prikaz = new DisplayFeedVM { ID = obj.ID, naziv = obj.Naziv };
                ViewBag.feed = prikaz;
                return View();
            }
            return Redirect("/Feed/Neuspjeh");
        }
        public IActionResult PotvrdaBrisanja(int id)
        {
            Feed obj = db.Feeds.Find(id);
            if(obj != null)
            {
                db.Remove(obj);
                db.SaveChanges();
                return Redirect("Index");
            }
            return Redirect("Neuspjeh");
        }
        public IActionResult Uspjeh()
        {
            return View();
        }
        public IActionResult Neuspjeh()
        {
            return View();
        }
        public IActionResult Postoji()
        {
            return View();
        }
    }
}