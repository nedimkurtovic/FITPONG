using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Hubs
{
    public class LampicaHub:Hub
    {
        static private string finalnisrc = "/lampice/lampicaoff2.jpg";
        public Task PromijeniStanje(string trenutnaSlika)
        {
          finalnisrc = trenutnaSlika;
            if (trenutnaSlika == "/lampice/lampicaoff2.jpg")
                finalnisrc = "/lampice/lampicaon2.png";
            else
                finalnisrc = "/lampice/lampicaoff2.jpg";
            return Clients.All.SendAsync("PromjenaStatusa", finalnisrc);
        }
        public Task VratiTrenutno()
        {
           return Clients.Caller.SendAsync("trenutnostanje", finalnisrc);
        }
    }
}
