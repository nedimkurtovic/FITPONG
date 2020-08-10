using FIT_PONG.Mobile.APIServices;
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
        public NaslovnicaViewModel()
        {
            lampicaServis = new LampicaService();
            lampicaServis.Init();
            ListaPoruka = new ObservableCollection<string>
            {
                "testiranje","sta je ovo", "da li je ovo nova poruka", "hoce li se desiti ista"
            };
            
            PromijeniSliku = new Command(async () => await PromijeniSlikuFunkcija());
        }

        public Command PromijeniSliku { get; set; }
        async Task PromijeniSlikuFunkcija()
        {
            await lampicaServis.PromijeniStanjeAsync(lampicaServis.TrenutnaSlika);
        }
    }
}
