using FIT_PONG.Mobile.PomocniObjekti;
using FIT_PONG.Mobile.ViewModels.Takmicenja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Takmicenja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakmicenjaFavoriti : ContentPage
    {
        TakmicenjaFavoritiViewModel viewModel;
        public TakmicenjaFavoriti(SharedModels.Takmicenja takmicenje)
        {
            BindingContext = viewModel = new TakmicenjaFavoritiViewModel(takmicenje);
            InitializeComponent();
        }

        private async void btnDodajUkloni_Clicked(object sender, EventArgs e)
        {
            lblBusy.Text = "Označavanje utakmice u toku ...";
            viewModel.IsBusy = true;
            var x = int.Parse(((Button)sender).BindingContext.ToString());
            var klasa = ((Button)sender).ClassId;

            var rezultat = await viewModel.OznaciUtakmicu(x);

            if (rezultat != default(SharedModels.Favoriti))
            {
                if (klasa == "dodaj")
                {
                    var i = viewModel.neoznaceneUtakmice.Where(d => d.UtakmicaId == x).SingleOrDefault();
                    viewModel.neoznaceneUtakmice.Remove(i);
                    viewModel.oznaceneUtakmice.Add(i);
                }
                else if (klasa=="ukloni")
                {
                    var i = viewModel.oznaceneUtakmice.Where(d => d.UtakmicaId == x).SingleOrDefault();
                    viewModel.oznaceneUtakmice.Remove(i);
                    viewModel.neoznaceneUtakmice.Add(i);
                }
                else
                {
                    await DisplayAlert("Greska", "Doslo je do greske prilikom oznacavanja utakmice.", "OK");
                }
            }
            else
                 await DisplayAlert("Greska", "Doslo je do greske prilikom oznacavanja utakmice.", "OK");

            viewModel.IsBusy = false;
        }
        
    }
}