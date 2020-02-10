using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using FIT_PONG.Models;
using FIT_PONG.ViewModels.TakmicenjeVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json;

namespace FIT_PONG.Controllers
{
    public class FitApiController : ApiController
    {
        private readonly MyDb db;

        public FitApiController(MyDb db)
        {
            this.db = db;
        }

        public string Get(int id)
        {
            Takmicenje takm = db.Takmicenja
               .Include(x => x.Bracketi)
               .ThenInclude(x => x.Runde)
               .ThenInclude(x => x.Utakmice)
               .ThenInclude(x => x.UcescaNaUtakmici)
               .ThenInclude(x => x.Igrac)
               .SingleOrDefault(y => y.ID == id);
            List<int> l = new List<int>();
            for (int i = 0; i < takm.Bracketi[0].Runde.Count(); i++)
            {
                for (int j = 0; j < takm.Bracketi[0].Runde[i].Utakmice.Count(); j++)
                {
                    if (takm.Bracketi[0].Runde[i].Utakmice[j].UcescaNaUtakmici[0].IgracID == 60
                        || takm.Bracketi[0].Runde[i].Utakmice[j].UcescaNaUtakmici[1].IgracID == 60)
                        l.Add(takm.Bracketi[0].Runde[i].Utakmice[j].BrojUtakmice);
                }
            }

            var lista = (from t in db.Takmicenja
                         join b in db.Brackets
                             on t.ID equals b.TakmicenjeID
                         join r in db.Runde
                             on b.ID equals r.BracketID
                         join u in db.Utakmice
                             on r.ID equals u.RundaID
                         join iu in db.IgraciUtakmice
                             on u.ID equals iu.UtakmicaID
                         join pi in db.PrijaveIgraci on iu.IgracID equals pi.IgracID into tempJoin
                         from pi in tempJoin.DefaultIfEmpty()
                         join p in db.Prijave on pi.PrijavaID equals p.ID into tJoin
                         from p in tJoin.DefaultIfEmpty()
                         where t.ID == id 
                         select new 
                         {
                             IgUtId = iu.IgID,
                             Naziv = p.Naziv,
                             OsvojeniSetovi = iu.OsvojeniSetovi,
                             BrojUtakmice = u.BrojUtakmice
                         }).OrderBy(x => x.BrojUtakmice).ToList();
            lista = lista.DistinctBy(x => new { x.IgUtId }).ToList();

            List<(string tim1, int? set1, int? set2, string tim2)> parovi = new List<(string tim1, int? set1, int? set2, string tim2)>();
            for (int i = 0; i < lista.Count(); i += 2)
            {
                parovi.Add((lista[i].Naziv, lista[i].OsvojeniSetovi, lista[i + 1].OsvojeniSetovi, lista[i + 1].Naziv));
            }

            var jsonObj = JsonConvert.SerializeObject(parovi);

            return jsonObj;
        }

        public string GetUtakmiceEvidencija(int id)
        {
            Takmicenje takm = db.Takmicenja
               .Include(x => x.Bracketi)
               .ThenInclude(x => x.Runde)
               .ThenInclude(x => x.Utakmice)
               .ThenInclude(x => x.UcescaNaUtakmici)
               .ThenInclude(x => x.Igrac)
               .SingleOrDefault(y => y.ID == id);
            List<int> l = new List<int>();
            for (int i = 0; i < takm.Bracketi[0].Runde.Count(); i++)
            {
                for (int j = 0; j < takm.Bracketi[0].Runde[i].Utakmice.Count(); j++)
                {
                    if (takm.Bracketi[0].Runde[i].Utakmice[j].UcescaNaUtakmici[0].IgracID == 60
                        || takm.Bracketi[0].Runde[i].Utakmice[j].UcescaNaUtakmici[1].IgracID == 60)
                        l.Add(takm.Bracketi[0].Runde[i].Utakmice[j].BrojUtakmice);
                }
            }

            var lista = (from t in db.Takmicenja
                         join b in db.Brackets
                             on t.ID equals b.TakmicenjeID
                         join r in db.Runde
                             on b.ID equals r.BracketID
                         join u in db.Utakmice
                             on r.ID equals u.RundaID
                         join iu in db.IgraciUtakmice
                             on u.ID equals iu.UtakmicaID
                         join pi in db.PrijaveIgraci on iu.IgracID equals pi.IgracID into tempJoin
                         from pi in tempJoin.DefaultIfEmpty()
                         join p in db.Prijave on pi.PrijavaID equals p.ID into tJoin
                         from p in tJoin.DefaultIfEmpty()
                         where t.ID == id && l.Contains(u.BrojUtakmice)//dodati neki bool parametar kako bi se isti query 
                                                                       //koristio kao api i za evidenciju meca i za rezultate
                         select new
                         {
                             IgUtId = iu.IgID,
                             Naziv = p.Naziv,
                             OsvojeniSetovi = iu.OsvojeniSetovi,
                             BrojUtakmice = u.BrojUtakmice
                         }).OrderBy(x => x.BrojUtakmice).ToList();
            lista = lista.DistinctBy(x => new { x.IgUtId }).ToList();

            List<(string tim1, int? set1, int? set2, string tim2)> parovi = new List<(string tim1, int? set1, int? set2, string tim2)>();
            for (int i = 0; i < lista.Count(); i += 2)
            {
                parovi.Add((lista[i].Naziv, lista[i].OsvojeniSetovi, lista[i + 1].OsvojeniSetovi, lista[i + 1].Naziv));
            }

            var jsonObj = JsonConvert.SerializeObject(parovi);

            return jsonObj;
        }


    }
}