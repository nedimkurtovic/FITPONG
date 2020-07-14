using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.SharedModels.Requests.Account
{
    public class SlikaPromjenaRequest
    {
        public string Naziv { get; set; }
        public byte[] Slika{ get; set; }
    }
}
