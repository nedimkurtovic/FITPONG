using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.Database.DTOs;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Reports;
using FIT_PONG.WebAPI.Services.Bazni;
using FITPONG.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime;
using System.Text;

namespace FIT_PONG.Services.Services
{
    public class ReportsService : BaseService<SharedModels.Reports, Database.DTOs.Report, object>, IReportsService
    {
        public ReportsService(MyDb _context, IMapper _mapko):base(_context,_mapko)
        {
            
        }

        public Reports Add(ReportsInsert obj)
        {
            throw new NotImplementedException();
        }
        //public bool Validiraj(ReportsInsert obj)
        //{
        //    UserException ex = new UserException();
        //    if (!OdgovarajuciFajlovi(obj.Prilozi))
        //        ex.AddError(nameof(obj.Prilozi), "Možete uploadovati samo slike");
        //    if (ex.Errori.Count > 0)
        //        throw ex;
        //    return true;
        //}

        //private bool OdgovarajuciFajlovi(List<byte[]> prilozi)
        //{
        //    if (prilozi == null || prilozi.Count == 0)
        //        return true;
        //    foreach (byte[] i in prilozi) 
        //    {
        //        //logika
        //        //ukratko izgleda da se ne moze saznati tip fajla ako je byte stream..
        //        //samo da je skontat kako uvalit iformfile umjesto byte streama 
        //    }
        //}

        //public SharedModels.Reports Add(ReportsInsert obj) // razmisliti da se iz httpcontext. izvuce root path aplikacije kako bi se
        //    //uploadovo fajl tj da se ovoj metodi posalje taj parametar a ne ovako da ovaj nagadja sa ~/reports
        //{
        //    Validiraj(obj);
        //    using (var transakcija = db.Database.BeginTransaction())
        //    {
        //        List<string> bekapPlan = new List<string>();
        //        try
        //        {
        //            var bazaObj = mapko.Map<Database.DTOs.Report>(obj);
        //            bazaObj.Prilozi = new List<Database.DTOs.Attachment>();
        //            db.Reports.Add(bazaObj);
        //            db.SaveChanges();
        //            if(obj.Prilozi != null)
        //            {
        //                foreach(byte[] i in obj.Prilozi)
        //                {
        //                    using(MemoryStream ms = new MemoryStream(i))
        //                    {
        //                        Directory.CreateDirectory("~/reports");
        //                        string ImeFajla = Guid.NewGuid().ToString();
        //                        string pathSpremanja = Path.Combine("~/reports", ImeFajla);
        //                        using (FileStream strim = new FileStream(pathSpremanja,FileMode.Create))
        //                            ms.CopyTo(strim);
        //                        bekapPlan.Add(pathSpremanja);
        //                        Attachment noviAttachment = new Attachment { DatumUnosa = DateTime.Now, Path = pathSpremanja };
        //                        db.Attachments.Add(noviAttachment);
        //                    }
        //                }
        //                db.SaveChanges();
        //                transakcija.Commit();
        //                //ovdje treba pozvati email servis i obavijestiti adminsitratora o novom reportu
        //            }
        //        }
        //        catch (DbUpdateException)
        //        {
        //            transakcija.Rollback();
        //            foreach (string i in bekapPlan)
        //                if (System.IO.File.Exists(i))
        //                    System.IO.File.Delete(i);
        //        }


        //    }

        //}
    }
}
