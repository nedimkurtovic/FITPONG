using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels.Requests.Account;
using FIT_PONG.Mobile.ViewModels;
using FIT_PONG.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using FIT_PONG.SharedModels;
using FIT_PONG.Mobile.Views.Users;

namespace FIT_PONG.Mobile.ViewModels.Users
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly UsersAPIService _usersServis = new UsersAPIService();
        public LoginViewModel()
        {
            LoginKomanda = new Command(async () => await PozoviApi());
        }

        private string _userName;
        public string Username { get => _userName; set {SetProperty(ref _userName, value);} }
        
        private string _password;
        public string Password { get => _password; set { SetProperty(ref _password, value); } }

        public ICommand LoginKomanda { get; set; }

        async Task PozoviApi()
        {
            IsBusy = true;
            if (!RegulisiPrazneInpute())
            {
                IsBusy = false;
                await Application.Current.MainPage.DisplayAlert("Greška", "Molimo popunite polja", "OK");
                return;
            }
            var noviLogin = new SharedModels.Requests.Account.Login
            {
                UserName = this.Username,
                Password = this.Password,
                RememberMe = false
            };
            var rezultat = await _usersServis.Login(noviLogin);
            if(rezultat != (default(SharedModels.Users)))
            {
                BaseAPIService.Username = noviLogin.UserName;
                BaseAPIService.Password = noviLogin.Password;
                BaseAPIService.ID = rezultat.ID;
                BaseAPIService.User = rezultat;
                IsBusy = false;
                //await Application.Current.MainPage.DisplayAlert("Uspjeh", "Uspješna prijava", "OK");
                //otvoriti mainpage
                Application.Current.MainPage = new MainPage();
            }
            IsBusy = false;

        }
        private bool RegulisiPrazneInpute()
        {
            if (String.IsNullOrEmpty(Username) || String.IsNullOrWhiteSpace(Username))
                return false;
            if (String.IsNullOrEmpty(Password) || String.IsNullOrWhiteSpace(Password))
                return false;
            return true;
        }
    }
}
