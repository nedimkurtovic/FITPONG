using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Services
{
    public class AuthorizeException:Exception
    {
        public AuthorizeException(string msg):base(msg)
        {

        }
    }
}
