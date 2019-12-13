using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;
using FIT_PONG.ViewModels.ReportVMs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace FIT_PONG.Controllers
{
    public class ReportController : Controller
    {
        private readonly MyDb db;
        private readonly IWebHostEnvironment _host;

        public ReportController(MyDb instanca,IWebHostEnvironment _webhost)
        {
            db = instanca;
            _host = _webhost;
        }
        public IActionResult Dodaj()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Dodaj(ReportUniversalVM ReportObj)
        {
            if(ModelState.IsValid)
            {
                if (!SamoSlike(ReportObj.Prilozi))
                {
                    ModelState.AddModelError(nameof(ReportObj.Prilozi), "Mozete samo slike upload");
                }
                else
                {
                    using (var transakcija = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //otkriti nacin slanja mejla administratoru ako uspjesno prodje report
                            Report noviReport = new Report
                            {
                                Naslov = ReportObj.Naslov,
                                Email = ReportObj.Email,
                                Sadrzaj = ReportObj.Sadrzaj,
                                DatumKreiranja = DateTime.Now,
                                Prilozi = new List<Attachment>()
                            };
                            db.Reports.Add(noviReport);
                            db.SaveChanges();
                            foreach (IFormFile x in ReportObj.Prilozi)
                            {
                                /*
                                dakle nakon kracih probavanja ustanovljeno je da c# ima problema sqa brisanjem fajlova,neki govore da je do 
                                garbage collectora neki da je do samog procesa dodavanja problem(ostavi fajl otvorenim ili nesto) bilo kako bilo 
                                Konkretno gdje je ovdje problem : Dakle u slucaju da je korisnik dodao novi report i slike su sve prosla validacija,
                                i dodaju se slike u bazu a i pisu u reports folder i nekim cudom nakon npr prve slike pukne ili iz nekog razloga ne moze
                                dodati u bazu attachment,nije problem postoji nas catch blok koji kaze transakcija.Rollback i svi sretni mi vratimo usera
                                opet na dodavanje novog reporta zamolimo da ponovi unos i to je to,however,
                                svi fajlovi koje je korisnik dodao ostaju u nasem folderu kako trenutno stvari stoje,i to je taj mali problem sto ce se pojaviti
                                junk vrijednosti vremenom
                                 */

                                 //ova linija koda ce kreirati reports folder u wwwroot folderu gdje god da je hostana app,u slucaju da vec postoji folder
                                 //samo ce vratiti informacije o njemu 
                                Directory.CreateDirectory(Path.Combine(_host.WebRootPath, "reports").ToString());
                                string ImeFajla = Guid.NewGuid().ToString() + "_" + x.FileName;
                                string PathSpremanja = Path.Combine(_host.WebRootPath, "reports", ImeFajla);
                                x.CopyTo(new FileStream(PathSpremanja, FileMode.Create));
                                Attachment Attachmentnovi = new Attachment
                                {
                                    DatumUnosa = DateTime.Now,
                                    Path = PathSpremanja
                                };
                                db.Attachments.Add(Attachmentnovi);
                                db.SaveChanges();
                                noviReport.Prilozi.Add(Attachmentnovi);
                                db.SaveChanges();
                                
                            }

                            transakcija.Commit();
                            return Redirect("/Home/Index");
                        }
                        catch(DbUpdateException err)
                        {
                            transakcija.Rollback();
                            ModelState.AddModelError("", "Nesto je krenulo po zlu prilikom spasavanja u bazu,ponovite unos");
                        }
                    }
                }
            }
            return View(ReportObj);
        }
        public bool SamoSlike(List<IFormFile> prilozi)
        {
            if (prilozi.Count() > 0)
            {
                foreach (IFormFile x in prilozi)
                {
                    if (!x.ContentType.Contains("image"))
                        return false;
                }
            }
            if (!prilozi[0].ContentType.Contains("image"))
                return false;
            return true;
        }
    }
}