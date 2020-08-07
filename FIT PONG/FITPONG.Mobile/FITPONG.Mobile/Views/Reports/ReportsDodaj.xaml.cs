using FIT_PONG.Mobile.Helpers;
using FIT_PONG.Mobile.Models;
using FIT_PONG.Mobile.ViewModels.Reports;
using FIT_PONG.SharedModels;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Reports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportsDodaj : ContentPage
    {
        ReportsDodajViewModel viewModel;
        public ReportsDodaj()
        {
            //kako sam vidio da raja radi -> initialize component(u xamlu je referencirao viewmodel) pa onda ovdje kuca obrnutno =>
            //viewModel = (imeviewmodela) BindingContext; a ne ovako kako smo mi radili pa se krieraju 2 razlicite instance, al kare dobra je praksa
            //imati i ovdje i tamo, jest kad se instancira dobro
            InitializeComponent();
            BindingContext = viewModel = new ReportsDodajViewModel();
        }

        private async void Posalji_Clicked(object sender, EventArgs e)
        {
            var rezultat = await viewModel.PosaljiReport();
            if (rezultat != default(SharedModels.Reports))
            {
                Navigation.InsertPageBefore(new ReportsDodajUspjeh(), this);
                await Navigation.PopAsync();
            }

        }

        private async void btnDodajSlike_Clicked(object sender, EventArgs e)
        {

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Greska", "Nije moguće odabrati sliku na ovom uređaju", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotosAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                CompressionQuality = 80
            }, new Plugin.Media.Abstractions.MultiPickerOptions { MaximumImagesCount = 3 });

            if (file == null)
                return;
            viewModel.Prilozi.Clear();
            foreach (var i in file)
            {
                var nizBajtova = Helperi.ReadToEnd(i.GetStream());
                var naziv = i.Path.Substring(i.Path.LastIndexOf("\\") + 1);
                Fajl testni = new Fajl
                {
                    Naziv = naziv,
                    BinarniZapis = nizBajtova
                };
                viewModel.Prilozi.Add(testni);
            }
            //await DisplayAlert("File Location", file.Path, "OK");
            //var nizBajtova = ReadToEnd(file.GetStream());


            //image.Source = ImageSource.FromStream(() =>
            //{
            //    var stream = file.GetStream();
            //    return stream;
            //});
        }
    }
    
}