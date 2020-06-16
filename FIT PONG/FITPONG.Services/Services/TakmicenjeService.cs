using FIT_PONG.Database;
using FIT_PONG.Services.BL;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace FIT_PONG.Services.Services
{
    public class TakmicenjeService:ITakmicenjeService
    {
        private readonly MyDb db;
        private readonly Evidentor evidentor;
        private readonly InitTakmicenja initTakmicenja;
        private readonly TakmicenjeValidator validator;

        public TakmicenjeService(MyDb _db, Evidentor _evidentor, InitTakmicenja _initTakmicenja,
            TakmicenjeValidator _validator)
        {
            db = _db;
            evidentor = _evidentor;
            initTakmicenja = _initTakmicenja;
            validator = _validator;
        }

        public Takmicenja Add(TakmicenjaInsert obj)
        {
            throw new NotImplementedException();
        }

        public Takmicenja Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Takmicenja> Get(TakmicenjeSearch obj)
        {
            throw new NotImplementedException();
        }

        public Takmicenja GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public Takmicenja Initialize(int id)
        {
            throw new NotImplementedException();
        }

        public Takmicenja Update(int id, TakmicenjaUpdate obj)
        {
            throw new NotImplementedException();
        }
    }
}
