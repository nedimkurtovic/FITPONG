using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.SharedModels;
using FITPONG.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FIT_PONG.Services.Services
{
    class StatistikeService : IStatistikeService
    {
        private readonly MyDb db;
        private readonly IMapper mapper;

        public StatistikeService(MyDb db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        private void Validiraj(int userId)
        {
            UserException exception = new UserException();

            var user = db.Igraci.Find(userId);
            if (user == null)
                exception.AddError("", "Igrac ne postoji u bazi.");

            var statistika = db.Statistike.Where(s => s.IgracID == userId && s.AkademskaGodina == DateTime.Now.Year).FirstOrDefault();
            if (statistika != null)
                exception.AddError("", "Statistika za ovog igraca i ovu akademsku godinu vec postoji.");

            if (exception.Errori.Count > 0)
                throw exception;
        }


        public Statistike Add(int userID)
        {
            Validiraj(userID);

            var statistika = new Database.DTOs.Statistika(userID);
            db.Add(statistika);
            db.SaveChanges();

            return mapper.Map<SharedModels.Statistike>(statistika);
        }

        public List<Statistike> Get(int userID)
        {
            var user = db.Igraci.Find(userID);
            if (user == null)
                throw new UserException("Igrac ne postoji u bazi.");

            var statistike = db.Statistike.Where(s => s.IgracID == userID).ToList();

            return mapper.Map<List<SharedModels.Statistike>>(statistike);
        }

        public Statistike GetByID(int id)
        {
            var statistika = db.Statistike.Find(id);
            if (statistika == null)
                throw new UserException("Statistika ne postoji u bazi.");

            return mapper.Map<SharedModels.Statistike>(statistika);
        }

        public Statistike Update(int id, bool pobjeda, bool isTim)
        {
            var statistika = db.Statistike.Find(id);
            if (statistika == null)
                throw new UserException("Statistika ne postoji u bazi.");

            statistika.BrojOdigranihMeceva++;

            if (pobjeda)
            {
                if (isTim)
                    statistika.BrojTimskihPobjeda++;
                else
                    statistika.BrojSinglePobjeda++;
            }

            db.Update(statistika);
            db.SaveChanges();

            return mapper.Map<SharedModels.Statistike>(statistika);
        }
    }
}
