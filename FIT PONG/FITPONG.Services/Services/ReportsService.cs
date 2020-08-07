using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.Database.DTOs;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Reports;
using FIT_PONG.WebAPI.Services.Bazni;
using FIT_PONG.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using System.Text;
using FIT_PONG.Services.BL;
using System.Linq;
using System.Drawing;

namespace FIT_PONG.Services.Services
{
    public class ReportsService : BaseService<SharedModels.Reports, Database.DTOs.Report, object>, IReportsService
    {
        private readonly iEmailServis emailServis;

        public ReportsService(MyDb _context, IMapper _mapko, iEmailServis _emailServis):base(_context,_mapko)
        {
            emailServis = _emailServis;
        }
        
        public List<SharedModels.Reports> Get(ReportsSearch obj)
        {
            var query = db.Reports.Include(x=>x.Prilozi).AsQueryable();
            if (!String.IsNullOrWhiteSpace(obj.Naslov))
                query = query.Where(x => x.Naslov.Contains(obj.Naslov));
            if (obj.Datum != null)
                query = query.Where(x => x.DatumKreiranja.Date == obj.Datum.GetValueOrDefault().Date);

            var rezultat = query.ToList();
            var povratni = mapko.Map<List<Reports>>(rezultat);
            foreach(var i in povratni)
            {
                foreach(var j in i.Prilozi)
                {
                    Fajl fajl = GetFajl(j);
                    i.RawPrilozi.Add(fajl);
                }
            }
            return povratni;
        }



        public SharedModels.Reports Add(ReportsInsert obj , string rootFolder = "content") // razmisliti da se iz httpcontext. izvuce root path aplikacije kako bi se
         //uploadovo fajl tj da se ovoj metodi posalje taj parametar a ne ovako da ovaj nagadja sa ~/reports
         // kad se stavi ~ frajer napravi folder koji se zove ~ i onda u njemu reports, ne valja, ne moze tako, dakle on po defaultu kad ides create
         //directory pravi u ovom svom folderu gdje se pokrece web api, tako da stavit cu content da je folder a u njega reports, pa tamo za igrace
         //ce biti 
        {
            Validiraj(obj);
            using (var transakcija = db.Database.BeginTransaction())
            {
                List<string> bekapPlan = new List<string>();
                try
                {
                    //var bazaObj = mapko.Map<Database.DTOs.Report>(obj);
                    var bazaObj = new Database.DTOs.Report
                    {
                        DatumKreiranja = obj.DatumKreiranja ?? DateTime.Now,
                        Email = obj.Email,
                        Naslov = obj.Naslov,
                        Sadrzaj = obj.Sadrzaj
                    };
                    bazaObj.Prilozi = new List<Database.DTOs.Attachment>();
                    db.Reports.Add(bazaObj);
                    db.SaveChanges();
                    if (obj.Prilozi != null)
                    {
                        foreach (Fajl i in obj.Prilozi)
                        {
                            using (MemoryStream ms = new MemoryStream(i.BinarniZapis))
                            {
                                //valja istraziti tj kako spremiti u zajednicki folder za webapp i webapi
                                Directory.CreateDirectory(Path.Combine(rootFolder, "reports"));
                                string ImeFajla = Guid.NewGuid().ToString() + "_" + i.Naziv;
                                string pathSpremanja = Path.Combine(rootFolder, "reports", ImeFajla);
                                using (FileStream strim = new FileStream(pathSpremanja, FileMode.Create))
                                    ms.CopyTo(strim);
                                bekapPlan.Add(pathSpremanja);
                                Attachment noviAttachment = new Attachment { DatumUnosa = DateTime.Now, Path = pathSpremanja };
                                db.Attachments.Add(noviAttachment);
                                bazaObj.Prilozi.Add(noviAttachment);
                            }
                        }
                        db.SaveChanges();
                        transakcija.Commit();
                        //ovdje treba pozvati email servis i obavijestiti adminsitratora o novom reportu
                        emailServis.PosaljiMejlReport(bazaObj);
                        var povratni = new SharedModels.Reports
                        {
                            ID = bazaObj.ID,
                            Email = bazaObj.Email,
                            DatumKreiranja = bazaObj.DatumKreiranja,
                            Naslov = bazaObj.Naslov,
                            Sadrzaj = bazaObj.Sadrzaj
                        };
                        //mapko ovdje ne moze osvojit,  zbog problema Prilozi su u klasi database.dtos.reports
                        //tipa list<attachment> dok su u sharedmodels.requests.reportsinsert zbog potrebe prenosa binarnih fajlova u formi list<Fajl>
                        //tako da ovdje svakako ne treba slat fajlove, mada se mogu 
                        return povratni;
                    }
                }
                catch (DbUpdateException)
                {
                    transakcija.Rollback();
                    foreach (string i in bekapPlan)
                        if (System.IO.File.Exists(i))
                            System.IO.File.Delete(i);
                }
                UserException ex = new UserException();
                ex.AddError("error", "Došlo je do greške prilikom pohrane fajlova, pokušajte ponovo");
                throw ex;
            }

        }
        private bool Validiraj(ReportsInsert obj)
        {
            UserException ex = new UserException();
            if (!OdgovarajuciFajlovi(obj.Prilozi))
                ex.AddError(nameof(obj.Prilozi), "Možete uploadovati samo slike");
            if (ex.Errori.Count > 0)
                throw ex;
            return true;
        }

        private bool OdgovarajuciFajlovi(List<Fajl> prilozi)
        {
            if (prilozi == null || prilozi.Count == 0)
                return true;
            foreach (Fajl i in prilozi)
            {
                int poz = i.Naziv.LastIndexOf(".") + 1; // + 1 zbog ovih dole uslova, ili sam mogao samo dodati tacku u uslove dole svejedno
                string ekstenzija = i.Naziv.Substring(poz);
                if (ekstenzija != "png" && ekstenzija != "jpg")
                    return false;
            }
            return true;
        }
        private Fajl GetFajl(Attachmenti j)
        {
            var bajtovi = File.ReadAllBytes(j.Path);
            int pozicija = j.Path.LastIndexOf("/");
            var naziv = j.Path.Substring(pozicija);
            var povratniFajl = new Fajl();
            povratniFajl.BinarniZapis = bajtovi;
            povratniFajl.Naziv = naziv;
            return povratniFajl;
        }
    }
}
