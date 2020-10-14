using FIT_PONG.SharedModels;
using FIT_PONG.WebAPI.Services.Bazni;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services
{
    public interface IAktivnostiService: IBaseService<FIT_PONG.SharedModels.BrojKorisnikaLogs
        , FIT_PONG.SharedModels.Requests.Aktivnosti.AktivnostiSearch>
    {

        StanjeStranice GetStanjeStranice(int granica);
    }
}
