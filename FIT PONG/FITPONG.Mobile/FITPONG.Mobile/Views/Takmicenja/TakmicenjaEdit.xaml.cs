using FIT_PONG.Mobile.ViewModels.Takmicenja;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    public partial class TakmicenjaEdit : ContentPage
    {
        TakmicenjaEditViewModel viewModel;
        public TakmicenjaEdit(SharedModels.Takmicenja takm)
        {
            InitializeComponent();
            BindingContext = viewModel = new TakmicenjaEditViewModel(takm);
            datumiPrijavaLayout.IsVisible = viewModel.DatumiPrijavaVisible();
        }


        private void cbDatumPP_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            dtpDatumPP.IsEnabled = !dtpDatumPP.IsEnabled;
            if (!dtpDatumPP.IsEnabled)
                viewModel.RokPocetkaPrijave = null;
        }

        private void cbDatumZP_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            dtpDatumZP.IsEnabled = !dtpDatumZP.IsEnabled;
            if (!dtpDatumZP.IsEnabled)
                viewModel.RokZavrsetkaPrijave = null;
        }
        private void cbDatumP_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            dtpDatumP.IsEnabled = !dtpDatumP.IsEnabled;
            if (!dtpDatumZP.IsEnabled)
                viewModel.DatumPocetka = null;
        }

        private async void btnSpasi_Clicked(object sender, EventArgs e)
        {
            var rezultat = await viewModel.PozoviApi();
            if (rezultat != default(SharedModels.Takmicenja))
            {
                var mainStranica = Navigation.NavigationStack[1];
                Navigation.InsertPageBefore(new TakmicenjaMain(new TakmicenjaDetaljiViewModel(rezultat)), mainStranica);
                bool brisi = false;

                List<Page> listaBrisanja = new List<Page>();
                foreach(var i in Navigation.NavigationStack)
                {
                    if (brisi)
                        listaBrisanja.Add(i);
                    if (i is TakmicenjaMain && !brisi)
                        brisi = true;
                }
                //kad naleti na prvu main stavlja na true i sve ostale stranice brise poslije
                var a = Navigation.NavigationStack[0].BindingContext as TakmicenjaListaViewModel;
                if (a.DobaviTakmicenja.CanExecute(null))
                    a.DobaviTakmicenja.Execute(null);
                foreach (var i in listaBrisanja)
                    Navigation.RemovePage(i);

            }

        }
    }
}