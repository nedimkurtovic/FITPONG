using FIT_PONG.Database.DTOs;
using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.PomocniObjekti;
using FIT_PONG.SharedModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    class TakmicenjaFavoritiViewModel: BaseViewModel
    {
        TakmicenjeAPIService takmicenejApiService = new TakmicenjeAPIService();
        public TakmicenjaFavoritiViewModel(SharedModels.Takmicenja takmicenje)
        {
            Takmicenje = takmicenje;
            GetFavoriti();
        }


        public SharedModels.Takmicenja Takmicenje { get; set; }
        public ObservableCollection<FavoritiPomocni> oznaceneUtakmice { get; set; } = new ObservableCollection<FavoritiPomocni>();
        public ObservableCollection<FavoritiPomocni> neoznaceneUtakmice { get; set; } = new ObservableCollection<FavoritiPomocni>();
        

        private async void GetFavoriti()
        {
            IsBusy = true;
            var rezultat = await takmicenejApiService.GetFavoriti(Takmicenje.ID);

            if (rezultat != default(SharedModels.Favoriti))
                NapuniComboListe(rezultat);
            else
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Greska", "Doslo je do greske prilikom dobavljanja favorita.", "OK");
        }

        public async Task<SharedModels.Favoriti> OznaciUtakmicu(int utakmicaId)
        {
            var rezultat = await takmicenejApiService.OznaciUtakmicu(utakmicaId);

            return rezultat;    
        }



        public void NapuniComboListe(SharedModels.Favoriti fav)
        {
            foreach (var item in fav.oznaceneUtakmice)
                oznaceneUtakmice.Add(new FavoritiPomocni
                {
                    NazivTima = item.tim1 + " - " + item.tim2,
                    UtakmicaId = item.utakmicaId
                });

            foreach (var item in fav.neoznaceneUtakmice)
                neoznaceneUtakmice.Add(new FavoritiPomocni
                {
                    NazivTima = item.tim1 + " - " + item.tim2,
                    UtakmicaId = item.utakmicaId
                });

            IsBusy = false;
        }

    }
}
