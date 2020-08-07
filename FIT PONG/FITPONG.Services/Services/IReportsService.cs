using FIT_PONG.WebAPI.Services.Bazni;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services
{
    public interface IReportsService : IBaseService<SharedModels.Reports,object>
    {
        SharedModels.Reports Add(SharedModels.Requests.Reports.ReportsInsert obj, string rootFolder);
    }
}
