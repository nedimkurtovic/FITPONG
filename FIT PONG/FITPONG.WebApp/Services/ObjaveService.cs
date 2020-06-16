using AutoMapper;
using FIT_PONG.Filters;
using FIT_PONG.Database;
using FIT_PONG.Services.Bazni;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Objave;
using FIT_PONG.Services;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Services
{
    public class ObjaveService:BaseService<SharedModels.Objave,Database.DTOs.Objava,object>,IObjaveService
    {
        public ObjaveService(MyDb db, IMapper mapko) : base(db, mapko)
        {

        }
        public bool Validiraj(int FeedID)
        {
            UserException ex = new UserException();
            var feed = db.Feeds.Where(x => x.ID == FeedID).FirstOrDefault();
            if (feed == null)
                ex.AddError("", "Feed ne postoji ili je obrisan");
            if (ex.Errori.Count > 0)
                throw ex;
            return true;
        }

        public Objave Add(int FeedID, ObjaveInsertUpdate obj)
        {
            Validiraj(FeedID);
            var bazaObj = mapko.Map<Database.DTOs.Objava>(obj);
            db.Objave.Add(bazaObj);

            var FidObjava = new Database.DTOs.FeedObjava();
            FidObjava.ObjavaID = bazaObj.ID;
            FidObjava.FeedID = FeedID;

            db.FeedsObjave.Add(FidObjava);
            db.SaveChanges();

            var povratni = mapko.Map<SharedModels.Objave>(bazaObj);
            return povratni;
        }

        public Objave Update(int id, ObjaveInsertUpdate obj)
        {
            var bazaObjekat = db.Objave.Find(id);
            if (bazaObjekat == null)
                throw new UserException("Objekat ne postoji");
            
            mapko.Map(obj, bazaObjekat);
            db.SaveChanges();
            var povratni = mapko.Map<SharedModels.Objave>(bazaObjekat);
            return povratni;
        }

        public void Delete(int id)
        {
            var bazaObjekat = db.Objave.Find(id);
            if (bazaObjekat == null)
                throw new UserException("Objekat ne postoji");
            db.Objave.Remove(bazaObjekat);
            db.SaveChanges();
        }

        public Objave Add(ObjaveInsertUpdate obj)//ovo za adminstratore kad hoce na glavnu stranicu da dodaju objave
        {
            var bazaObj = mapko.Map<Database.DTOs.Objava>(obj);
            db.Objave.Add(bazaObj);
            db.SaveChanges();
            var povratni = mapko.Map<SharedModels.Objave>(bazaObj);
            return povratni;
        }
    }
}
