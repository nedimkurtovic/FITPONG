using FIT_PONG.Mobile.ViewModels.Takmicenja;
using FIT_PONG.SharedModels;
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
    public partial class TakmicenjaDodaj : ContentPage
    {
        TakmicenjaDodajViewModel viewModel;
        public TakmicenjaDodaj()
        {
            BindingContext = viewModel = new TakmicenjaDodajViewModel();
            viewModel.NapuniComboBoxes();
            InitializeComponent();
            //ako stavis u xamlu binding context, kreira on za sebe viewmodel i briga ga za ostalo, probisvijet pravi.
            //uglavnom tamo stavis binding context ako oces malo intellisense ali cim zavrsis izbacuj. Konkretno ovdje
            //je problem sto sam htio prije initialize component pozvati punjenje comboboxova, a viewmodel se na xamlu
            //tek kreira kad pozoves initialize component pa nije bilo bas nacina da to izvedem osim ovako, znaci 
            //obrisao sam tamo binding context i ovdje ga manualno inicirao, svi problemi su bili to sto kad pozovem
            //komandu dodaj, u viewmodelu bude sve prazno jer je ovaj sve dodavao u viewmodel koji je tamo u binding
            //contextu
            katPiker.ItemDisplayBinding = new Binding("Opis");
            vrsPiker.ItemDisplayBinding = new Binding("Naziv");
            sisPiker.ItemDisplayBinding = new Binding("Opis");

            katPiker.ItemsSource = viewModel.ListaKategorija;
            vrsPiker.ItemsSource = viewModel.ListaVrsta;
            sisPiker.ItemsSource = viewModel.ListaSistema;
        }

        private void RucniOdabir_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            viewModel.rucnaVidljivost = !viewModel.rucnaVidljivost;

            //a mogao sam ih sve fino u stacklayout stavit i jednom linijom rijesiti sve, ALI JOK!!!!
            lblKorisnicka.IsVisible = viewModel.rucnaVidljivost;
            editorKorisnicka.IsVisible = viewModel.rucnaVidljivost;

            lblDatumPP.IsVisible = !viewModel.rucnaVidljivost;
            dtpDatumPP.IsVisible = !viewModel.rucnaVidljivost;

            lblDatumZP.IsVisible = !viewModel.rucnaVidljivost;
            dtpDatumZP.IsVisible = !viewModel.rucnaVidljivost;

            lblMinELO.IsVisible = !viewModel.rucnaVidljivost;
            txtMinELO.IsVisible = !viewModel.rucnaVidljivost;

            if (viewModel.rucnaVidljivost)
            {
                var vrs = viewModel.ListaVrsta.Where(x => x.Naziv == "Single").FirstOrDefault();
                var index = vrsPiker.ItemsSource.IndexOf(vrs);
                vrsPiker.SelectedIndex = index;
                vrsPiker.IsEnabled = false;
            }
            else
                vrsPiker.IsEnabled = true;
        }

        private void Seeded_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            viewModel.Seeded = !viewModel.Seeded;
        }

        private void Sistem_SelectedIndexChanged(object sender, EventArgs e)
        {
            var piker = (Picker)sender;
            if (piker.SelectedIndex != -1)
            {
                //var sistemski = piker.SelectedItem as SistemiTakmicenja;
                var sistemski = piker.ItemsSource[piker.SelectedIndex] as SistemiTakmicenja;
                //viewModel.SistemID = sistemski.ID;
                var modl = BindingContext as TakmicenjaDodajViewModel;
                modl.SistemID = sistemski.ID;
            }
            else
                viewModel.SistemID = -1;
        }

        private void Vrsta_SelectedIndexChanged(object sender, EventArgs e)
        {
            var piker = (Picker)sender;
            if (piker.SelectedIndex != -1)
            {
                var vrstovski = piker.ItemsSource[piker.SelectedIndex] as VrsteTakmicenja;
                viewModel.VrstaID = vrstovski.ID;
            }
            else
                viewModel.VrstaID = -1;
        }

        private void Kategorija_SelectedIndexChanged(object sender, EventArgs e)
        {
            var piker = (Picker)sender;
            if (piker.SelectedIndex != -1)
            {
                var kategorijski = piker.ItemsSource[piker.SelectedIndex] as KategorijeTakmicenja;
                viewModel.KategorijaID = kategorijski.ID;
            }
            else
                viewModel.KategorijaID = -1;
        }

        private async void Dodaj_Clicked(object sender, EventArgs e)
        {
            var rezultat = await viewModel.DodajTakmicenjeFunkcija();
            if (rezultat != default(SharedModels.Takmicenja))
            {
                Navigation.InsertPageBefore(new TakmicenjaMain(new TakmicenjaDetaljiViewModel(rezultat)), this);
                await Navigation.PopAsync();
            }

        }
    }
}