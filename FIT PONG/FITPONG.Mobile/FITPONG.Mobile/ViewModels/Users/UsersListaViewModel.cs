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
        }
        private string iduciPage;
        private bool btnvisible;
        public bool btnDobaviJosVisible { get => btnvisible; set { SetProperty(ref btnvisible, value); } }
        private UsersAPIService _apiServis = new UsersAPIService();
        public ObservableCollection<SharedModels.Users> ListaUsera { get; set; }
        public List<SharedModels.Users> ListaUsera1 { get; set; }
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
                foreach (var i in rezultat.Stavke)
                    ListaUsera.Add(i);
                iduciPage = rezultat.IducaStranica?.ToString();
                btnDobaviJosVisible = iduciPage == null ? false : true;
            }
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
            if(rezultat != null)
            {
                foreach (var i in rezultat.Stavke)
                    ListaUsera.Add(i);
                iduciPage = rezultat.IducaStranica?.ToString();
            }
            
        }
        public ObservableCollection<SharedModels.Users> DummyLista { get; set; } = new ObservableCollection<SharedModels.Users>
        {
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni007"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni008"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni009"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0010"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0011"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0012"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0013"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0014"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0015"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0016"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0017"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0018"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0019"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0020"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0021"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0022"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0023"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0024"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0025"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0026"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0027"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0028"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0029"},
            new SharedModels.Users{ELO=1000,PrikaznoIme="Testni0030"},
        };
    }
}
