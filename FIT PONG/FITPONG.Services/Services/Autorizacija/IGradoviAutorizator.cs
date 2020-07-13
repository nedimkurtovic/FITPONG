using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public interface IGradoviAutorizator
    {
        bool AuthorizeAdd(string loggedInUserEmail);
        bool AuthorizeDelete(string loggedInUserEmail);
        bool AuthorizeUpdate(string loggedInUserEmail);
        bool AuthorizeGet(string loggedInUserEmail);

    }
}
