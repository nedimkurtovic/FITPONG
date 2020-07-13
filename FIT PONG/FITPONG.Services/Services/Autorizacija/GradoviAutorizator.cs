using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services.Services.Autorizacija
{
    public class GradoviAutorizator : IGradoviAutorizator
    {
        public bool AuthorizeAdd(string loggedInUserEmail)
        {
            return AuthorizeAdministrator(loggedInUserEmail);
        }

        public bool AuthorizeDelete(string loggedInUserEmail)
        {
            return AuthorizeAdministrator(loggedInUserEmail);
        }

        public bool AuthorizeGet(string loggedInUserEmail)
        {
            return AuthorizeAdministrator(loggedInUserEmail);
        }

        public bool AuthorizeUpdate(string loggedInUserEmail)
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
