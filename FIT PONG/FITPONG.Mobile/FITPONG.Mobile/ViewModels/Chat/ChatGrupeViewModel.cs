using FIT_PONG.Mobile.APIServices;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Chat
{
    public class ChatGrupeViewModel:BaseViewModel
    {
        public ChatGrupeViewModel(ChatService _chatServis)
        {
            ChatServis = _chatServis;
        }

        public ChatService ChatServis { get; private set; }
    }
    
}
