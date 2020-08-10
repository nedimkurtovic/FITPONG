using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Account;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Users
{
    public class UsersListaViewModel:BaseViewModel
    {
        public UsersListaViewModel()
        {
            Title = "Igrači";
            ListaUsera = new ObservableCollection<SharedModels.Users>();
            DobaviUsere = new Command(async () => await PozoviApi());
            DobaviJosUsera = new Command(async () => await DobaviJos());
            iduciPage = "";
            btnDobaviJosVisible = false;
        }
        private string iduciPage;

        private bool btnvisible;
        public bool btnDobaviJosVisible { get => btnvisible; set { SetProperty(ref btnvisible, value); } }

        private UsersAPIService _apiServis = new UsersAPIService();
        public ObservableCollection<SharedModels.Users> ListaUsera { get; set; }

        private string _prikaznoIme;
        public string PrikaznoIme{ get => _prikaznoIme; set{SetProperty(ref _prikaznoIme, value);} }

        public Command DobaviUsere { get; }
        public Command DobaviJosUsera { get; set; }
        async Task DobaviJos()
        {
            IsBusy = true;
            var rezultat = await _apiServis.GetAllPaged<PagedResponse<SharedModels.Users>>(iduciPage);
            if(rezultat != default(PagedResponse<SharedModels.Users>))
            {
                dodajUListu(rezultat);
            }
            IsBusy = false;
        }
        async Task PozoviApi()
        {
            IsBusy = true;
            ListaUsera.Clear();
            AccountSearchRequest novi = new AccountSearchRequest
            {
                PrikaznoIme = _prikaznoIme
            };
            var rezultat = await _apiServis.GetAll<PagedResponse<SharedModels.Users>>(novi);
            if(rezultat != default(PagedResponse<SharedModels.Users>))
            {
                dodajUListu(rezultat);
            }
            IsBusy = false;
        }

        private void dodajUListu(PagedResponse<SharedModels.Users> rez)
        {
            foreach (var i in rez.Stavke)
                ListaUsera.Add(i);
            iduciPage = rez.IducaStranica?.ToString();
            btnDobaviJosVisible = iduciPage == null ? false : true;
        }
       
    }
}
