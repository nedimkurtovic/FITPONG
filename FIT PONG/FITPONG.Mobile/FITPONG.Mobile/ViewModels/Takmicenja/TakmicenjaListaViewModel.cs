using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    public class TakmicenjaListaViewModel:BaseViewModel
    {
        public TakmicenjaListaViewModel()
        {
            Title = "Takmičenja";
            ListaTakmicenja = new ObservableCollection<SharedModels.Takmicenja>();
            DobaviTakmicenja = new Command(async () => await PozoviApi());
            DobaviJosTakmicenja = new Command(async () => await DobaviJos());
            iducaStranica = "";
        }
        public TakmicenjeAPIService takmicenjeServis { get; set; } = new TakmicenjeAPIService();
        private string _nazivTakmicenja;
        public string NazivTakmicenja { get => _nazivTakmicenja; set { SetProperty(ref _nazivTakmicenja, value); } }

        public ObservableCollection<SharedModels.Takmicenja> ListaTakmicenja{ get; set; }

        private string iducaStranica;
        public bool btnDobaviJosVisible { get; set; } = true;
        public ICommand DobaviTakmicenja { get; set; }
        public ICommand DobaviJosTakmicenja { get; set; }
        async Task PozoviApi()
        {
            ListaTakmicenja.Clear();
            var rqst = new TakmicenjeSearch
            {
                Naziv = NazivTakmicenja
            };
            var rezultat =  await takmicenjeServis.GetAll<PagedResponse<SharedModels.Takmicenja>>(rqst);
            if(rezultat != default(PagedResponse<SharedModels.Takmicenja>))
            {
                dodajUListu(rezultat);
            }
        }
        async Task DobaviJos()
        {
            IsBusy = true;
            var rezultat = await takmicenjeServis.GetAllPaged<PagedResponse<SharedModels.Takmicenja>>(iducaStranica);
            if(rezultat != default(PagedResponse<SharedModels.Takmicenja>))
            {
                dodajUListu(rezultat);
            }
        }
        private void dodajUListu(PagedResponse<SharedModels.Takmicenja> rez)
        {
            foreach (var i in rez.Stavke)
                ListaTakmicenja.Add(i);
            iducaStranica = rez.IducaStranica?.ToString();
            btnDobaviJosVisible = iducaStranica == null ? false : true;
        }

    }
}
