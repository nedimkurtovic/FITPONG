using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public interface IObjaveAutorizator
    {
        bool AuthorizeAddGlavniFeed(string loggedInUserEmail);
    }
}
