using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    public class TakmicenjaDetaljiViewModel:BaseViewModel
    {
        public SharedModels.Takmicenja Takmicenje { get; set; }
        public TakmicenjaDetaljiViewModel(SharedModels.Takmicenja _takmicenje = null)
        {
            Title = _takmicenje?.Naziv;
            Takmicenje = _takmicenje;
        }
    }
}
