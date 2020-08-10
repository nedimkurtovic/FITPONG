using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.ViewModels.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatKonverzacija : ContentPage
    {
        ChatKonverzacijaViewModel viewModel;
        public ChatKonverzacija(ChatService _chatservis, string _GrupaPrimatelj)
        {
            BindingContext = viewModel = new ChatKonverzacijaViewModel(_chatservis, _GrupaPrimatelj);
            InitializeComponent();
        }
        public void SkrolajNaDno()
        {
            if(viewModel.Poruke.Count != 0)
                LVPoruke.ScrollTo(viewModel.Poruke[viewModel.Poruke.Count -1], ScrollToPosition.End, true);
        }

    }
}