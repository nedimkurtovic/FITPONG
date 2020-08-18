using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FIT_PONG.Mobile.APIServices
{
    public class NotifikacijeService
    {
#if DEBUG
        protected string hubUrl = "http://localhost:4260/chathub";
#endif
#if RELEASE
        protected string hubUrl = "http://p1869.app.fit.ba/ChatHub";
#endif

        public event EventHandler<MessageEventArgs> startaj;
        public event EventHandler<MessageEventArgs> primiNotifikacije;

        public ObservableCollection<string> ListaKonekcija { get; set; }

        HubConnection hubKonekcija;
        Random random;

        public bool IsConnected { get; set; }
    }
}
