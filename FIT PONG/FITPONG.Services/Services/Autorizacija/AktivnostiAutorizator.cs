using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public class AktivnostiAutorizator: IAktivnostiAutorizator
    {
        public bool AuthorizeGet(string loggedInUserEmail)
        {
            return AuthorizeAdministrator(loggedInUserEmail);
        }
        private bool AuthorizeAdministrator(string loggedInUserEmail)
        {
            if (loggedInUserEmail != "nedim.kurtovic@edu.fit.ba" && loggedInUserEmail != "aldin.talic@edu.fit.ba"
                && loggedInUserEmail != "desktop")
                throw new AuthorizeException("Niste autorizovani za takvu radnju.");
            return true;
        }
    }
}
