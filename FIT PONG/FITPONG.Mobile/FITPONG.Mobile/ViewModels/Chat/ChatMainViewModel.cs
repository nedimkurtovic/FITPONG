using FIT_PONG.Mobile.APIServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Mobile.ViewModels.Chat
{
    public class ChatMainViewModel:BaseViewModel
    {
        public ChatService ChatServis { get; set; }
        public ChatMainViewModel()
        {
            ChatServis = new ChatService();

            ChatServis.Init();
        }
    }
}
