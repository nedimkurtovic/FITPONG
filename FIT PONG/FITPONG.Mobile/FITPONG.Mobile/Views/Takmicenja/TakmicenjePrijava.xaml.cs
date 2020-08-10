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
    public partial class TakmicenjePrijava : ContentPage
    {

        TakmicenjePrijavaViewModel viewModel;

        public TakmicenjePrijava(SharedModels.Takmicenja _takmicenje)
        {
            BindingContext = viewModel = new TakmicenjePrijavaViewModel(_takmicenje);
            viewModel.NapuniComboBox();
            InitializeComponent();

            igrac1Picker.ItemDisplayBinding = new Binding("PrikaznoIme");
            igrac2Picker.ItemDisplayBinding = new Binding("PrikaznoIme");

            igrac1Picker.ItemsSource = viewModel.listaIgraca;
            igrac2Picker.ItemsSource = viewModel.listaIgraca;
            
            btnPrijava.Margin = new Thickness(80, 0);
        }

        private async void btnPrijava_Clicked(object sender, EventArgs e)
        {
            var rezultat = await viewModel.DodajPrijavu();
            if (rezultat != default(SharedModels.Prijave)) 
                await Navigation.PopAsync();
        }

        private void igrac1Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var piker = (Picker)sender;
            if (piker.SelectedIndex != -1)
            {
                var igrac1 = piker.ItemsSource[piker.SelectedIndex] as SharedModels.Users;
                viewModel.Igrac1 = igrac1.ID;
            }
            else
                viewModel.Igrac1 = -1;
        }

        private void igrac2Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var piker = (Picker)sender;
            if (piker.SelectedIndex != -1)
            {
                var igrac2 = piker.ItemsSource[piker.SelectedIndex] as SharedModels.Users;
                viewModel.Igrac2 = igrac2.ID;
            }
            else
                viewModel.Igrac2 = -1;
        }
    }
}