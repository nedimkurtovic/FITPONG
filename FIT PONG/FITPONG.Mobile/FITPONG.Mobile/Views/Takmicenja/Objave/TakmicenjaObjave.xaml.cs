using FIT_PONG.Mobile.ViewModels.Takmicenja;
using FIT_PONG.Mobile.ViewModels.Takmicenja.Objave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Takmicenja.Objave
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TakmicenjaObjave : ContentPage
    {
        TakmicenjaObjaveViewModel viewModel;
        public TakmicenjaObjave(SharedModels.Takmicenja _takmicenje)
        {
            InitializeComponent();
            BindingContext = viewModel = new TakmicenjaObjaveViewModel(_takmicenje);
            btnDobaviJos.IsVisible = false;
            btnDodaj.IsVisible = viewModel.Vlasnik();
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            if (viewModel.DobaviObjaveKomanda.CanExecute(null))
                viewModel.DobaviObjaveKomanda.Execute(null);
            btnDobaviJos.IsVisible = viewModel.btnDobaviJosVisible;
        }


        private void btnDobaviJos_Clicked(object sender, EventArgs e)
        {
            if (viewModel.DobaviJosObjavaKomanda.CanExecute(null))
                viewModel.DobaviJosObjavaKomanda.Execute(null);
            btnDobaviJos.IsVisible = viewModel.btnDobaviJosVisible;
        }

        private async void btnDodaj_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TakmicenjaObjaveDodaj(viewModel.Takmicenje));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var djesmo = Navigation.NavigationStack;
            var steklejaut = (BindableObject)sender;
            var objekatKliknuti = (SharedModels.Objave)steklejaut.BindingContext;
            await Navigation.PushAsync(new TakmicenjaObjaveDetalji(objekatKliknuti, viewModel.Takmicenje));
        }
    }
}