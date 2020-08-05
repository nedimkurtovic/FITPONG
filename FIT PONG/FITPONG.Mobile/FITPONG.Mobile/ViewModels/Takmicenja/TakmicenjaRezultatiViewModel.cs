using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    public class TakmicenjaRezultatiViewModel
    {
        private readonly SharedModels.Takmicenja takmicenje;
        private List<SharedModels.Requests.Takmicenja.RasporedStavka> listaStavki { get; set; }
        public ObservableCollection<RasporedLista> listaGrupisanihStavki { get; set; }
        public TakmicenjeAPIService takmicenjeAPIService { get; set; } = new TakmicenjeAPIService();
        public ICommand DobaviRezultateKomanda { get; set; }
        public TakmicenjaRezultatiViewModel(SharedModels.Takmicenja _takmicenje)
        {
            takmicenje = _takmicenje;
            listaGrupisanihStavki = new ObservableCollection<RasporedLista>();
            listaStavki = new List<RasporedStavka>();
            DobaviRezultateKomanda = new Command(async () => await napuniListu());
        }

        private async Task napuniListu()
        {
            var lista = await takmicenjeAPIService.GetRaspored(takmicenje.ID);
            if (lista == null || lista == default(List<RasporedStavka>))
                return;

            listaStavki = lista;

            var unikati = listaStavki.Select(x => x.Runda).Distinct();
            foreach(var i in unikati)
            {
                var grupisani = listaStavki.Where(x => x.Runda == i).ToList();
                var novaGrupisana = new RasporedLista("Runda " + i, grupisani);
                listaGrupisanihStavki.Add(novaGrupisana);
            }
            
        }
    }
    public class RasporedLista : ObservableCollection<RasporedStavka>
    {
        public string NaslovGrupe { get; set; }
        public RasporedLista(string naslov, List<RasporedStavka> odgovarajuciClanovi)
        {
            NaslovGrupe = naslov;
            foreach (var i in odgovarajuciClanovi)
                this.Items.Add(i);
        }
    }
}
