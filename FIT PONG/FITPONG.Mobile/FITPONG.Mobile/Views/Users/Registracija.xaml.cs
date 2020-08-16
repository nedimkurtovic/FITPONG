using FIT_PONG.Mobile.Helpers;
using FIT_PONG.Mobile.ViewModels.Users;
using FIT_PONG.SharedModels;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registracija : ContentPage
    {
        RegistracijaViewModel viewModel;
        public Registracija()
        {
            BindingContext = viewModel = new RegistracijaViewModel();
            InitializeComponent();
            //container.Margin = new Thickness(20, 0);
            //container.Padding = new Thickness(40, 0);
        }

        private void spolPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var piker = (Picker)sender;
            if (piker.SelectedIndex != -1)
            {
                var spol = piker.ItemsSource[piker.SelectedIndex] as string;
                viewModel.Spol = spol[0];
            }
            else
                viewModel.Spol = default(char);
        }

        private void gradPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var piker = (Picker)sender;
            if (piker.SelectedIndex != -1)
            {
                var grad = piker.ItemsSource[piker.SelectedIndex] as Gradovi;
                viewModel.GradId = grad.ID;
            }
            else
                viewModel.GradId = -1;

        }

        private void jacarukaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var piker = (Picker)sender;
            if (piker.SelectedIndex != -1)
            {
                var jacaruka = piker.ItemsSource[piker.SelectedIndex] as string;
                viewModel.JacaRuka = jacaruka;
            }
            else
                viewModel.JacaRuka = null;
        }

        private async void btnDodajSliku_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Greska", "Nije moguće odabrati sliku na ovom uređaju", "OK");
                return;
            }

            //var file = await CrossMedia.Current.PickPhotosAsync(new Plugin.Media.Abstractions.PickMediaOptions
            //{
            //    CompressionQuality = 80
            //});

            var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());

            if (file == null)
                return;

            var nizBajtova = Helperi.ReadToEnd(file.GetStream());
            var naziv = file.Path.Substring(file.Path.LastIndexOf("\\") + 1);
            Fajl slikaProfila = new Fajl
            {
                Naziv = naziv,
                BinarniZapis = nizBajtova
            };

            viewModel.SlikaProfila = slikaProfila;
        }
    }
}