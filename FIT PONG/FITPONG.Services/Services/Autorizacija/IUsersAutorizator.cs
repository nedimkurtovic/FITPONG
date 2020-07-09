using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public interface IUsersAutorizator
    {
        
        bool AuthorizePromjenaPasswordaMail();
        bool AuthorizePromjenaPasswordaPotvrda();
        bool AuthorizePostovanje();
    }
}
