using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.ViewModels.Chat;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatGrupe : ContentPage
    {
        ChatGrupeViewModel viewModel;
        public ChatGrupe(ChatService _chatservis)
        {
            InitializeComponent();
            BindingContext = viewModel = new ChatGrupeViewModel(_chatservis);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var mainStranica = Navigation.NavigationStack[0] as ChatMain;
            var snd = (BindableObject)sender;
            var naziv = (string) snd.BindingContext;
            mainStranica.PromijeniStranicu(naziv);
        }
    }
}