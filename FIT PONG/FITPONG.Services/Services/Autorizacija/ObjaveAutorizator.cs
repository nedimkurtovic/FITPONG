using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public class ObjaveAutorizator : IObjaveAutorizator
    {
        public bool AuthorizeAddGlavniFeed(string loggedInUserEmail)
        {
            return AuthorizeAdministrator(loggedInUserEmail);
        }

        private bool AuthorizeAdministrator(string loggedInUserEmail)
        {
            if (loggedInUserEmail != "nedim.kurtovic@edu.fit.ba" && loggedInUserEmail != "aldin.talic@edu.fit.ba")
                throw new AuthorizeException("Niste autorizovani za takvu radnju.");
            return true;
        }
    }
}
