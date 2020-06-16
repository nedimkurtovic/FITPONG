using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services
{
    public interface IPrijaveService
    {
        List<SharedModels.Prijave> Get(int TakmicenjeID);
        SharedModels.Prijave GetByID(int id);
        SharedModels.Prijave Add(int TakmicenjeID, SharedModels.Requests.Takmicenja.PrijavaInsert obj);
        SharedModels.Prijave Delete(int id);

    }
}
