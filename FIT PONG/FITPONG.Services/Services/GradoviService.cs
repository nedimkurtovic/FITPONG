using AutoMapper;
using FIT_PONG.Database.DTOs;
using FIT_PONG.WebAPI.Services.Bazni;
using FIT_PONG.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Database;
using FIT_PONG.Services;

namespace FIT_PONG.Services.Services
{
    public class GradoviService : 
        CRUDService<SharedModels.Gradovi, 
            SharedModels.Requests.Gradovi.GradoviInsertUpdate,
            SharedModels.Requests.Gradovi.GradoviInsertUpdate, 
            Database.DTOs.Grad, 
            object>,
        IGradoviService
    {

        public GradoviService(MyDb _db, IMapper _mapko):base(_db,_mapko)
        {
        }
        public void Validiraj(Database.DTOs.Grad obj)
        {
            UserException ex = new UserException();
            
            int count = db.Gradovi.Where(x => x.Naziv == obj.Naziv).Count();
            if ((obj.ID == 0 && count > 0) || (obj.ID > 0 && count > 1)) // razlika insert i  update
                ex.AddError("Naziv", "Naziv već postoji");
            if (ex.Errori.Count > 0)
                throw ex;
        }

        public override SharedModels.Gradovi Add(
            SharedModels.Requests.Gradovi.GradoviInsertUpdate obj)
        {
            var bazaObj = mapko.Map<Database.DTOs.Grad>(obj);
            Validiraj(bazaObj);
            db.Set<Grad>().Add(bazaObj);
            db.SaveChanges();
            return mapko.Map<SharedModels.Gradovi>(bazaObj);
        }

        public override Gradovi Update(int id, SharedModels.Requests.Gradovi.GradoviInsertUpdate obj)
        {
            var bazaObjekat = db.Gradovi.Find(id);
            if (bazaObjekat == null)
                throw new UserException("Objekat ne postoji");

            mapko.Map(obj, bazaObjekat);
            Validiraj(bazaObjekat);
            db.SaveChanges();
            return mapko.Map<SharedModels.Gradovi>(bazaObjekat);
        }

        public List<Gradovi> Get()
        {
            var gradovi = db.Gradovi.ToList();

            return mapko.Map<List<SharedModels.Gradovi>>(gradovi);
        }

        
    }
}
