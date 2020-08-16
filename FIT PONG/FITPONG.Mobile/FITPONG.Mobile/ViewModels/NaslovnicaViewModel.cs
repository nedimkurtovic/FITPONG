using FIT_PONG.Mobile.APIServices;
using Flurl.Http.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels
{
    public class NaslovnicaViewModel:BaseViewModel
    {
        public LampicaService lampicaServis { get; }
        public ObservableCollection<string> ListaPoruka { get; set; }
        public BaseAPIService apiServis { get; set; }
        public SharedModels.Naslovnica NaslovnicaModel { get; set; }
        public ObservableCollection<SharedModels.Objave> Objave { get; set; }
        public ObservableCollection<string> Rezultati { get; set; }
        public NaslovnicaViewModel()
        {
            lampicaServis = new LampicaService();
            lampicaServis.Init();
            ListaPoruka = new ObservableCollection<string>();
            apiServis = new BaseAPIService("naslovnica");
            Objave = new ObservableCollection<SharedModels.Objave>();
            Rezultati = new ObservableCollection<string>();
            PromijeniSliku = new Command(async () => await PromijeniSlikuFunkcija());
            GetNaslovnicaModel();
        }

        public Command PromijeniSliku { get; set; }
        async Task PromijeniSlikuFunkcija()
        {
            await lampicaServis.PromijeniStanjeAsync(lampicaServis.TrenutnaSlika);
        }
        async Task GetNaslovnicaModel()
        {
            var rezultat = await apiServis.GetAll<SharedModels.Naslovnica>();
            if(rezultat != default(SharedModels.Naslovnica))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    NaslovnicaModel = rezultat;
                    foreach (var i in rezultat.ZadnjeObjave)
                        Objave.Add(i);

                    foreach (var i in rezultat.ZadnjiRezultati)
                        Rezultati.Add(i);
                });
            }
        }
    }
}
