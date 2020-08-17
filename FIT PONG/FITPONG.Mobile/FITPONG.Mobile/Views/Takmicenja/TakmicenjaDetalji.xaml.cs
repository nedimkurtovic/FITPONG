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
    public partial class TakmicenjaDetalji : ContentPage
    {
        readonly TakmicenjaDetaljiViewModel viewModel;
        
        public TakmicenjaDetalji(TakmicenjaDetaljiViewModel vm)
        {
            InitializeComponent();
            BindingContext = viewModel = vm;
            if (vm.Vlasnik())
                DodatneOpcijeLayout.IsVisible = true;
            else
                DodatneOpcijeLayout.IsVisible = false;

            lblEmpty.IsVisible = viewModel.listaPrijava.Count == 0;
            btnGenerisiRaspored.IsVisible = !vm.Takmicenje.Inicirano ?? true;
            btnPrijava.IsVisible = !vm.Takmicenje.Inicirano ?? true;

        }
        public TakmicenjaDetalji()
        {
            InitializeComponent();
        }

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TakmicenjaEdit(viewModel.Takmicenje));
        }

        private async void btnGenerisiRaspored_Clicked(object sender, EventArgs e)
        {
            var rezultat = await viewModel.InicirajTakmicenje();
            if(rezultat != default(SharedModels.Takmicenja))
            {
                var mainStranica = Navigation.NavigationStack[1];
                Navigation.InsertPageBefore(new TakmicenjaMain(new TakmicenjaDetaljiViewModel(rezultat)), mainStranica);
                bool brisi = false;

                List<Page> listaBrisanja = new List<Page>();
                foreach (var i in Navigation.NavigationStack)
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

        private async void btnPrijava_Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new TakmicenjePrijava(viewModel.Takmicenje));
        }

        private async void btnBlokiraj_Clicked(object sender, EventArgs e)
        {
            var x = ((Button)sender).BindingContext;

            await viewModel.BlokirajPrijavu(int.Parse(x.ToString()));
        }
    }
}