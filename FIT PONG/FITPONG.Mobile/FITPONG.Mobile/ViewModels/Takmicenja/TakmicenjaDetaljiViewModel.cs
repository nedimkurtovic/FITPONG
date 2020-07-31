using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    public class TakmicenjaDetaljiViewModel:BaseViewModel
    {
        public SharedModels.Takmicenja Takmicenje { get; set; }
        public string datumP { get; set; }
        public string datumZ { get; set; }
        public string datumPP { get; set; }
        public string datumZP { get; set; }

        public Command promjena { get; set; }
        private bool _vidljiv;
        public bool vidljiv { get=>_vidljiv; set 
            {
                _vidljiv = value;
                OnPropertyChanged(nameof(vidljiv));
            } }
        public TakmicenjaDetaljiViewModel(SharedModels.Takmicenja _takmicenje = null)
        {
            Title = _takmicenje?.Naziv;
            Takmicenje = _takmicenje;
            datumP = Takmicenje.DatumPocetka != null ? Takmicenje.DatumPocetka.ToString() : "Nije postavljen";
            datumZ = Takmicenje.DatumZavrsetka != null ? Takmicenje.DatumPocetka.ToString() : "Nije postavljen";
            datumPP = Takmicenje.DatumPocetkaPrijava != null ? Takmicenje.DatumPocetkaPrijava.ToString() : "Nije postavljen";
            datumZP = Takmicenje.DatumZavrsetkaPrijava != null ? Takmicenje.DatumZavrsetkaPrijava.ToString() : "Nije postavljen";
            vidljiv = true;
            promjena = new Command(async => { _vidljiv = !_vidljiv; });
        }
         public TakmicenjaDetaljiViewModel()
        {

        }
    }
}
