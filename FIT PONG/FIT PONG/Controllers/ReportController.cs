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
                                 Ako ce igdje doci do greske onda ce ovdje,nije problem uhvatiti exception,problem je izbrisati
                                  fajlove koji su se dodali(npr padne baza,postane offline server ili ne znam ni ja sta sve moze krenuti
                                  po zlu,marfi kaze ako moze onda i hoce)i zatraziti ponovni unos
                                 */
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