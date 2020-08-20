using FIT_PONG.Database.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public interface ISuspenzijaService
    {
        Suspenzija ImaVazecuSuspenziju(int UserID, string VrstaSuspenzije);
    }
}
