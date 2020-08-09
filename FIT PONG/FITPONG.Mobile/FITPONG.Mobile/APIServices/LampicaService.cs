using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FIT_PONG.Mobile.APIServices
{
    public class LampicaService
    {
        public event EventHandler<MessageEventArgs> StigloTrenutnoStanje;
        public event EventHandler<MessageEventArgs> PromijenjenaSlika;
        public event EventHandler<MessageEventArgs> PromijenjenaPoruka;
        public event EventHandler<MessageEventArgs> UgasenaKonekcija;


        HubConnection hubKonekcija;
        public string TrenutnaSlika { get; set; }
        Random random;
#if DEBUG
        //protected string hubUrl = "http://localhost:5766/lampica";
        protected string hubUrl = "http://localhost:4260/lampica";
#endif
#if RELEASE
        protected string hubUrl = "http://p1869.app.fit.ba/lampica";
#endif
        public void Init()
        {
            random = new Random();
            IsConnected = false;
            TrenutnaSlika = "lampicaoff2.jpg";

            hubKonekcija = new HubConnectionBuilder()
                .WithUrl(hubUrl).Build();

            hubKonekcija.Closed += async (error) =>
            {
                UgasenaKonekcija?.Invoke(this, new MessageEventArgs("Disconnected.."));
                IsConnected = false;
                await Task.Delay(random.Next(0, 5) * 1000);
                try
                {
                    await ConnectAsync();
                }
                catch (Exception ex)
                {
                    // Exception!
                }
            };

            hubKonekcija.On<string>("trenutnostanje", (finalnisrc) =>
            {
                var ispravljeniFajl = finalnisrc.Substring(finalnisrc.LastIndexOf("/") + 1);
                StigloTrenutnoStanje?.Invoke(this, new MessageEventArgs(ispravljeniFajl));
            });

            hubKonekcija.On<string>("PromjenaPoruke", (poruka) =>
            {
                PromijenjenaPoruka?.Invoke(this, new MessageEventArgs(poruka));
            });

            hubKonekcija.On<string>("PromjenaStatusa", (finalnisrc) =>
            {
                //Obzirom da se salje /lampica/imefajla.png moram doc do samo ovog imefajla.png za ove 
                //mobile varijane, nsiam siguran kako se ponasa kad stavis u folder sliku
                var ispravljeniFajl = finalnisrc.Substring(finalnisrc.LastIndexOf("/") + 1);
                PromijenjenaSlika?.Invoke(this, new MessageEventArgs(ispravljeniFajl));
            });
        }

        public bool IsConnected { get; set; }

        public async Task ConnectAsync()
        {
            if (IsConnected)
                return;

            await hubKonekcija.StartAsync();
            IsConnected = true;

        }

        public async Task DisconnectAsync()
        {
            if (!IsConnected)
                return;
            await hubKonekcija.StopAsync();
            IsConnected = false;

        }

        public async Task GetTrenutnuSlikuAsync()
        {
            if (!IsConnected)
                return;
            await hubKonekcija.InvokeAsync("VratiTrenutno");
        }

        public async Task PromijeniStanjeAsync(string Trenutnapic)
        {
            if (!IsConnected)
                return;
            var userName = BaseAPIService.Username;
            await hubKonekcija.InvokeAsync("PromijeniStanje","/lampice/" + Trenutnapic,userName);
        }
    }
}
