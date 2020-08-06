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
    public partial class TakmicenjaObjavaEdit : ContentPage
    {
        TakmicenjaObjaveEditViewModel viewModel;
        private readonly SharedModels.Takmicenja takmicenje;
        public TakmicenjaObjavaEdit(SharedModels.Objave obj, SharedModels.Takmicenja _takmicenje)
        {
            InitializeComponent();
            takmicenje = _takmicenje;
            BindingContext = viewModel = new TakmicenjaObjaveEditViewModel(obj);
        }

        private async void Spasi_Clicked(object sender, EventArgs e)
        {
            var dajstek = Navigation.NavigationStack;
            var ovojelista = Navigation.NavigationStack[2];

            var rezultat = await viewModel.UpdateObjavu();
            if (rezultat != default(SharedModels.Objave))
            {
                Page stranica=null;
                foreach (var i in Navigation.NavigationStack)
                {
                    if (i is TakmicenjaObjaveDetalji)
                        stranica = i;
                }
                Navigation.InsertPageBefore(new TakmicenjaObjaveDetalji(rezultat,takmicenje), stranica);
                bool brisi = false;
                List<Page> listaBrisanja = new List<Page>();
                foreach (var i in Navigation.NavigationStack)
                {
                    if (brisi)
                        listaBrisanja.Add(i);
                    if (i is TakmicenjaObjaveDetalji && !brisi)
                        brisi = true;
                }
                var takmicenjamain = Navigation.NavigationStack[1] as TakmicenjaMain;
                takmicenjamain.OsvjeziListuObjava();

                foreach (var i in listaBrisanja)
                    Navigation.RemovePage(i);

            }
        }
    }
}