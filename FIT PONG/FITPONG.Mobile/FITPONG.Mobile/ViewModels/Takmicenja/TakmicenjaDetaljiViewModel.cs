using FIT_PONG.Database.DTOs;
using FIT_PONG.Mobile.APIServices;
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
    public class TakmicenjaDetaljiViewModel : BaseViewModel
    {
        public TakmicenjaDetaljiViewModel(SharedModels.Takmicenja _takmicenje = null)
        {
            Title = _takmicenje?.Naziv;
            Takmicenje = _takmicenje;
            datumP = Takmicenje.DatumPocetka != null ? Takmicenje.DatumPocetka.GetValueOrDefault().Date.ToString() : "Nije postavljen";
            datumZ = Takmicenje.DatumZavrsetka != null ? Takmicenje.DatumPocetka.GetValueOrDefault().Date.ToString() : "Nije postavljen";
            datumPP = Takmicenje.DatumPocetkaPrijava != null ? Takmicenje.DatumPocetkaPrijava.GetValueOrDefault().Date.ToString() : "Nije postavljen";
            datumZP = Takmicenje.DatumZavrsetkaPrijava != null ? Takmicenje.DatumZavrsetkaPrijava.GetValueOrDefault().Date.ToString() : "Nije postavljen";
            vidljiv = true;
            promjena = new Command(async => { _vidljiv = !_vidljiv; });
            prijaveVisible = _takmicenje.Inicirano == false && BaseAPIService.ID == _takmicenje.KreatorID ? true:false;

        }
        public TakmicenjaDetaljiViewModel()
        {

        }
        public TakmicenjeAPIService takmicenjeAPIService { get; set; } = new TakmicenjeAPIService();
        public SharedModels.Takmicenja Takmicenje { get; set; }
        public string datumP { get; set; }
        public string datumZ { get; set; }
        public string datumPP { get; set; }
        public string datumZP { get; set; }
        public ObservableCollection<Prijava> listaPrijava { get; set; } = new ObservableCollection<Prijava>();
        public ICommand initCommandPrijave { get; set; }
        public Command promjena { get; set; }
        public bool prijaveVisible { get; set; }
        private bool _vidljiv;
        public bool vidljiv
        {
            get => _vidljiv; set
            {
                _vidljiv = value;
                OnPropertyChanged(nameof(vidljiv));
            }
        }
        public bool Vlasnik()
        {
            return BaseAPIService.ID == Takmicenje.KreatorID;
        }
        public async Task<SharedModels.Takmicenja> InicirajTakmicenje()
        {
            var rezultat = await takmicenjeAPIService.Init(Takmicenje.ID);
            if (rezultat != default(SharedModels.Takmicenja))
            {
                await Application.Current.MainPage.DisplayAlert("Uspjeh", "Uspješno inicirano takmičenje", "OK");
            }
            return rezultat;
        }



    }
}
