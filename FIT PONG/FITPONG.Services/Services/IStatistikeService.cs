using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services
{
    public interface IStatistikeService
    {
        List<SharedModels.Statistike> Get(int userID);
        SharedModels.Statistike GetByID(int id);
        SharedModels.Statistike Add(int userID, int AkademskaGodina);
        SharedModels.Statistike Update(int id, bool pobjeda, bool isTim);

    }
}
