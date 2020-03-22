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
        public Task PromijeniStanje(string trenutnaSlika , string rezultz)
        {
          finalnisrc = trenutnaSlika;
            string poruka = "";
            if (trenutnaSlika == "/lampice/lampicaoff2.jpg")
            {
                finalnisrc = "/lampice/lampicaon2.png";
                poruka = rezultz + " je upalio lampicu!";
            }
            else
            {
                finalnisrc = "/lampice/lampicaoff2.jpg";
                poruka = rezultz + " je ugasio lampicu!";
            }

            Clients.All.SendAsync("PromjenaStatusa", finalnisrc);
            return Clients.All.SendAsync("PromjenaPoruke", poruka);
        }
        public Task VratiTrenutno()
        {
           return Clients.Caller.SendAsync("trenutnostanje", finalnisrc);
        }
    }
}
