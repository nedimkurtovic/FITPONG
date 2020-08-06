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
    public partial class TakmicenjaObjaveDetalji : ContentPage
    {
        private TakmicenjaObjaveDetaljiViewModel viewModel;
        public TakmicenjaObjaveDetalji(SharedModels.Objave _objava,SharedModels.Takmicenja takmicenje)
        {
            InitializeComponent();
            BindingContext = viewModel = new TakmicenjaObjaveDetaljiViewModel(_objava,takmicenje);

            dodatniLayout.IsVisible = viewModel.Vlasnik();

        }

        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TakmicenjaObjavaEdit(viewModel.Objava,viewModel.Takmicenje));
        }

        private async void btnObrisi_Clicked(object sender, EventArgs e)
        {

            var rez = await Application.Current.MainPage.DisplayAlert("Upozorenje", "Ovo je nepovratna radnja, nastaviti?", "DA", "NE");
            if (rez)
            {
                var rezultat = await viewModel.ObrisiObjavu();
                if (rezultat != default(SharedModels.Objave))
                {
                    var takmicenjamain = Navigation.NavigationStack[1] as TakmicenjaMain;
                    takmicenjamain.OsvjeziListuObjava();

                    await Navigation.PopAsync();
                }
            }
        }
    }
}