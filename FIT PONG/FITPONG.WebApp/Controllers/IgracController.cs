using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services;
using FIT_PONG.ViewModels.IgracVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.Controllers
{
    public class IgracController : Controller
    {

        private readonly FIT_PONG.Database.MyDb db;
        private readonly IWebHostEnvironment _host;

        public IgracController(FIT_PONG.Database.MyDb context, IWebHostEnvironment host)
        {
            db = context;
            _host = host;
        }

        public IActionResult Index(string searchBy, string search, string sortExpression= "PrikaznoIme", int page=1)
        {
            if (search == null)
                ViewData["prazno"] = "nema igraca";
            var userId = db.Users.Where(d => d.Email == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.userId = userId;
            
            if (searchBy == "JacaRuka")
            {
                List<IgracVM> igraci = db.Igraci.Select(x => new IgracVM
                {
                    ID = x.ID,
                    BrojPosjetaNaProfil = x.BrojPosjetaNaProfil,
                    JacaRuka = x.JacaRuka,
                    PrikaznoIme = x.PrikaznoIme,
                    ProfileImagePath = x.ProfileImagePath,
                    Visina = x.Visina,
                    ELO = x.ELO
                }).Where(x => x.JacaRuka == search || search == null).ToList();
                var igraciPaged = PagingList.Create(igraci, 4, page, sortExpression, "ID");
                ViewBag.igraci = igraci;
                return View(igraciPaged);
            }
            else
            {
                List<IgracVM> igraci = db.Igraci.Select(x => new IgracVM
                {
                    ID = x.ID,
                    BrojPosjetaNaProfil = x.BrojPosjetaNaProfil,
                    JacaRuka = x.JacaRuka,
                    PrikaznoIme = x.PrikaznoIme,
                    ProfileImagePath = x.ProfileImagePath,
                    Visina = x.Visina,
                    ELO = x.ELO
                }).Where(x => x.PrikaznoIme.Contains(search) || search == null).OrderBy(x => x.PrikaznoIme).ToList();
                var igraciPaged = PagingList.Create(igraci, 4, page, sortExpression, "ID");
                ViewBag.igraci = igraci;
                return View(igraciPaged);
            }
            
        }

        public IActionResult PrikazProfila(int id)
        {
            Igrac objIgrac = db.Igraci.Include(d=>d.Grad).SingleOrDefault(x => x.ID == id);
            if (objIgrac == null)
                return View("Greska");
            IgracVM igrac = new IgracVM(objIgrac);
            igrac.statistika = db.Statistike.Where(s => s.IgracID == id && s.AkademskaGodina==DateTime.Now.Year).SingleOrDefault();
            igrac.BrojPostovanja=db.Postovanja.Count(p => p.PostovaniID == id);
            igrac.listaPrijava = (from pi in db.PrijaveIgraci
                                  join pr in db.Prijave on pi.PrijavaID equals pr.ID
                                  where pi.IgracID == id
                                  select new Prijava
                                  {
                                      ID=pr.ID,
                                      Naziv=pr.Naziv,
                                      Takmicenje=pr.Takmicenje
                                  }).ToList();
            ViewBag.userId = db.Users.Where(d => d.Email == User.Identity.Name).FirstOrDefault().Id;
            var userId = db.Users.Where(d => d.Email == User.Identity.Name).FirstOrDefault().Id;
            if (userId != id)
            {
                Igrac i = db.Igraci.Find(id);
                i.BrojPosjetaNaProfil++;
                db.Update(i);
                db.SaveChanges();
            }
            
            return View(igrac);
        }
        
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Dodaj(IgracDodajVM igrac)
        {
            if (ModelState.IsValid)
            {
                if (igrac.ID == null || igrac.Token == "")
                    return RedirectToAction("Neuspjeh", new { poruka = "Igrac se moze kreirati samo prilikom potvrde mejla" });
                if (igrac.PrikaznoIme!=null && !JeLiUnique(igrac.PrikaznoIme))
                {
                    ModelState.AddModelError(nameof(igrac.PrikaznoIme), "Prikazno ime je zauzeto.");
                    return View(igrac);
                }
                //int id=KreirajLoginIUsera();
                Igrac novi = new Igrac
                {
                    ID = igrac.ID.GetValueOrDefault(),
                    BrojPosjetaNaProfil = 0,
                    ELO = 1000,
                    JacaRuka = igrac.JacaRuka,
                    PrikaznoIme = igrac.PrikaznoIme,
                    Visina = igrac.Visina,
                    GradID=igrac.GradId,
                    Spol=igrac.Spol 
                };
                Statistika statistika = new Statistika(novi.ID);

                if (igrac.Slika!=null){
                    if (!igrac.Slika.ContentType.Contains("image"))
                    {
                        ModelState.AddModelError(nameof(igrac.Slika), "Mozete uploadat samo sliku.");
                    }
                    else
                    {
                        string ImeFajla = ProcesSpremanjaSlike(igrac);
                        novi.ProfileImagePath = "~/igraci/"+ImeFajla;
                    }
                }
                else
                {
                    novi.ProfileImagePath = "/profile_picture_default.png";
                }
                var user = db.Users.Where(x => x.Id == novi.ID).AsNoTracking().FirstOrDefault();
                db.Entry(user).State = EntityState.Detached;

                db.Igraci.Add(novi);
                db.Statistike.Add(statistika);
                db.SaveChanges();
                db.ChangeTracker.Entries().ToList(); // visak samo zbog testiranja stavljeno
                return RedirectToAction("PotvrdiMail", "Account", new
                {
                    userid = novi.ID.ToString(),
                    token = igrac.Token
                });
            }
            ViewBag.id = igrac.ID.GetValueOrDefault();
            ViewBag.token = igrac.Token;
            GetGradove();
            return View(igrac);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Dodaj(int id, string token)
        {
            //nedostaje input za grad i spol
            ViewBag.id = id;
            ViewBag.token = token;
            GetGradove();
            return View();
        }

        [HttpGet]
        public IActionResult EditPodatke(int igracID)
        {
            Igrac igrac = db.Igraci.Find(igracID);

            if (igrac == null)
            {
                return View("Greska");
            }
            var userId = db.Users.Where(d => d.Email == User.Identity.Name).FirstOrDefault().Id;

            if (userId != igracID)
                return VratiNijeAutorizovan();


            IgracEditPodatkeVM obj = new IgracEditPodatkeVM
            {
                ID = igrac.ID,
                JacaRuka = igrac.JacaRuka,
                PrikaznoIme = igrac.PrikaznoIme,
                Visina = igrac.Visina,
                ProfileImagePath=igrac.ProfileImagePath,
                TwoFactorEnabled=igrac.TwoFactorEnabled,
                GradId=igrac.GradID
            };
            GetGradove();
            
            return View(obj);
        }

        [HttpPost]
        public IActionResult EditPodatke(IgracEditPodatkeVM obj)
        {
            Igrac igrac = db.Igraci.Find(obj.ID);
            
            
            if (igrac!=null && ModelState.IsValid)
            {
                if (obj.PrikaznoIme!=igrac.PrikaznoIme && !JeLiUnique(obj.PrikaznoIme))
                {
                    ModelState.AddModelError(nameof(obj.PrikaznoIme), "Prikazno ime je zauzeto.");
                    return View(obj);
                }

                igrac.JacaRuka = obj.JacaRuka;
                igrac.Visina= obj.Visina;
                igrac.PrikaznoIme = obj.PrikaznoIme;
                if (obj.GradId != 0)
                    igrac.GradID = obj.GradId;
                db.Update(igrac);
                db.SaveChanges();

                return Redirect("/Igrac/PrikazProfila/" + igrac.ID);
            }
            return View(obj);
        }

        public IActionResult UkloniSliku(int igracID)
        {
            Igrac obj = db.Igraci.Find(igracID);
            if (obj == null)
            {
                return View("Greska");
            }
            var userId = db.Users.Where(d => d.Email == User.Identity.Name).FirstOrDefault().Id;

            if (userId != igracID)
                return VratiNijeAutorizovan();
            if (obj.ProfileImagePath != "/profile_picture_default.png")
            {
                ProcesBrisanjaSlike(obj.ProfileImagePath);
                obj.ProfileImagePath = "/profile_picture_default.png";
                db.SaveChanges();
            }
            return Redirect("/Igrac/PrikazProfila/" + igracID);

        }

        [HttpGet]
        public IActionResult EditSliku(int igracID)
        {
            Igrac igrac = db.Igraci.Find(igracID);

            if (igrac == null)
                return View("Greska");
            var userId = db.Users.Where(d => d.Email == User.Identity.Name).FirstOrDefault().Id;

            if (userId != igracID)
                return VratiNijeAutorizovan();
            IgracEditSlikuVM obj = new IgracEditSlikuVM
            {
                ID = igracID,
                ExistingProfileImagePath=igrac.ProfileImagePath,
                JacaRuka=igrac.JacaRuka,
                BrojPosjetaNaProfil=igrac.BrojPosjetaNaProfil,
                ELO=igrac.ELO,
                PrikaznoIme=igrac.PrikaznoIme,
                Visina=igrac.Visina
            };

            return View(obj);
        }

        [HttpPost]
        public IActionResult EditSliku(IgracEditSlikuVM obj)
        {
            Igrac igrac = db.Igraci.Find(obj.ID);
            if (igrac != null)
            {
                if (obj.Slika != null)
                {
                    if (!obj.Slika.ContentType.Contains("image"))
                    {
                        ModelState.AddModelError(nameof(obj.Slika), "Mozete uploadat samo sliku.");
                        return View(obj);
                    }
                    else
                    {
                        if (obj.ExistingProfileImagePath != null && obj.ExistingProfileImagePath!="/profile_picture_default.png")
                            ProcesBrisanjaSlike(obj.ExistingProfileImagePath);
                        igrac.ProfileImagePath= "~/igraci/" + ProcesSpremanjaSlike(obj);
                    }
                }
                
                db.Update(igrac);
                db.SaveChanges();
                return Redirect("/Igrac/PrikazProfila/" + obj.ID);
            }
            return View("Greska");

        }

        public IActionResult Statistike(int igracID)
        {
            List<Statistika> stats = db.Statistike.Where(s => s.IgracID == igracID).OrderByDescending(s => s.AkademskaGodina).ToList();
            ViewBag.statistike = stats;
            Igrac i = db.Igraci.Find(igracID);
            if (i == null)
                return View("Greska");
            ViewBag.igrac = i.PrikaznoIme;
            return View();
        }

        public IActionResult PostujIgraca(int postovaniID)
        {
            Igrac igrac2 = db.Igraci.Include(d => d.User).Where(d => d.User.Email == User.Identity.Name).SingleOrDefault();
            Igrac igrac1 = db.Igraci.Find(postovaniID);
            int postivalacID = igrac2.ID;
            Postovanje postovanje = db.Postovanja.Where(d => d.PostivalacID == postivalacID && d.PostovaniID == postovaniID).SingleOrDefault();
            if (igrac1 == null || igrac2 == null)
                return View("Greska");
            else if (postovanje != null)
            {
                db.Remove(postovanje);
            }
            else
            {
                Postovanje novo = new Postovanje(postivalacID, postovaniID);
                db.Add(novo);
            }
            db.SaveChanges();
            return Redirect("/Igrac/PrikazProfila/" + postovaniID);

        }
        private string ProcesSpremanjaSlike(IgracDodajVM obj)
        {
            string ImeFajla = null;
            if (obj != null)
            {
                Directory.CreateDirectory(Path.Combine(_host.WebRootPath, "igraci").ToString());
                ImeFajla = Guid.NewGuid().ToString() + "_" + obj.PrikaznoIme + obj.Slika.FileName.Substring(obj.Slika.FileName.IndexOf("."));
                string PathSpremanja = Path.Combine(_host.WebRootPath, "igraci", ImeFajla);
                using (var fileStream = new FileStream(PathSpremanja, FileMode.Create)) 
                {
                    obj.Slika.CopyTo(fileStream);
                }
            }
            return ImeFajla;
        }

        private void ProcesBrisanjaSlike(string putanja)
        {
            string filePutanja = Path.Combine(_host.WebRootPath, putanja.Substring(putanja.IndexOf("/") + 1));
            System.IO.File.Delete(filePutanja);
        } 

        private List<IgracVM> Sort(List<IgracVM> igraci, string sortBy)
        {
            switch (sortBy)
            {
                case "PrikaznoIme":
                    return igraci.OrderBy(x => x.PrikaznoIme).ToList();
                case "EloRaiting":
                    return igraci = igraci.OrderByDescending(x => x.ELO).ToList();
                case "BrojPosjetaNaProfil":
                    return igraci = igraci.OrderByDescending(x => x.BrojPosjetaNaProfil).ToList();
                default:
                    return igraci;
            }
           
        }
        private bool JeLiUnique(string username)
        {
            List<Igrac> igraci = db.Igraci.ToList();
            foreach (var item in igraci)
            {
                if (item.PrikaznoIme == username)
                    return false;
            }
            return true;
        }
        public IActionResult Neuspjeh(string poruka)
        {
            ViewBag.poruka = poruka;
            return View();
        }


        public IActionResult VratiNijeAutorizovan()
        {
            ViewBag.poruka = "Niste autorizovani za takvu radnju";
            return View("Neuspjeh");
        }


        private void GetGradove()
        {
            List<Grad> gradovi = db.Gradovi.ToList();
            List<SelectListItem> gradoviCombo = new List<SelectListItem>();
            foreach (var item in gradovi)
            {
                SelectListItem x = new SelectListItem();
                x.Value = item.ID.ToString();
                x.Text = item.Naziv;
                gradoviCombo.Add(x);
            }
            ViewBag.gradovi = gradoviCombo;
        }

        //funkcija sluzi za generisanje usera i logina koji su potrebni da bi se kreirao igrac,
        //jer igrac preuzima id od usera, a u user posjeduje loginID, tako da je to dvoje neophodno
        //ovo je naravno samo privremeno, dok se ne rijesi microsoft identity
        //samo je tu zbog testiranja funkcionalnosti
        //private int KreirajLoginIUsera()
        //{
        //    Login l = new Login
        //    {
        //        Username = "Username " + BROJAC,
        //        PasswordHash = "PasswordHash " + BROJAC,
        //        PasswordSalt = "PasswordSalt " + BROJAC
        //    };
        //    db.Add(l);
        //    db.SaveChanges();
        //    User u = new User
        //    {
        //        Ime = "Ime - " + BROJAC,
        //        Prezime = "Prezime " + BROJAC,
        //        DatumRodjenja = DateTime.Now,
        //        Email = $"ime{BROJAC}.prezime{BROJAC}@edu.fit.ba",
        //        LoginID = l.ID,
        //        GradID = 21
        //    };
        //    db.Add(u);
        //    db.SaveChanges();
        //    BROJAC++;
        //    return u.ID;
        //}


    }
}