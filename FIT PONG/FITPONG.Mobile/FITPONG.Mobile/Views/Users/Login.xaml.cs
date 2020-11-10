using FIT_PONG.Mobile.ViewModels.Users;
using FIT_PONG.Mobile.Views.Takmicenja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        LoginViewModel viewModel;
        public Login()
        {
            InitializeComponent();
            BindingContext = viewModel = new LoginViewModel();
        }

        private async void btnRegistracija_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registracija());
            
        }

        private async void btnResendMail_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UnesiMejl("resetMail"));
            
        }

        private async void btnResetPassword_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UnesiMejl("resetPassword"));
            //Application.Current.MainPage = new UnesiMejl("resetPassword");
        }
    }
}