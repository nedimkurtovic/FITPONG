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
    public partial class TakmicenjaObjaveDodaj : ContentPage
    {
        public TakmicenjaObjaveDodajViewModel viewModel;
        public TakmicenjaObjaveDodaj(SharedModels.Takmicenja _takmicenje)
        {
            InitializeComponent();
            BindingContext = viewModel = new TakmicenjaObjaveDodajViewModel(_takmicenje);
        }

        private async void Dodaj_Clicked(object sender, EventArgs e)
        {
            var rezultat = await viewModel.DodajNovuObjavu();
            if(rezultat != default(SharedModels.Objave))
            {
                var takmicenjamain = Navigation.NavigationStack[1] as TakmicenjaMain;
                takmicenjamain.OsvjeziListuObjava();

                Navigation.InsertPageBefore(new TakmicenjaObjaveDetalji(rezultat,viewModel.Takmicenje), this);
                await Navigation.PopAsync();
            }
        }
    }
}