using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Aktivnosti;
using FIT_PONG.WebAPI.Services.Bazni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIT_PONG.Services.Services
{
    public class AktivnostiService: BaseService<FIT_PONG.SharedModels.BrojKorisnikaLogs
        ,FIT_PONG.Database.DTOs.BrojKorisnikaLog
        ,FIT_PONG.SharedModels.Requests.Aktivnosti.AktivnostiSearch> , IAktivnostiService
    {
        public AktivnostiService(MyDb _db, IMapper _mapko):base(_db,_mapko)
        {

        }
        public override List<BrojKorisnikaLogs> Get(AktivnostiSearch obj = null)
        {
            var dbRez = db.BrojKorisnikaLog.OrderByDescending(x => x.Datum).ToList();
            return mapko.Map<List<BrojKorisnikaLogs>>(dbRez);
        }
        public StanjeStranice GetStanjeStranice(int granica = 10)
        {
            StanjeStranice rez = new StanjeStranice();
            rez.DatumZagusenja = GetDatumZagusenja(granica);
            rez.MaxAktivno = GetMaxBrojKorisnika();
            rez.TrenutnoAktivno = GetTrenutnoAktivno();
            return rez;
        }

        private DateTime? GetDatumZagusenja(int granica)
        {
            var dbRez = db.BrojKorisnikaLog.OrderByDescending(x => x.Datum)
                .Where(x => x.BrojKorisnika >= granica).FirstOrDefault();
            if (dbRez != null)
                return dbRez.Datum.Date;
            return null;
        }

        private int GetMaxBrojKorisnika()
        {
            int max = 0;
            var dbRez = db.BrojKorisnikaLog.OrderByDescending(x => x.Datum).ToList();
            foreach (var i in dbRez)
                if (i.MaxBrojKorisnika > max)
                    max = i.MaxBrojKorisnika;
            return max;
        }
        private int GetTrenutnoAktivno()
        {
            var dbRez = db.BrojKorisnikaLog.Where(x => x.Datum.Date == DateTime.Now.Date).FirstOrDefault();
            if (dbRez != null)
                return dbRez.BrojKorisnika;
            return 0;
        }
    }
}
